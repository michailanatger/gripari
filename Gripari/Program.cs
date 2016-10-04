using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using Hucksters.Gripari.Input;
using Hucksters.Gripari.Output;

namespace Hucksters.Gripari
{
    class Program
    {
        public static ClickOnceUpdate.Updater.SilentUpdater SilentUpdater { get; set; }
        static void Main(string[] args)
        {
            LanguageSwitch.questionLanguage();
            int number = 1;
            Console.WriteLine(number);
            AutoUpgrade upgr = new AutoUpgrade();
            upgr.checkVersionUpdate();

            if(upgr.needUpdate == true)
            {
                SilentUpdater = new ClickOnceUpdate.Updater.SilentUpdater();
                SilentUpdater.Restart();

            }
            var input1c = new OneC();
            var outputWeb = new WebOut();

            input1c.GotEventLog += outputWeb.OnEventLog;
            
            input1c.run();
        }
    }
    
}
