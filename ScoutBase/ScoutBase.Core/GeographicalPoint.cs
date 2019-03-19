/*
 * Author: G. Monz (DK7IO), 2016-07-30
 * This file is distributed without any warranty.
 * 
 */

using System;
using System.Collections.Generic;

namespace ScoutBase.Core
{
	/// <summary>
	/// The position of a geographical coordinate in a rectangle.
	/// </summary>
	public enum PositionInRectangle
	{
		/// <summary>
		/// TopLeft
		/// </summary>
		TopLeft,

		/// <summary>
		/// TopMiddle
		/// </summary>
		TopMiddle,

		/// <summary>
		/// TopRight
		/// </summary>
		TopRight,

		/// <summary>
		/// BottomLeft
		/// </summary>
		BottomLeft,

		/// <summary>
		/// BottomMiddle
		/// </summary>
		BottomMiddle,

		/// <summary>
		/// BottomRight
		/// </summary>
		BottomRight,

		/// <summary>
		/// MiddleLeft
		/// </summary>
		MiddleLeft,

		/// <summary>
		/// MiddleRight
		/// </summary>
		MiddleRight,

		/// <summary>
		/// MiddleMiddle
		/// </summary>
		MiddleMiddle,
	}

	/// <summary>
	/// The options to format a degree value.
	/// </summary>
	public enum AngleFormat
	{
		/// <summary>
		/// Sign, degrees (at least one integer digit).
		/// </summary>
		sD,

		/// <summary>
		/// Sign, degrees (at least two integer digits).
		/// </summary>
		sDD,

		/// <summary>
		/// Sign, degrees (at least one integer digit), minutes (at least one integer digit).
		/// </summary>
		sDM,

		/// <summary>
		/// Sign, degrees (at least two integer digits), minutes (two integer digits).
		/// </summary>
		sDDMM,

		/// <summary>
		/// Sign, degrees (at least one integer digit), minutes (at least one integer digit), seconds (at least one integer digit).
		/// </summary>
		sDMS,

		/// <summary>
		/// Sign, degrees (at least two integer digits), minutes (two integer digits), seconds (two integer digits).
		/// </summary>
		sDDMMSS,
	}

	/// <summary>
	/// A representation of a geographical point.
	/// </summary>
	[Serializable]
	public class GeographicalPoint
	{
		#region Constants
		#region Earth
		/// <summary>
		/// Mean Earth radius in km, according WGS84.
		/// </summary>
		public const double MeanEarthRadius_WGS84 = 6371.0008;
		#endregion

		#region Units
		/// <summary>
		/// The length of a statute mile in kilometres.
		/// </summary>
		public const double StatuteMilesToKilometres = 1.609344;

		/// <summary>
		/// The length of a nautical mile in kilometres.
		/// </summary>
		public const double NauticalMilesToKilometres = 1.852;
		#endregion

		#region Coordinate limits
		/// <summary>
		/// Lower latitude limit.
		/// </summary>
		public const int LowerLatitudeLimit = -90;

		/// <summary>
		/// Upper latitude limit.
		/// </summary>
		public const int UpperLatitudeLimit = +90;

		/// <summary>
		/// Lower longitude limit.
		/// </summary>
		public const int LowerLongitudeLimit = -180;

		/// <summary>
		/// Upper longitude limit.
		/// </summary>
		public const int UpperLongitudeLimit = +180;
		#endregion
		#endregion

		#region Information texts
		/// <summary>
		/// Information about an invalid value of the latitude.
		/// </summary>
		internal static string InvalidLatitudeText = "Invalid value. " +
			"Allowed interval: [-" + Math.Abs(LowerLatitudeLimit) + "...+" + Math.Abs(UpperLatitudeLimit) + "]";

		/// <summary>
		/// Information about an invalid value of the longitude.
		/// </summary>
		internal static string InvalidLongitudeText = "Invalid value. " +
			"Allowed interval: [-" + Math.Abs(LowerLongitudeLimit) + "...+" + Math.Abs(UpperLongitudeLimit) + "]";
		#endregion

		double _latitude;
		/// <summary>
		/// The geographical latitude, in degrees.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// If the latitude exceeds its allowed interval
		/// (can occur in setter only).
		/// </exception>
		public double Latitude
		{
			get
			{
				return _latitude;
			}
			set
			{
				CheckLatitude(value);
				_latitude = value;
			}
		}

		double _longitude;
		/// <summary>
		/// The geographical longitude, in degrees.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// If the longitude exceeds its allowed interval
		/// (can occur in setter only).
		/// </exception>
		public double Longitude
		{
			get
			{
				return _longitude;
			}
			set
			{
				CheckLongitude(value);
				_longitude = value;
			}
		}

