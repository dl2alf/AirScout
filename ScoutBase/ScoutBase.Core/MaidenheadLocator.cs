/*
 * Author: G. Monz (DK7IO), 2011-11-30
 * This file is distributed without any warranty.
 * */

using System;
using System.Collections.Generic;
using System.Windows;

namespace ScoutBase.Core
{
	/// <summary>
	/// Converts geographical coordinates to a 'Maidenhead Locator' and vice versa.
	/// </summary>
	public static class MaidenheadLocator
	{
		#region Constants
		#region Number of zones
		/// <summary>
		/// Number of zones for 'Field' (Precision step 1).
		/// </summary>
		public const int ZonesOddStep1 = 18;

		/// <summary>
		/// Number of zones for 'Subsquare', 'Subsubsubsquare', etc. (Precision steps 3, 5, etc.).
		/// </summary>
		public const int ZonesOddStepsExcept1 = 24;

		/// <summary>
		/// Number of zones for 'Square', 'Subsubsquare', etc. (Precision steps 2, 4, etc.).
		/// </summary>
		public const int ZonesEvenSteps = 10;
		#endregion

		#region First characters for locator text
		/// <summary>
		/// The first character for 'Field' (Precision step 1).
		/// </summary>
		public const char FirstOddStep1Character = 'A';

		/// <summary>
		/// The first character for 'Subsquare', 'Subsubsubsquare', etc. (Precision steps 3, 5, etc.).
		/// </summary>
		public const char FirstOddStepsExcept1Character = 'a';

		/// <summary>
		/// The first character for 'Square', 'Subsubsquare', etc. (Precision steps 2, 4, etc.).
		/// </summary>
		public const char FirstEvenStepsCharacter = '0';
		#endregion

		#region Implementation constraints
		/// <summary>
		/// The lowest allowed precision.
		/// </summary>
		public const int MinPrecision = 1;

		/// <summary>
		/// The highest allowed precision.
		/// </summary>
		public const int MaxPrecision = 9;
		#endregion
		#endregion

		/// <summary>
		/// The subgrids filter within a grid.
		/// </summary>
		public enum SubGridsFilter
		{
			/// <summary>
			/// All subgrids.
			/// </summary>
			All,

			/// <summary>
			/// Top subgrids only.
			/// </summary>
			Top,

			/// <summary>
			/// Bottom subgrids only.
			/// </summary>
			Bottom,

			/// <summary>
			/// Left subgrids only.
			/// </summary>
			Left,

			/// <summary>
			/// Right subgrids only.
			/// </summary>
			Right,
		}

