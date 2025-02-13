using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private readonly SystemManager r_SystemManager;
        private int m_CurrentPhotoIndexInAlbum;

        public FormMain(SystemManager i_SystemManager)
        {
            InitializeComponent();
            r_SystemManager = i_SystemManager;
        }
        private void FormMain_Shown(object sender, EventArgs e)
        {
            new Thread(initAllUserInformation).Start();
        }
        private void initAllUserInformation()
        {
            string profilePictureUrl, userName;

            profilePictureUrl = r_SystemManager.GetProfilePicture();
            userName = r_SystemManager.GetUserName();
            Invoke(new Action(() =>
            {
                pictureBoxUserProfile.ImageLocation = profilePictureUrl;
                labelUserName.Text = userName;
            }));
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
            string name, gender, birthday, email;

            name = r_SystemManager.GetUserName();
            gender = r_SystemManager.GetUserGender();
            birthday = r_SystemManager.GetUserBirthday();
            email = r_SystemManager.GetUserEmail();
            Invoke(new Action(() =>
            {
                labelName.Text = name;
                labelGender.Text = gender;
                LabelBirthday.Text = birthday;
                labelMail.Text = email;
            }));
        }
        private void initUserPages()
        {
            FacebookObjectCollection<Page> pages = r_SystemManager.GetPagesList();
            Invoke(new Action(() =>
            {
                pageBindingSource.DataSource = pages;
                if (listBoxPages.Items.Count == 0)
                {
                    MessageBox.Show("No liked pages to retrieve :(");
                }
            }));
           
        }
        private void initUserGroups()
        {
            FacebookObjectCollection<Group> groups;

            try
            {
                groups = r_SystemManager.GetGroupsList();
                Invoke(new Action(() =>
                {
                    listBoxGroups.Items.Clear();
                    listBoxGroups.DisplayMember = "Name";
                    foreach (Group group in groups)
                    {
                        listBoxGroups.Items.Add(group);
                    }
                }));               
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show(ex.Message)));
            }

            validateEmptyUserGroups();
        }
        private void validateEmptyUserGroups()
        {
            Invoke(new Action(() =>
            {
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
            }));
        }
        private void initUserPosts()
        {
            FacebookObjectCollection<Post> posts;

            try
            {
                posts = r_SystemManager.GetPostsList();
                Invoke(new Action(() =>
                {
                    listBoxPosts.Items.Clear();
                    foreach (Post post in posts)
                    {
                        string postMessage = post.Message;
                        if (postMessage != null)
                        {
                            listBoxPosts.Items.Add(
                                $"{post.CreatedTime?.ToString("MM/dd/yyyy HH:mm")} : {postMessage}");
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show(ex.Message)));
            }

            validateEmptyUserPosts();
        }
        private void validateEmptyUserPosts()
        {
            Invoke(new Action(() =>
            {
                if (listBoxPosts.Items.Count == 0)
                {
                    MessageBox.Show("No posts to show :(");
                }
            }));
        }
        private void initUserAlbums()
        {
            FacebookObjectCollection<Album> albums;
           
            try
            {
                albums = r_SystemManager.GetAlbumsList();
                Invoke(new Action(() =>
                {
                    listBoxAlbums.Items.Clear();
                    listBoxAlbums.DisplayMember = "Name";
                    foreach (Album album in albums)
                    {
                        listBoxAlbums.Items.Add(album);
                    }
                }));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show(ex.Message)));
            }
        }
        private void initCheckListBoxDatesOptions()
        {
            List<string> datesList;

            datesList = GuessTheMomentManager.GuessTheMomentInstance.InitializeGuessDatesList();
            Invoke(new Action(() =>
            {
                checkedListBoxDatesOptions.Items.Clear();
                foreach (string date in datesList)
                {
                    checkedListBoxDatesOptions.Items.Add(date);
                }
            }));
        }
        private void initAlbumsOptionsForDownload()
        {
            FacebookObjectCollection<Album> albums;

            try
            {
                albums = r_SystemManager.GetAlbumsList();
                Invoke(new Action(() =>
                {
                    comboBoxAlbums.Items.Clear();
                    comboBoxAlbums.DisplayMember = "Name";
                    foreach (Album album in albums)
                    {
                        if (album.Photos.Count != 0)
                        {
                            comboBoxAlbums.Items.Add(album);
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show(ex.Message)));
            }

            validateEmptyUserAlbums();
        }
        private void validateEmptyUserAlbums()
        {
            Invoke(new Action(() =>
            {
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
            }));  
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
            int numberPhotosInAlbum = chosenAlbum.Photos.Count;
            string albumPath = Path.Combine(i_SaveAlbumPath, chosenAlbum.Name);
            Directory.CreateDirectory(albumPath);
            progressBarDownload.Maximum = numberPhotosInAlbum;
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
                PictureBox pictureBox = new PictureBox 
                {
                    ImageLocation = photo.PictureNormalURL,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 100),
                    Margin = new Padding(5)
                };
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
                r_SystemManager.ClearLoginResutlForLogout();
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
            hideAllPanelsOfUserSelection(panelPersonalInfo);
            new Thread(initPersonalInfo).Start();
        }
        private void buttonPages_Click(object sender, EventArgs e)
        {
            hideAllPanelsOfUserSelection(panelPages);
            new Thread(initUserPages).Start();
        }
        private void buttonGroups_Click(object sender, EventArgs e)
        {
            hideAllPanelsOfUserSelection(panelGroups);
            new Thread(initUserGroups).Start();
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
            hideAllPanelsOfUserSelection(panelPosts);
            new Thread(initUserPosts).Start();
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
            new Thread(() =>
            {
                GuessTheMomentManager.GuessTheMomentInstance.
                       GetRandomPhotoFromRandomAlbum(r_SystemManager.GetAlbumsList());
                initCheckListBoxDatesOptions();
                Invoke(new Action(() =>
                {
                    pictureBoxRandomPhoto.ImageLocation = GuessTheMomentManager.GuessTheMomentInstance
                    .GetCurrentRandomPhotoUrl();
                    buttonCheckMatch.Enabled = true;
                    checkedListBoxDatesOptions.Enabled = true;
                    hideAllPanelsOfUserSelection(panelGuessTheMoment);
                }));
            }).Start();   
        }
        private void buttonCheckMatch_Click(object sender, EventArgs e)
        {
            string chosenDate;

            chosenDate  = checkedListBoxDatesOptions.CheckedItems[0] as string;
            if (GuessTheMomentManager.GuessTheMomentInstance.IsGuessDateRight(chosenDate))
            {
                DialogResult dialogResult = MessageBox.
                    Show("You are a champion! Do you want another game?", "Winner",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    new Thread(() =>
                    {
                        GuessTheMomentManager.GuessTheMomentInstance.
                       GetRandomPhotoFromRandomAlbum(r_SystemManager.GetAlbumsList());
                        initCheckListBoxDatesOptions();
                        pictureBoxRandomPhoto.Invoke(new Action(() =>
                        {
                            pictureBoxRandomPhoto.ImageLocation = GuessTheMomentManager.GuessTheMomentInstance
                            .GetCurrentRandomPhotoUrl();
                        }));
                    }).Start(); 
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
            hideAllPanelsOfUserSelection(panelDownloadPhotos);
            new Thread(initAlbumsOptionsForDownload).Start();
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