		/// <summary>
		/// The name of the instance.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Creates a new instance.
		/// Geographical latitude and longitude is 0.
		/// </summary>
		public GeographicalPoint()
		{
		}

		/// <summary>
		/// Creates a new instance, based on geographical coordinates.
		/// </summary>
		/// <param name="latitude">The geographical latitude.</param>
		/// <param name="longitude">The geographical longitude.</param>
		/// <exception cref="ArgumentException">If latitude or longitude exceeds its allowed interval.</exception>
		public GeographicalPoint(double latitude, double longitude)
			: this()
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		/// <summary>
		/// Creates a new instance, based on geographical coordinates in degrees and minutes.
		/// There is no check on the numerical correctness of the arguments.
		/// </summary>
		/// <param name="latitudeIsNegative">True if the sign of the latitude is negative. False if positive.</param>
		/// <param name="latitudeDegrees">The degrees of the latitude.</param>
		/// <param name="latitudeMinutes">The minutes of the latitude.</param>
		/// <param name="longitudeIsNegative">True if the sign of the longitude is negative. False if positive.</param>
		/// <param name="longitudeDegrees">The degrees of the longitude.</param>
		/// <param name="longitudeMinutes">The minutes of the longitude.</param>
		/// <exception cref="ArgumentException">If the resulting latitude or longitude exceeds its allowed interval.</exception>
		public GeographicalPoint(
			bool latitudeIsNegative, int latitudeDegrees, double latitudeMinutes,
			bool longitudeIsNegative, int longitudeDegrees, double longitudeMinutes
		)
			: this()
		{
			Latitude = Utilities.GetDegrees(latitudeIsNegative, latitudeDegrees, latitudeMinutes);
			Longitude = Utilities.GetDegrees(longitudeIsNegative, longitudeDegrees, longitudeMinutes);
		}

		/// <summary>
		/// Creates a new instance, based on geographical coordinates in degrees, minutes and seconds.
		/// There is no check on the numerical correctness of the arguments.
		/// </summary>
		/// <param name="latitudeIsNegative">True if the sign of the latitude is negative. False if positive.</param>
		/// <param name="latitudeDegrees">The degrees of the latitude.</param>
		/// <param name="latitudeMinutes">The minutes of the latitude.</param>
		/// <param name="latitudeSeconds">The seconds of the latitude.</param>
		/// <param name="longitudeIsNegative">True if the sign of the longitude is negative. False if positive.</param>
		/// <param name="longitudeDegrees">The degrees of the longitude.</param>
		/// <param name="longitudeMinutes">The minutes of the longitude.</param>
		/// <param name="longitudeSeconds">The seconds of the longitude.</param>
		/// <exception cref="ArgumentException">If the resulting latitude or longitude exceeds its allowed interval.</exception>
		public GeographicalPoint(
			bool latitudeIsNegative, int latitudeDegrees, int latitudeMinutes, double latitudeSeconds,
			bool longitudeIsNegative, int longitudeDegrees, int longitudeMinutes, double longitudeSeconds
		)
			: this()
		{
			Latitude = Utilities.GetDegrees(latitudeIsNegative, latitudeDegrees, latitudeMinutes, latitudeSeconds);
			Longitude = Utilities.GetDegrees(longitudeIsNegative, longitudeDegrees, longitudeMinutes, longitudeSeconds);
		}

		/// <summary>
		/// Creates a new instance, based on a 'Maidenhead Locator'.
		/// </summary>
		/// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
		/// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
		/// <exception cref="ArgumentException">
		/// If the length of the locator text is null or not an even number.
		/// If the locator text contains invalid characters.
		/// </exception>
		public GeographicalPoint(string maidenheadLocator, PositionInRectangle positionInRectangle)
			: this()
		{
			double latitude;
			double longitude;
			MaidenheadLocator.LatLonFromLoc(maidenheadLocator, positionInRectangle, out latitude, out longitude);

			Latitude = latitude;
			Longitude = longitude;
		}

		/// <summary>
		/// Gives a precision sorted list of 'Maidenhead Locators', based on the geographical coordinates.
		/// </summary>
		/// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
		/// <returns>The list of 'Maidenhead Locators'.</returns>
		/// <exception cref="ArgumentException">If the latitude or longitude exceeds its allowed interval.</exception>
		public List<string> GetMaidenheadLocators(bool smallLettersForSubsquares)
		{
			var result = new List<string>(MaidenheadLocator.MaxPrecision);
			{
				for (int precision = MaidenheadLocator.MinPrecision; precision <= MaidenheadLocator.MaxPrecision; precision++)
				{
					string maidenheadLocator = GetMaidenheadLocator(smallLettersForSubsquares, precision);
					result.Add(maidenheadLocator);
				}
			}

			return result;
		}

