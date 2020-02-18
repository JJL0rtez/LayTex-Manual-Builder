using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    public class Element
    {
        string title;// { get; set; }
        string Body { get; set; }
        string ImageLoc { get; set; }
        string ElementType { get; set; }
        int ImageSize = 1;


        List<string> listItems;
        List<List<string>> TableContents { get; set; }


        public Element()
        {
            title = "";
            listItems = new List<string>();
        }

        public Element(string c, string d)
        {
            listItems = new List<string>();
            this.title = c;
        }

        public Element(string currentTitle, List<string> data)
        {
            this.title = currentTitle;
            this.listItems = data;
        }

        public string getElementType()
        {
            return this.ElementType;
        }
        public void setElementType(string str)
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

        public void setImagLoc(string filePath)
        {
            this.ImageLoc = filePath;
        }

        internal void setBody(string text)
        {
            this.Body = text;
        }

        internal void setListItems(List<string> listItems)
        {
            this.listItems = listItems;
        }
        internal List<string> getListItems()
        {
            return this.listItems;
        }

        internal string GetBody()
        {
            return this.Body;
        }
    }
}
