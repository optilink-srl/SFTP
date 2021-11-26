using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using System.IO;

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

        public void download(string RemoteDirResultados, string LocalDirResultados, bool RemoteDelete)
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
                        using (Stream file1 = File.OpenWrite(LocalDirResultados+"\\" + remoteFileName))
                        {
                            sftp.DownloadFile(RemoteDirResultados + remoteFileName, file1);
                            if (RemoteDelete)
                            {
                                sftp.DeleteFile(RemoteDirResultados + remoteFileName);
                            }
                        }
                    }
                }
            }
        }

        public void upload(string RemoteDirPeticiones, string LocalDirPeticiones,  string localDirBackUp="")
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
                        sftp.UploadFile(a.BaseStream, RemoteDirPeticiones + file.Substring(file.LastIndexOf("\\") +1 ));
                    }
                    if (localDirBackUp != "")
                    {
                        File.Move(file, localDirBackUp +"\\"+ file.Substring(file.LastIndexOf("\\") + 1));
                    }
                }
            }
        }
    }
}