		/// <summary>
		/// Converts the geographical coordinates to a 'Maidenhead Locator'.
		/// </summary>
		/// <param name="smallLettersForSubsquares">If true: generate small (if false: big) letters for 'Subsquares', 'Subsubsquare', etc.</param>
		/// <param name="precision">The precision for conversion, must be &gt;=1 and &lt;=10.</param>
		/// <returns>The 'Maidenhead Locator'.</returns>
		/// <exception cref="ArgumentException">If the latitude or longitude exceeds its allowed interval.</exception>
		public string GetMaidenheadLocator(bool smallLettersForSubsquares, int precision)
		{
			string result = MaidenheadLocator.LocFromLatLon(Latitude, Longitude, smallLettersForSubsquares, precision);
			return result;
		}

		/// <summary>
		/// Gives a text representation of the instance.
		/// </summary>
		/// <param name="angleFormat">A <see cref="AngleFormat" /> member.</param>
		/// <param name="decimalPlaces">The number of decimal places (after decimal point) for the last unit.</param>
		/// <param name="separator">The text between latitude and longitude.</param>
		/// <param name="forceAsciiCharacters">If true: Use 7 Bit ASCII characters only instead of special characters.</param>
		/// <returns>The text representation.</returns>
		public string ToString(
			AngleFormat angleFormat, int decimalPlaces,
			string separator, bool forceAsciiCharacters
		)
		{
			string result =
				GetFormattedText(Latitude, angleFormat, decimalPlaces, forceAsciiCharacters) + " N" +
				separator +
				GetFormattedText(Longitude, angleFormat, decimalPlaces, forceAsciiCharacters) + " E"
			;

			return result;
		}

		/// <summary>
		/// Shifts geographical coordinates within a rectangle.
		/// </summary>
		/// <param name="latitude">The geographical latitude.</param>
		/// <param name="longitude">The geographical longitude.</param>
		/// <param name="positionInRectangle">The desired <see cref="PositionInRectangle" /> value.</param>
		/// <param name="height">The rectangle height.</param>
		/// <param name="width">The rectangle width.</param>
		internal static void ShiftPositionInRectangle(
			ref double latitude, ref double longitude,
			PositionInRectangle positionInRectangle,
			double height, double width
		)
		{
			switch (positionInRectangle)
			{
				case PositionInRectangle.TopLeft:
				case PositionInRectangle.TopMiddle:
				case PositionInRectangle.TopRight:
					latitude += height;
					break;
			}

			switch (positionInRectangle)
			{
				case PositionInRectangle.MiddleLeft:
				case PositionInRectangle.MiddleMiddle:
				case PositionInRectangle.MiddleRight:
					latitude += height / 2;
					break;
			}

			switch (positionInRectangle)
			{
				case PositionInRectangle.TopRight:
				case PositionInRectangle.MiddleRight:
				case PositionInRectangle.BottomRight:
					longitude += width;
					break;
			}

			switch (positionInRectangle)
			{
				case PositionInRectangle.TopMiddle:
				case PositionInRectangle.MiddleMiddle:
				case PositionInRectangle.BottomMiddle:
					longitude += width / 2;
					break;
			}
		}

		/// <summary>
		/// Checks geographical coordinates.
		/// </summary>
		/// <param name="latitude">The geographical latitude.</param>
		/// <param name="longitude">The geographical longitude.</param>
		/// <exception cref="ArgumentException">If latitude or longitude exceeds its allowed interval.</exception>
		public static bool Check(double latitude, double longitude)
		{
			return CheckLatitude(latitude) && CheckLongitude(longitude);
		}

		/// <summary>
		/// Checks a geographical latitude.
		/// </summary>
		/// <param name="latitude">The geographical latitude.</param>
		/// <exception cref="ArgumentException">If the latitude exceeds its allowed interval.</exception>
		public static bool CheckLatitude(double latitude)
		{
            return (latitude >= LowerLatitudeLimit) && (latitude <= UpperLatitudeLimit);
		}

		/// <summary>
		/// Checks a geographical longitude.
		/// </summary>
		/// <param name="longitude">The geographical longitude.</param>
		/// <exception cref="ArgumentException">If the longitude exceeds its allowed interval.</exception>
		public static bool CheckLongitude(double longitude)
		{
            return (longitude >= LowerLongitudeLimit) && (longitude <= UpperLongitudeLimit);
		}