        /// <summary>
        /// Checks if a string is a valid Maidenhead Locator.
        /// </summary>
        /// <param name="maidenheadLocator">The Maidenhead Locator.</param>
        /// <returns>True if the given string is a valid Maidenhead Locator.</returns>
        public static bool Check(string maidenheadLocator)
        {
            // chekc for empty string
            if (String.IsNullOrEmpty(maidenheadLocator))
                return false;
            // make string upper
            maidenheadLocator = maidenheadLocator.ToUpper();
            // string must have even length
            if (maidenheadLocator.Length % 2 != 0)
                return false;
            // string must have MinPrecision
            if (maidenheadLocator.Length / 2 < MinPrecision)
                return false;
            // string must not have more than MaxPrecision
            if (maidenheadLocator.Length / 2 > MaxPrecision)
                return false;
            for (int i = 0; i < maidenheadLocator.Length; i++)
            {
                if ((i % 4) <= 1)
                {
                    if ((maidenheadLocator[i] < 'A') || (maidenheadLocator[i] > 'X'))
                        return false;
                }
                else
                {
                    if (!char.IsDigit(maidenheadLocator[i]))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a given location (by lat/lon) is precise, e.g. is not located at midpoint of a given Maidenhead Locator
        /// </summary>
        /// <param name="latitude">The geographic latitude.</param>
        /// <param name="longitude">The geographic longitude.</param>
        /// <param name="precision">
        /// The precision for conversion, must be &gt;=1 and &lt;=9.
        /// <para></para>
        /// <list type="bullet">
        ///		<listheader>
        ///			<description>Examples for precision use:</description>
        ///		</listheader>
        ///		<item><term>precision1</term><description>HF: 'Field' only is needed -&gt; precision=1 -&gt; JN</description></item>
        ///		<item><term>precision2</term><description>6m: 'Field' and 'Square' is needed -&gt; precision=2 -&gt; JN39</description></item>
        ///		<item><term>precision3</term><description>VHF/UHF: 'Field' until 'Subsquare' is needed -&gt; precision=3 -&gt; JN39ml</description></item>
        ///		<item><term>precision4</term><description>SHF/EHF: 'Field' until 'Subsubsquare' is needed -&gt; precision=4 -&gt; JN39ml36</description></item>
        /// </list>
        /// <returns>True if the given location is precise .</returns>
        public static bool IsPrecise(double latitude, double longitude, int precision)
        {
            double mlat, mlon;
            string loc = LocFromLatLon(latitude, longitude, false, precision);
            if (String.IsNullOrEmpty(loc))
                return false;
            LatLonFromLoc(loc, PositionInRectangle.MiddleMiddle, out mlat, out mlon);
            return (Math.Abs(mlat - latitude) > 0.00001) || (Math.Abs(mlon - longitude) > 0.00001);
        }

        /// <summary>
        /// Converts a given Maidenhead Locator with format options
        /// </summary>
        /// <param name="maidenheadLocator">The Maidenhead Locator.</param>
        /// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
        /// <returns>The new formatted Maidenhead Locator.</returns>
        /// <exception cref="ArgumentException">If the string is not a Maidenhead locator.</exception>
        public static string Convert(string maidenheadLocator, bool smallLettersForSubsquares)
        {
            // return null on null
            if (maidenheadLocator == null)
                return null;
            // return empty string on empty
            if (maidenheadLocator == "")
                return "";
            // check
            if (!Check(maidenheadLocator))
                throw new ArgumentException("This is not a valid Maidenhead Locator: " + maidenheadLocator);
            string loc = "";
            for (int i = 0; i < maidenheadLocator.Length; i++)
            {
                if (i <= 1)
                    loc = loc + Char.ToUpper(maidenheadLocator[i]);
                else if (smallLettersForSubsquares)
                {
                    loc = loc + Char.ToLower(maidenheadLocator[i]);
                }
                else
                {
                    loc = loc + Char.ToUpper(maidenheadLocator[i]);
                }
            }
            return loc;
        }

        /// <summary>
        /// Converts geographical coordinates (latitude and longitude, in degrees)
        /// to a 'Maidenhead Locator' until a specific precision.
        /// The maximum precision is 9 due to numerical limits of floating point operations.
        /// </summary>
        /// <param name="latitude">
        /// The latitude to convert ([-90...+90]).
        /// +90 is handled like +89.999...
        /// </param>
        /// <param name="longitude">
        /// The longitude to convert ([-180...+180]).
        /// +180 is handled like +179.999...
        /// </param>
        /// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
        /// <param name="precision">
        /// The precision for conversion, must be &gt;=1 and &lt;=9.
        /// <para></para>
        /// <list type="bullet">
        ///		<listheader>
        ///			<description>Examples for precision use:</description>
        ///		</listheader>
        ///		<item><term>precision1</term><description>HF: 'Field' only is needed -&gt; precision=1 -&gt; JN</description></item>
        ///		<item><term>precision2</term><description>6m: 'Field' and 'Square' is needed -&gt; precision=2 -&gt; JN39</description></item>
        ///		<item><term>precision3</term><description>VHF/UHF: 'Field' until 'Subsquare' is needed -&gt; precision=3 -&gt; JN39ml</description></item>
        ///		<item><term>precision4</term><description>SHF/EHF: 'Field' until 'Subsubsquare' is needed -&gt; precision=4 -&gt; JN39ml36</description></item>
        /// </list>
        /// </param>
        /// <returns>The 'Maidenhead Locator'.</returns>
        /// <exception cref="ArgumentException">If the latitude or longitude exceeds its allowed interval.</exception>
        public static string LocFromLatLon(double latitude, double longitude, bool smallLettersForSubsquares, int precision, bool autolength = false)
        {
            string loc;
            // autolength = false -- > return loc with fixed length 
            if (!autolength)
                return LocFromLatLon(latitude, longitude, smallLettersForSubsquares, precision);

            // autolength = true
            int p = 3;
            do
            {
                loc = LocFromLatLon(latitude, longitude, smallLettersForSubsquares, p);
                if (IsPrecise(latitude, longitude, p))
                    p++;
                else
                    break;
            }
            while (p <= precision);
            return loc;
        }
        /// <summary>
        /// Converts geographical coordinates (latitude and longitude, in degrees)
        /// to a 'Maidenhead Locator' until a specific precision.
        /// The maximum precision is 9 due to numerical limits of floating point operations.
        /// </summary>
        /// <param name="latitude">
        /// The latitude to convert ([-90...+90]).
        /// +90 is handled like +89.999...
        /// </param>
        /// <param name="longitude">
        /// The longitude to convert ([-180...+180]).
        /// +180 is handled like +179.999...
        /// </param>
        /// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
        /// <param name="precision">
        /// The precision for conversion, must be &gt;=1 and &lt;=9.
        /// <para></para>
        /// <list type="bullet">
        ///		<listheader>
        ///			<description>Examples for precision use:</description>
        ///		</listheader>
        ///		<item><term>precision1</term><description>HF: 'Field' only is needed -&gt; precision=1 -&gt; JN</description></item>
        ///		<item><term>precision2</term><description>6m: 'Field' and 'Square' is needed -&gt; precision=2 -&gt; JN39</description></item>
        ///		<item><term>precision3</term><description>VHF/UHF: 'Field' until 'Subsquare' is needed -&gt; precision=3 -&gt; JN39ml</description></item>
        ///		<item><term>precision4</term><description>SHF/EHF: 'Field' until 'Subsubsquare' is needed -&gt; precision=4 -&gt; JN39ml36</description></item>
        /// </list>
        /// </param>
        /// <returns>The 'Maidenhead Locator'.</returns>
        /// <exception cref="ArgumentException">If the latitude or longitude exceeds its allowed interval.</exception>
        private static string LocFromLatLon(double latitude, double longitude, bool smallLettersForSubsquares, int precision)
        {
            //Check arguments
            {
                if (!GeographicalPoint.Check(latitude, longitude))
                    return null;
            }

            //Corrections
            {
                //MinPrecision <= precision <= MaxPrecision
                precision = Math.Min(MaxPrecision, Math.Max(MinPrecision, precision));
            }

            //Work
            string result;
            {
                List<char> locatorCharacters = new List<char>();

                double latitudeWork = latitude + (-GeographicalPoint.LowerLatitudeLimit);
                double longitudeWork = longitude + (-GeographicalPoint.LowerLongitudeLimit);

                //Zone size for step "0"
                double height;
                double width;
                InitializeZoneSize(out height, out width);

                for (int step = MinPrecision; step <= precision; step++)
                {
                    int zones;
                    char firstCharacter;
                    RetrieveStepValues(step, smallLettersForSubsquares, out zones, out firstCharacter);

                    //Zone size of current step
                    height /= zones;
                    width /= zones;

                    //Retrieve zones and locator characters
                    int latitudeZone;
                    int longitudeZone;
                    {
                        longitudeZone = GetZone(longitudeWork, width);
                        {
                            char locatorCharacter = (char)(firstCharacter + longitudeZone);
                            locatorCharacters.Add(locatorCharacter);
                        }

                        latitudeZone = GetZone(latitudeWork, height);
                        {
                            char locatorCharacter = (char)(firstCharacter + latitudeZone);
                            locatorCharacters.Add(locatorCharacter);
                        }
                    }

                    if (step <= MaxPrecision - 1)
                    {
                        //Prepare the next step
                        {
                            latitudeWork -= latitudeZone * height;
                            longitudeWork -= longitudeZone * width;

                            //Numerical corrections
                            {
                                if (latitudeWork < 0)
                                {
                                    latitudeWork = 0;
                                }

                                if (longitudeWork < 0)
                                {
                                    longitudeWork = 0;
                                }
                            }
                        }
                    }
                }

                //Build the result (Locator text)
                result = new string(locatorCharacters.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Converts a 'Maidenhead Locator' to geographical coordinates (latitude and longitude, in degrees).
        /// </summary>
        /// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
        /// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
        /// <returns>The geographical latitude.</returns>
        /// <exception cref="ArgumentException">
        /// If the length of the locator text is null or not an even number.
        /// If the locator text contains invalid characters.
        /// </exception>

        public static double LatFromLoc(string maidenheadLocator, PositionInRectangle positionInRectangle = PositionInRectangle.MiddleMiddle)
        {
            double lat, lon;
            LatLonFromLoc(maidenheadLocator, positionInRectangle, out lat, out lon);
            return lat;
        }

        /// <summary>
        /// Converts a 'Maidenhead Locator' to geographical coordinates (latitude and longitude, in degrees).
        /// </summary>
        /// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
        /// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
        /// <returns>The geographical longitude.</returns>
        /// <exception cref="ArgumentException">
        /// If the length of the locator text is null or not an even number.
        /// If the locator text contains invalid characters.
        /// </exception>

        public static double LonFromLoc(string maidenheadLocator, PositionInRectangle positionInRectangle = PositionInRectangle.MiddleMiddle)
        {
            double lat, lon;
            LatLonFromLoc(maidenheadLocator, positionInRectangle, out lat, out lon);
            return lon;
        }

        /// <summary>
        /// Converts a 'Maidenhead Locator' to geographical point (latitude and longitude, in degrees).
        /// </summary>
        /// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
        /// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
        /// <returns>The geographical point as LatLon.GPoint.</returns>
        /// <exception cref="ArgumentException">
        /// If the length of the locator text is null or not an even number.
        /// If the locator text contains invalid characters.
        /// </exception>
        /// <summary>
        public static LatLon.GPoint GPointFromLoc(string maidenheadLocator, PositionInRectangle positionInRectangle = PositionInRectangle.MiddleMiddle)
        {
            double lat, lon;
            LatLonFromLoc(maidenheadLocator, positionInRectangle, out lat, out lon);
            return new LatLon.GPoint(lat, lon);
        }


        /// <summary>
        /// Gives the bounds of a 'Maidenhead Locator' (latitudes and longitudes, in degrees).
        /// </summary>
        /// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
        /// <returns>The geographical bounds as LatLon.GRect.</returns>
        /// <exception cref="ArgumentException">
        /// If the length of the locator text is null or not an even number.
        /// If the locator text contains invalid characters.
        /// </exception>
        /// <summary>
        public static LatLon.GRect BoundsFromLoc(string maidenheadLocator)
        {
            LatLon.GPoint gmin = GPointFromLoc(maidenheadLocator, PositionInRectangle.BottomLeft);
            LatLon.GPoint gmax = GPointFromLoc(maidenheadLocator, PositionInRectangle.TopRight);
            return new LatLon.GRect(gmin.Lat, gmin.Lon, gmax.Lat, gmax.Lon);
        }


        /// <summary>
        /// Converts a 'Maidenhead Locator' to geographical coordinates (latitude and longitude, in degrees).
        /// </summary>
        /// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
        /// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
        /// <param name="latitude">The geographical latitude.</param>
        /// <param name="longitude">The geographical longitude.</param>
        /// <exception cref="ArgumentException">
        /// If the length of the locator text is null or not an even number.
        /// If the locator text contains invalid characters.
        /// </exception>
        public static void LatLonFromLoc(string maidenheadLocator, PositionInRectangle positionInRectangle, out double latitude, out double longitude)
        {
            //Check arguments
            {
                if (
					string.IsNullOrEmpty(maidenheadLocator) ||
					maidenheadLocator.Length % 2 != 0
				)
				{
					throw new ArgumentException("Length of locator text is null or not an even number.", "maidenheadLocator");
				}
			}

			//Corrections
			{
				//Upper cases
				maidenheadLocator = maidenheadLocator.ToUpper();
			}
            //Work
            {
                int precision = maidenheadLocator.Length / 2;

				latitude = GeographicalPoint.LowerLatitudeLimit;
				longitude = GeographicalPoint.LowerLongitudeLimit;

				//Zone size for step "0"
				double height;
				double width;
				InitializeZoneSize(out height, out width);

				for (int step = 1; step <= precision; step++)
				{
					int zones;
					char firstCharacter;
					RetrieveStepValues(step, false, out zones, out firstCharacter);

					//Zone size of current step
					height /= zones;
					width /= zones;

					//Retrieve precision specific geographical coordinates
					double longitudeStep = 0;
					double latitudeStep = 0;
					{
						bool error = false;
						int position = -1;

						if (!error)
						{
							//Longitude

							position = step * 2 - 2;
							char locatorCharacter = maidenheadLocator[position];
							int zone = (int)(locatorCharacter - firstCharacter);

							if (zone >= 0 && zone < zones)
							{
								longitudeStep = zone * width;
							}
							else
							{
								error = true;
							}
						}

						if (!error)
						{
							//Latitude

							position = step * 2 - 1;
							char locatorCharacter = maidenheadLocator[position];
							int zone = (int)(locatorCharacter - firstCharacter);

							if (zone >= 0 && zone < zones)
							{
								latitudeStep = zone * height;
							}
							else
							{
								error = true;
							}
						}

						if (error)
						{
							throw new ArgumentException("Locator text contains an invalid character at position " + (position + 1) + " (Current precision step is " + step + ").", "maidenheadLocator");
						}
					}
					longitude += longitudeStep;
					latitude += latitudeStep;
				}

				//Corrections according argument positionInRectangle
				GeographicalPoint.ShiftPositionInRectangle(ref latitude, ref longitude, positionInRectangle, height, width);
			}
		}

		/// <summary>
		/// Retrieves a list of subgrids for a given 'Maidenhead Locator'.
		/// There is no check for invalid input characters.
		/// </summary>
		/// <param name="subGridsFilter">A <see cref="SubGridsFilter" /> member (subgrids filter).</param>
		/// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
		/// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
		/// <param name="reverse">If true: reverse the order.</param>
		/// <returns>The list of subgrids.</returns>
		/// <exception cref="ArgumentException">If the length of the locator text is not an even number.</exception>
		public static List<string> GetSubGrids(string maidenheadLocator, SubGridsFilter subGridsFilter, bool smallLettersForSubsquares, bool reverse)
		{
			if (maidenheadLocator == null)
			{
				maidenheadLocator = string.Empty;
			}

			int length = maidenheadLocator.Length;

			//Check arguments
			{
				if (length % 2 != 0)
				{
					throw new ArgumentException("Length of locator text is not an even number.", "maidenheadLocator");
				}
			}

			List<string> result = new List<string>();

			int subPrecision = length / 2 + 1;

			int zones;
			char firstCharacter;
			RetrieveStepValues(subPrecision, smallLettersForSubsquares, out zones, out firstCharacter);

			int wStart = 1;
			int wEnd = zones;
			int hStart = 1;
			int hEnd = zones;

			switch (subGridsFilter)
			{
				case SubGridsFilter.Top:
					{
						wStart = 1;
						wEnd = hStart = hEnd = zones;
					}
					break;
				case SubGridsFilter.Bottom:
					{
						wStart = hStart = hEnd = 1;
						wEnd = zones;
					}
					break;
				case SubGridsFilter.Left:
					{
						wStart = wEnd = hStart = 1;
						hEnd = zones;
					}
					break;
				case SubGridsFilter.Right:
					{
						wStart = wEnd = hEnd = zones;
						hStart = 1;
					}
					break;
			}

			for (int wZone = wStart; wZone <= wEnd; wZone++)
			{
				for (int hZone = hStart; hZone <= hEnd; hZone++)
				{
					string subGrid =
						maidenheadLocator +
						(char)(firstCharacter + wZone - 1) +
						(char)(firstCharacter + hZone - 1)
					;
					result.Add(subGrid);
				}
			}

			if (reverse)
			{
				result.Reverse();
			}

			return result;
		}

		static void InitializeZoneSize(out double height, out double width)
		{
			height = GeographicalPoint.UpperLatitudeLimit - GeographicalPoint.LowerLatitudeLimit;
			width = GeographicalPoint.UpperLongitudeLimit - GeographicalPoint.LowerLongitudeLimit;
		}

		static void RetrieveStepValues(int step, bool smallLettersForSubsquares, out int zones, out char firstCharacter)
		{
			if (step % 2 == 0)
			{
				//Step is even

				zones = ZonesEvenSteps;
				firstCharacter = FirstEvenStepsCharacter;
			}
			else
			{
				//Step is odd

				zones = (step == 1 ? ZonesOddStep1 : ZonesOddStepsExcept1);
				firstCharacter = ((step >= 3 && smallLettersForSubsquares) ? FirstOddStepsExcept1Character : FirstOddStep1Character);
			}
		}

		static int GetZone(double value, double interval)
		{
			double factor = value / interval;
			int result = (int)factor;

			//Numerical corrections
			{
				double roundedValue = Utilities.Round(value, interval);
				double diff = roundedValue - value;

				if (diff < 0.0000000000001)
				{
					if (roundedValue > value)
					{
						result++;
					}
				}
			}

			return result;
		}
	}
}