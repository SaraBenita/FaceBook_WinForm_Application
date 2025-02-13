using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private SystemManager m_SystemManager;
        private int m_CurrentPhotoIndexInAlbum;
        private readonly GuessTheMomentManager r_GuessTheMomentManager;
        public FormMain(SystemManager i_SystemManager)
        {
            InitializeComponent();
            m_SystemManager = i_SystemManager;
            r_GuessTheMomentManager = new GuessTheMomentManager();
        }
        private void FormMain_Shown(object sender, EventArgs e)
        {
            initAllUserInformation();
        }
        private void initAllUserInformation()
        {
            pictureBoxUserProfile.ImageLocation = m_SystemManager.LoggedInUser.PictureNormalURL;
            labelUserName.Text = m_SystemManager.LoggedInUser.Name;
        }
        private void hideAllPanelsOfUserSelection(Panel i_PanelActivateVisible)
        {
            panelPersonalInfo.Visible = false;
            panelPages.Visible = false;
            panelGroups.Visible = false;
            panelAlbums.Visible = false;
            panelPosts.Visible = false;
            panelGuessTheMoment.Visible = false;
            panelDownloadPhotos.Visible = false;
            i_PanelActivateVisible.Visible = true;
        }
        private void initPersonalInfo()
        {
            labelName.Text = m_SystemManager.LoggedInUser.Name;
            labelGender.Text = m_SystemManager.LoggedInUser.Gender.ToString();
            LabelBirthday.Text = m_SystemManager.LoggedInUser.Birthday;
            labelMail.Text = m_SystemManager.LoggedInUser.Email;
        }
        private void initUserPages()
        {
            FacebookObjectCollection<Page> pages;

            listBoxPages.Items.Clear();
            listBoxPages.DisplayMember = "Name";
            try
            {
                pages = m_SystemManager.LoggedInUser.LikedPages;
                foreach (Page page in pages)
                {
                    listBoxPages.Items.Add(page);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void initUserGroups()
        {
            FacebookObjectCollection<Group> groups;

            listBoxGroups.Items.Clear();
            listBoxGroups.DisplayMember = "Name";
            try
            {
                groups = m_SystemManager.LoggedInUser.Groups;
                foreach (Group group in groups)
                {
                    listBoxGroups.Items.Add(group);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void initUserPosts()
        {
            FacebookObjectCollection<Post> posts;

            listBoxPosts.Items.Clear();
            try
            {
                posts = m_SystemManager.LoggedInUser.Posts;
                foreach (Post post in posts)
                {
                    string postMessage = post.Message;
                    if (postMessage != null)
                    {
                        listBoxPosts.Items.Add
                            ($"{post.CreatedTime?.ToString("MM/dd/yyyy HH:mm")}  : {postMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void initUserAlbums()
        {
            listBoxAlbums.Items.Clear();
            listBoxAlbums.DisplayMember = "Name";
            try
            {
                foreach (Album album in m_SystemManager.LoggedInUser.Albums)
                {
                    listBoxAlbums.Items.Add(album);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void initCheckListBoxDatesOptions()
        {
            List<string> datesList;

            checkedListBoxDatesOptions.Items.Clear();
            datesList = r_GuessTheMomentManager.InitializeGuessDatesList();
            foreach (string date in datesList)
            {
                checkedListBoxDatesOptions.Items.Add(date);
            }
        }
        private void initAlbumsOptionsForDownload()
        {
            comboBoxAlbums.Items.Clear();
            comboBoxAlbums.DisplayMember = "Name";
            try
            {
                foreach (Album album in m_SystemManager.LoggedInUser.Albums)
                {
                    if (album.Photos.Count != 0)
                    {
                        comboBoxAlbums.Items.Add(album);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void downloadPhoto(string i_PhotoPath, string i_PhotoName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(i_PhotoPath, i_PhotoName);
            }
        }
        private void downloadAlbum(string i_SaveAlbumPath)
        {
            Album chosenAlbum = comboBoxAlbums.SelectedItem as Album;
            string albumPath = Path.Combine(i_SaveAlbumPath, chosenAlbum.Name);
            Directory.CreateDirectory(albumPath);
            progressBarDownload.Maximum = chosenAlbum.Photos.Count;
            foreach (Photo photo in chosenAlbum.Photos)
            {
                string fileName = $"{progressBarDownload.Value + 1}_{photo.Name}.jpg";
                try
                {
                    downloadPhoto(photo.PictureNormalURL, Path.Combine(albumPath, fileName));
                }
                catch (Exception ignored)
                {
                    MessageBox.Show(ignored.Message);
                }
                progressBarDownload.Value++;
            }
        }
        private void displayImages(Album i_ChosenAlbum)
        {
            flowLayoutPanelViewPhotos.Controls.Clear();
            foreach (Photo photo in i_ChosenAlbum.Photos)
            {
                PictureBox pictureBox = new PictureBox {
                    ImageLocation = photo.PictureNormalURL,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 100),
                    Margin = new Padding(5)};
                flowLayoutPanelViewPhotos.Controls.Add(pictureBox);
            }
        }
        private void pictureBoxUserProfile_Paint(object sender, PaintEventArgs e)
        {
            PictureBox userPictureBox;

            userPictureBox = sender as PictureBox;
            if (userPictureBox != null && userPictureBox.Image != null)
            {
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddEllipse(0, 0, userPictureBox.Width - 1, userPictureBox.Height - 1);
                Region rg = new Region(gp);
                userPictureBox.Region = rg;
                e.Graphics.SetClip(gp);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawImage(userPictureBox.Image, 0, 0, userPictureBox.Width, userPictureBox.Height);
            }
        }
        private void buttonLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to log out?",
                "Logout", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                m_SystemManager.ClearLoginResutlForLogout();
                this.Close();
            }
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormLogin loginForm = Application.OpenForms.
            OfType<FormLogin>().FirstOrDefault();
            loginForm.Show();
        }
        private void buttonProfile_Click(object sender, EventArgs e)
        {
            initPersonalInfo();
            hideAllPanelsOfUserSelection(panelPersonalInfo);
        }
        private void buttonPages_Click(object sender, EventArgs e)
        {
            initUserPages();
            hideAllPanelsOfUserSelection(panelPages);
            if (listBoxPages.Items.Count == 0)
            {
                MessageBox.Show("No liked pages to retrieve :(");
            }
            else
            {
                labelPagePhoto.Visible = false;
                pictureBoxPagePhoto.Visible = false;
                listBoxPages.ClearSelected();
            }
        }
        private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPages.SelectedItems.Count == 1)
            {
                Page selectedPage = listBoxPages.SelectedItem as Page;
                pictureBoxPagePhoto.LoadAsync(selectedPage.PictureNormalURL);
                labelPagePhoto.Visible = true;
                pictureBoxPagePhoto.Visible = true;
            }
        }
        private void buttonGroups_Click(object sender, EventArgs e)
        {
            initUserGroups();
            hideAllPanelsOfUserSelection(panelGroups);
            if (listBoxGroups.Items.Count == 0)
            {
                MessageBox.Show("No groups to retrieve :(");
            }
            else
            {
                labelGroupPhoto.Visible = false;
                pictureBoxGroup.Visible = false;
                listBoxGroups.ClearSelected();
            }
        }
        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGroups.SelectedItems.Count == 1)
            {
                Group selectedGroup = listBoxGroups.SelectedItem as Group;
                pictureBoxGroup.LoadAsync(selectedGroup.PictureNormalURL);
                labelGroupPhoto.Visible = true;
                pictureBoxGroup.Visible = true;
            }
        }
        private void buttonPosts_Click(object sender, EventArgs e)
        {
            initUserPosts();
            hideAllPanelsOfUserSelection(panelPosts);
            if (listBoxPosts.Items.Count == 0)
            {
                MessageBox.Show("No posts to show :(");
            }
        }
        private void listBoxAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAlbums.SelectedItems.Count == 1)
            {
                Album selectedAlbum = listBoxAlbums.SelectedItem as Album;
                if (selectedAlbum.Photos.Count != 0)
                {
                    pictureBoxPhoto.LoadAsync(selectedAlbum.Photos[0].PictureNormalURL);
                    m_CurrentPhotoIndexInAlbum = 0;
                    labelPhoto.Visible = true;
                    pictureBoxPhoto.Visible = true;
                    buttonNext.Visible = true;
                    buttonPrev.Visible = true;
                }
                else
                {
                    labelPhoto.Visible = false;
                    pictureBoxPhoto.Visible = false;
                    buttonNext.Visible = false;
                    buttonPrev.Visible = false;
                    MessageBox.Show("No photos to show in the album :(");
                }
            }
        } 
        private void buttonAlbums_Click(object sender, EventArgs e)
        {
            initUserAlbums();
            hideAllPanelsOfUserSelection(panelAlbums);
            if (listBoxAlbums.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
            else
            {
                labelPhoto.Visible = false;
                pictureBoxPhoto.Visible = false;
                buttonNext.Visible = false;
                buttonPrev.Visible = false;
                listBoxAlbums.ClearSelected();
            }
        } 
        private void buttonNext_Click(object sender, EventArgs e)
        {
            Album selectedAlbum;

            selectedAlbum = listBoxAlbums.SelectedItem as Album;
            
            if (m_CurrentPhotoIndexInAlbum != selectedAlbum.Photos.Count - 1)
            {
                m_CurrentPhotoIndexInAlbum++;
                pictureBoxPhoto.LoadAsync(selectedAlbum.
                    Photos[m_CurrentPhotoIndexInAlbum].PictureNormalURL);
            }
            else
            {
                MessageBox.Show("No next photo to show :(");
            }
        }
        private void buttonPrev_Click(object sender, EventArgs e)
        {
            Album selectedAlbum;

            selectedAlbum = listBoxAlbums.SelectedItem as Album;
            if (m_CurrentPhotoIndexInAlbum != 0)
            {
                m_CurrentPhotoIndexInAlbum--;
                pictureBoxPhoto.LoadAsync(selectedAlbum.
                    Photos[m_CurrentPhotoIndexInAlbum].PictureNormalURL);
            }
            else
            {
                MessageBox.Show("No prev photo to show :(");
            }
        }
        private void buttonGuessTheMoment_Click(object sender, EventArgs e)
        {
            Photo randomPhoto = r_GuessTheMomentManager.
                GetRandomPhotoFromRandomAlbum(m_SystemManager.LoggedInUser);
            pictureBoxRandomPhoto.ImageLocation = randomPhoto.PictureNormalURL;
            initCheckListBoxDatesOptions();
            buttonCheckMatch.Enabled = true;
            checkedListBoxDatesOptions.Enabled = true;
            hideAllPanelsOfUserSelection(panelGuessTheMoment);
        }
        private void buttonCheckMatch_Click(object sender, EventArgs e)
        {
            string chosenDate;

            chosenDate  = checkedListBoxDatesOptions.CheckedItems[0] as string;
            if (chosenDate == r_GuessTheMomentManager.CurrentRandomPhotoDate)
            {
                DialogResult dialogResult = MessageBox.
                    Show("You are a champion! Do you want another game?", "Winner",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Photo randomPhoto = r_GuessTheMomentManager.
                        GetRandomPhotoFromRandomAlbum(m_SystemManager.LoggedInUser);
                    pictureBoxRandomPhoto.ImageLocation = randomPhoto.PictureNormalURL;
                    initCheckListBoxDatesOptions();
                }
                else
                {
                    buttonCheckMatch.Enabled = false;
                    checkedListBoxDatesOptions.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Wrong Date Selected! Try again :(");
            }
        }
        private void checkedListBoxDatesOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int index = 0; index < checkedListBoxDatesOptions.Items.Count; index++)
            {
                if (index != e.Index)
                {
                    checkedListBoxDatesOptions.SetItemChecked(index, false);
                }
            }
        }      
        private void buttonPost_Click(object sender, EventArgs e)
        {
            if (textBoxPost.Text != "")
            {
                MessageBox.Show("Post shared successfully!");
                textBoxPost.Clear();
            }
            else
            {
                MessageBox.Show("Oops! It looks like you forgot to write something.!");
            }
        }
        private void comboBoxAlbums_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxAlbums.SelectedItem != null)
            {
                progressBarDownload.Value = 0;
                buttonDownload.Enabled = true;
                Album selectedAlbum = comboBoxAlbums.SelectedItem as Album;
                displayImages(selectedAlbum);
                flowLayoutPanelViewPhotos.Visible = true;
            }
        }
        private void buttonDownloadAlbum_Click(object sender, EventArgs e)
        {
            initAlbumsOptionsForDownload();
            hideAllPanelsOfUserSelection(panelDownloadPhotos);
            if (comboBoxAlbums.Items.Count == 0)
            {
                MessageBox.Show("No albums to retrieve :(");
            }
            else
            {
                flowLayoutPanelViewPhotos.Visible = false;
                progressBarDownload.Value = 0;
                buttonDownload.Enabled = false;
                comboBoxAlbums.SelectedIndex = -1;
                comboBoxAlbums.Text = string.Empty;
            }
        }
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            string saveToPath;

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    saveToPath = dialog.SelectedPath;
                    downloadAlbum(saveToPath);
                    MessageBox.Show("Album download successfully!");
                }
                else
                {
                    MessageBox.Show("An error has occured!");
                }
            }   
        }
    }
}