		#region GetDistance
		/// <summary>
		/// Calculates the distance to a 'Maidenhead Locator'.
		/// </summary>
		/// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
		/// <param name="positionInRectangle">>The position of the geographical coordinates in the locator.</param>
		/// <returns>The distance in km.</returns>
		/// <exception cref="ArgumentException">
		/// If the length of the locator text is null or not an even number.
		/// If the locator text contains invalid characters.
		/// </exception>
		public double GetDistance(string maidenheadLocator, PositionInRectangle positionInRectangle)
		{
			GeographicalPoint geographicalPoint = new GeographicalPoint(maidenheadLocator, positionInRectangle);
			double result = GetDistance(geographicalPoint);
			return result;
		}

		/// <summary>
		/// Calculates the distance to another <see cref="GeographicalPoint" /> instance.
		/// </summary>
		/// <param name="geographicalPoint">The <see cref="GeographicalPoint" /> instance.</param>
		/// <returns>The distance in km.</returns>
		/// <exception cref="ArgumentNullException">If the another <see cref="GeographicalPoint" /> instance is null.</exception>
		public double GetDistance(GeographicalPoint geographicalPoint)
		{
			if (geographicalPoint == null)
			{
				throw new ArgumentNullException("geographicalPoint");
			}
			else
			{
				double result = GetDistance(geographicalPoint.Latitude, geographicalPoint.Longitude);
				return result;
			}
		}

		/// <summary>
		/// Calculates the distance to another geographical coordinate.
		/// </summary>
		/// <param name="latitude">The latitude of the another geographical coordinate.</param>
		/// <param name="longitude">The longitude of the another geographical coordinate.</param>
		/// <returns>The distance in km.</returns>
		public double GetDistance(double latitude, double longitude)
		{
			double result = GetDistance(
				Latitude, Longitude,
				latitude, longitude
			);
			return result;
		}

		/// <summary>
		/// Calculates the distance from a 'Maidenhead Locator' 1 to a 'Maidenhead Locator' 2.
		/// </summary>
		/// <param name="maidenheadLocator1">The 'Maidenhead Locator' 1.</param>
		/// <param name="positionInRectangle1">The position of the geographical coordinates in the locator 1.</param>
		/// <param name="maidenheadLocator2">The 'Maidenhead Locator' 2.</param>
		/// <param name="positionInRectangle2">>The position of the geographical coordinates in the locator 2.</param>
		/// <returns>The distance in km.</returns>
		/// <exception cref="ArgumentException">
		/// If the length of any locator text is null or not an even number.
		/// If any locator text contains invalid characters.
		/// </exception>
		public static double GetDistance(
			string maidenheadLocator1, PositionInRectangle positionInRectangle1,
			string maidenheadLocator2, PositionInRectangle positionInRectangle2
		)
		{
			var geographicalPoint1 = new GeographicalPoint(maidenheadLocator1, positionInRectangle1);
			var geographicalPoint2 = new GeographicalPoint(maidenheadLocator2, positionInRectangle2);

			double result = GetDistance(geographicalPoint1, geographicalPoint2);
			return result;
		}

		/// <summary>
		/// Calculates the distance between two <see cref="GeographicalPoint" /> instances.
		/// </summary>
		/// <param name="geographicalPoint1">The <see cref="GeographicalPoint" /> 1.</param>
		/// <param name="geographicalPoint2">The <see cref="GeographicalPoint" /> 2.</param>
		/// <returns>The distance in km.</returns>
		/// <exception cref="ArgumentNullException">If any of the instances is null.</exception>
		public static double GetDistance(
			GeographicalPoint geographicalPoint1,
			GeographicalPoint geographicalPoint2
		)
		{
			if (geographicalPoint1 == null)
			{
				throw new ArgumentNullException("geographicalPoint1");
			}
			else if (geographicalPoint2 == null)
			{
				throw new ArgumentNullException("geographicalPoint2");
			}
			else
			{
				double result = GetDistance(
					geographicalPoint1.Latitude, geographicalPoint1.Longitude,
					geographicalPoint2.Latitude, geographicalPoint2.Longitude
				);
				return result;
			}
		}

