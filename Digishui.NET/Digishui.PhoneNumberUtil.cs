using PhoneNumbers;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public class PhoneNumberUtil
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the supplied phone number formatted for display purposes.  Phone numbers in the +1 country code are returned
    ///   in AAA-PPP-NNNN format.  International phone numbers are returned in an industry-standard format including the
    ///   country code.
    /// </summary>
    /// <param name="PhoneNumber">Phone number to format for display purposes.</param>
    /// <returns>Phone number formatted for display purposes, or null if the phone number is invalid.</returns>
    public static string DisplayFormat(string PhoneNumber)
    {
      if (PhoneNumber == null) return null;

      PhoneNumbers.PhoneNumberUtil MyPhoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

      PhoneNumber = PhoneNumber.Trim();

      if (MyPhoneNumberUtil.IsPossibleNumber(PhoneNumber, RegionCode.US) == false) return null;

      PhoneNumbers.PhoneNumber MyPhoneNumber = MyPhoneNumberUtil.Parse(PhoneNumber, RegionCode.US);

      if (MyPhoneNumber.CountryCode == 1)
      {
        return MyPhoneNumberUtil.Format(MyPhoneNumberUtil.Parse(PhoneNumber, RegionCode.US), PhoneNumberFormat.NATIONAL).Replace("(", "").Replace(") ", "-");
      }
      else
      {
        return MyPhoneNumberUtil.Format(MyPhoneNumberUtil.Parse(PhoneNumber, RegionCode.US), PhoneNumberFormat.INTERNATIONAL);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns the supplied phone number in E164 format for storage purposes.
    /// </summary>
    /// <param name="PhoneNumber">Phone number to format for storage purposes.</param>
    /// <returns>Phone number formatted for storage purposes, or null if the phone number is invalid.</returns>
    public static string StorageFormat(string PhoneNumber)
    {
      if (PhoneNumber == null) return null;

      PhoneNumbers.PhoneNumberUtil MyPhoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

      PhoneNumber = PhoneNumber.Trim();

      if (MyPhoneNumberUtil.IsPossibleNumber(PhoneNumber, RegionCode.US) == false) return null;

      return MyPhoneNumberUtil.Format(MyPhoneNumberUtil.Parse(PhoneNumber, RegionCode.US), PhoneNumberFormat.E164);
    }
  }
}
