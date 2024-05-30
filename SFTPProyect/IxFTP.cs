using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPProyect
{
    internal interface IxFTP
    {
        public void download(string RemoteDirResultados, string LocalDirResultados, bool RemoteDelete, string[] ext = null);
        public void upload(string RemoteDirPeticiones, string LocalDirPeticiones, string localDirBackUp = "", string[] ext = null);
    }
}