		/// <summary>
		/// Calculates the distance between two geographical coordinates.
		/// </summary>
		/// <param name="latitude1">The latitude of coordinate 1.</param>
		/// <param name="longitude1">The longitude of coordinate 1.</param>
		/// <param name="latitude2">The latitude of coordinate 2.</param>
		/// <param name="longitude2">The longitude of coordinate 2.</param>
		/// <returns>The distance in km.</returns>
		public static double GetDistance(
			double latitude1, double longitude1,
			double latitude2, double longitude2
		)
		{
			const double degreeToRadians = Math.PI / 180;

			double latitude1Radians = latitude1 * degreeToRadians;
			double longitude1Radians = longitude1 * degreeToRadians;
			double latitude2Radians = latitude2 * degreeToRadians;
			double longitude2Radians = longitude2 * degreeToRadians;

			double cosD =
				Math.Sin(latitude1Radians) * Math.Sin(latitude2Radians) +
				Math.Cos(longitude1Radians - longitude2Radians) * Math.Cos(latitude1Radians) * Math.Cos(latitude2Radians)
			;

			double dUnitCircle;
			{
				//Limit for use of more precisely formula
				const double limitArcMinutes = 10.0;

				if (
					Math.Abs(cosD) <
					Math.Cos(limitArcMinutes / 60.0 * degreeToRadians)
				)
				{
					//Common formula
					dUnitCircle = Math.Acos(cosD);
				}
				else
				{
					//More precisely formula for angles near 0 or 180 degrees, respectively.
					//Warning: Formula is valid for exactely these cases only!
					dUnitCircle = Math.Sqrt(
						Math.Pow(
							(longitude2Radians - longitude1Radians) * Math.Cos((latitude1Radians + latitude2Radians) / 2),
							2
						) +
						Math.Pow(
							latitude2Radians - latitude1Radians,
							2
						)
					);
				}
			}

			double result = dUnitCircle * MeanEarthRadius_WGS84;
			return result;
		}
		#endregion

		#region GetAzimuth
		/// <summary>
		/// Calculates the azimuth to a 'Maidenhead Locator'.
		/// </summary>
		/// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
		/// <param name="positionInRectangle">>The position of the geographical coordinates in the locator.</param>
		/// <returns>The azimuth in degrees.</returns>
		/// <exception cref="ArgumentException">
		/// If the length of the locator text is null or not an even number.
		/// If the locator text contains invalid characters.
		/// </exception>
		public double GetAzimuth(string maidenheadLocator, PositionInRectangle positionInRectangle)
		{
			var geographicalPoint = new GeographicalPoint(maidenheadLocator, positionInRectangle);
			double result = GetAzimuth(geographicalPoint);
			return result;
		}

		/// <summary>
		/// Calculates the azimuth to another <see cref="GeographicalPoint" /> instance.
		/// </summary>
		/// <param name="geographicalPoint">The <see cref="GeographicalPoint" /> instance.</param>
		/// <returns>The azimuth in degrees.</returns>
		/// <exception cref="ArgumentNullException">If the another <see cref="GeographicalPoint" /> instance is null.</exception>
		public double GetAzimuth(GeographicalPoint geographicalPoint)
		{
			if (geographicalPoint == null)
			{
				throw new ArgumentNullException("geographicalPoint");
			}
			else
			{
				double result = GetAzimuth(geographicalPoint.Latitude, geographicalPoint.Longitude);
				return result;
			}
		}

		/// <summary>
		/// Calculates the azimuth to another geographical coordinate.
		/// </summary>
		/// <param name="latitude">The latitude of the another geographical coordinate.</param>
		/// <param name="longitude">The longitude of the another geographical coordinate.</param>
		/// <returns>The azimuth in degrees.</returns>
		public double GetAzimuth(double latitude, double longitude)
		{
			double result = GetAzimuth(
				Latitude, Longitude,
				latitude, longitude
			);
			return result;
		}

		/// <summary>
		/// Calculates the azimuth from a 'Maidenhead Locator' 1 to a 'Maidenhead Locator' 2.
		/// </summary>
		/// <param name="maidenheadLocator1">The 'Maidenhead Locator' 1.</param>
		/// <param name="positionInRectangle1">The position of the geographical coordinates in the locator 1.</param>
		/// <param name="maidenheadLocator2">The 'Maidenhead Locator' 2.</param>
		/// <param name="positionInRectangle2">>The position of the geographical coordinates in the locator 2.</param>
		/// <returns>The azimuth in degrees.</returns>
		/// <exception cref="ArgumentException">
		/// If the length of any locator text is null or not an even number.
		/// If any locator text contains invalid characters.
		/// </exception>
		public static double GetAzimuth(
			string maidenheadLocator1, PositionInRectangle positionInRectangle1,
			string maidenheadLocator2, PositionInRectangle positionInRectangle2
		)
		{
			var geographicalPoint1 = new GeographicalPoint(maidenheadLocator1, positionInRectangle1);
			var geographicalPoint2 = new GeographicalPoint(maidenheadLocator2, positionInRectangle2);

			double result = GetAzimuth(geographicalPoint1, geographicalPoint2);
			return result;
		}

