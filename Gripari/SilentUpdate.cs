using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClickOnceUpdate.Updater
{
    public class SilentUpdater : INotifyPropertyChanged
    {
        private Mutex instanceMutex;
        private readonly ApplicationDeployment applicationDeployment;
        private readonly System.Timers.Timer timer = new System.Timers.Timer(60000);
        private bool processing;

        //public event EventHandler<UpdateProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<EventArgs> Completed;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool updateAvailable;
        public bool UpdateAvailable
        {
            get { return updateAvailable; }
            private set { updateAvailable = value; OnPropertyChanged("UpdateAvailable"); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCompleted()
        {
            var handler = Completed;
            if (handler != null) handler(this, null);
        }

        /*private void OnProgressChanged(UpdateProgressChangedEventArgs e)
        {
            var handler = ProgressChanged;
            if (handler != null) handler(this, e);
        }*/

        public SilentUpdater()
        {

            if (!ApplicationDeployment.IsNetworkDeployed)
                return;
            applicationDeployment = ApplicationDeployment.CurrentDeployment;
            applicationDeployment.UpdateCompleted += UpdateCompleted;
            //applicationDeployment.UpdateProgressChanged += UpdateProgressChanged;
            timer.Elapsed += (sender, args) =>
            {
                if (processing)
                    return;
                processing = true;
                try
                {
                    if (applicationDeployment.CheckForUpdate(false))
                        applicationDeployment.UpdateAsync();
                    else
                        processing = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Check for update failed. " + ex.Message);
                    processing = false;
                }
            };
            timer.Start();
        }

        /*void UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            OnProgressChanged(new UpdateProgressChangedEventArgs(e));
        }*/

        void UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            processing = false;
            if (e.Cancelled || e.Error != null)
            {
                Debug.WriteLine("Could not install the latest version of the application.");
                return;
            }
            UpdateAvailable = true;
            OnCompleted();
        }

        public void Restart()
        {
            //var shortcutFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".appref-ms");
            //CreateClickOnceShortcut(tmpFile);

            var shortcutFile = GetShortcutPath();
            var proc = new Process { StartInfo = { FileName = shortcutFile, UseShellExecute = true } };

            ReleaseMutex();
            proc.Start();
            
            //app.Shutdown();
        }

        public static string GetShortcutPath()
        {
            return String.Format(@"{0}\{1}\{2}.appref-ms", Environment.GetFolderPath(Environment.SpecialFolder.Programs), GetPublisher(), GetDeploymentInfo().Name.Replace(".application", ""));
        }

        private void ReleaseMutex()
        {
            if (instanceMutex == null)
                return;
            instanceMutex.ReleaseMutex();
            instanceMutex.Close();
            instanceMutex = null;
        }
        public static string GetPublisher()
        {
            XDocument xDocument;
            using (var memoryStream = new MemoryStream(AppDomain.CurrentDomain.ActivationContext.DeploymentManifestBytes))
            using (var xmlTextReader = new XmlTextReader(memoryStream))
                xDocument = XDocument.Load(xmlTextReader);

            if (xDocument.Root == null)
                return null;

            var description = xDocument.Root.Elements().First(e => e.Name.LocalName == "description");
            var publisher = description.Attributes().First(a => a.Name.LocalName == "publisher");
            return publisher.Value;
        }

        private static ApplicationId GetDeploymentInfo()
        {
            var appSecurityInfo = new System.Security.Policy.ApplicationSecurityInfo(AppDomain.CurrentDomain.ActivationContext);
            return appSecurityInfo.DeploymentId;
        }
    }
}