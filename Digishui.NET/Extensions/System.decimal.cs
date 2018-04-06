using System;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.decimal Extensions
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
    public static decimal RoundUpToNearest(this decimal Value, decimal Boundary)
    {
      return (Math.Ceiling(Value / Boundary) * Boundary);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied decimal to an integer.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>Integer that the string represents.</returns>
    public static int ToInt(this decimal Value)
    {
      return (int)Math.Round(Value);
    }
  }
}