		/// <summary>
		/// Calculates the azimuth between two <see cref="GeographicalPoint" /> instances.
		/// </summary>
		/// <param name="geographicalPoint1">The <see cref="GeographicalPoint" /> 1.</param>
		/// <param name="geographicalPoint2">The <see cref="GeographicalPoint" /> 2.</param>
		/// <returns>The azimuth in degrees.</returns>
		/// <exception cref="ArgumentNullException">If any of the instances is null.</exception>
		public static double GetAzimuth(
			GeographicalPoint geographicalPoint1,
			GeographicalPoint geographicalPoint2
		)
		{
			if (geographicalPoint1 == null)
			{
				throw new ArgumentNullException("geographicalPoint1");
			}
			else if (geographicalPoint2 == null)
			{
				throw new ArgumentNullException("geographicalPoint2");
			}
			else
			{
				double result = GetAzimuth(
					geographicalPoint1.Latitude, geographicalPoint1.Longitude,
					geographicalPoint2.Latitude, geographicalPoint2.Longitude
				);
				return result;
			}
		}

		/// <summary>
		/// Calculates the azimuth from a geographical coordinate 1 to a geographical coordinate 2.
		/// </summary>
		/// <param name="latitude1">The latitude of coordinate 1.</param>
		/// <param name="longitude1">The longitude of coordinate 1.</param>
		/// <param name="latitude2">The latitude of coordinate 2.</param>
		/// <param name="longitude2">The longitude of coordinate 2.</param>
		/// <returns>The azimuth in degrees.</returns>
		public static double GetAzimuth(
			double latitude1, double longitude1,
			double latitude2, double longitude2
		)
		{
			double result;
			{
				if (
					(latitude1 == latitude2 && longitude1 == longitude2) ||
					(latitude1 == UpperLatitudeLimit && latitude2 == UpperLatitudeLimit) ||
					(latitude1 == LowerLatitudeLimit && latitude2 == LowerLatitudeLimit)
				)
				{
					result = double.NaN;
				}
				else
				{
					const double degreeToRadians = Math.PI / 180;

					double latitude1Radians = latitude1 * degreeToRadians;
					double longitude1Radians = longitude1 * degreeToRadians;
					double latitude2Radians = latitude2 * degreeToRadians;
					double longitude2Radians = longitude2 * degreeToRadians;

					double x =
						Math.Cos(latitude1Radians) * Math.Sin(latitude2Radians) -
						Math.Sin(latitude1Radians) * Math.Cos(latitude2Radians) * Math.Cos(longitude2Radians - longitude1Radians);
					double y = Math.Cos(latitude2Radians) * Math.Sin(longitude2Radians - longitude1Radians);
					double z = Math.Atan2(y, x);

					if (z < 0)
					{
						z += 2 * Math.PI;
					}

					result = z / degreeToRadians;
				}
			}

			return result;
		}
		#endregion

		#region GetWaypointProjection
		/// <summary>
		/// Gets a waypoint projection, based on a azimuth and a distance, respectively.
		/// </summary>
		/// <param name="azimuth">The azimuth in degrees [0...360).</param>
		/// <param name="distance">The distance in km.</param>
		/// <returns>The waypoint projection as a <see cref="GeographicalPoint" /> instance.</returns>
		/// <exception cref="ArgumentException">If the azimuth exceeds its allowed interval.</exception>
		public GeographicalPoint GetWaypointProjection(
			double azimuth, double distance
		)
		{
			var geographicalPoint = new GeographicalPoint(Latitude, Longitude);
			var result = GetWaypointProjection(
				geographicalPoint,
				azimuth, distance
			);
			return result;
		}

		/// <summary>
		/// Gets a waypoint projection, based on a 'Maidenhead Locator',
		/// a azimuth and a distance, respectively.
		/// </summary>
		/// <param name="maidenheadLocator">The 'Maidenhead Locator'.</param>
		/// <param name="positionInRectangle">The position of the geographical coordinates in the locator.</param>
		/// <param name="azimuth">The azimuth in degrees [0...360).</param>
		/// <param name="distance">The distance in km.</param>
		/// <returns>The waypoint projection as a <see cref="GeographicalPoint" /> instance.</returns>
		/// <exception cref="ArgumentException">
		/// If the length of the locator text is null or not an even number.
		/// If the locator text contains invalid characters.
		/// </exception>
		/// <exception cref="ArgumentException">If the azimuth exceeds its allowed interval.</exception>
		public static GeographicalPoint GetWaypointProjection(
			string maidenheadLocator, PositionInRectangle positionInRectangle,
			double azimuth, double distance
		)
		{
			var geographicalPoint = new GeographicalPoint(maidenheadLocator, positionInRectangle);
			var result = GetWaypointProjection(
				geographicalPoint,
				azimuth, distance
			);
			return result;
		}

