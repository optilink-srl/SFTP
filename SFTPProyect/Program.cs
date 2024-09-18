using FTPProyect;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
//using System.Timers;

namespace SFTPProyect
{
    class Program
    {
        private static Config config;
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            config = ini();
            Timer t = new Timer(eventoTimer,0 , 0, config.T*1000);
            

            while (true)
            {
                string a = Console.ReadLine();
                if (a == "exit")
                {
                    Tools.printlog("Aplicacion cerrada por el usuario");
                    return;
                }
                if (a == "cls")
                {
                    Console.Clear();
                }
            }
        }
        private static void eventoTimer(object State)
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            IxFTP xftp;
            switch (config.TipoConexion)
            {
                case "SFTP":
                    xftp = new SFTP(config.ServerSFTP, config.UserSFTP, config.PassSFTP, config.PortSFTP);
                    break;
                case "FTP":
                    xftp = new FTP(config.ServerSFTP, config.UserSFTP, config.PassSFTP, config.PortSFTP);
                    break;
                default:
                    xftp = null;
                    Tools.printlog("No se ha definiodo un tipo correcto de conxion. Puede ser FTP ó SFTP");
                    break;
            }
            
            if (config.Upload == 1)
            {
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + ": Verificando archivos a enviar");
                xftp.upload(config.RemoteDirPeticiones, config.LocalDirPeticiones, config.LocalDirBackUp, config.Ext);

            }
            if (config.Download == 1)
            {
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + ": Verificando archivos para descargar");
                xftp.download(config.RemoteDirResultados, config.LocalDirResultados, config.RemoteDelete,config.Ext);
            }
            isRunning = false;
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
    }
}
