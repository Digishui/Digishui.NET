using Renci.SshNet;
using System.IO;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public static class SftpUtil
  {
    public static void UploadStream(Stream stream, string path, string host, int port, string username, string password)
    {
      using (SftpClient MySftpClient = new SftpClient(host, port, username, password))
      {
        MySftpClient.Connect();

        if (MySftpClient.IsConnected == true)
        {
          stream.Position = 0;

          MySftpClient.BufferSize = 4 * 1024;
          MySftpClient.UploadFile(stream, path);
          MySftpClient.Disconnect();
        }
      }
    }
  }
}