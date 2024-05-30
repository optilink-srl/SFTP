using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SFTPProyect
{
    public class Tools
    {
        public static void printlog(string a, string ruta = "")
        {
            if (ruta == "")
            {
                ruta = string.Format(@"{0}\Log\{1}\{2}", Directory.GetCurrentDirectory(), DateTime.Today.Year, DateTime.Today.Month);
            }
            if (!Directory.Exists(ruta))
            {
                //Console.WriteLine("No existe");
                try
                {
                    Directory.CreateDirectory(ruta);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear el directorio {0}: {1}", ruta, ex.Message);
                    return;
                }
            }
            //Creamos el archivo temporal y 
            //añadimos el texto pasado como parámetro 
            string nombreArchivo = string.Format(@"{0}\\{1}.txt", ruta, DateTime.Today.ToString("yyyy-MM-dd"));
            //Console.WriteLine(nombreArchivo);
            try
            {
                StreamWriter archivo = File.AppendText(nombreArchivo);  //System.IO.StreamWriter(nombreArchivo);
                archivo.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), a));
                archivo.Close();
                Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), a));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

            }

        }
    }
}
