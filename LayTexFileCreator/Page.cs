using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    [DataContract]
    public class Page
    {
        // [DataMember]
        public string name;
        //[DataMember]
        public string dateCreated;
        //[DataMember]
        public List<Element> elements;
        //[DataMember]
        public string dateEdited;


        public string Getname()
        {
            return name;
        }
        public void Setname(string value)
        {
            name = value;
        }


        public string GetdateEdited()
        {
            return dateEdited;
        }
        public void SetdateEdited(string value)
        {
            dateEdited = value;
        }


        public string GetDateCreated()
        {
            return dateCreated;
        }
        public void SetDateCreated(string value)
        {
            dateCreated = value;
        }

        public List<Element> GetElements()
        {
            return elements;
        }
        public void SetElements(List<Element> value)
        {
            elements = value;
        }


        public Page()
        {
        }
        public Page(string name, string dateCreated, List<Element> elements)
        {
            this.Setname(name);
            this.SetDateCreated(dateCreated);
            this.SetElements(elements);
        }
        public Page(string name, string dateEdited, string dateCreated, List<Element> elements)
        {
            this.Setname(name);
            this.SetdateEdited(dateEdited);
            this.SetDateCreated(dateCreated); 
            this.SetElements(elements);
        }
    }
}
