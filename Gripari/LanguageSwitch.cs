using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hucksters.Gripari
{
    class LanguageSwitch
    {
        public static void questionLanguage()
        {
            Console.WriteLine("Choose your language, write English or Russian,E or R");
            string language = Console.ReadLine().ToString();

            if (language == "English" || language == "En" || language == "en" || language == "E")
            {

            }

            if (language == "Russian" || language == "Ru" || language == "ru" || language == "R")
            {

            }
            Console.WriteLine(language);
        }

    }
}
