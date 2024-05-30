using SFTPProyect;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FTPProyect
{
    public class FTP : IxFTP
    {
        public string ServerFTP { get; set; }
        public string UserFTP { get; set; }
        public string PassFTP { get; set; }

        public FTP(string server, string user, string pass, int port)
        {
            this.ServerFTP = server;
            this.UserFTP = user;
            this.PassFTP = pass;
        }

        private FtpWebRequest CreateRequest(string uri, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Credentials = new NetworkCredential(UserFTP, PassFTP);
            return request;
        }

        public void download(string remoteDir, string localDir, bool remoteDelete, string[] ext = null)
        {
            try
            {
                string uri = $"ftp://{ServerFTP}/{remoteDir}";
                FtpWebRequest listRequest = CreateRequest(uri, WebRequestMethods.Ftp.ListDirectory);

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (StreamReader reader = new StreamReader(listResponse.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        string remoteFileName = reader.ReadLine();
                        string remoteFileUri = $"{uri}/{remoteFileName}";
                        string localFilePath = Path.Combine(localDir, remoteFileName);
                        string extension = Path.GetExtension(remoteFileName)?.TrimStart('.').ToLower();

                        if (ext == null || ext.Contains(extension))
                        {
                            FtpWebRequest downloadRequest = CreateRequest(remoteFileUri, WebRequestMethods.Ftp.DownloadFile);

                            using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
                            using (Stream responseStream = downloadResponse.GetResponseStream())
                            using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
                            {
                                responseStream.CopyTo(fileStream);
                                Tools.printlog("Archivo descargado: " + remoteFileName);
                            }

                            if (remoteDelete)
                            {
                                FtpWebRequest deleteRequest = CreateRequest(remoteFileUri, WebRequestMethods.Ftp.DeleteFile);
                                deleteRequest.GetResponse().Close();
                                Tools.printlog("Archivo eliminado: " + remoteFileName);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Tools.printlog("Error en download de FTP: " + e.Message);
            }
        }

        public void upload(string remoteDir, string localDir, string localDirBackUp = "", string[] ext = null)
        {
            try
            {
                var files = Directory.GetFiles(localDir);
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file)?.TrimStart('.').ToLower();
                    if (ext == null || ext.Contains(extension))
                    {
                        try
                        {
                            string remoteFileUri = $"ftp://{ServerFTP}/{remoteDir}/{Path.GetFileName(file)}";
                            FtpWebRequest uploadRequest = CreateRequest(remoteFileUri, WebRequestMethods.Ftp.UploadFile);

                            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            using (Stream requestStream = uploadRequest.GetRequestStream())
                            {
                                fileStream.CopyTo(requestStream);
                                Tools.printlog("Archivo subido: " + Path.GetFileName(file));
                            }

                            if (!string.IsNullOrEmpty(localDirBackUp))
                            {
                                string backupFilePath = Path.Combine(localDirBackUp, DateTime.Now.ToString("ddMMyyhhmmss") + "_" + Path.GetFileName(file));
                                File.Move(file, backupFilePath);
                                Tools.printlog("Archivo movido a backup: " + Path.GetFileName(file));
                            }
                        }
                        catch (Exception ex)
                        {
                            Tools.printlog("El archivo " + file + " no será enviado: " + ex.Message);

                            if (!string.IsNullOrEmpty(localDirBackUp))
                            {
                                string backupFilePath = Path.Combine(localDirBackUp, Path.GetFileName(file));
                                File.Move(file, backupFilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Tools.printlog("Error en upload de FTP: " + e.Message);
            }
        }
    }
}
