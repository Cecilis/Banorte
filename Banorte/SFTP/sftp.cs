using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
namespace Banorte.FTP
{
   
     public class sftp
     {
        //public static void UploadSFTPFile(string host, string username, 
        //string password, string sourcefile, string remoteFile,string destinationpath, int port)
        //{
        //    using (SftpClient client = new SftpClient(host, port, username, password))
        //    {
        //        client.Connect();
        //        client.ChangeDirectory(destinationpath);
        //        using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
        //        {
        //            client.BufferSize = 4 * 1024;
        //            client.UploadFile(fs, Path.GetFileName(remoteFile));
     
        //        }
        //    }
        //}


         public static void UploadSFTPFile(string host, string username, string password, Stream sourcefile, string remoteFile, string destinationpath, int port)
        {
            try
            { 
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    try
                    {
                        client.Connect();
                        client.ChangeDirectory(destinationpath);
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(sourcefile, Path.GetFileName(remoteFile));
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
         public static void DeleteSFTPFile(string host, string username,
        string password, string remoteFile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory(destinationpath);
                client.Delete(remoteFile);
            }
        }

     }
}