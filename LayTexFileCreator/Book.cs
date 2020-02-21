using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    [DataContract]
    public class Book
    {
        private List<Chapter> Chapters;

        public Book() { }

        public Book(List<Chapter> chapters)
        {
            Chapters = chapters;
        }

        public List<Chapter> GetChapters()
        {
            return this.Chapters;
        }
        public void SetChapters(List<Chapter> chapters)
        {
            this.Chapters = chapters;
        }
    }
}