		/// <summary>
		/// Gets a waypoint projection, based on a geographical coordinate,
		/// a azimuth and a distance, respectively.
		/// </summary>
		/// <param name="latitude">The latitude of the geographical coordinate.</param>
		/// <param name="longitude">The longitude of the geographical coordinate.</param>
		/// <param name="azimuth">The azimuth in degrees [0...360).</param>
		/// <param name="distance">The distance in km.</param>
		/// <returns>The waypoint projection as a <see cref="GeographicalPoint" /> instance.</returns>
		/// <exception cref="ArgumentException">If the azimuth exceeds its allowed interval.</exception>
		public static GeographicalPoint GetWaypointProjection(
			double latitude, double longitude,
			double azimuth, double distance
		)
		{
			var geographicalPoint = new GeographicalPoint(latitude, longitude);
			var result = GetWaypointProjection(
				geographicalPoint,
				azimuth, distance
			);
			return result;
		}

		/// <summary>
		/// Gets a waypoint projection, based on a <see cref="GeographicalPoint" /> instance,
		/// a azimuth and a distance, respectively.
		/// </summary>
		/// <param name="geographicalPoint">The <see cref="GeographicalPoint" /> instance.</param>
		/// <param name="azimuth">The azimuth in degrees [0...360).</param>
		/// <param name="distance">The distance in km.</param>
		/// <returns>The waypoint projection as a <see cref="GeographicalPoint" /> instance.</returns>
		/// <exception cref="ArgumentNullException">If the <see cref="GeographicalPoint" /> instance is null.</exception>
		/// <exception cref="ArgumentException">If the azimuth exceeds its allowed interval.</exception>
		public static GeographicalPoint GetWaypointProjection(
			GeographicalPoint geographicalPoint,
			double azimuth, double distance
		)
		{
			if (geographicalPoint == null)
			{
				throw new ArgumentNullException("geographicalPoint");
			}
			else if (azimuth < 0 || azimuth >= 360)
			{
				throw new ArgumentException("Allowed interval: [0...360)", "azimuth");
			}
			else
			{
				const double degreeToRadians = Math.PI / 180;

				double latitude1 = geographicalPoint.Latitude;
				double longitude1 = geographicalPoint.Longitude;

				double c = distance / MeanEarthRadius_WGS84;
				double a = Math.Acos(
					Math.Sin(latitude1 * degreeToRadians) * Math.Cos(c) +
					Math.Cos(latitude1 * degreeToRadians) * Math.Sin(c) * Math.Cos(azimuth * degreeToRadians)
				);
				double gamma = Math.Acos(
					(Math.Cos(c) - Math.Cos(a) * Math.Sin(latitude1 * degreeToRadians)) /
					(Math.Sin(a) * Math.Cos(latitude1 * degreeToRadians))
				);

				double latitude2 = Math.PI / 2 - a;
				double longitude2 = longitude1 * degreeToRadians;

				if (!double.IsNaN(gamma))
				{
					if (azimuth * degreeToRadians < Math.PI)
					{
						longitude2 += gamma;
					}
					else if (azimuth * degreeToRadians > Math.PI)
					{
						longitude2 -= gamma;
					}
				}

				var result = new GeographicalPoint(
					latitude2 / degreeToRadians,
					longitude2 / degreeToRadians);
				return result;
			}
		}
		#endregion

		/// <summary>
		/// Retrieves the symbol for a mathematical sign.
		/// </summary>
		/// <param name="isNegative">True if the sign is negative. False if positive.</param>
		/// <returns>The symbol.</returns>
		public static string GetSignSymbol(bool isNegative)
		{
			string result = isNegative ? "-" : "+";
			return result;
		}

