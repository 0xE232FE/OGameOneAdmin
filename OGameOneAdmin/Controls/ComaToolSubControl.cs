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
using GF.BrowserGame.Schema.Internal;
using System.Threading;
using LibCommonUtil;

namespace OGameOneAdmin.Controls
{
    public partial class ComaToolSubControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private GameManager _gameManager;
        private TicketManager _ticketManager;
        private ComaToolControl _comaToolControl;
        private CommunityTicket _communityTicket;
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;

        private bool _myTicketLinkSelected = false;
        private bool _openTicketLinkSelected = false;
        private bool _closedTicketLinkSelected = false;

        private bool _myTicketHasNextPage = false;
        private bool _myTicketHasPrevPage = false;
        private bool _openTicketHasNextPage = false;
        private bool _openTicketHasPrevPage = false;
        private bool _closedTicketHasNextPage = false;
        private bool _closedTicketHasPrevPage = false;

        private TicketGUI selectedTicketGUI = null;
        private Ticket selectedTicket = null;
        private int currentlyAnsweredTicketId;
        private List<AnswerTemplate> answerTemplateList = new List<AnswerTemplate>();
        private AnswerTemplate selectedTemplate;
        private string noteTitle;
        private string comNumberActive = "";


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public ComaToolSubControl(GameManager gameManager, ComaToolControl comaToolControl, int communityId, string communityName)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _ticketManager = gameManager.GetTicketManager();
            _comaToolControl = comaToolControl;
            _communityTicket = new CommunityTicket();
            _communityTicket.CommunityId = communityId;
            _communityTicket.CommunityName = communityName;
            SetLinkPosition();
            ResizeLabelPageNr();
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Events ------


