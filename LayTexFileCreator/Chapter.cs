using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LayTexFileCreator
{
    [DataContract]
    public class Chapter
    {
        public List<Page> Pages;
        public string ChapterName;

        public Chapter(){ }

        public Chapter(List<Page> pages, string chapterName)
        {
            Pages = pages;
            ChapterName = chapterName;
        }

        public List<Page> GetPages()
        {
            return this.Pages;
        }
        public void SetPages(List<Page> pages)
        {
            this.Pages = pages;
        }

        public string GetChapterName()
        {
            return this.ChapterName;
        }
        public void SetChapterName(string chapterName)
        {
            this.ChapterName = chapterName;
        }

        internal void SetDateEdited(string v)
        {
            
        }

        internal void SetDateCreated(string v)
        {
            
        }

        internal string GetDateCreated()
        {
            return "Fill in latter";
        }
    }
}
