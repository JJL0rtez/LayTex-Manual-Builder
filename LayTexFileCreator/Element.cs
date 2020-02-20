using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    public class Element
    {
        public string title;// { get; set; }
        public string Body { get; set; }
        public string ImageLoc { get; set; }
        public string ElementType { get; set; }
        public int ImageSize = 1;
        public List<string> listItems;
        public List<List<string>> TableContents { get; set; }


        public Element()
        {
            title = "";
            listItems = new List<string>();
        }

        public Element(string c)
        {
            listItems = new List<string>();
            this.title = c;
        }
        public Element(string currentTitle, List<string> data)
        {
            this.title = currentTitle;
            this.listItems = data;
        }

        public string GetElementType()
        {
            return this.ElementType;
        }
        public void SetElementType(string str)
        {
            this.ElementType = str;
        }
        public string GetTitle()
        {
            Console.WriteLine(this.title);
            return this.title;
        }
        public void SetTitle(string str)
        {
            this.title = str;
        }
        public void SetImageSize(int v)
        {
            this.ImageSize = v;
        }
        public int GetImageSize()
        {
            return this.ImageSize;
        }
        public void SetImagLoc(string filePath)
        {
            this.ImageLoc = filePath;
        }
        internal void SetBody(string text)
        {
            this.Body = text;
        }
        internal void SetListItems(List<string> listItems)
        {
            this.listItems = listItems;
        }
        internal List<string> GetListItems()
        {
            return this.listItems;
        }
        internal string GetBody()
        {
            return this.Body;
        }
        internal string GetImageLocation()
        {
            return this.ImageLoc;
        }
        internal void SetImageLocation(string loc)
        {
            this.ImageLoc = loc;
        }
    }
}
