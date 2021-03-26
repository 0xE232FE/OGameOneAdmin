using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class Note
    {
        private int _id;
        private string _universeId;
        private string _author;
        private DateTime _creationDateTime = DateTime.MaxValue;
        private string _preview;
        private string _content;

        public string UniqueId
        {
            get { return _universeId + "|" + _id; }
        }

        public bool IsNoteMessageNullOrEmpty
        {
            get { return string.IsNullOrEmpty(_content); }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string Preview
        {
            get { return _preview; }
            set { _preview = value; }
        }

        public DateTime CreationDateTime
        {
            get { return _creationDateTime; }
            set { _creationDateTime = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string UniverseId
        {
            get { return _universeId; }
            set { _universeId = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
