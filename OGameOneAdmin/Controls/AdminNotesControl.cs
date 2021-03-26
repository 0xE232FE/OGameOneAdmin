using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OGameOneAdmin.Object;
using GF.BrowserGame;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;

namespace OGameOneAdmin.Controls
{
    public partial class AdminNotesControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private NoteSubControl _personalNotesControl;
        private NoteSubControl _generalNotesControl;
        private GameManager _gameManager;
        private NoteManager _noteManager;
        private List<string> _universeIdList = new List<string>();
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void UniverseSelectionEventHandler(List<string> universeIdList);


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public AdminNotesControl(GameManager gameManager)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _noteManager = new NoteManager(gameManager);
            _noteManager.NewNote += new NoteManager.NewNoteEventHandler(_noteManager_NewNote);
            _noteManager.NoteDeleted += new NoteManager.NoteDeletedEventHandler(_noteManager_NoteDeleted);
            _noteManager.NewNotes += new NoteManager.NewNotesEventHandler(_noteManager_NewNotes);
            _noteManager.NotesDeleted += new NoteManager.NotesDeletedEventHandler(_noteManager_NotesDeleted);
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Events ------


        private void tabControlAdminNotes_Selected(object sender, TabControlEventArgs e)
        {
            ApplyFocus();
        }


