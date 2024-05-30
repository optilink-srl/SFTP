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
            Timer t = new Timer(eventoTimer,0 , 0, config.T*1000);
            while(true)
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
            SFTP sftp = new SFTP(config.ServerSFTP, config.UserSFTP, config.PassSFTP, config.PortSFTP);
            if (config.Upload == 1)
            {
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + ": Verificando derivaciones a enviar");
                sftp.upload(config.RemoteDirPeticiones, config.LocalDirPeticiones, config.LocalDirBackUp, config.Ext);

            }
            if (config.Download == 1)
            {
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + ": Verificando derivaciones a descargar");
                sftp.download(config.RemoteDirResultados, config.LocalDirResultados, config.RemoteDelete,config.Ext);
            }
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