		/// <summary>
		/// Retrieves a formatted text for a degree value
		/// according to a given angle format.
		/// </summary>
		/// <param name="decDegrees">The degree value (with decimal places).</param>
		/// <param name="angleFormat">A <see cref="AngleFormat" /> member.</param>
		/// <param name="decimalPlaces">The number of decimal places (after decimal point) for the last unit.</param>
		/// <param name="forceAsciiCharacters">If true: Use 7 Bit ASCII characters only instead of special characters.</param>
		/// <returns>The formatted text.</returns>
		public static string GetFormattedText(double decDegrees, AngleFormat angleFormat, int decimalPlaces, bool forceAsciiCharacters)
		{
			string degreeSymbol, minuteSymbol, secondsSymbol;

			if (forceAsciiCharacters)
			{
				degreeSymbol = "d";
				minuteSymbol = "'";
				secondsSymbol = "\"";
			}
			else
			{
				degreeSymbol = "\x00B0".ToString(); //DEGREE SIGN
				minuteSymbol = "\x2032".ToString(); //PRIME
				secondsSymbol = "\x2033".ToString(); //DOUBLE PRIME
			}

			string separator = " ";

			string result;

			switch (angleFormat)
			{
				case AngleFormat.sD:
					{
						bool isNegative;
						double degrees;
						Utilities.DegreesToD(decDegrees, out isNegative, out degrees);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(1, decimalPlaces), degrees) + degreeSymbol
						;
					}
					break;
				case AngleFormat.sDD:
					{
						bool isNegative;
						double degrees;
						Utilities.DegreesToD(decDegrees, out isNegative, out degrees);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(2, decimalPlaces), degrees) + degreeSymbol
						;
					}
					break;
				case AngleFormat.sDM:
					{
						bool isNegative;
						int degrees;
						double minutes;
						Utilities.DegreesToDM(decDegrees, out isNegative, out degrees, out minutes);

						Utilities.Round(decimalPlaces, ref degrees, ref minutes);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(1, 0), degrees) + degreeSymbol + separator +
							string.Format(Utilities.GetFormatString(1, decimalPlaces), minutes) + minuteSymbol
						;
					}
					break;
				case AngleFormat.sDDMM:
					{
						bool isNegative;
						int degrees;
						double minutes;
						Utilities.DegreesToDM(decDegrees, out isNegative, out degrees, out minutes);

						Utilities.Round(decimalPlaces, ref degrees, ref minutes);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(2, 0), degrees) + degreeSymbol + separator +
							string.Format(Utilities.GetFormatString(2, decimalPlaces), minutes) + minuteSymbol
						;
					}
					break;
				case AngleFormat.sDMS:
					{
						bool isNegative;
						int degrees;
						int minutes;
						double seconds;
						Utilities.DegreesToDMS(decDegrees, out isNegative, out degrees, out minutes, out seconds);

						Utilities.Round(decimalPlaces, ref degrees, ref minutes, ref seconds);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(1, 0), degrees) + degreeSymbol + separator +
							string.Format(Utilities.GetFormatString(1, 0), minutes) + minuteSymbol + separator +
							string.Format(Utilities.GetFormatString(1, decimalPlaces), seconds) + secondsSymbol
						;
					}
					break;
				case AngleFormat.sDDMMSS:
					{
						bool isNegative;
						int degrees;
						int minutes;
						double seconds;
						Utilities.DegreesToDMS(decDegrees, out isNegative, out degrees, out minutes, out seconds);

						Utilities.Round(decimalPlaces, ref degrees, ref minutes, ref seconds);

						result =
							GetSignSymbol(isNegative) +
							string.Format(Utilities.GetFormatString(2, 0), degrees) + degreeSymbol + separator +
							string.Format(Utilities.GetFormatString(2, 0), minutes) + minuteSymbol + separator +
							string.Format(Utilities.GetFormatString(2, decimalPlaces), seconds) + secondsSymbol
						;
					}
					break;
				default:
					throw new NotSupportedException(angleFormat.ToString());
			}

			return result;
		}

        /// <summary>
        /// Checks if two given locations (by GeographicalPoint) are equal
        /// </summary>
        /// <param name="point1">The 1st location.</param>
        /// <param name="point2">The 2nd location.</param>
        /// <returns>True if the given locations are equal .</returns>
        public static bool IsEqual(GeographicalPoint point1, GeographicalPoint point2)
        {
            return IsEqual(point1.Latitude, point1.Longitude, point2.Latitude, point2.Longitude);
        }

        /// <summary>
        /// Checks if two given locations (by lat/lon) are equal
        /// </summary>
        /// <param name="latitude1">The geographic latitude of 1st location.</param>
        /// <param name="longitude1">The geographic longitude of 1st location.</param>
        /// <param name="latitude2">The geographic latitude of 2nd location.</param>
        /// <param name="longitude2">The geographic longitude of 2nd location.</param>
        /// <returns>True if the given locations are equal .</returns>
        public static bool IsEqual(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            if ((latitude1 == double.NaN) || (longitude1 == double.NaN) || (latitude2 == double.NaN) || (longitude2 == double.NaN))
                return false;
            return (Math.Abs(latitude1 - latitude2) < 0.00001) && (Math.Abs(longitude1 - longitude2) < 0.00001);
        }


        /// <summary>
        /// Gives a new List instance for a single <see cref="GeographicalPoint" /> instance.
        /// </summary>
        /// <param name="geographicalPoint">The <see cref="GeographicalPoint" /> instance.</param>
        /// <returns>The new List instance.</returns>
        public static IEnumerable<GeographicalPoint> GetNewList(GeographicalPoint geographicalPoint)
		{
			var result = new List<GeographicalPoint>();

			if (geographicalPoint != null)
			{
				result.Add(geographicalPoint);
			}

			return result;
		}
	}
}