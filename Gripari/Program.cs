using System;
using System.ServiceProcess;
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

        #region Nested classes to support running as service
        public const string ServiceName = "Gripari";

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }
        #endregion
        public static ClickOnceUpdate.Updater.SilentUpdater SilentUpdater { get; set; }
        static void Main(string[] args)
        {
            /***if (!Environment.UserInteractive)
                // running as service
                using (var service = new Service())
                    ServiceBase.Run(service);
            else
            {
                // running as console app
                Start(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                Stop();
            }*/
            Service SerciceToRun = new Service();
            ServiceBase.Run(SerciceToRun);

        }
        private static void Start(string[] args)
        {
            // onstart code here
            LanguageSwitch.questionLanguage();
            int number = 1;
            Console.WriteLine(number);
            AutoUpgrade upgr = new AutoUpgrade();
            upgr.checkVersionUpdate();

            if (upgr.needUpdate == true)
            {
                SilentUpdater = new ClickOnceUpdate.Updater.SilentUpdater();
                SilentUpdater.Restart(AppDomain.CurrentDomain);
            }


            var input1c = new OneC();
            var outputWeb = new WebOut();

            input1c.GotEventLog += outputWeb.OnEventLog;

            input1c.run();
        }

        private static void Stop()
        {
            // onstop code here
            /*_timer.Stop();
            _timer.Elapsed -= TimerOnElapsed;*/
        }
    }
    
}
