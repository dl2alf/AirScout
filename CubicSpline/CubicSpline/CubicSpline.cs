//
// Author: Ryan Seghers
//
// Copyright (C) 2013 Ryan Seghers
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the irrevocable, perpetual, worldwide, and royalty-free
// rights to use, copy, modify, merge, publish, distribute, sublicense, 
// display, perform, create derivative works from and/or sell copies of 
// the Software, both in source and object code form, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

namespace CubicSpline
{
	/// <summary>
	/// Cubic spline interpolation.
	/// Call Fit to compute spline coefficients, then Eval to evaluate the spline at other X coordinates.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is implemented based on the wikipedia article:
	/// http://en.wikipedia.org/wiki/Spline_interpolation
	/// I'm not sure I have the right to include a copy of the article so the equation numbers referenced in 
	/// comments will end up being wrong at some point.
	/// </para>
	/// <para>
	/// This is not optimized, and is not MT safe.
	/// This can extrapolate off the ends of the splines.
	/// You must provide points in X sort order.
	/// </para>
	/// </remarks>
	public class CubicSpline
	{
		// N-1 spline coefficients for N points
		private float[] a;
		private float[] b;

		// Save the original x and y for Eval
		private float[] xOrig;
		private float[] yOrig;

		/// <summary>
		/// Fit x,y and then eval at points xs and return the corresponding y's.
		/// This does the "natural spline" style for ends.
		/// This can extrapolate off the ends of the splines.
		/// You must provide points in X sort order.
		/// </summary>
		/// <param name="x">Input. X coordinates to fit.</param>
		/// <param name="y">Input. Y coordinates to fit.</param>
		/// <param name="xs">Input. X coordinates to evaluate the fitted curve at.</param>
		/// <returns>The computed y values for each xs.</returns>
		public float[] FitAndEval(float[] x, float[] y, float[] xs, bool debug = false)
		{
			Fit(x, y, debug);
			return Eval(xs, debug);
		}

		/// <summary>
		/// Compute spline coefficients for the specified x,y points.
		/// This does the "natural spline" style for ends.
		/// This can extrapolate off the ends of the splines.
		/// You must provide points in X sort order.
		/// </summary>
		/// <param name="x">Input. X coordinates to fit.</param>
		/// <param name="y">Input. Y coordinates to fit.</param>
		/// <param name="debug">Turn on console output. Default is false.</param>
		public void Fit(float[] x, float[] y, bool debug = false)
		{
			// Save x and y for eval
			this.xOrig = x;
			this.yOrig = y;

			int n = x.Length;
			float[] r = new float[n]; // the right hand side numbers: wikipedia page overloads b

			TriDiagonalMatrixF m = new TriDiagonalMatrixF(n);
			float dx1, dx2, dy1, dy2;

			// First row is different (equation 16 from the article)
			dx1 = x[1] - x[0];
			m.C[0] = 1.0f / dx1;
			m.B[0] = 2.0f * m.C[0];
			r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);

			// Body rows (equation 15 from the article)
			for (int i = 1; i < n - 1; i++)
			{
				dx1 = x[i] - x[i - 1];
				dx2 = x[i + 1] - x[i];

				m.A[i] = 1.0f / dx1;
				m.C[i] = 1.0f / dx2;
				m.B[i] = 2.0f * (m.A[i] + m.C[i]);

				dy1 = y[i] - y[i - 1];
				dy2 = y[i + 1] - y[i];
				r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
			}

			// Last row also different (equation 17 from the article)
			dx1 = x[n - 1] - x[n - 2];
			dy1 = y[n - 1] - y[n - 2];
			m.A[n - 1] = 1.0f / dx1;
			m.B[n - 1] = 2.0f * m.A[n - 1];
			r[n - 1] = 3 * (dy1 / (dx1 * dx1));

			if (debug) Console.WriteLine("Tri-diagonal matrix:\n{0}", m.ToDisplayString(":0.0000", "  "));
			if (debug) Console.WriteLine("r: {0}", ArrayUtil.ToString<float>(r));

			// k is the solution to the matrix
			float[] k = m.Solve(r);
			if (debug) Console.WriteLine("k = {0}", ArrayUtil.ToString<float>(k));

			// a and b are each spline's coefficients
			this.a = new float[n - 1];
			this.b = new float[n - 1];

			for (int i = 1; i < n; i++)
			{
				dx1 = x[i] - x[i - 1];
				dy1 = y[i] - y[i - 1];
				a[i - 1] = k[i - 1] * dx1 - dy1; // equation 10 from the article
				b[i - 1] = -k[i] * dx1 + dy1; // equation 11 from the article
			}

			if (debug) Console.WriteLine("a: {0}", ArrayUtil.ToString<float>(a));
			if (debug) Console.WriteLine("b: {0}", ArrayUtil.ToString<float>(b));
		}

		/// <summary>
		/// Evaluate the spline at the specified x coordinates.
		/// This can extrapolate off the ends of the splines.
		/// You must provide X's in ascending order.
		/// </summary>
		/// <param name="x">Input. X coordinates to evaluate the fitted curve at.</param>
		/// <param name="debug">Turn on console output. Default is false.</param>
		/// <returns>The computed y values for each x.</returns>
		public float[] Eval(float[] x, bool debug = false)
		{
			int n = x.Length;
			float[] y = new float[n];
			_lastIndex = 0; // Reset simultaneous traversal in case there are multiple calls

			for (int i = 0; i < n; i++)
			{
				// Find which spline can be used to compute this x
				int j = GetNextXIndex(x[i]);

				// Evaluate using j'th spline
				float t = (x[i] - xOrig[j]) / (xOrig[j + 1] - xOrig[j]);
				y[i] = (1 - t) * yOrig[j] + t * yOrig[j + 1] + t * (1 - t) * (a[j] * (1 - t) + b[j] * t); // equation 9

				if (debug) Console.WriteLine("[{0}]: xs = {1}, j = {2}, t = {3}", i, x[i], j, t);
			}

			return y;
		}

		private int _lastIndex = 0;

		/// <summary>
		/// Find where in xOrig the specified x falls, by simultaneous traverse.
		/// This allows xs to be less than x[0] and/or greater than x[n-1]. So allows extrapolation.
		/// This keeps state, so requires that x be sorted and xs called in ascending order.
		/// </summary>
		private int GetNextXIndex(float x)
		{
			while ((_lastIndex < xOrig.Length - 2) && (x > xOrig[_lastIndex + 1]))
			{
				_lastIndex++;
			}

			return _lastIndex;
		}
	}
}
