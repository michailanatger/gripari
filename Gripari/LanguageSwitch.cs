using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hucksters.Gripari
{
    class LanguageSwitch
    {
        public static string lang;
        public static void questionLanguage()
        {
            
            string language="";
            string path = @"C:\Users\micha\Documents\Visual Studio 2015\Projects\gripari\LanguageConfiguration.ini";// изменить после того как будет написан инсталятор
            if (File.Exists(path))
            {

                byte[] readBuffer = System.IO.File.ReadAllBytes(path);//.ToString().Split('=');
                string[] langFromFile= System.Text.Encoding.Default.GetString(readBuffer).Split('=');
                if (langFromFile[1]=="English")
                 {
                     LanguageSwitch.lang = "English";
                 }else
                 if (langFromFile[1] == "Russian")
                 {
                     LanguageSwitch.lang = "Russian";
                 }


                return;
            }
            else
            Console.WriteLine("Choose your language, write English or Russian,E or R");
            language = Console.ReadLine().ToString();
            if (language == "English" || language == "En" || language == "en" || language == "E")
            {
                FileStream fs = File.Create(path);
                Byte[] info = new UTF8Encoding(true).GetBytes("Language=English");
                // Add some information to the file.
                Console.ReadLine();
                fs.Write(info, 0, info.Length);
            }

            if (language == "Russian" || language == "Ru" || language == "ru" || language == "R")
            {
                FileStream fs = File.Create(path);
                Byte[] info = new UTF8Encoding(true).GetBytes("Language=Russian");
                // Add some information to the file.
                Console.ReadLine();
                fs.Write(info, 0, info.Length);
            }
        }

    }
}
