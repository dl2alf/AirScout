/*
 * Author: G. Monz (DK7IO), 2011-11-30
 * This file is distributed without any warranty.
 * */

using System;

namespace ScoutBase
{
	/// <summary>
	/// Utility methods.
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Retrieves a numerical format string for .NET string formatting.
		/// </summary>
		/// <param name="integerDigits">The number of integer digits (before decimal point).</param>
		/// <param name="decimalPlaces">The number of decimal places (after decimal point).</param>
		/// <returns>The format string.</returns>
		public static string GetFormatString(int integerDigits, int decimalPlaces)
		{
			string result = "{0:" + new string('0', integerDigits);

			if (decimalPlaces > 0)
			{
				result += "." + new string('0', decimalPlaces);
			}

			result += "}";

			return result;
		}

		/// <summary>
		/// Rounds angle values to specific decimal places.
		/// </summary>
		/// <param name="decimalPlaces">The number of decimal places (after decimal point).</param>
		/// <param name="degrees">The degrees without sign (as integer).</param>
		/// <param name="minutes">The minutes without sign (with decimal places).</param>
		public static void Round(
			int decimalPlaces,
			ref int degrees, ref double minutes
		)
		{
			if (Math.Round(minutes, decimalPlaces) == 60)
			{
				minutes = 0;
				degrees++;
			}
		}

		/// <summary>
		/// Rounds angle values to specific decimal places.
		/// </summary>
		/// <param name="decimalPlaces">The number of decimal places (after decimal point).</param>
		/// <param name="degrees">The degrees without sign (as integer).</param>
		/// <param name="minutes">The minutes without sign (as integer).</param>
		/// <param name="seconds">The seconds without sign (with decimal places).</param>
		public static void Round(
			int decimalPlaces,
			ref int degrees, ref int minutes, ref double seconds
		)
		{
			if (Math.Round(seconds, decimalPlaces) == 60)
			{
				seconds = 0;
				minutes++;
			}

			if (minutes == 60)
			{
				minutes = 0;
				degrees++;
			}
		}

		/// <summary>
		/// Rounds to a specified interval.
		/// </summary>
		/// <param name="value">A double-precision floating-point number to be rounded.</param>
		/// <param name="interval">The interval.</param>
		/// <returns>The rounded value.</returns>
		public static double Round(double value, double interval)
		{
			double y = value / interval;
			int q = (int)Math.Round(y, MidpointRounding.AwayFromZero);
			double result = q * interval;

			return result;
		}

		/// <summary>
		/// Converts a degree value into units of angle.
		/// </summary>
		/// <param name="decDegrees">The degree value (with decimal places).</param>
		/// <param name="isNegative">True if the value is negative. False if positive.</param>
		/// <param name="degrees">The degrees without sign (with decimal places).</param>
		public static void DegreesToD(
			double decDegrees,
			out bool isNegative, out double degrees
		)
		{
			isNegative = (decDegrees < 0);
			degrees = Math.Abs(decDegrees);
		}

		/// <summary>
		/// Converts a degree value into units of angle.
		/// </summary>
		/// <param name="decDegrees">The degree value (with decimal places).</param>
		/// <param name="isNegative">True if the value is negative. False if positive.</param>
		/// <param name="degrees">The degrees without sign (as integer).</param>
		/// <param name="minutes">The minutes without sign (with decimal places).</param>
		public static void DegreesToDM(
			double decDegrees,
			out bool isNegative, out int degrees, out double minutes
		)
		{
			double absDegrees;
			DegreesToD(decDegrees, out isNegative, out absDegrees);

			degrees = (int)absDegrees;
			double fraction = absDegrees - degrees;
			minutes = 60 * fraction;
		}

		/// <summary>
		/// Converts a degree value into units of angle.
		/// </summary>
		/// <param name="decDegrees">The degree value (with decimal places).</param>
		/// <param name="isNegative">True if the value is negative. False if positive.</param>
		/// <param name="degrees">The degrees without sign (as integer).</param>
		/// <param name="minutes">The minutes without sign (as integer).</param>
		/// <param name="seconds">The seconds without sign (with decimal places).</param>
		public static void DegreesToDMS(
			double decDegrees,
			out bool isNegative, out int degrees, out int minutes, out double seconds
		)
		{
			double decMinutes;
			DegreesToDM(decDegrees, out isNegative, out degrees, out decMinutes);

			minutes = (int)decMinutes;
			double fraction = decMinutes - minutes;
			seconds = 60 * fraction;
		}

		/// <summary>
		/// Converts degrees, minutes and seconds to degrees.
		/// </summary>
		/// <param name="isNegative">True if the sign is negative. False if positive.</param>
		/// <param name="degrees">The degrees.</param>
		/// <param name="minutes">The minutes.</param>
		/// <returns>The new degrees.</returns>
		public static double GetDegrees(
			bool isNegative,
			int degrees, double minutes
		)
		{
			double result =
				Math.Abs(degrees) +
				Math.Abs(minutes) / 60d
			;

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}

		/// <summary>
		/// Converts degrees, minutes and seconds to degrees.
		/// </summary>
		/// <param name="isNegative">True if the sign is negative. False if positive.</param>
		/// <param name="degrees">The degrees.</param>
		/// <param name="minutes">The minutes.</param>
		/// <param name="seconds">The seconds.</param>
		/// <returns>The new degrees.</returns>
		public static double GetDegrees(
			bool isNegative,
			int degrees, int minutes, double seconds
		)
		{
			double result =
				Math.Abs(degrees) +
				Math.Abs(minutes) / 60d +
				Math.Abs(seconds) / 3600d
			;

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}
	}
}