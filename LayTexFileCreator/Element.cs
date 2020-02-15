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
        int ImageSize = 1;

        List<string> data;
        List<List<string>> TableContents { get; set; }


        public Element()
        {
            title = "";
            data = new List<string>();
        }

        public Element(string c, string d)
        {
            data = new List<string>();
            this.title = c;
            this.data.Add(d);
        }

        public Element(string currentTitle, List<string> data)
        {
            this.title = currentTitle;
            this.data = data;
        }

        public string GetTitle()
        {
            Console.WriteLine(this.title);
            return this.title;
        }
        public List<string> GetData()
        {
            return this.data;
        }
        public void SetTitle(string str)
        {
            this.title = str;
        }
        public void SetData(int pos, string str)
        {
            if (this.data.Count() > pos)
            {
                data[pos] = str;
            }
        }

        public void SetImageSize(int v)
        {
            this.ImageSize = v;
        }

        public void setImagLoc(string filePath)
        {
            this.ImageLoc = filePath;
        }
    }
}
