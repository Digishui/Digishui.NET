using System;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.DateTime Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the supplied DateTime value changed to include the supplied year.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Year"></param>
    /// <returns></returns>
    public static DateTime ChangeYear(this DateTime Value, int Year)
    {
      return new DateTime(Year, Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the supplied DateTime value changed to include the supplied month.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Year"></param>
    /// <returns></returns>
    public static DateTime ChangeMonth(this DateTime Value, int Month)
    {
      return new DateTime(Value.Year, Month, Value.Day, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the supplied DateTime value changed to include the supplied day.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Day"></param>
    /// <returns></returns>
    public static DateTime ChangeDay(this DateTime Value, int Day)
    {
      return new DateTime(Value.Year, Value.Month, Day, Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Kind);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied UTC DateTime value to a Eastern Standard DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime ToEasternStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied Eastern Standard DateTime value to a UTC DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime FromEasternStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeToUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied UTC DateTime value to a Central Standard DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime ToCentralStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied Central Standard DateTime value to a UTC DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime FromCentralStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeToUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied UTC DateTime value to a Mountain Standard DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime ToMountainStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied Mountain Standard DateTime value to a UTC DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime FromMountainStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeToUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied UTC DateTime value to a Pacific Standard DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime ToPacificStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Converts the supplied Pacific Standard DateTime value to a UTC DateTime.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static DateTime FromPacificStandard(this DateTime Value)
    {
      return TimeZoneInfo.ConvertTimeToUtc(Value, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static bool IsHoliday(this DateTime dt)
    {
      if (dt.DayOfYear == 1) return true; //New Year's Day
      else if (dt.IsEasterSunday() == true) return true; //Easter Sunday
      else if (dt.IsMemorialDay() == true) return true; //Memorial Day
      else if ((dt.Month == 7) && (dt.Day == 4)) return true; //Independence Day
      else if (dt.IsLaborDay() == true) return true; //Labor Day
      else if (dt.IsThanksgiving() == true) return true; //Thanksgiving
      else if ((dt.Month == 12) && (dt.Day == 25)) return true; //Christmas
      else return false;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <remarks>
    ///   https://social.msdn.microsoft.com/Forums/vstudio/en-US/36d8ad39-647f-40b0-b745-36d3b639a576/c-monthcalendar-and-easter?forum=csharpgeneral
    /// </remarks>
    private static bool IsEasterSunday(this DateTime dt)
    {
      int day = 0;
      int month = 0;
      int year = dt.Year;

      int g = year % 19;
      int c = year / 100;
      int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
      int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

      day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
      month = 3;

      if (day > 31)
      {
        month++;
        day -= 31;
      }

      return (dt.Date == (new DateTime(year, month, day)));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static bool IsMemorialDay(this DateTime dt)
    {
      DateTime MemorialDay = new DateTime(dt.Year, 5, 31);

      while (MemorialDay.DayOfWeek != DayOfWeek.Monday)
      {
        MemorialDay = MemorialDay.AddDays(-1);
      }

      return (dt.Date == MemorialDay);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static bool IsLaborDay(this DateTime dt)
    {
      DateTime LaborDay = new DateTime(dt.Year, 9, 1);

      while (LaborDay.DayOfWeek != DayOfWeek.Monday)
      {
        LaborDay = LaborDay.AddDays(1);
      }

      return (dt.Date == LaborDay);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static bool IsThanksgiving(this DateTime dt)
    {
      DateTime Thanksgiving = new DateTime(dt.Year, 11, 1);

      while (Thanksgiving.DayOfWeek != DayOfWeek.Thursday)
      {
        Thanksgiving = Thanksgiving.AddDays(1);
      }

      Thanksgiving = Thanksgiving.AddDays(21);

      return (dt.Date == Thanksgiving);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static DateTime Round(this DateTime dt, TimeSpan ts, bool up)
    {
      var remainder = dt.Ticks % ts.Ticks;
      if (remainder == 0)
      {
        return dt;
      }

      long delta;
      if (up)
      {
        delta = ts.Ticks - remainder;
      }
      else
      {
        delta = -remainder;
      }

      return dt.AddTicks(delta);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static DateTime RoundDown(this DateTime dt, TimeSpan ts)
    {
      return dt.Round(ts, false);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static DateTime RoundUp(this DateTime dt, TimeSpan ts)
    {
      return dt.Round(ts, true);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the start of the week for the supplied datetime.
    /// </summary>
    /// <remarks>
    ///   https://stackoverflow.com/questions/38039/how-can-i-get-the-datetime-for-the-start-of-the-week
    /// </remarks>
    /// <param name="dt">Datetime for which the start of the week is desired.</param>
    /// <param name="startOfWeek">Starting day of the week. Default = Sunday.</param>
    /// <returns>Start of the week for the supplied datetime.</returns>
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
    {
      int diff = dt.DayOfWeek - startOfWeek;

      if (diff < 0) { diff += 7; }

      return dt.AddDays(-1 * diff).Date;
    }
  }
}