        private void comboBoxCommunityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCommunityList.SelectedIndex == -1)
                {
                    comboBoxCommunityList.SelectedIndex = 0;
                    return;
                }

                if (comboBoxCommunityList.SelectedIndex == 0)
                {
                    comboBoxUniverseList.Items.Clear();

                    comboBoxUniverseList.Items.Add("-- All --");

                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
                            comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
                    }

                    if (comboBoxUniverseList.Items.Count == 2)
                    {
                        comboBoxUniverseList.Enabled = false;
                        comboBoxUniverseList.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBoxUniverseList.Enabled = true;
                        comboBoxUniverseList.SelectedIndex = 0;
                    }
                }
                else
                {
                    ItemObject communityObj = comboBoxCommunityList.SelectedItem as ItemObject;
                    string communityName = communityObj.Key;
                    string communityId = communityObj.ValueOfKey.ToString();

                    comboBoxUniverseList.Items.Clear();
                    comboBoxUniverseList.Items.Add("-- All --");

                    int count = 0;
                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (universe.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
                        {
                            comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
                            count++;
                        }
                    }

                    if (count == 1)
                    {
                        comboBoxUniverseList.Enabled = false;
                        comboBoxUniverseList.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBoxUniverseList.Enabled = true;
                        comboBoxUniverseList.SelectedIndex = 0;
                    }
                }
            }
            catch { }
        }


        private void comboBoxUniverseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxUniverseList.SelectedIndex == -1)
                {
                    comboBoxUniverseList.SelectedIndex = 0;
                    return;
                }

                if (comboBoxUniverseList.SelectedIndex == 0)
                {
                    if (comboBoxCommunityList.SelectedIndex == 0)
                    {
                        _universeIdList = _gameManager.UniverseList.Where(p => _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, p.Id)).Select(r => r.Id).ToList();
                        SetSubControls(_universeIdList);
                    }
                    else
                    {
                        ItemObject communityObj = comboBoxCommunityList.SelectedItem as ItemObject;
                        string communityName = communityObj.Key;
                        string communityId = communityObj.ValueOfKey.ToString();
                        _universeIdList = _gameManager.UniverseList.Where(r => r.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, r.Id)).Select(r => r.Id).ToList();
                        SetSubControls(_universeIdList);
                    }
                }
                else
                {
                    _universeIdList.Clear();
                    ItemObject universeObj = comboBoxUniverseList.SelectedItem as ItemObject;
                    string universeId = universeObj.ValueOfKey.ToString();
                    _universeIdList.Add(universeId);
                    SetSubControls(_universeIdList);
                }
            }
            catch { }
        }


        private void btnMenu_Click(object sender, EventArgs e)
        {
            //contextMenuStrip1.Show(btnMenu, Point.Empty);
            contextMenuStrip1.Show(btnMenu, btnMenu.Bounds.Left - 7, btnMenu.Bounds.Bottom - 8);
        }


        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }


        private void checkForNewNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxUniverseList.SelectedIndex == -1 || comboBoxUniverseList.SelectedIndex == 0)
            {
                foreach (string universeId in _universeIdList)
                {
                    if (!_gameManager.GetUniManager(universeId).IsSessionStatusValid())
                        continue;
                    var worker = new BackgroundWorker();
                    worker.DoWork += (workerSender, workerEvent) =>
                    {
                        OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                        _noteManager.UpdateNotes(workerEvent.Argument.ToString());
                    };
                    worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                    {
                        OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                    };
                    worker.RunWorkerAsync(universeId);
                }
            }
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    _noteManager.UpdateNotes(_universeIdList.FirstOrDefault());
                };
                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                };
                worker.RunWorkerAsync();
            }
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void LoadControl()
        {
            _personalNotesControl = new NoteSubControl(this);
            _personalNotesControl.ReadNote += new NoteSubControl.ReadNoteEventHandler(_personalNotesControl_ReadNote);
            _personalNotesControl.DeleteNote += new NoteSubControl.DeleteNoteEventHandler(_personalNotesControl_DeleteNote);
            _personalNotesControl.Dock = DockStyle.Fill;
            tabPagePersonalNotes.Controls.Add(_personalNotesControl);
            //_personalNotesControl.LoadControl();

            _generalNotesControl = new NoteSubControl(this);
            _generalNotesControl.ReadNote += new NoteSubControl.ReadNoteEventHandler(_generalNotesControl_ReadNote);
            _generalNotesControl.DeleteNote += new NoteSubControl.DeleteNoteEventHandler(_generalNotesControl_DeleteNote);
            _generalNotesControl.Dock = DockStyle.Fill;
            tabPageGeneralNotes.Controls.Add(_generalNotesControl);
            //_generalNotesControl.LoadControl();

            SetupComboBox();
        }


        public void ApplyFocus()
        {
            if (tabControlAdminNotes.SelectedTab == tabPagePersonalNotes)
            {
                _personalNotesControl.ApplyFocus();
            }
            else
            {
                _generalNotesControl.ApplyFocus();
            }
        }


        public string GetUniverseDomain(string uniId)
        {
            return _gameManager.GetUniverseDomain(uniId);
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void SetupComboBox()
        {
            comboBoxCommunityList.Items.Add("-- All --");

            foreach (Community community in _gameManager.GetCommunityList())
            {
                if (_gameManager.HasCommunitySecureObject(Constants.SecureObject.VIEW_TAB_NOTES, community.Id))
                    comboBoxCommunityList.Items.Add(new ItemObject(community.Name, community.Id));
            }

            comboBoxUniverseList.Items.Add("-- All --");

            foreach (Universe universe in _gameManager.UniverseList)
            {
                if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
                    comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
            }

            if (comboBoxUniverseList.Items.Count == 2)
            {
                comboBoxUniverseList.Enabled = false;
            }


            if (comboBoxCommunityList.Items.Count > 2)
            {
                comboBoxCommunityList.Enabled = true;
                comboBoxCommunityList.SelectedIndex = 0;
            }
            else
            {
                comboBoxCommunityList.Enabled = false;
                comboBoxCommunityList.SelectedIndex = 1;
            }
        }


        private void SetSubControls(List<string> universeIdList)
        {
            _personalNotesControl.SetNoteListView(_noteManager.GetPersonalNotes(universeIdList));
            _generalNotesControl.SetNoteListView(_noteManager.GetGeneralNotes(universeIdList));
        }


        private void ShowHideInProgressBar()
        {
            lock (_locker)
            {
                if (_backgroundWorkerRunning == 0 && _isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = false));
                }
                else if (_backgroundWorkerRunning > 0 && !_isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = true));
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


        void OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE bgwState)
        {
            lock (_locker)
            {
                if (bgwState.Equals(Enums.BACKGROUNDWORKER_STATE.START_WORK))
                    _backgroundWorkerRunning++;
                else
                    _backgroundWorkerRunning--;

                ShowHideInProgressBar();
            }
        }


        void _noteManager_NoteDeleted(Enums.NOTE_TYPE noteType, Note note)
        {
            if (noteType.Equals(Enums.NOTE_TYPE.PERSONAL))
            {
                _personalNotesControl.DeleteNoteFromListView(note);
            }
            else
            {
                _generalNotesControl.DeleteNoteFromListView(note);
            }
        }


        void _noteManager_NewNote(Enums.NOTE_TYPE noteType, Note note)
        {
            if (noteType.Equals(Enums.NOTE_TYPE.PERSONAL))
            {
                _personalNotesControl.AddNoteToListView(note);
            }
            else
            {
                _generalNotesControl.AddNoteToListView(note);
            }
        }


        void _noteManager_NotesDeleted(Enums.NOTE_TYPE noteType, List<Note> notes)
        {
            if (noteType.Equals(Enums.NOTE_TYPE.PERSONAL))
            {
                _personalNotesControl.DeleteNotesFromListView(notes);
            }
            else
            {
                _generalNotesControl.DeleteNotesFromListView(notes);
            }
        }


        void _noteManager_NewNotes(Enums.NOTE_TYPE noteType, List<Note> notes)
        {
            if (noteType.Equals(Enums.NOTE_TYPE.PERSONAL))
            {
                _personalNotesControl.AddNotesToListView(notes);
            }
            else
            {
                _generalNotesControl.AddNotesToListView(notes);
            }
        }


        void _generalNotesControl_ReadNote(string noteUniqueId)
        {
            string universeId = noteUniqueId.Split('|')[0];
            int noteId = int.Parse(noteUniqueId.Split('|')[1]);

            Note note = _gameManager.UniverseGeneralNotesList(universeId).SingleOrDefault(r => r.Id == noteId);

            if (note == null || !string.IsNullOrEmpty(note.Content))
                _generalNotesControl.SetNoteDetails(note);
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    Note tempNote = _noteManager.GetNoteDetails(Enums.NOTE_TYPE.GENERAL, universeId, noteId);
                    _generalNotesControl.SetNoteDetails(tempNote == null ? note : tempNote);
                    if (tempNote != null && !tempNote.IsNoteMessageNullOrEmpty)
                        _generalNotesControl.UpdateListViewNoteDateTime(noteUniqueId, note.CreationDateTime);
                    else if (tempNote != null && tempNote.IsNoteMessageNullOrEmpty)
                        _noteManager.UpdateNotes(universeId);
                };
                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                };
                worker.RunWorkerAsync();
            }
        }


        void _personalNotesControl_ReadNote(string noteUniqueId)
        {
            string universeId = noteUniqueId.Split('|')[0];
            int noteId = int.Parse(noteUniqueId.Split('|')[1]);

            Note note = _gameManager.UniversePersonalNotesList(universeId).SingleOrDefault(r => r.Id == noteId);

            if (note == null || !string.IsNullOrEmpty(note.Content))
                _personalNotesControl.SetNoteDetails(note);
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    Note tempNote = _noteManager.GetNoteDetails(Enums.NOTE_TYPE.PERSONAL, universeId, noteId);
                    _personalNotesControl.SetNoteDetails(tempNote == null ? note : tempNote);
                    if (tempNote != null && !tempNote.IsNoteMessageNullOrEmpty)
                        _personalNotesControl.UpdateListViewNoteDateTime(noteUniqueId, note.CreationDateTime);
                    else if (tempNote != null && tempNote.IsNoteMessageNullOrEmpty)
                        _noteManager.UpdateNotes(universeId);

                };
                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                };
                worker.RunWorkerAsync();
            }
        }


        void _generalNotesControl_DeleteNote(string noteUniqueId)
        {
            string universeId = noteUniqueId.Split('|')[0];
            int noteId = int.Parse(noteUniqueId.Split('|')[1]);

            Note note = _gameManager.UniverseGeneralNotesList(universeId).SingleOrDefault(r => r.Id == noteId);

            if (note == null)
                return;
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    _noteManager.DeleteNote(universeId, noteId);
                };
                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                };
                worker.RunWorkerAsync();
            }
        }


        void _personalNotesControl_DeleteNote(string noteUniqueId)
        {
            string universeId = noteUniqueId.Split('|')[0];
            int noteId = int.Parse(noteUniqueId.Split('|')[1]);

            Note note = _gameManager.UniversePersonalNotesList(universeId).SingleOrDefault(r => r.Id == noteId);

            if (note == null)
                return;
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    _noteManager.DeleteNote(universeId, noteId);
                };
                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                };
                worker.RunWorkerAsync();
            }
        }


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/
    }
}
