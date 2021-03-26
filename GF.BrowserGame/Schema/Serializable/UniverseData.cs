using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class UniverseData
    {
        private string _universeId;
        public string UniverseId
        {
            get { return _universeId; }
            set { _universeId = value; }
        }

        private List<Note> _personalNotesList = new List<Note>();
        public List<Note> PersonalNotesList
        {
            get { return _personalNotesList; }
            set { _personalNotesList = value; }
        }

        private List<Note> _generalNotesList = new List<Note>();
        public List<Note> GeneralNotesList
        {
            get { return _generalNotesList; }
            set { _generalNotesList = value; }
        }
    }
}
