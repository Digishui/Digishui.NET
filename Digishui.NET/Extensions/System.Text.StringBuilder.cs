using System.IO;
using System.Text;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.Text.StringBuilder Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that writes the supplied StringBuilder content the the supplied output stream.
    /// </summary>
    /// <param name="Value">StringBuilder content to write to the supplied output stream.</param>
    /// <param name="OutputStream">Output stream to which the StringBuilder should be written.</param>
    public static void WriteToStream(this StringBuilder Value, Stream OutputStream)
    {
      Value.ToString().WriteToStream(OutputStream);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Extension method that writes the supplied StringBuilder content the the supplied output stream.
    /// </summary>
    /// <param name="Value">StringBuilder content to write to the supplied output stream.</param>
    /// <param name="OutputStream">Output stream to which the StringBuilder should be written.</param>
    public static async Task WriteToStreamAsync(this StringBuilder Value, Stream OutputStream)
    {
      await Value.ToString().WriteToStreamAsync(OutputStream);
    }
  }
}
