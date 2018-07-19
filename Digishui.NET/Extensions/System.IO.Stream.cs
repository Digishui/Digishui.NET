using System.IO;
using System.Text;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.IO.Stream Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Generates an MD5 hash of the supplied input.
    /// </summary>
    /// <param name="Value">Value to hash</param>
    /// <returns>MD5 hash of the supplied input</returns>
    public static string GetMD5Hash(this Stream Value)
    {
      //Start from the beginning of the stream.
      Value.Position = 0;

      // Use input string to calculate MD5 hash
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      byte[] hashBytes = md5.ComputeHash(Value);

      // Convert the byte array to lowercase hexadecimal string
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < hashBytes.Length; i++)
      {
        sb.Append(hashBytes[i].ToString("x2"));
      }

      //Return the MDS hash.
      return sb.ToString();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that rewinds the supplied InputStream and copies it to the supplied OutputStream.
    /// </summary>
    /// <param name="InputStream">Stream to rewind and copy to the OutputStream.</param>
    /// <param name="OutputStream">Stream to which the rewinded InputStream should be copied.</param>
    public static void CopyTo(this Stream InputStream, Stream OutputStream, bool RewindFirst = false)
    {
      if (RewindFirst == true) InputStream.Position = 0;
      InputStream.CopyTo(OutputStream);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that rewinds the supplied InputStream and copies it to the supplied OutputStream.
    /// </summary>
    /// <param name="InputStream">Stream to rewind and copy to the OutputStream.</param>
    /// <param name="OutputStream">Stream to which the rewinded InputStream should be copied.</param>
    public static async Task CopyToAsync(this Stream InputStream, Stream OutputStream, bool RewindFirst = false)
    {
      if (RewindFirst == true) InputStream.Position = 0;
      await InputStream.CopyToAsync(OutputStream);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returns a string representation of the supplied stream using the supplied or system default 
    ///   text encoding.
    /// </summary>
    /// <param name="value">Stream for which a string representation will be returned.</param>
    /// <param name="encoding">Text encoding for supplied stream.</param>
    /// <returns>String representation of the supplied stream using the supplied or system default text encoding.</returns>
    public static string GetString(this Stream value, Encoding encoding = null)
    {
      if (encoding == null) { encoding = Encoding.Default; }

      using (MemoryStream MyMemoryStream = new MemoryStream())
      {
        value.CopyTo(MyMemoryStream, true);
        return encoding.GetString(MyMemoryStream.ToArray());
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that returns a string representation of the supplied stream using the supplied or system default 
    ///   text encoding.
    /// </summary>
    /// <param name="value">Stream for which a string representation will be returned.</param>
    /// <param name="encoding">Text encoding for supplied stream.</param>
    /// <returns>String representation of the supplied stream using the supplied or system default text encoding.</returns>
    public static async Task<string> GetStringAsync(this Stream value, Encoding encoding = null)
    {
      if (encoding == null) { encoding = Encoding.Default; }

      using (MemoryStream MyMemoryStream = new MemoryStream())
      {
        await value.CopyToAsync(MyMemoryStream, true);
        return encoding.GetString(MyMemoryStream.ToArray());
      }
    }
  }
}
