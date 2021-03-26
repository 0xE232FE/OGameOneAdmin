using System.Windows.Forms;
using OGameOneAdmin.Utilities;
using System.Drawing;

namespace OGameOneAdmin.Controls
{
    public class FlickerFreeListView : ListView
    {
        /***********************************************************************************************************/


        #region ------ Delegates ------


        private delegate void ResizeColumnsDelegate();
        private delegate string GetListViewItemTextDelegate(int itemIndex);
        private delegate int GetListViewItemIndexByItemTextDelegate(string itemText);
        private delegate int GetListViewItemIndexByItemTagDelegate(string itemTag);
        private delegate int GetListViewItemIndexBySubItemTextDelegate(int subItemIndex, string itemText);
        private delegate ListViewItem GetListViewItemDelegate(int subItemIndex, string itemText);


        #endregion ------ Delegates ------


        /***********************************************************************************************************/

        private ListViewColumnSorter _listViewColumnSorter;


        public FlickerFreeListView()
            : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            _listViewColumnSorter = new ListViewColumnSorter();
            _listViewColumnSorter.SortColumn = 0;
            this.ListViewItemSorter = _listViewColumnSorter;
            this.Sorting = SortOrder.Ascending;
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg != 0x14)
        //        base.WndProc(ref m);
        //}


        public ListViewColumnSorter ListViewColumnSorter
        {
            get { return _listViewColumnSorter; }
            set
            {
                _listViewColumnSorter = value;
                this.ListViewItemSorter = _listViewColumnSorter;
            }
        }


        /***********************************************************************************************************/


        #region ------ ListView Access Methods (thread safe) ------


