using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GF.BrowserGame.Schema.Serializable;
using LibCommonUtil;

namespace OGameOneAdmin.Controls
{
    public partial class NoteSubControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private int _lastSelectedIndex = -1;
        private int _lastTopItemIndex = -1;
        private string _lastSelectedNoteUniqueId;
        private bool _resizeColumns = false;
        private AdminNotesControl _adminNotesControl;


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void ReadNoteEventHandler(string noteUniqueId);
        public delegate void DeleteNoteEventHandler(string noteUniqueId);


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        public event ReadNoteEventHandler ReadNote;
        public event DeleteNoteEventHandler DeleteNote;


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public NoteSubControl(AdminNotesControl adminNotesControl)
        {
            InitializeComponent();
            this.listViewNotes.ListViewColumnSorter.DateTimeColumnIndex = 1;
            this.listViewNotes.ListViewColumnSorter.SortColumn = 1;
            this.listViewNotes.ListViewColumnSorter.Order = SortOrder.Descending;
            _adminNotesControl = adminNotesControl;
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Events ------


        private void NoteSubControl_Resize(object sender, EventArgs e)
        {
            listViewNotes.ResizeColumns();
        }


        private void listViewNotes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            FlickerFreeListView myListView = (FlickerFreeListView)sender;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == myListView.ListViewColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (myListView.ListViewColumnSorter.Order == SortOrder.Ascending)
                {
                    myListView.ListViewColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    myListView.ListViewColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                myListView.ListViewColumnSorter.SortColumn = e.Column;
                myListView.ListViewColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            myListView.ListViewItemSorter = myListView.ListViewColumnSorter;
            myListView.Sort();
        }


        private void listViewNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNotes.SelectedItems.Count == 1)
            {
                _lastSelectedIndex = listViewNotes.SelectedItems[0].Index;
                listViewNotes.SelectedItems[0].Focused = true;
                string noteUniqueId = listViewNotes.SelectedItems[0].Tag.ToString();
                _lastSelectedNoteUniqueId = noteUniqueId;
                OnReadNote(noteUniqueId);
            }
            else if (_lastSelectedIndex != -1)
            {
                try
                {
                    listViewNotes.Items[_lastSelectedIndex].Focused = false;
                }
                catch { }
            }
        }


        private void deleteNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewNotes.SelectedItems.Count == 1)
            {
                _lastTopItemIndex = listViewNotes.TopItem.Index;
                string noteUniqueId = listViewNotes.SelectedItems[0].Tag.ToString();
                OnDeleteNote(noteUniqueId);
            }
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listViewNotes.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
        }


        private void richTextBoxNoteDetails_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                EssentialUtil.OpenDefaultWebBrowser(e.LinkText);
            }
            catch (Exception ex)
            {
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("This link could not be opened in your web browser.", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error)));
            }
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void ApplyFocus()
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                listViewNotes.Focus();
                if (listViewNotes.Items.Count > 0 && listViewNotes.SelectedItems.Count == 0)
                {
                    try
                    {
                        listViewNotes.Items[0].Selected = true;
                    }
                    catch { }
                }
                else if (listViewNotes.SelectedItems.Count == 0)
                    richTextBoxNoteDetails.Text = "";
            }));
        }


        public void SetNoteListView(List<Note> noteList)
        {
            _lastTopItemIndex = 0;
            _lastSelectedIndex = 0;
            _resizeColumns = true;
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                List<ListViewItem> listViewItems = new List<ListViewItem>();
                foreach (Note note in noteList)
                {
                    string itemText = _adminNotesControl.GetUniverseDomain(note.UniverseId);
                    string itemTag = note.UniqueId;
                    string[] subItemsTextArray = new string[3];
                    Color[] subItemsForeColorArray = new Color[3];
                    FontStyle[] subItemsFontStyleArray = new FontStyle[3];

                    subItemsTextArray[0] = note.IsNoteMessageNullOrEmpty ? "" : note.CreationDateTime.ToString();
                    subItemsForeColorArray[0] = Color.Black;
                    subItemsFontStyleArray[0] = FontStyle.Regular;

                    subItemsTextArray[1] = note.Author;
                    subItemsForeColorArray[1] = Color.Black;
                    subItemsFontStyleArray[1] = FontStyle.Regular;

                    subItemsTextArray[2] = note.Preview;
                    subItemsForeColorArray[2] = Color.Black;
                    subItemsFontStyleArray[2] = FontStyle.Regular;

                    listViewItems.Add(listViewNotes.AddListViewItem2(itemText, itemTag, subItemsTextArray, subItemsForeColorArray, subItemsFontStyleArray, false));
                }

                BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        listViewNotes.BeginUpdate();
                        listViewNotes.Items.Clear();
                        richTextBoxNoteDetails.Text = "";
                        listViewNotes.Items.AddRange(listViewItems.ToArray());
                        listViewNotes.ResizeColumns();
                        listViewNotes.Sort();
                        listViewNotes.Focus();

                        if (listViewNotes.Items.Count > 0)
                            listViewNotes.Items[0].Selected = true;
                    }
                    catch { }
                    finally { listViewNotes.EndUpdate(); }
                }));
            };
            worker.RunWorkerAsync();
        }


        public void AddNoteToListView(Note note)
        {
            string itemText = _adminNotesControl.GetUniverseDomain(note.UniverseId);
            string itemTag = note.UniqueId;
            string[] subItemsTextArray = new string[3];
            Color[] subItemsForeColorArray = new Color[3];
            FontStyle[] subItemsFontStyleArray = new FontStyle[3];

            subItemsTextArray[0] = note.IsNoteMessageNullOrEmpty ? "" : note.CreationDateTime.ToString();
            subItemsForeColorArray[0] = Color.Black;
            subItemsFontStyleArray[0] = FontStyle.Regular;

            subItemsTextArray[1] = note.Author;
            subItemsForeColorArray[1] = Color.Black;
            subItemsFontStyleArray[1] = FontStyle.Regular;

            subItemsTextArray[2] = note.Preview;
            subItemsForeColorArray[2] = Color.Black;
            subItemsFontStyleArray[2] = FontStyle.Regular;

            ListViewItem listViewItem = listViewNotes.AddListViewItem2(itemText, itemTag, subItemsTextArray, subItemsForeColorArray, subItemsFontStyleArray, false);

            BeginInvoke(new MethodInvoker(() =>
           {
               try
               {
                   listViewNotes.BeginUpdate();
                   listViewNotes.Items.Add(listViewItem);
                   listViewNotes.ResizeColumns();
                   listViewNotes.Sort();
               }
               catch { }
               finally { listViewNotes.EndUpdate(); }
           }));
        }


        public void AddNotesToListView(List<Note> noteList)
        {
            _resizeColumns = true;
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                List<ListViewItem> newListViewItems = new List<ListViewItem>();
                foreach (Note note in noteList)
                {
                    string itemText = _adminNotesControl.GetUniverseDomain(note.UniverseId);
                    string itemTag = note.UniqueId;
                    string[] subItemsTextArray = new string[3];
                    Color[] subItemsForeColorArray = new Color[3];
                    FontStyle[] subItemsFontStyleArray = new FontStyle[3];

                    subItemsTextArray[0] = note.IsNoteMessageNullOrEmpty ? "" : note.CreationDateTime.ToString();
                    subItemsForeColorArray[0] = Color.Black;
                    subItemsFontStyleArray[0] = FontStyle.Regular;

                    subItemsTextArray[1] = note.Author;
                    subItemsForeColorArray[1] = Color.Black;
                    subItemsFontStyleArray[1] = FontStyle.Regular;

                    subItemsTextArray[2] = note.Preview;
                    subItemsForeColorArray[2] = Color.Black;
                    subItemsFontStyleArray[2] = FontStyle.Regular;

                    newListViewItems.Add(listViewNotes.AddListViewItem2(itemText, itemTag, subItemsTextArray, subItemsForeColorArray, subItemsFontStyleArray, false));
                }

                BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        listViewNotes.BeginUpdate();
                        listViewNotes.Items.AddRange(newListViewItems.ToArray());
                        listViewNotes.ResizeColumns();
                        listViewNotes.Focus();

                        if (listViewNotes.Items.Count > 0 && listViewNotes.SelectedItems.Count == 0)
                            listViewNotes.Items[0].Selected = true;
                    }
                    catch { }
                    finally { listViewNotes.EndUpdate(); }
                }));
            };
            worker.RunWorkerAsync();
        }


        public void DeleteNoteFromListView(Note note)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    int noteIndex = listViewNotes.GetListViewItemIndexByTag(note.UniqueId);
                    if (noteIndex != -1)
                    {
                        listViewNotes.BeginUpdate();
                        listViewNotes.Items[noteIndex].Focused = false;
                        listViewNotes.Items.RemoveAt(noteIndex);
                    }
                }
                catch { }
                finally { listViewNotes.EndUpdate(); }
            }));
        }


        public void DeleteNotesFromListView(List<Note> noteList)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    listViewNotes.BeginUpdate();
                    foreach (Note note in noteList)
                    {
                        int noteIndex = listViewNotes.GetListViewItemIndexByTag(note.UniqueId);
                        if (noteIndex != -1)
                        {
                            listViewNotes.Items[noteIndex].Focused = false;
                            listViewNotes.Items.RemoveAt(noteIndex);
                        }
                    }
                    if (listViewNotes.SelectedItems.Count == 0)
                        richTextBoxNoteDetails.Text = "";
                }
                catch { }
                finally { listViewNotes.EndUpdate(); }
            }));
        }


        public void SetNoteDetails(Note note)
        {
            Invoke(new MethodInvoker(() =>
            {
                try
                {
                    ListViewItem listViewItem = listViewNotes.Items[_lastSelectedIndex];
                    if (listViewItem.Selected && listViewItem.Tag.ToString().Equals(note.UniqueId))
                    {
                        if (note != null)
                            richTextBoxNoteDetails.Text = note.Content;
                        else
                            richTextBoxNoteDetails.Text = "Error occurred...";
                    }
                }
                catch { }
            }));
        }


        public void UpdateListViewNoteDateTime(string noteUniqueId, DateTime noteDateTime)
        {
            Invoke(new MethodInvoker(() =>
            {
                try
                {
                    int itemIndex = listViewNotes.GetListViewItemIndexByTag(noteUniqueId);

                    if (itemIndex == -1) return;

                    listViewNotes.BeginUpdate();
                    listViewNotes.UpdateListViewSubItem(itemIndex, 1, noteDateTime.ToString());
                    if (_resizeColumns)
                    {
                        _resizeColumns = false;
                        listViewNotes.ResizeColumns();
                    }
                    listViewNotes.Sort();
                    listViewNotes.TopItem = listViewNotes.SelectedItems[0];
                }
                catch { }
                finally { listViewNotes.EndUpdate(); }
            }));
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


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
        protected void OnReadNote(string noteUniqueId)
        {
            // Check if there are any Subscribers
            if (ReadNote != null)
            {
                // Call the Event
                ReadNote(noteUniqueId);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        protected void OnDeleteNote(string noteUniqueId)
        {
            // Check if there are any Subscribers
            if (DeleteNote != null)
            {
                // Call the Event
                DeleteNote(noteUniqueId);
            }
        }


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/
    }
}
