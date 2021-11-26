using System;
using System.Collections.Generic;
using System.Text;

namespace SFTPProyect
{
    public class Config
    {
        public string ServerSFTP { get; set; }
        public string UserSFTP { get; set; }
        public string PassSFTP { get; set; }
        public int PortSFTP { get; set; }
        public string RemoteDirPeticiones { get; set; }
        public string RemoteDirResultados { get; set; }
        public string LocalDirPeticiones { get; set; }
        public string LocalDirResultados { get; set; }
        public string LocalDirBackUp { get; set; }
        public bool RemoteDelete { get; set; }
        public int T { get; set; }
    }
}
