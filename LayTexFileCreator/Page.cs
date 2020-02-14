using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    public class Page
    {
        private string name;

        private string Getname()
        {
            return name;
        }

        private void Setname(string value)
        {
            name = value;
        }

        private string dateEdited;

        private string GetdateEdited()
        {
            return dateEdited;
        }

        private void SetdateEdited(string value)
        {
            dateEdited = value;
        }

        private string dateCreated;

        private string GetDateCreated()
        {
            return dateCreated;
        }

        private void SetDateCreated(string value)
        {
            dateCreated = value;
        }

        private string gitHash;

        private string GetGitHash()
        {
            return gitHash;
        }

        private void SetGitHash(string value)
        {
            gitHash = value;
        }

        private List<Element> elements;

        private List<Element> Getelements()
        {
            return elements;
        }

        private void Setelements(List<Element> value)
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
            this.Setelements(elements);
        }

        public Page(string name, string dateEdited, string dateCreated, string gitHash, List<Element> elements)
        {
            this.Setname(name);
            this.SetdateEdited(dateEdited);
            this.SetDateCreated(dateCreated);
            this.SetGitHash(gitHash);
            this.Setelements(elements);
        }
    }
}
