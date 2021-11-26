using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace SFTPProyect
{
    class Program
    {
        private static Config config;
        static void Main(string[] args)
        {
            config = ini();
            //
            //eventoTimer(0);
            Timer t = new Timer(eventoTimer,0 , 0, config.T*1000);
            while(true)
            {
                string a = Console.ReadLine();
                if (a == "exit")
                {
                    return;
                }
                
            }
        }
        private static void eventoTimer(object State)
        {
            SFTP sftp = new SFTP(config.ServerSFTP, config.UserSFTP, config.PassSFTP, config.PortSFTP);
            sftp.upload(config.RemoteDirPeticiones, config.LocalDirPeticiones, config.LocalDirBackUp);
            sftp.download(config.RemoteDirResultados, config.LocalDirResultados, config.RemoteDelete);
        }

        public static Config ini()
        {
            string basePath = AppContext.BaseDirectory;
            string stringJson = "";
            using (StreamReader r = new StreamReader(basePath + "\\config.json"))
            {
                stringJson = r.ReadToEnd();

            }
            return JsonConvert.DeserializeObject<Config>(stringJson);
        }

        /*static void ini()
        {
            ServerSFTP = @"181.30.184.162";
            userSFTP = "argus";
            passSFTP= "ArgUs$08";
            PortSFTP=2022;
            RemoteDirPeticiones = "/peticiones/";
            RemoteDirResultados = "/resultados/";
            LocalDirPeticiones="C:\\SFTP\\Peticiones";
            LocalDirResultados="C:\\SFTP\\";
            LocalDirBackUp=LocalDirResultados+"bkp\\";
            RemoteDelete=true;
            T = 10;//segundos
        }*/
    }
}
