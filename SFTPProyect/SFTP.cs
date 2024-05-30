using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using System.IO;
using System.Linq;

namespace SFTPProyect
{
    public class SFTP
    {
        public string ServerSFTP { get; set; }
        public string userSFTP { get; set; }
        public string passSFTP { get; set; }
        public int PortSFTP { get; set; }

        public SFTP(string server, string user, string pass, int port)
        {
            this.ServerSFTP = server;
            this.userSFTP = user;
            this.passSFTP = pass;
            this.PortSFTP = port;
        }

        public void download(string RemoteDirResultados, string LocalDirResultados, bool RemoteDelete, string[] ext = null)
        {
            try
            {
                using (var sftp = new SftpClient(ServerSFTP, PortSFTP, userSFTP, passSFTP))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(RemoteDirResultados);
                    foreach (var file in files)
                    {
                        if (!file.Name.StartsWith("."))
                        {
                            string remoteFileName = file.Name;
                            if (remoteFileName.Contains("."))
                            { // para descartar carpetas
                                string e = remoteFileName.Substring(remoteFileName.LastIndexOf(".") + 1);
                                if (ext.Contains(e)) { 
                                    using (Stream file1 = File.OpenWrite(LocalDirResultados + "\\" + remoteFileName))
                                {
                                    sftp.DownloadFile(RemoteDirResultados + remoteFileName, file1);
                                    Tools.printlog("Archivo descargado: " + remoteFileName);
                                    if (RemoteDelete)
                                    {
                                        sftp.DeleteFile(RemoteDirResultados + remoteFileName);
                                    }
                                }
                            }
                            }
                        }
                    }
                }
            }
            catch(Exception e   )
            {
                Tools.printlog("Error en upload de SFTP: " + e.Message);
            }
        }

        public void upload(string RemoteDirPeticiones, string LocalDirPeticiones,  string localDirBackUp="", string[] ext = null)
        {
            try
            {
                DirectoryInfo dir;
                using (var sftp = new SftpClient(ServerSFTP, PortSFTP, userSFTP, passSFTP))
                {
                    sftp.Connect();
                    dir = new DirectoryInfo(LocalDirPeticiones);
                    var files = dir.GetFiles();
                    foreach (string file in Directory.GetFiles(LocalDirPeticiones))
                    {
                        using (StreamReader a = new StreamReader(file))
                        {
                            if (ext.Length==0)
                            {
                                sftp.UploadFile(a.BaseStream, RemoteDirPeticiones + file.Substring(file.LastIndexOf("\\") + 1));
                                //Console.WriteLine ("Archivivo subido: " + file.Substring(file.LastIndexOf("\\") + 1));
                                Tools.printlog("Archivivo subido: " + file.Substring(file.LastIndexOf("\\") + 1));
                            }
                            else
                            {
                                string e = file.Substring(file.LastIndexOf(".") + 1);
                                if (ext.Contains(e))
                                {
                                    try
                                    {
                                        sftp.UploadFile(a.BaseStream, RemoteDirPeticiones + file.Substring(file.LastIndexOf("\\") + 1));
                                        //Console.WriteLine("Archivivo subido: " + file.Substring(file.LastIndexOf("\\") + 1));
                                        Tools.printlog("Archivivo subido: " + file.Substring(file.LastIndexOf("\\") + 1));
                                    }
                                    catch (Exception ex)
                                    {
                                        Tools.printlog("El archivo " + file+"no sera enviado: " + ex.Message);
                                        if (localDirBackUp != "")
                                        {
                                            File.Move(file, localDirBackUp + "\\" + file.Substring(file.LastIndexOf("\\") + 1));
                                        }
                                    }
                                    
                                }

                            }
                        }
                        if (localDirBackUp != "")
                        {
                            File.Move(file, localDirBackUp + "\\" + DateTime.Now.ToString("ddMMyyhhmmss") +"_"+ file.Substring(file.LastIndexOf("\\") + 1));
                            //Console.WriteLine("Archivo movido");
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Tools.printlog("Error en upload de SFTP: "+e.Message);
            }
        }
    }
}
