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
  }
}