using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;
using System.Threading;

namespace GF.BrowserGame
{
    public class NoteManager
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------

        private GameManager _gameManager;
        private object _locker = new object();


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void NewNoteEventHandler(Enums.NOTE_TYPE noteType, Note note);
        public delegate void NewNotesEventHandler(Enums.NOTE_TYPE noteType, List<Note> notes);
        public delegate void NoteDeletedEventHandler(Enums.NOTE_TYPE noteType, Note note);
        public delegate void NotesDeletedEventHandler(Enums.NOTE_TYPE noteType, List<Note> notes);


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        public event NewNoteEventHandler NewNote;
        public event NewNotesEventHandler NewNotes;
        public event NoteDeletedEventHandler NoteDeleted;
        public event NotesDeletedEventHandler NotesDeleted;


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public NoteManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public List<Note> GetGeneralNotes(List<string> universeIdList)
        {
            List<Note> generalNotes = new List<Note>();

            foreach (string universeId in universeIdList)
            {
                List<Note> universeGeneralNotes = _gameManager.UniverseGeneralNotesList(universeId);
                if (universeGeneralNotes != null)
                {
                    generalNotes.AddRange(universeGeneralNotes);
                }
            }
            return generalNotes;
        }


        public void UpdateGeneralNotes(string universeId)
        {
            UniManager uniManager = _gameManager.GetUniManager(universeId);

            List<Note> newGeneralNotes = _gameManager.GetUniverseGeneralNotes(universeId);

            if (newGeneralNotes == null)
                return;

            FindDeletedNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
            FindNewNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
        }


        public List<Note> GetPersonalNotes(List<string> universeIdList)
        {
            List<Note> personalNotes = new List<Note>();

            foreach (string universeId in universeIdList)
            {
                List<Note> universePersonalNotes = _gameManager.UniversePersonalNotesList(universeId);
                if (universePersonalNotes != null)
                {
                    personalNotes.AddRange(universePersonalNotes);
                }
            }
            return personalNotes;
        }


        public void UpdatePersonalNotes(string universeId)
        {
            UniManager uniManager = _gameManager.GetUniManager(universeId);

            List<Note> newPersonalNotes = _gameManager.GetUniversePersonalNotes(universeId);

            if (newPersonalNotes == null)
                return;

            FindDeletedNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
            FindNewNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
        }


        public void UpdateNotes(string universeId)
        {
            UniManager uniManager = _gameManager.GetUniManager(universeId);

            List<Note> newPersonalNotes = new List<Note>();
            List<Note> newGeneralNotes = new List<Note>();

            _gameManager.GetUniverseGeneralAndPersonalNotes(universeId, out newGeneralNotes, out newPersonalNotes);

            if (newGeneralNotes == null || newPersonalNotes == null)
                return;

            //FindDeletedNotes(Enums.NOTE_TYPE.PERSONAL, uniManager.GetUniverse().PersonalNotesList, newPersonalNotes);
            //FindDeletedNotes(Enums.NOTE_TYPE.GENERAL, uniManager.GetUniverse().GeneralNotesList, newGeneralNotes);
            //FindNewNotes(Enums.NOTE_TYPE.PERSONAL, uniManager.GetUniverse().PersonalNotesList, newPersonalNotes);
            //FindNewNotes(Enums.NOTE_TYPE.GENERAL, uniManager.GetUniverse().GeneralNotesList, newGeneralNotes);

            FindDeletedNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
            FindDeletedNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
            FindNewNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
            FindNewNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
        }


        public Note GetNoteDetails(Enums.NOTE_TYPE noteType, string universeId, int noteId)
        {
            return _gameManager.GetUniverseNoteDetails(noteType, universeId, noteId);
        }