        private void linkLabelMyTicket_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EnableDisableAll(false);
            _myTicketLinkSelected = true;
            _openTicketLinkSelected = false;
            _closedTicketLinkSelected = false;
            linkLabelMyTicket.LinkBehavior = LinkBehavior.AlwaysUnderline;
            linkLabelOpenTickets.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabelClosedTicket.LinkBehavior = LinkBehavior.HoverUnderline;

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                _ticketManager.GetMyTicket(_communityTicket, _communityTicket.MyTicketCurrentPageNr);
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                dataGridViewTicket.DataSource = _communityTicket.MyTicketList;
                UpdateLinkText();
                SetLinkPosition();
                ReOrderColumns();
                PrevNextButtonMyTicket();
                EnableDisableAll(true);
            };
            worker.RunWorkerAsync();
        }


        private void linkLabelOpenTickets_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EnableDisableAll(false);
            _myTicketLinkSelected = false;
            _openTicketLinkSelected = true;
            _closedTicketLinkSelected = false;
            linkLabelMyTicket.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabelOpenTickets.LinkBehavior = LinkBehavior.AlwaysUnderline;
            linkLabelClosedTicket.LinkBehavior = LinkBehavior.HoverUnderline;

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                _ticketManager.GetOpenTicket(_communityTicket, _communityTicket.OpenTicketCurrentPageNr);
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                dataGridViewTicket.DataSource = _communityTicket.OpenTicketList;
                UpdateLinkText();
                SetLinkPosition();
                ReOrderColumns();
                PrevNextButtonOpenTicket();
                EnableDisableAll(true);
            };
            worker.RunWorkerAsync();
        }


        private void linkLabelClosedTicket_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EnableDisableAll(false);
            _myTicketLinkSelected = false;
            _openTicketLinkSelected = false;
            _closedTicketLinkSelected = true;
            linkLabelMyTicket.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabelOpenTickets.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabelClosedTicket.LinkBehavior = LinkBehavior.AlwaysUnderline;

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                _ticketManager.GetClosedTicket(_communityTicket, _communityTicket.ClosedTicketCurrentPageNr);
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                dataGridViewTicket.DataSource = _communityTicket.ClosedTicketList;
                UpdateLinkText();
                SetLinkPosition();
                ReOrderColumns();
                PrevNextButtonClosedTicket();
                EnableDisableAll(true);
            };
            worker.RunWorkerAsync();
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            EnableDisablePrevNextButton(false);
            if (linkLabelMyTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.MyTicketCurrentPageNr--;
                linkLabelMyTicket_LinkClicked(null, null);
            }
            else if (linkLabelOpenTickets.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.OpenTicketCurrentPageNr--;
                linkLabelOpenTickets_LinkClicked(null, null);
            }
            else if (linkLabelClosedTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.ClosedTicketCurrentPageNr--;
                linkLabelClosedTicket_LinkClicked(null, null);
            }
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            EnableDisablePrevNextButton(false);
            if (linkLabelMyTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.MyTicketCurrentPageNr++;
                linkLabelMyTicket_LinkClicked(null, null);
            }
            else if (linkLabelOpenTickets.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.OpenTicketCurrentPageNr++;
                linkLabelOpenTickets_LinkClicked(null, null);
            }
            else if (linkLabelClosedTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                _communityTicket.ClosedTicketCurrentPageNr++;
                linkLabelClosedTicket_LinkClicked(null, null);
            }
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }


        private void ComaToolSubControl_Resize(object sender, EventArgs e)
        {
            ResizeLabelPageNr();
        }


        private void dataGridViewTicket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    TicketGUI ticketGUI = new TicketGUI();
                    ticketGUI.TicketId = int.Parse(dataGridViewTicket.Rows[e.RowIndex].Cells["TicketId"].Value.ToString());
                    selectedTicketGUI = FindTicketGUI(ticketGUI.TicketId);

                    if (selectedTicketGUI != null)
                    {
                        if (selectedTicket == null || ticketGUI.TicketId != selectedTicket.TicketId)
                            selectedTicket = TicketGUIToTicket(selectedTicketGUI);

                        EnableDisableAll(false);

                        var worker = new BackgroundWorker();

                        worker.DoWork += (workerSender, workerEvent) =>
                        {
                            OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                            selectedTicket = _ticketManager.ReadTicket(_communityTicket, selectedTicket, out answerTemplateList);
                        };

                        worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                        {
                            OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                            EnableDisableAll(true);
                            if (selectedTicket == null)
                                return;
                            FillAnswerTemplateList(answerTemplateList);
                            FillNoteTitle();

                            string communityId = _gameManager.GetCommunityId(_communityTicket.CommunityName);
                            string uniId = _gameManager.GetUniId(communityId, selectedTicket.Server);

                            if (selectedTicket.TicketId != currentlyAnsweredTicketId)
                            {
                                currentlyAnsweredTicketId = selectedTicket.TicketId;
                                ResetAnswerTicketAndNoteView();
                                listBoxAccountList.Items.Clear();
                                tabControlTicket.SelectedTab = tabPageReadTicket;
                                txtBoxAddAccount.Text = selectedTicket.NickName;

                                if (selectedTicket.Server != 0 && !string.IsNullOrEmpty(communityId) && !string.IsNullOrEmpty(uniId) && _gameManager.GetUniManager(uniId).IsSessionSet(false))
                                {
                                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, true, false);
                                    btnAddToList_Click(null, null);
                                }
                                else
                                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, false, false);
                            }
                            else
                            {
                                if (selectedTicket.Server != 0 && !string.IsNullOrEmpty(communityId) && !string.IsNullOrEmpty(uniId) && _gameManager.GetUniManager(uniId).IsSessionSet(false) && selectedTicket.Account.Uid == 0)
                                {
                                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, true, false);
                                    btnAddToList_Click(null, null);
                                }
                                else if (selectedTicket.Server != 0 && !string.IsNullOrEmpty(communityId) && !string.IsNullOrEmpty(uniId) && _gameManager.GetUniManager(uniId).IsSessionSet(false) && selectedTicket.Account.Uid != 0)
                                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, true, true);
                                else
                                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, false, false);
                            }
                        };
                        worker.RunWorkerAsync();
                    }
                    else
                    {
                        selectedTicketGUI = null;
                        selectedTicket = null;
                        webBrowserTicketView.DocumentText = "Error... ticket unknown....";
                    }
                }
                else
                    return;
            }

            catch { }
        }


        private void btnAddToList_Click(object sender, EventArgs e)
        {
            if (selectedTicket == null)
                return;

            if (selectedTicket.Server == 0)
            {
                MessageBox.Show("Server 0 does not exist, therefore you cannot search/add a player", "Invalid server");
                return;
            }

            string communityId = _gameManager.GetCommunityId(_communityTicket.CommunityName);

            if (string.IsNullOrEmpty(communityId))
            {
                MessageBox.Show("The community " + _communityTicket.CommunityName + " does not appear in your dashboard, therefore you cannot search/add a player for this server", "Invalid community");
                return;
            }

            string uniId = _gameManager.GetUniId(communityId, selectedTicket.Server);

            if (string.IsNullOrEmpty(uniId))
            {
                MessageBox.Show("The server " + selectedTicket.Server + " does not appear in your dashboard, therefore you cannot search/add a player for this server", "Invalid server");
                return;
            }

            Int64 playerId = -1;
            string searchPlayerName = txtBoxAddAccount.Text.Trim();
            string playerEmail = "";

            if (string.IsNullOrEmpty(searchPlayerName))
            {
                MessageBox.Show("Please enter a player name that you wish to search and add to the list", "Field is empty");
                return;
            }

            if (PlayerNameExistInAccountInvolved(searchPlayerName) || selectedTicket.Account.Name.ToLower().Equals(searchPlayerName.ToLower()))
            {
                MessageBox.Show(searchPlayerName + " has already been added to the list.", "Player already in the list");
                txtBoxAddAccount.Text = "";
                return;
            }

            EnableDisableAll(false);

            var worker = new BackgroundWorker();

            worker.DoWork += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);

                playerId = _gameManager.SearchPlayerId(uniId, searchPlayerName);

                if (playerId != -1 && playerId != 0)
                {
                    if (selectedTicket.NickName.ToLower().Equals(searchPlayerName.ToLower()))
                    {
                        playerEmail = _gameManager.RetrievePlayerEmail(uniId, int.Parse(playerId.ToString()));
                    }
                }
            };

            worker.RunWorkerCompleted += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);

                if (playerId != -1)
                {
                    if (playerId != 0)
                    {
                        if (PlayerIdExistInAccountInvolved(playerId))
                        {
                            MessageBox.Show(searchPlayerName + " has already been added to the list", "Player already in the list");
                            txtBoxAddAccount.Text = "";
                        }
                        else
                        {
                            Account account = new Account();
                            account.Uid = playerId;
                            account.Name = searchPlayerName;
                            if (selectedTicket.NickName.ToLower().Equals(searchPlayerName.ToLower()))
                            {
                                selectedTicket.IsNickMatch = true;
                                if (!string.IsNullOrEmpty(playerEmail))
                                {
                                    account.PermaEmail = playerEmail;
                                    if (playerEmail.ToLower().Equals(selectedTicket.Email.ToLower()))
                                        selectedTicket.IsPermaEmailMatch = true;
                                    else
                                        selectedTicket.IsPermaEmailMatch = false;
                                }
                                selectedTicket.Account = account;
                                selectedTicket.IsNickMatch = true;
                            }
                            listBoxAccountList.Items.Add(account.Name);
                            selectedTicket.AccountInvolvedList.Add(account);
                            txtBoxAddAccount.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show(searchPlayerName + " could not be found on server " + selectedTicket.Server, "Player not found");
                    }
                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, true, true);
                }
                else
                    webBrowserTicketView.DocumentText = GenerateTicketView(selectedTicket, false, false);
            };
            worker.RunWorkerAsync();
        }


        private void btnRemoveAccountFromList_Click(object sender, EventArgs e)
        {
            if (listBoxAccountList.SelectedItems.Count == 0 || selectedTicket == null)
                return;

            string playerName = listBoxAccountList.SelectedItem.ToString();

            if (selectedTicket.NickName.ToLower().Equals(playerName.ToLower()))
            {
                MessageBox.Show("You cannot remove " + playerName + " as it is the author of the ticket.", "Error");
                return;
            }

            if (PlayerNameExistInAccountInvolved(playerName))
            {
                List<Account> accList = new List<Account>();
                foreach (Account acc in selectedTicket.AccountInvolvedList)
                {
                    if (!acc.Name.Equals(playerName))
                    {
                        accList.Add(acc);
                    }
                }
                selectedTicket.AccountInvolvedList = accList;
                listBoxAccountList.Items.Remove(playerName);
            }
        }


        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAccountList.SelectedItems.Count > 0)
                {
                    string communityId = _gameManager.GetCommunityId(_communityTicket.CommunityName);
                    string uniId = _gameManager.GetUniId(communityId, selectedTicket.Server);

                    string nickname = listBoxAccountList.SelectedItem.ToString();
                    int playerId = 0;

                    foreach (Account acc in selectedTicket.AccountInvolvedList)
                    {
                        if (acc.Name.ToLower().Equals(nickname.ToLower()))
                            playerId = int.Parse(acc.Uid.ToString());
                    }

                    _gameManager.GetUniManager(uniId).OpenWebBrowserUserAccount(_gameManager.GetUserApplicationConfig().ExternalWebBrowser, playerId);
                }
            }
            catch {

                MessageBox.Show("Error occurred");
            }
        }


        private void btnSubmitAnswerTicket_Click(object sender, EventArgs e)
        {
            if (selectedTicket != null && currentlyAnsweredTicketId == selectedTicket.TicketId)
            {
                bool result = false;
                string ticketAnswer = txtBoxAnswerTicket.Text;

                EnableDisableAll(false);

                var worker = new BackgroundWorker();

                worker.DoWork += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                    result = _ticketManager.PostTicketAnswer(_communityTicket, selectedTicket, ticketAnswer);
                };

                worker.RunWorkerCompleted += (workerSender, workerEvent) =>
                {
                    OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                    EnableDisableAll(true);
                    if (_myTicketLinkSelected)
                    {
                        linkLabelMyTicket_LinkClicked(null, null);
                    }
                    else if (_openTicketLinkSelected)
                    {
                        linkLabelOpenTickets_LinkClicked(null, null);
                    }
                    else if (_closedTicketLinkSelected)
                    {
                        linkLabelClosedTicket_LinkClicked(null, null);
                    }
                    if (result)
                        MessageBox.Show("Your answer has been submitted successfully.", "Info");
                    else
                        MessageBox.Show("Your answer could not be submitted.", "Error");
                };
                worker.RunWorkerAsync();
            }
        }


        private void btnSelectAnswerTemplate_Click(object sender, EventArgs e)
        {
            if (selectedTicket == null)
            {
                return;
            }

            string ticketPreAnswer = "";

            EnableDisableAll(false);

            var worker = new BackgroundWorker();

            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                ticketPreAnswer = _ticketManager.GetTicketAnswerTemplate(_communityTicket, selectedTicket, selectedTemplate.Link);
            };

            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                txtBoxAnswerTicket.Text = ticketPreAnswer;
            };
            worker.RunWorkerAsync();
        }


        private void comboBoxAnswerTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (answerTemplateList.Count > 0)
            {
                foreach (AnswerTemplate template in answerTemplateList)
                {
                    if (template.Title.Equals(comboBoxAnswerTemplate.SelectedItem.ToString()))
                    {
                        selectedTemplate = template;
                    }
                }
            }
        }


        private void comboBoxNoteTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            noteTitle = comboBoxNoteTitle.SelectedItem.ToString();

            if (noteTitle.Equals("None"))
            {
                noteTitle = "";
            }
        }


        private void btnGenerateNote_Click(object sender, EventArgs e)
        {
            if (selectedTicket == null)
                return;

            if (!checkBoxAddToAllPlayers.Checked && selectedTicket.Account.Uid == 0)
            {
                txtBoxNote.Text = "The Author of the ticket does not possess an account or you did not retrieve his account details!\r\n\r\nIf you wish to submit a note to other accounts, add each player to the list and tick the box \"Add note to all players involved\".";
                return;
            }
            else if (checkBoxAddToAllPlayers.Checked && selectedTicket.Account.Uid == 0 && selectedTicket.AccountInvolvedList.Count == 0)
            {
                txtBoxNote.Text = "The Author of the ticket does not possess an account or you did not retrieve his account details!\r\n\r\nOr you did not add any players to the list.";
                return;
            }

            DateTime dateNote = System.TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now).AddHours(1);
            string month = "";
            switch (dateNote.Month)
            {
                case 1:
                    month = "Jan";
                    break;
                case 2:
                    month = "Feb";
                    break;
                case 3:
                    month = "Mar";
                    break;
                case 4:
                    month = "Apr";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "Jun";
                    break;
                case 7:
                    month = "Jul";
                    break;
                case 8:
                    month = "Aug";
                    break;
                case 9:
                    month = "Sep";
                    break;
                case 10:
                    month = "Oct";
                    break;
                case 11:
                    month = "Nov";
                    break;
                case 12:
                    month = "Dec";
                    break;
                default:
                    month = "Unknown";
                    break;
            }
            //String.Format("{0:dd/MM/yyyy HH:mm:ss}", System.TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now).AddHours(1));

            string note = dateNote.Day + " " + month + " " + dateNote.Year + ", " + noteTitle + "\r\n\r\n";

            if (checkBoxInclUID.Checked)
            {
                if (!checkBoxAddToAllPlayers.Checked)
                {
                    if (selectedTicket.Account.Uid != 0)
                    {
                        note += "- Player Nick: " + selectedTicket.Account.Name + ", Player ID: " + selectedTicket.Account.Uid.ToString() + "\r\n\r\n";
                    }
                }
                else
                {
                    if (selectedTicket.AccountInvolvedList.Count > 0)
                    {
                        foreach (Account account in selectedTicket.AccountInvolvedList)
                        {
                            note += "- Player Nick: " + account.Name + ", Player ID: " + account.Uid.ToString() + "\r\n";
                        }
                        note += "\r\n";
                    }
                }
            }
            note += "--------------------------------------------------\r\nTicket Details\r\n--------------------------------------------------\r\n";
            note += "Ticket-ID: " + selectedTicket.TicketId + "\r\n";
            note += "Subject: " + selectedTicket.Subject + "\r\n";
            //note += "Created on: " + String.Format("{0:dd/MM/yyyy HH:mm:ss}", selectedTicket.CreatedOn) + "\r\n";
            //note += "Last reply: " + String.Format("{0:dd/MM/yyyy HH:mm:ss}", selectedTicket.LastReply) + "\r\n";
            note += "Author: " + selectedTicket.NickName + "\r\n";
            note += "Email: " + selectedTicket.Email + "\r\n";
            note += "IP Address: " + selectedTicket.IpAddress + "\r\n";

            PropertyComparer<TicketMessage> comparer = new PropertyComparer<TicketMessage>("MessageDateTime asc");
            selectedTicket.TicketMessageList.Sort(comparer);

            int countIgnoreMessage = 0;
            foreach (TicketMessage message in selectedTicket.TicketMessageList)
            {
                if (string.IsNullOrEmpty(message.NickName))
                {
                    countIgnoreMessage++;
                }
            }

            int count = 1;
            foreach (TicketMessage message in selectedTicket.TicketMessageList)
            {
                if (string.IsNullOrEmpty(message.NickName))
                    continue;
                string mess = message.Message.Replace("<br>", "\n");
                //mess = message.Message.Replace("<br>", "\r\n");
                note += "--------------------------------------------------\r\n";
                note += "Message " + count + "/" + (selectedTicket.TicketMessageList.Count - countIgnoreMessage) + "\r\n";
                note += "--------------------------------------------------\r\n";
                //note += "------ Message " + count + "/" + selectedTicket.TicketMessageList.Count + " ------\r\n";
                note += "Date: " + String.Format("{0:dd/MM/yyyy HH:mm:ss}", message.MessageDateTime) + "\r\n";
                if (selectedTicket.NickName.ToLower().Equals(message.NickName.ToLower()))
                {
                    note += "IP Address: " + message.IpAddress + "\r\n";
                }
                note += "Author: " + message.NickName + "\r\n\r\n";
                note += mess + "\r\n";
                count++;
            }

            txtBoxNote.Text = note;
        }


        private void btnSubmitNote_Click(object sender, EventArgs e)
        {
            if (selectedTicket == null)
                return;

            if (selectedTicket.Server == 0)
            {
                MessageBox.Show("Server 0 does not exist.", "Invalid server");
                return;
            }

            if (string.IsNullOrEmpty(txtBoxNote.Text.Trim()))
            {
                MessageBox.Show("Note is empty.", "Error");
                return;
            }

            if (!checkBoxAddToAllPlayers.Checked && selectedTicket.Account.Uid == 0)
            {
                MessageBox.Show("The Author of the ticket does not possess an account or you did not retrieve his account details!\r\n\r\nIf you wish to submit a note to other accounts, add each player to the list and tick the box \"Add note to all players involved\".", "No player found");
                return;
            }
            else if (checkBoxAddToAllPlayers.Checked && selectedTicket.Account.Uid == 0 && selectedTicket.AccountInvolvedList.Count == 0)
            {
                MessageBox.Show("The Author of the ticket does not possess an account or you did not retrieve his account details!\r\n\r\nOr you did not add any players to the list.", "No player found");
                return;
            }

            string communityId = _gameManager.GetCommunityId(_communityTicket.CommunityName);

            if (string.IsNullOrEmpty(communityId))
            {
                MessageBox.Show("The community " + _communityTicket.CommunityName + " does not appear in your dashboard, therefore you cannot add note to players from this server", "Invalid community");
                return;
            }

            string uniId = _gameManager.GetUniId(communityId, selectedTicket.Server);

            if (string.IsNullOrEmpty(uniId))
            {
                MessageBox.Show("The server " + selectedTicket.Server + " does not appear in your dashboard, therefore you cannot add note to players for this server", "Invalid server");
                return;
            }

            if (checkBoxAddToAllPlayers.Checked)
            {
                string nickList = "";

                foreach (Account account in selectedTicket.AccountInvolvedList)
                {
                    if (!string.IsNullOrEmpty(nickList))
                        nickList += ", ";
                    nickList += account.Name;
                }
                if (MessageBox.Show("You are about to submit a note to the following player(s):\r\r" + nickList, "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }
            else
            {
                if (MessageBox.Show("You are about to submit a note to the following player:\r\r" + selectedTicket.Account.Name, "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }

            string noteToPost = txtBoxNote.Text;
            string results = "";
            bool errorOccurred = false;

            EnableDisableAll(false);

            var worker = new BackgroundWorker();

            worker.DoWork += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                
                bool stopped = false;
                int result = 0;

                if (checkBoxAddToAllPlayers.Checked)
                {
                    foreach (Account account in selectedTicket.AccountInvolvedList)
                    {
                        if (!stopped)
                        {
                            result = _gameManager.AddLongNoteToPlayer(uniId, int.Parse(account.Uid.ToString()), noteToPost);
                            if (result == -1)
                            {
                                errorOccurred = true;
                                results += account.Name + ": FAILED TO ADD NOTE\r";
                                break;
                            }
                            if (result == 0)
                            {
                                errorOccurred = true;
                                results += account.Name + ": FAILED TO ADD NOTE\r";
                            }
                            else if (result == 1)
                                results += account.Name + ": Note added successfully.\r";
                        }
                        else
                            results += account.Name + ": FAILED TO ADD NOTE\r";
                    }
                }
                else
                {
                    result = _gameManager.AddLongNoteToPlayer(uniId, int.Parse(selectedTicket.Account.Uid.ToString()), noteToPost);
                    if (result == -1)
                    {
                        errorOccurred = true;
                        results += selectedTicket.Account.Name + ": FAILED TO ADD NOTE\r";
                    }
                    if (result == 0)
                    {
                        errorOccurred = true;
                        results += selectedTicket.Account.Name + ": FAILED TO ADD NOTE\r";
                    }
                    else if (result == 1)
                        results += selectedTicket.Account.Name + ": Note added successfully.\r";
                }
            };

            worker.RunWorkerCompleted += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);

                if (errorOccurred)
                {
                    MessageBox.Show(results, "Error occurred");
                }
                else
                {
                    MessageBox.Show(results, "Info");
                    //results = "---------------------------------\r*** RESULTS ***\r---------------------------------\r\r" + results;
                    //txtBoxNote.Text = results + "---------------------------------\r\r" + txtBoxNote.Text;
                }
            };
            worker.RunWorkerAsync();
        }


        private void linkLabelAllPlayerQuestion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If ticked, the same note will be added to all players from the list 'Player involved'.\r\rIf not ticked, the note will only be added to the author's account of the ticket", "Info");
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void LoadControl()
        {
            if (!HasLinkBeenSelected())
                linkLabelOpenTickets_LinkClicked(null, null);
        }


        public void ApplyFocus()
        {
        }


        public void SelectActiveCommunity()
        {
            EnableDisableAll(false);
            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                _ticketManager.SelectActiveCommunity(_communityTicket.CommunityId);
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                LoadControl();
            };
            worker.RunWorkerAsync();
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private bool PlayerIdExistInAccountInvolved(Int64 playerId)
        {
            if (selectedTicket != null)
            {
                if (selectedTicket.AccountInvolvedList.FirstOrDefault(r => r.Uid == playerId) != null)
                    return true;
            }
            return false;
        }


        private bool PlayerNameExistInAccountInvolved(string playerName)
        {
            if (selectedTicket != null)
            {
                if (selectedTicket.AccountInvolvedList.FirstOrDefault(r => r.Name.ToLower().Equals(playerName.ToLower())) != null)
                    return true;
            }
            return false;
        }


        private TicketGUI FindTicketGUI(int ticketId)
        {
            if (_myTicketLinkSelected)
            {
                return _communityTicket.MyTicketList.FirstOrDefault(r => r.TicketId == ticketId);
            }
            else if (_openTicketLinkSelected)
            {
                return _communityTicket.OpenTicketList.FirstOrDefault(r => r.TicketId == ticketId);
            }
            else if (_closedTicketLinkSelected)
            {
                return _communityTicket.ClosedTicketList.FirstOrDefault(r => r.TicketId == ticketId);
            }
            else
                return null;
        }


        private Ticket TicketGUIToTicket(TicketGUI ticketGUI)
        {
            if (ticketGUI == null)
                return null;

            Ticket ticket = new Ticket();
            ticket.DateString = ticketGUI.DateString;
            ticket.NickName = ticketGUI.NickName;
            ticket.OpenedTickets = ticketGUI.OpenedTickets;
            ticket.Server = ticketGUI.Server;
            ticket.StaffNickName = ticketGUI.StaffNickName;
            ticket.Subject = ticketGUI.Subject;
            ticket.TicketId = ticketGUI.TicketId;
            ticket.TicketValue = ticketGUI.TicketValue;
            ticket.Email = "";
            return ticket;
        }


        private string GenerateTicketView(Ticket ticket, bool canRetrieveDetails, bool accountDetailsRetrieved)
        {
            if (ticket == null)
                return "Error, could not read ticket. Try again.";

            string html = "<html><head><link href='https://coma.gameforge.com/ticket/style/style.css' rel='stylesheet'/></head><body>";

            html += "<table border='0' cellspacing='0' cellpadding='2' width='100%' class='nav'>";
            html += "<tr><td class='head' width='50%'><b>Ticket info</b></td><td class='head' width='50%'><b>Matching account on Server " + ticket.Server + "</b></td>";
            html += "<tr><td bgcolor='#E9E9E9' class='text'><b>Author:</b> " + ticket.NickName + "</td>";


            if (canRetrieveDetails)
            {
                if (accountDetailsRetrieved)
                {
                    if (ticket.IsNickMatch)
                        html += "<td bgcolor='#E9E9E9' class='text'><b>Name:</b> " + ticket.Account.Name + ", <b>Player ID:</b> " + ticket.Account.Uid.ToString() + "</td></tr>";
                    else
                        html += "<td bgcolor='#E9E9E9' class='text'><b>ACCOUNT NOT FOUND</b></td></tr>";
                }
                else
                    html += "<td class='text'><b>Retrieving acount now...</b></td></tr>";
            }
            else if (!canRetrieveDetails)
            {
                if (ticket.Server != 0)
                    html += "<td class='text'>(cannot verify - login to the right server)</td></tr>";
                else
                    html += "<td class='text'>(cannot verify - server does not exist)</td></tr>";
            }

            //html += "Subject = " + ticket.Subject + "<br />";
            html += "<tr><td class='text'><b>Email:</b> " + ticket.Email + "</td>";

            if (canRetrieveDetails)
            {
                if (accountDetailsRetrieved)
                {
                    if (ticket.IsNickMatch)
                    {
                        if (ticket.IsPermaEmailMatch)
                            html += "<td class='text'><b>Email:</b> " + ticket.Account.PermaEmail + "</td>";
                        else
                            html += "<td class='text'><font color='red'><b>WRONG EMAIL:</b></font> " + ticket.Account.PermaEmail + "</td>";
                    }
                    else
                        html += "<td class='text'>(not verified)</td>";
                }
                else
                    html += "<td class='text'><b>Retrieving email now...</b></td></tr>";
            }
            else if (!canRetrieveDetails)
            {
                if (ticket.Server != 0)
                    html += "<td class='text'>(cannot verify - login to the right server)</td></tr>";
                else
                    html += "<td class='text'>(cannot verify - server does not exist)</td></tr>";
            }

            //html += "Server = " + ticket.Server + "<br />";
            //html += "IP Address = " + ticket.IpAddress + "<br />";
            //html += "Created on = " + ticket.CreatedOn + "<br />";
            //html += "Last reply = " + ticket.LastReply + "<br />";
            html += "</table>";

            html += "<br />";
            html += "<table border='0' cellspacing='0' cellpadding='0' width='100%' class='nav'>";
            foreach (TicketMessage message in ticket.TicketMessageList)
            {
                html += "<tr><td class='head'>";
                html += "</td><td class='head'><b>";
                html += message.NickName;
                html += " </b></td><td class='head'>";
                html += message.MessageDateTime.ToString();
                html += "</td><td class='head' align='right'>" + message.IpAddress + "</td></tr><tr><td class='text'>&nbsp;</td><td class='text' valign='top' width='150'></td><td class='text'>";
                html += "</td></tr><tr><td class='text'>&nbsp;</td><td class='text' valign='top' width='150'><b>Message</b></td><td class='text' colspan='2'>";
                html += message.Message;
                html += "<br /><br /></td></tr>";
            }
            html += "</table></body></html>";
            return html;
        }


        private void FillAnswerTemplateList(List<AnswerTemplate> templateList)
        {
            if (templateList.Count > 0)
            {
                comboBoxAnswerTemplate.Items.Clear();
                foreach (AnswerTemplate template in templateList)
                {
                    comboBoxAnswerTemplate.Items.Add(template.Title);
                    if (template.Link.Equals("0"))
                    {
                        selectedTemplate = template;
                        comboBoxAnswerTemplate.SelectedItem = template.Title;
                    }
                }
            }
        }


        private void FillNoteTitle()
        {
            comboBoxNoteTitle.Items.Clear();
            comboBoxNoteTitle.Items.Add("None");
            comboBoxNoteTitle.Items.Add("Moonshot");
            comboBoxNoteTitle.Items.Add("IP Sharing");
            comboBoxNoteTitle.Items.Add("ACS Split");
            comboBoxNoteTitle.Items.Add("Account Sitting");
            comboBoxNoteTitle.Items.Add("Account Exchange");
            comboBoxNoteTitle.Items.Add("New Owner");
            comboBoxNoteTitle.Items.Add("Recycle Help");
            comboBoxNoteTitle.Items.Add("Pushing");
            comboBoxNoteTitle.Items.Add("Bashing");
            comboBoxNoteTitle.Items.Add("IP Check D/A Denied");
            comboBoxNoteTitle.Items.Add("IP Check D/A Approved");
            comboBoxNoteTitle.Items.Add("Account Sharing");
            comboBoxNoteTitle.Items.Add("Ban");
            comboBoxNoteTitle.Items.Add("Unban");
            comboBoxNoteTitle.SelectedItem = "None";
        }


        private void ResetAnswerTicketAndNoteView()
        {
            txtBoxAnswerTicket.Text = "";
            txtBoxNote.Text = "";
            txtBoxAddAccount.Text = "";
        }


        private void EnableDisableAll(bool state)
        {
            EnableDisableLink(state);
            EnableDisablePrevNextButton(state);
            dataGridViewTicket.Enabled = state;
            splitContainer3.Enabled = state;
            btnLogout.Enabled = state;
        }


        private void SetLinkPosition()
        {
            linkLabelOpenTickets.Location = new Point(linkLabelMyTicket.Size.Width + linkLabelMyTicket.Location.X + 6, linkLabelMyTicket.Location.Y);
            linkLabelClosedTicket.Location = new Point(linkLabelOpenTickets.Size.Width + linkLabelOpenTickets.Location.X + 6, linkLabelOpenTickets.Location.Y);
            pictureBoxInProgress.Location = new Point(linkLabelClosedTicket.Size.Width + linkLabelClosedTicket.Location.X + 10, pictureBoxInProgress.Location.Y);
        }


        private void UpdateLinkText()
        {
            linkLabelMyTicket.Text = "My Tickets (" + _communityTicket.TotalMyTicket + ")";
            linkLabelOpenTickets.Text = "Open Tickets (" + _communityTicket.TotalOpenTicket + ")";
        }


        private void ReOrderColumns()
        {
            dataGridViewTicket.ClearSelection();
            dataGridViewTicket.Columns["Server"].DisplayIndex = 0;
            dataGridViewTicket.Columns["Subject"].DisplayIndex = 1;
            dataGridViewTicket.Columns["NickName"].DisplayIndex = 2;
            dataGridViewTicket.Columns["StaffNickName"].DisplayIndex = 3;
            dataGridViewTicket.Columns["AnswerNumber"].DisplayIndex = 4;
            dataGridViewTicket.Columns["Date"].DisplayIndex = 5;
            dataGridViewTicket.Columns["Date2"].DisplayIndex = 6;
            dataGridViewTicket.Columns["TicketId"].DisplayIndex = 7;
            dataGridViewTicket.Columns["TicketValue"].DisplayIndex = 8;
        }


        private bool HasAnyTicket()
        {
            if (_communityTicket.MyTicketList.Count + _communityTicket.OpenTicketList.Count > 0)
                return true;
            else
                return false;
        }


        private bool HasLinkBeenSelected()
        {
            bool result = false;
            if (linkLabelMyTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
                result = true;
            else if (linkLabelOpenTickets.LinkBehavior == LinkBehavior.AlwaysUnderline)
                result = true;
            else if (linkLabelClosedTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
                result = true;
            return result;
        }


        private void EnableDisableLink(bool state)
        {
            linkLabelMyTicket.Enabled = linkLabelClosedTicket.Enabled = linkLabelOpenTickets.Enabled = state;
        }


        private void EnableDisablePrevNextButton(bool state)
        {
            btnNext.Enabled = btnPrevious.Enabled = state;
        }


        private void PrevNextButtonMyTicket()
        {
            if (_communityTicket.MyTicketTotalPageNr > 1)
            {
                if (_communityTicket.MyTicketCurrentPageNr == _communityTicket.MyTicketTotalPageNr)
                {
                    _myTicketHasPrevPage = true;
                    _myTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.MyTicketCurrentPageNr == 1)
                {
                    _myTicketHasPrevPage = false;
                    _myTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.MyTicketCurrentPageNr < _communityTicket.MyTicketTotalPageNr && _communityTicket.MyTicketCurrentPageNr > 1)
                {
                    _myTicketHasPrevPage = true;
                    _myTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else
                {
                    _myTicketHasPrevPage = false;
                    _myTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
            }
            else
            {
                _myTicketHasPrevPage = false;
                _myTicketHasNextPage = false;
                PrevNextButtonEnabled();
            }
        }


        private void PrevNextButtonOpenTicket()
        {
            if (_communityTicket.OpenTicketTotalPageNr > 1)
            {
                if (_communityTicket.OpenTicketCurrentPageNr == _communityTicket.OpenTicketTotalPageNr)
                {
                    _openTicketHasPrevPage = true;
                    _openTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.OpenTicketCurrentPageNr == 1)
                {
                    _openTicketHasPrevPage = false;
                    _openTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.OpenTicketCurrentPageNr < _communityTicket.OpenTicketTotalPageNr && _communityTicket.OpenTicketCurrentPageNr > 1)
                {
                    _openTicketHasPrevPage = true;
                    _openTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else
                {
                    _openTicketHasPrevPage = false;
                    _openTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
            }
            else
            {
                _openTicketHasPrevPage = false;
                _openTicketHasNextPage = false;
                PrevNextButtonEnabled();
            }
        }


        private void PrevNextButtonClosedTicket()
        {
            if (_communityTicket.ClosedTicketTotalPageNr > 1)
            {
                if (_communityTicket.ClosedTicketCurrentPageNr == _communityTicket.ClosedTicketTotalPageNr)
                {
                    _closedTicketHasPrevPage = true;
                    _closedTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.ClosedTicketCurrentPageNr == 1)
                {
                    _closedTicketHasPrevPage = false;
                    _closedTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else if (_communityTicket.ClosedTicketCurrentPageNr < _communityTicket.ClosedTicketTotalPageNr && _communityTicket.ClosedTicketCurrentPageNr > 1)
                {
                    _closedTicketHasPrevPage = true;
                    _closedTicketHasNextPage = true;
                    PrevNextButtonEnabled();
                }
                else
                {
                    _closedTicketHasPrevPage = false;
                    _closedTicketHasNextPage = false;
                    PrevNextButtonEnabled();
                }
            }
            else
            {
                _closedTicketHasPrevPage = false;
                _closedTicketHasNextPage = false;
                PrevNextButtonEnabled();
            }
        }


        private void PrevNextButtonEnabled()
        {
            if (linkLabelMyTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                labelPageNr.Text = _communityTicket.MyTicketCurrentPageNr.ToString() + "/" + _communityTicket.MyTicketTotalPageNr.ToString();
                if (_myTicketHasPrevPage)
                    btnPrevious.Enabled = true;
                else
                    btnPrevious.Enabled = false;

                if (_myTicketHasNextPage)
                    btnNext.Enabled = true;
                else
                    btnNext.Enabled = false;
            }
            else if (linkLabelOpenTickets.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                labelPageNr.Text = _communityTicket.OpenTicketCurrentPageNr.ToString() + "/" + _communityTicket.OpenTicketTotalPageNr.ToString();
                if (_openTicketHasPrevPage)
                    btnPrevious.Enabled = true;
                else
                    btnPrevious.Enabled = false;

                if (_openTicketHasNextPage)
                    btnNext.Enabled = true;
                else
                    btnNext.Enabled = false;
            }
            else if (linkLabelClosedTicket.LinkBehavior == LinkBehavior.AlwaysUnderline)
            {
                labelPageNr.Text = _communityTicket.ClosedTicketCurrentPageNr.ToString() + "/" + _communityTicket.ClosedTicketTotalPageNr.ToString();
                if (_closedTicketHasPrevPage)
                    btnPrevious.Enabled = true;
                else
                    btnPrevious.Enabled = false;

                if (_closedTicketHasNextPage)
                    btnNext.Enabled = true;
                else
                    btnNext.Enabled = false;
            }
            ResizeLabelPageNr();
        }


        private void ResizeLabelPageNr()
        {
            try
            {
                labelPageNr.Location = new Point((((btnNext.Location.X + btnNext.Size.Width) - btnPrevious.Location.X) / 2) - (labelPageNr.Size.Width / 2), (btnNext.Location.Y + (btnNext.Size.Height / 2)));
            }
            catch { }
        }


        private void Logout()
        {
            bool success = false;

            EnableDisableAll(false);

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                success = _ticketManager.Logout();
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                _comaToolControl.OnNotifyLoggedOut();
            };
            worker.RunWorkerAsync();
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


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/

    }
}
