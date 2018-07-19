using System;
using System.Globalization;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.object Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied object to a string, or if the object is null either the optional supplied 
    ///   string or a null value.
    /// </summary>
    /// <param name="Value">Object to convert to a string.</param>
    /// <param name="NullString">
    ///   Optional string to return if the object is null.  If this value is not supplied, this method will return a null
    ///   value if the object is null.
    /// </param>
    /// <returns>
    ///   String representation of the object, or if the object is null either the optional supplied string or a null value.
    /// </returns>
    /// <remarks>
    ///   Consider using null-coalescing operator instead.
    /// </remarks>
    public static string ToStringOrNull(this object Value, string NullString = null)
    {
      if (Value == null) return NullString;

      return Value.ToString();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Determines if the supplied object represents a datetime.
    /// </summary>
    /// <param name="Value">The value to inspect.</param>
    /// <returns></returns>
    public static bool IsDateTime(this object Value)
    {
      if (Value != null)
      {
        if (Value is DateTime)
        {
          return true;
        }
        else if (Value is string)
        {
          string stringValue = (string)Value;
          DateTime dateTime;

          if ((stringValue.Trim().Length == 8) && (stringValue.IsNumeric() == true))
          {
            return DateTime.TryParseExact(stringValue, "yyyyMMdd", null, DateTimeStyles.None, out dateTime);
          }
          else
          {
            return DateTime.TryParse(stringValue, out dateTime);
          }
        }
      }

      return false;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied object represents a number.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is numeric.</returns>
    public static bool IsNumeric(this object Value)
    {
      if (Value == null) { return false; }

      if (Value is byte) { return true; }
      if (Value is sbyte) { return true; }
      if (Value is short) { return true; }
      if (Value is ushort) { return true; }
      if (Value is int) { return true; }
      if (Value is uint) { return true; }
      if (Value is long) { return true; }
      if (Value is ulong) { return true; }
      if (Value is float) { return true; }
      if (Value is double) { return true; }
      if (Value is decimal) { return true; }

      if (Value is string)
      {
        string MyString = Value.ToString();

        if (MyString.IsEmpty() == true) return false;

        MyString = MyString.Replace(" ", "");
        MyString = MyString.Replace(",", "");
        MyString = MyString.Replace("$", "");

        decimal MyDecimal;

        return decimal.TryParse(MyString, out MyDecimal);
      }

      return false;
    }
  }
}