        public void DeleteNote(string universeId, int noteId)
        {
            UniManager uniManager = _gameManager.GetUniManager(universeId);

            List<Note> newPersonalNotes = new List<Note>();
            List<Note> newGeneralNotes = new List<Note>();

            _gameManager.DeleteAdminNote(universeId, noteId, out newGeneralNotes, out newPersonalNotes);

            if (newGeneralNotes == null || newPersonalNotes == null)
                return;

            //FindDeletedNotes(Enums.NOTE_TYPE.PERSONAL, uniManager.GetUniverse().PersonalNotesList, newPersonalNotes);
            //FindDeletedNotes(Enums.NOTE_TYPE.GENERAL, uniManager.GetUniverse().GeneralNotesList, newGeneralNotes);
            //FindNewNotes(Enums.NOTE_TYPE.PERSONAL, uniManager.GetUniverse().PersonalNotesList, newPersonalNotes);
            //FindNewNotes(Enums.NOTE_TYPE.GENERAL, uniManager.GetUniverse().GeneralNotesList, newGeneralNotes);

            FindDeletedNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
            FindDeletedNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
            FindNewNotes(Enums.NOTE_TYPE.PERSONAL, _gameManager.UniversePersonalNotesList(universeId), newPersonalNotes);
            FindNewNotes(Enums.NOTE_TYPE.GENERAL, _gameManager.UniverseGeneralNotesList(universeId), newGeneralNotes);
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void FindNewNote(Enums.NOTE_TYPE noteType, List<Note> existingNoteList, List<Note> newNoteList)
        {
            foreach (Note note in newNoteList)
            {
                if (!existingNoteList.Exists(r => r.Id == note.Id))
                {
                    existingNoteList.Add(note);
                    OnNewNote(noteType, note);
                }
            }
        }


        private void FindDeletedNote(Enums.NOTE_TYPE noteType, List<Note> existingNoteList, List<Note> newNoteList)
        {
            List<Note> deleteNoteList = new List<Note>();
            foreach (Note note in existingNoteList)
            {
                if (!newNoteList.Exists(r => r.Id == note.Id))
                    deleteNoteList.Add(note);
            }

            foreach (Note note in deleteNoteList)
            {
                existingNoteList.Remove(note);
                OnNoteDeleted(noteType, note);
            }
        }


        private void FindNewNotes(Enums.NOTE_TYPE noteType, List<Note> existingNoteList, List<Note> newNoteList)
        {
            lock (_locker)
            {
                List<Note> newNotes = new List<Note>();
                foreach (Note note in newNoteList)
                {
                    if (!existingNoteList.Exists(r => r.Id == note.Id))
                    {
                        existingNoteList.Add(note);
                        newNotes.Add(note);
                    }
                }
                if (newNotes.Count > 0)
                    OnNewNotes(noteType, newNotes);
            }
        }


        private void FindDeletedNotes(Enums.NOTE_TYPE noteType, List<Note> existingNoteList, List<Note> newNoteList)
        {
            lock (_locker)
            {
                List<Note> deletedNoteList = new List<Note>();
                foreach (Note note in existingNoteList)
                {
                    if (!newNoteList.Exists(r => r.Id == note.Id))
                        deletedNoteList.Add(note);
                }

                if (deletedNoteList.Count > 0)
                {
                    foreach (Note note in deletedNoteList)
                    {
                        existingNoteList.Remove(note);
                    }
                    OnNotesDeleted(noteType, deletedNoteList);
                }
            }
        }


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- BackgroundWorker DoWork Methods ------


        //void bgw_DoWorkIsSessionValid(object sender, DoWorkEventArgs e)
        //{
        //    IsSessionValid();
        //}


        #endregion ----- BackgroundWorker DoWork Methods ------


        /***********************************************************************************************************/


        #region ----- Events Callback ------


        //void OnSessionStatusChange(SessionStatusEventArgs onlineStatusEvent)
        //{
        //    _session.Status = onlineStatusEvent.SessionState;
        //    if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.VALID)
        //        OnLoggedIn();
        //    else if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.INVALID)
        //        ClearSession(); // This method will trigger the OnLoggedOut() event
        //    else if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.UNKNOWN)
        //        OnSessionInvalid();
        //}


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNewNote(Enums.NOTE_TYPE noteType, Note note)
        {
            // Check if there are any Subscribers
            if (NewNote != null)
            {
                // Call the Event
                NewNote(noteType, note);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNewNotes(Enums.NOTE_TYPE noteType, List<Note> notes)
        {
            // Check if there are any Subscribers
            if (NewNotes != null)
            {
                // Call the Event
                NewNotes(noteType, notes);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNoteDeleted(Enums.NOTE_TYPE noteType, Note note)
        {
            // Check if there are any Subscribers
            if (NoteDeleted != null)
            {
                // Call the Event
                NoteDeleted(noteType, note);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNotesDeleted(Enums.NOTE_TYPE noteType, List<Note> notes)
        {
            // Check if there are any Subscribers
            if (NotesDeleted != null)
            {
                // Call the Event
                NotesDeleted(noteType, notes);
            }
        }


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/
    }
}
