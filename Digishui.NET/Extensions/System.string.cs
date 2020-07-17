using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.string Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that collapses all white space in the string.
    /// </summary>
    /// <param name="OriginalString"></param>
    /// <returns></returns>
    public static string CollapseWhitespace(this string OriginalString)
    {
      //If the string is null or empty, there's no work to do.
      if (string.IsNullOrEmpty(OriginalString)) { return OriginalString; }

      StringBuilder MyStringBuilder = new StringBuilder();

      bool NewWhitespaceRegionDetected = false;
      bool FirstNonWhitespaceRegionDetected = false;

      foreach (char MyChar in OriginalString)
      {
        if (char.IsWhiteSpace(MyChar))
        {
          NewWhitespaceRegionDetected = true;
        }
        else
        {
          //Add space to the output string if we're in a new whitespace region, unless we have not yet detected a
          //non-whitespace region.  This ensures that all whitespace is condensed down to single spaces, and that there is no
          //leading whitespace.
          if (NewWhitespaceRegionDetected && FirstNonWhitespaceRegionDetected) { MyStringBuilder.Append(" "); }

          //Add the current character to the output string.
          MyStringBuilder.Append(MyChar);

          NewWhitespaceRegionDetected = false;
          FirstNonWhitespaceRegionDetected = true;

        }
      }

      return MyStringBuilder.ToString();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Generates an MD5 hash of the supplied string.
    /// </summary>
    /// <param name="Value">Value to hash</param>
    /// <returns>MD5 hash of the supplied input</returns>
    public static string GetMD5Hash(this string Value)
    {
      return Value.ToStream().GetMD5Hash();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Replaces the last instance of the specified string with the 
    /// </summary>
    /// <param name="sourceString"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceLast(this string sourceString, string oldValue, string newValue)
    {
      int oldValueStartIndex = sourceString.LastIndexOf(oldValue);

      if (oldValueStartIndex == -1) return sourceString;

      return sourceString.Remove(oldValueStartIndex, oldValue.Length).Insert(oldValueStartIndex, newValue);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the leftmost specified characters from the supplied string.
    /// </summary>
    /// <param name="Value">String to parse.</param>
    /// <param name="MaxLength">Leftmost number of characters to return.</param>
    /// <returns></returns>
    public static string Left(this string Value, int MaxLength)
    {
      //Check if the value is valid
      if (Value.IsEmpty() == true)
      {
        //Set valid empty string as string could be null
        Value = string.Empty;
      }
      else if (Value.Length > MaxLength)
      {
        //Make the string no longer than the max length
        Value = Value.Substring(0, MaxLength);
      }

      //Return the string
      return Value;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the rightmost specified characters from the supplied string.
    /// </summary>
    /// <param name="Value">String to parse.</param>
    /// <param name="MaxLength">Rightmost number of characters to return.</param>
    /// <returns></returns>
    public static string Right(this string Value, int MaxLength)
    {
      //Check if the value is valid
      if (Value.IsEmpty() == true)
      {
        //Set valid empty string as string could be null
        Value = string.Empty;
      }
      else if (Value.Length > MaxLength)
      {
        //Make the string no longer than the max length
        Value = Value.Substring(Value.Length - MaxLength, MaxLength);
      }

      //Return the string
      return Value;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is a valid representation of a GUID.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is a valid representation of a GUID.</returns>
    public static bool IsGuid(this string Value)
    {
      if (Value.IsEmpty() == true) return false;

      Guid MyGUID;

      return Guid.TryParse(Value, out MyGUID);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is a valid representation of a bool.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is a valid representation of a bool.</returns>
    public static bool IsBool(this string Value)
    {
      if (Value.IsEmpty() == true) return false;

      bool MyBool;

      return bool.TryParse(Value, out MyBool);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is a valid representation of a DateTime.
    /// </summary>
    /// <param name="value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is a valid representation of a DateTime.</returns>
    public static bool IsDateTime(this string value)
    {
      if (value.IsEmpty() == true) { return false; }

      value = value.Trim().ToLower();

      if ((value.IsInt() == true) && (value.ToInt() >= -657435) && (value.ToInt() <= 99999)) { value += ".0"; }

      if ((value.IsNumeric() == true) && (value.IsInt() == false) && (value.ToDecimal() >= -657435.0m) && (value.ToDecimal() <= 2958465.99999999m))
      {
        //OLE Automation Dates.
        return true;
      }
      else if ((value.IsNumeric() == true) && (value.IsInt() == true) && (value.Length == 8))
      {
        //yyyyMMdd.
        return DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out DateTime dateTime);
      }
      else if ((value.Length >= 23) && ((value.EndsWith(" am") == true) || (value.EndsWith(" pm") == true)))
      {
        //Format encountered in real world.
        return DateTime.TryParseExact(value, "dd-MMM-yy h.mm.sss.fff tt", null, DateTimeStyles.None, out DateTime dateTime);
      }
      else
      {
        return DateTime.TryParse(value, out DateTime dateTime);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is numeric.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is numeric.</returns>
    public static bool IsNumeric(this string Value)
    {
      if (Value.IsEmpty() == true) return false;

      Value = Value.Replace(" ", "");
      Value = Value.Replace(",", "");
      Value = Value.Replace("$", "");

      decimal MyDecimal;

      return decimal.TryParse(Value, out MyDecimal);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is an integer.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is an integer.</returns>
    public static bool IsInt(this string Value)
    {
      if (Value == null) return false;

      Value = Value.Replace(" ", "");
      Value = Value.Replace(",", "");
      Value = Value.Replace("$", "");

      int MyInt;

      return int.TryParse(Value, out MyInt);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns boolean value indicating if the supplied value is an email address.
    /// </summary>
    /// <remarks>
    ///   https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    /// </remarks>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static bool IsEmail(this string Value)
    {
      //If the string is null or empty, it is not an email address.
      if (String.IsNullOrEmpty(Value)) { return false; }

      //If the string matches this regex, we consider it to be an email address.
      return Regex.IsMatch(Value,
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that determines if the supplied string value is empty.  If the string is null, has no contents, or
    ///   contains only whitespace, it is considered empty.
    /// </summary>
    /// <param name="Value">String value to evaluate.</param>
    /// <returns>Boolean value indicating whether the supplied string is empty.</returns>
    public static bool IsEmpty(this string Value)
    {
      if (Value != null) Value = Value.Trim();

      return string.IsNullOrEmpty(Value);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns a reversed copy of the supplied string.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static string Reverse(this string s)
    {
      char[] charArray = s.ToCharArray();
      Array.Reverse(charArray);
      return new string(charArray);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returns the supplied string as a stream.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>Stream representation of the string.</returns>
    public static Stream ToStream(this string Value)
    {
      //We intentionally do not create MyMemoryStream with a using block here because MyMemoryStream needs to survive in an 
      //open state outside the scope of this function.  MemoryStream objects automatically dispose when they completely fall 
      //out of scope, so there is no concern of leaks.
      MemoryStream MyMemoryStream = new MemoryStream();

      //Write the supplied string to our stream.
      WriteToStream(Value, MyMemoryStream);

      //Rewind the stream so it can be used without additional required steps.
      MyMemoryStream.Position = 0;

      //Return the stream.
      return MyMemoryStream;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that writes the supplied string the the supplied output stream.
    /// </summary>
    /// <param name="Value">String value to write to the supplied stream.</param>
    /// <param name="OutputStream">Output stream to which the string should be written.</param>
    public static void WriteToStream(this string Value, Stream OutputStream)
    {
      using (StreamWriter MyStreamWriter = new StreamWriter(OutputStream, Encoding.Default, 1024, true))
      {
        MyStreamWriter.Write(Value);
        MyStreamWriter.Flush();
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that writes the supplied string the the supplied output stream.
    /// </summary>
    /// <param name="Value">String value to write to the supplied stream.</param>
    /// <param name="OutputStream">Output stream to which the string should be written.</param>
    public static async Task WriteToStreamAsync(this string Value, Stream OutputStream)
    {
      using (StreamWriter MyStreamWriter = new StreamWriter(OutputStream, Encoding.Default, 1024, true))
      {
        await MyStreamWriter.WriteAsync(Value);
        await MyStreamWriter.FlushAsync();
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied string to a datetime.
    /// </summary>
    /// <param name="value">String value to convert.</param>
    /// <returns>DateTime that the string represents.</returns>
    public static DateTime ToDateTime(this string value)
    {
      if (value.IsDateTime() == false) throw new ArgumentException("The string cannot be converted to a DateTime because it does not represent a DateTime value.");

      value = value.Trim().ToLower();
      int valueLength = value.Length;
      bool valueIsInt = value.IsInt();

      if (((valueIsInt == true) && (value.ToInt() >= -657435) && (value.ToInt() <= 99999)) || ((value.IsNumeric() == true) && (value.ToDecimal() >= -657435.0m) && (value.ToDecimal() <= 2958465.99999999m)))
      {
        //OLE Automation Dates.
        return DateTime.FromOADate(Convert.ToDouble(value));
      }
      else if ((valueIsInt == true) && (valueLength == 8))
      {
        //yyyyMMdd.
        return DateTime.ParseExact(value, "yyyyMMdd", null, DateTimeStyles.None);
      }
      else if ((valueLength >= 23) && ((value.EndsWith(" am") == true) || (value.EndsWith(" pm") == true)))
      {
        //Format encountered in real world.
        return DateTime.ParseExact(value, "dd-MMM-yy h.mm.sss.fff tt", null, DateTimeStyles.None);
      }
      else
      {
        return DateTime.Parse(value);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied string to a datetime or returns null.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>DateTime that the string represents or null.</returns>
    public static DateTime? ToDateTimeOrNull(this string Value)
    {
      if (Value.IsDateTime() == false) { return null; }

      return ToDateTime(Value);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied string to a decimal.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>Decimal that the string represents.</returns>
    public static decimal ToDecimal(this string Value)
    {
      if (Value.IsNumeric() == false) throw new ArgumentException("The string cannot be converted to a decimal because it does not represent a numeric value.");

      Value = Value.Replace(" ", "");
      Value = Value.Replace(",", "");
      Value = Value.Replace("$", "");

      return decimal.Parse(Value);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied string to an integer.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>Integer that the string represents.</returns>
    public static int ToInt(this string Value)
    {
      if (Value.IsInt() == false) throw new ArgumentException("The string cannot be converted to an integer because it does not represent an integer value.");

      Value = Value.Replace(" ", "");
      Value = Value.Replace(",", "");
      Value = Value.Replace("$", "");

      return int.Parse(Value);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that converts the supplied string to a Guid.
    /// </summary>
    /// <param name="Value">String value to convert.</param>
    /// <returns>Guid that the string represents.</returns>
    public static Guid ToGuid(this string Value)
    {
      if (Value.IsGuid() == false) throw new ArgumentException("The string cannot be converted to a Guid because it does not represent an Guid value.");

      return Guid.Parse(Value);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returns the number of times the specified value occurs in the extended string.
    /// </summary>
    /// <param name="ExtendedString">String in which we're counting the occurences of the value.</param>
    /// <param name="Value">String value to count.</param>
    /// <returns>Number of times the value occurs in the string.</returns>
    public static int Count(this string ExtendedString, string Value)
    {
      return ((ExtendedString.Length - ExtendedString.Replace(Value, "").Length) / Value.Length);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returs a masked string, keeping the specified number of characters from the beginning and
    ///   end of the string, if applicable.
    /// </summary>
    /// <param name="Value">String to mask.</param>
    /// <param name="KeepFirst">Number of characters to keep from the beginning of the string.</param>
    /// <param name="KeepLast">Number of characters to keep from the end of the string.</param>
    /// <param name="MaskCharacter">Character to use to mask the string.</param>
    /// <returns></returns>
    public static string Mask(this string Value, int KeepFirst = 0, int KeepLast = 4, char MaskCharacter = 'X')
    {
      if (Value.IsEmpty() == true) return Value;

      if (Value.Trim().Length <= (KeepFirst + KeepLast)) return Value;

      string Mask = new String(MaskCharacter, Value.Trim().Length - KeepFirst - KeepLast);

      return Value.Left(KeepFirst) + Mask + Value.Right(KeepLast);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Determines if the string is a publicly switched telephone number.
    /// </summary>
    /// <param name="value">String to evaluate.</param>
    /// <returns>Boolean indicating if the evaluated string is a publicly switched telephone number.</returns>
    public static bool IsPstn(this string value)
    {
      if (value.IsEmpty() == true) { return false; }

      return (PstnUtil.StorageFormat(value) != null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Determines if the string is a time zone.
    /// </summary>
    /// <param name="value">String to evaluate.</param>
    /// <returns>Boolean indicating if the evaluated string is a time zone.</returns>
    public static bool IsTimeZone(this string value)
    {
      List<string> timeZoneList = TimeZoneInfo.GetSystemTimeZones().Select(s => s.Id).ToList();

      return timeZoneList.Contains(value);
    }
  }
}