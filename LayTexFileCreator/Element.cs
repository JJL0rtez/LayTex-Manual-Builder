using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    public class Element
    {
        string title;
        List<string> data;

        public Element()
        {
            title = "";
            data = new List<string>();
        }

        public string getTitle()
        {
            return this.title;
        }
        public List<string> getData()
        {
            return this.data;
        }
    }
}
