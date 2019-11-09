using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class PaypalLogger
    {
        //public static string LogDirectoryPath = Environment.CurrentDirectory;
        public static string LogDirectoryPath = "E:\\";
        public static void Log(String messages)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(LogDirectoryPath + "\\PaypalError.log", true);
                streamWriter.WriteLine("{0}--->{1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), messages);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}