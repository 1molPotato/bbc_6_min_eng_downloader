using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Shell;

namespace Donwload6MinEnglish {
    class Program {
        static void Main(string[] args) {
            DownloadAll();
            Console.ReadKey(true);
        }
        
        #region ParseAndDownload
        static private void DownloadAll() {
            List<string> urlList = GetAllEpisodes();
            foreach (string url in urlList) {
                string download = GetDownloadURL(url);
                if (!download.StartsWith("http")) Console.WriteLine(download);
            }
            Console.WriteLine("Downloading completed!");
        }
        /// <summary>
        /// get the webpage's html doc through its URL
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        static private string GetHtmlByURL(string URL) {
            WebRequest request = WebRequest.Create(URL);
            WebResponse response = (WebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.Default);
            return reader.ReadToEnd();
        }
        /// <summary>
        /// get all 6-min-english episodes' url and return to a list
        /// </summary>
        /// <returns></returns>
        static private List<string> GetAllEpisodes() {
            const string preURL = "http://www.bbc.co.uk";
            const string targetUAL = "http://www.bbc.co.uk/learningenglish/english/features/6-minute-english";
            string strMainPage = GetHtmlByURL(targetUAL);
            const string searchTarget = "/learningenglish/english/features/6-minute-english/ep-";
            int cur = strMainPage.IndexOf(searchTarget, 0);
            List<string> urlList = new List<string>();          
            while (-1 != cur) {
                int end = strMainPage.IndexOf("\"", cur);
                string url = preURL + strMainPage.Substring(cur, end - cur);
                urlList.Add(url);
                cur = strMainPage.IndexOf(searchTarget, end);
                end = strMainPage.IndexOf("\"", cur);
                cur = strMainPage.IndexOf(searchTarget, end);
            }
            return urlList;
        }
        static private string GetDownloadURL(string url) {
            string childPage = GetHtmlByURL(url);
            int cur = childPage.IndexOf(".mp3");
            while (childPage[cur] != '\"') cur--;
            int end = childPage.IndexOf("\"", ++cur);
            string downloadURL = childPage.Substring(cur, end - cur);
            return downloadURL;
        }
        #endregion

    }
    
    //modify .mp3 files' title
    class MediaTags {
        public string Title { get; set; }
        private void Init(string path) {
            var obj = ShellObject.FromParsingName(path);
        }
    }

}

