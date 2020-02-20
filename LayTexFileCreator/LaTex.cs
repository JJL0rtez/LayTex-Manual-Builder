using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    class LaTex
    {
        public string CompilePage(Page page)
        {
            return page.ToString();
        }

        public void CompileBook(List<string> pages)
        {
            pages.Add("");
        }


    }
}
