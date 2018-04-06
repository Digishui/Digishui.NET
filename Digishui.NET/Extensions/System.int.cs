using System;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.int Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Rounds supplied value up to the nearest multiple of the supplied boundary.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Boundary"></param>
    /// <returns></returns>
    public static int RoundUpToNearest(this int Value, int Boundary)
    {
      return (int)(Math.Ceiling((decimal)Value / (decimal)Boundary) * (decimal)Boundary);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returns a number with a 'th', 'rd' type suffix (1 = 1st, 3 = 3rd, etc).
    /// </summary>
    /// <param name="Value">Value to suffix.</param>
    public static string ToSuffixedString(this int Value)
    {
      string ValueAsString = Value.ToString();

      if (ValueAsString.EndsWith("11")) return ValueAsString + "th";
      if (ValueAsString.EndsWith("12")) return ValueAsString + "th";
      if (ValueAsString.EndsWith("13")) return ValueAsString + "th";
      if (ValueAsString.EndsWith("1")) return ValueAsString + "st";
      if (ValueAsString.EndsWith("2")) return ValueAsString + "nd";
      if (ValueAsString.EndsWith("3")) return ValueAsString + "rd";
      return ValueAsString + "th";
    }
  }
}