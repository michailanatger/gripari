﻿using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Reflection;

namespace Hucksters.Gripari
{
    class AutoUpgrade
    {
        double CurrentVersion;
        public bool needUpdate;

        public AutoUpgrade()
        {
            string path = @"C:\Users\micha\Documents\Visual Studio 2015\Projects\gripari\Version.xml";
            CurrentVersion = retVer(path);
        } 

      

        public void checkVersionUpdate()
        {
            Thread checker = new Thread(new ThreadStart(delegate
             {
                 try
                 {
                     // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(;
                     // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                     XmlDocument _xmlDoc = new XmlDocument();
                     _xmlDoc.LoadXml("ne znayu hosta\\version.xml");
                     //тут надо оптимизировать с функцией
                     XmlElement _version = (XmlElement)_xmlDoc.GetElementsByTagName("version")[0];double ServerVersion = XmlConvert.ToDouble(_version.InnerText);
                     if (ServerVersion > CurrentVersion)
                             needUpdate = true;
                         else
                             needUpdate = false;
                 }
                 catch
                 {
                     needUpdate = false;
                 }
             }));
            checker.Start();
        }

        //надо по
        public void DownloadProgram()
        {
           /* WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            client.DownloadFileAsync(new Uri(@"что то там"), "launcher.update");*/
        }

        double retVer(string path)
        {
            double _retVer = 0.0;
            XmlDocument _verXml = new XmlDocument();
            FileStream _fs = new FileStream(path, FileMode.Open);
            _verXml.Load(_fs);
            XmlElement _version = (XmlElement)_verXml.GetElementsByTagName("version")[0];
            _retVer = XmlConvert.ToDouble(_version.InnerText);
            return _retVer;
        }

    }
}