        public void ResizeColumns()
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                this.Invoke(new ResizeColumnsDelegate(ResizeColumns));
            }
            else
            {
                this.BeginUpdate();
                int lastColumnIndex = this.Columns.Count - 1;
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    int columnLength = this.Columns[i].Text.Length;
                    bool resizeColumnContent = false;

                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (i == 0)
                        {
                            if (this.Items[j].Text.Length > columnLength)
                            {
                                resizeColumnContent = true;
                                break;
                            }
                        }
                        else
                            if (this.Items[j].SubItems[i].Text.Length > columnLength)
                            {
                                resizeColumnContent = true;
                                break;
                            }
                    }

                    if (resizeColumnContent && i != lastColumnIndex)
                        this.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    else
                        this.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                this.EndUpdate();
            }
        }


        public string GetListViewItemText(int itemIndex)
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (string)this.Invoke(new GetListViewItemTextDelegate(GetListViewItemText), new object[] { itemIndex });
            }
            else
            {
                return this.Items[itemIndex].Text;
            }
        }


        /// <summary>
        /// Get an itemIndex using an item.Text value.
        /// Returns -1 if the item does not exist
        /// </summary>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public int GetListViewItemIndexByItemText(string itemText)
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (int)this.Invoke(new GetListViewItemIndexByItemTextDelegate(GetListViewItemIndexByItemText), new object[] { itemText });
            }
            else
            {
                foreach (ListViewItem item in this.Items)
                {
                    if (item.Text.ToUpper().Equals(itemText.ToUpper()))
                        return item.Index;
                }
                return -1;
            }
        }


        /// <summary>
        /// Get an itemIndex using an item.Tag value.
        /// Returns -1 if the item does not exist
        /// </summary>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public int GetListViewItemIndexByTag(string itemTag)
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (int)this.Invoke(new GetListViewItemIndexByItemTagDelegate(GetListViewItemIndexByTag), new object[] { itemTag });
            }
            else
            {
                foreach (ListViewItem item in this.Items)
                {
                    if (item.Tag.ToString().ToUpper().Equals(itemTag.ToUpper()))
                        return item.Index;
                }
                return -1;
            }
        }


        /// <summary>
        /// Get an itemIndex using a subItemIndex and the corresponding itemText.
        /// Returns -1 if the item does not exist
        /// </summary>
        /// <param name="subItemIndex"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public int GetListViewItemIndex(int subItemIndex, string itemText)
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (int)this.Invoke(new GetListViewItemIndexBySubItemTextDelegate(GetListViewItemIndex), new object[] { subItemIndex, itemText });
            }
            else
            {
                foreach (ListViewItem item in this.Items)
                {
                    if (item.SubItems[subItemIndex].Text.ToUpper().Equals(itemText.ToUpper()))
                        return item.Index;
                }
                return -1;
            }
        }


        /// <summary>
        /// Get a ListViewItem using a subItemIndex and the corresponding itemText.
        /// Returns null if the item does not exist
        /// </summary>
        /// <param name="subItemIndex"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public ListViewItem GetListViewItem(int subItemIndex, string itemText)
        {
            if (this.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (ListViewItem)this.Invoke(new GetListViewItemDelegate(GetListViewItem), new object[] { subItemIndex, itemText });
            }
            else
            {
                foreach (ListViewItem item in this.Items)
                {
                    if (item.SubItems[subItemIndex].Text.ToUpper().Equals(itemText.ToUpper()))
                        return item;
                }
                return null;
            }
        }


        #endregion ------ ListView Access Methods (thread safe) ------


        /***********************************************************************************************************/


        #region ------ General ListView Methods (not thread safe) ------


        public void AddListViewItem(string text, string tag)
        {
            ListViewItem item = new ListViewItem();
            item.Text = text;
            item.Tag = tag;
            this.Items.Add(item);
        }


        public void AddListViewItem(string text, string tag, string subItemText, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            AddListViewSubItem(item, subItemText);
            this.Items.Add(item);
        }


        public void AddListViewItem(string text, string tag, string subItemText, bool useItemStyleForSubItems, bool isChecked)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            item.Checked = isChecked;
            AddListViewSubItem(item, subItemText);
            this.Items.Add(item);
        }


        public void AddListViewItem(string text, string tag, string[] subItemsTextArray, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            foreach (string subItemText in subItemsTextArray)
            {
                AddListViewSubItem(item, subItemText);
            }
            this.Items.Add(item);
        }


        public void AddListViewItem(string text, string tag, string[] subItemsTextArray, Color[] subItemsForeColorArray, FontStyle[] subItemsFontStyleArray, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            int i = 0;
            foreach (string subItemText in subItemsTextArray)
            {
                AddListViewSubItem(item, subItemText, subItemsForeColorArray[i], subItemsFontStyleArray[i]);
                i++;
            }
            this.Items.Add(item);
        }


        public ListViewItem AddListViewItem2(string text, string tag, string[] subItemsTextArray, Color[] subItemsForeColorArray, FontStyle[] subItemsFontStyleArray, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            int i = 0;
            foreach (string subItemText in subItemsTextArray)
            {
                AddListViewSubItem(item, subItemText, subItemsForeColorArray[i], subItemsFontStyleArray[i]);
                i++;
            }
            return item;
        }


        public void AddListViewSubItem(int itemIndex, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            this.Items[itemIndex].SubItems.Add(subItem);
        }


        public void AddListViewSubItem(string itemText, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            int itemIndex = GetListViewItemIndexByItemText(itemText);
            if (itemIndex != -1)
                this.Items[itemIndex].SubItems.Add(subItem);
        }


        public void AddListViewSubItem(ListViewItem item, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            item.SubItems.Add(subItem);
        }


        public void AddListViewSubItem(ListViewItem item, string subItemText, Color foreColor, FontStyle fontStyle)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            subItem.ForeColor = foreColor;
            FontFamily fontFamily = new FontFamily("Verdana");
            subItem.Font = new Font(fontFamily, (float)8.25, fontStyle);
            
            item.SubItems.Add(subItem);
        }


        public void UpdateListViewItem(int itemIndex, string itemText)
        {
            if (!this.Items[itemIndex].Text.Equals(itemText))
                this.Items[itemIndex].Text = itemText;
        }


        public void UpdateListViewItem(string itemText, string newItemText)
        {
            int itemIndex = GetListViewItemIndexByItemText(itemText);
            if (itemIndex != -1 && !this.Items[itemIndex].Text.Equals(newItemText))
                this.Items[itemIndex].Text = newItemText;
        }


        public void UpdateListViewSubItem(int itemIndex, int subItemIndex, string subItemText)
        {
            if (!this.Items[itemIndex].SubItems[subItemIndex].Text.Equals(subItemText))
            {
                this.Items[itemIndex].SubItems[subItemIndex].Text = subItemText;
            }
        }


        public void UpdateListViewSubItem(int itemIndex, int subItemIndex, string subItemText, Color foreColor, FontStyle fontStyle)
        {
            if (!this.Items[itemIndex].SubItems[subItemIndex].Text.Equals(subItemText))
            {
                this.Items[itemIndex].SubItems[subItemIndex].Text = subItemText;
                this.Items[itemIndex].SubItems[subItemIndex].ForeColor = foreColor;
                FontFamily fontFamily = new FontFamily("Verdana");
                this.Items[itemIndex].SubItems[subItemIndex].Font = new Font(fontFamily, (float)8.25, fontStyle);
            }
        }


        public void UpdateListViewSubItem(string itemText, int subItemIndex, string subItemText)
        {
            int itemIndex = GetListViewItemIndexByItemText(itemText);
            if (itemIndex != -1 && !this.Items[itemIndex].SubItems[subItemIndex].Text.Equals(subItemText))
                this.Items[itemIndex].SubItems[subItemIndex].Text = subItemText;
        }


        public void UpdateListViewSubItem(ListViewItem item, int subItemIndex, string subItemText)
        {
            if (!item.SubItems[subItemIndex].Text.Equals(subItemText))
                item.SubItems[subItemIndex].Text = subItemText;
        }


        #endregion ------ General ListView Methods (not thread safe) ------


        /***********************************************************************************************************/

    }
}
