using BasicFacebookFeatures.SortStrategy;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    internal sealed class DownloadAlbumManager
    {
        public Album m_AlbumToDownload { get; set; }
        private static DownloadAlbumManager s_DownloadAlbumInstance = null;
        private static readonly object sr_DoubleCheckLock = new object();
        private ISortPhotosStrategy m_SortPhotosStrategy { get; set; }

        public static DownloadAlbumManager DownloadAlbumInstance
        {
            get
            {
                if (s_DownloadAlbumInstance == null)
                {
                    lock (sr_DoubleCheckLock)
                    {
                        if (s_DownloadAlbumInstance == null)
                        {
                            s_DownloadAlbumInstance = new DownloadAlbumManager();
                        }
                    }
                }

                return s_DownloadAlbumInstance;
            }
        }
        internal void DownloadAlbum(ProgressBar i_ProgressBarDownload, string i_SaveAlbumPath)
        {
            int numberPhotosInAlbum = m_AlbumToDownload.Photos.Count;
            string albumPath = Path.Combine(i_SaveAlbumPath, m_AlbumToDownload.Name);
            Directory.CreateDirectory(albumPath);
            i_ProgressBarDownload.Maximum = numberPhotosInAlbum;
            List<Photo> newAlbum = m_SortPhotosStrategy.Sort(m_AlbumToDownload);
            foreach (Photo photo in newAlbum)
            {
                string fileName = $"{i_ProgressBarDownload.Value + 1}_{photo.Name}.jpg";
                try
                {
                    downloadPhoto(photo.PictureNormalURL, Path.Combine(albumPath, fileName));
                }
                catch (Exception ignored)
                {
                    MessageBox.Show(ignored.Message);
                }

                i_ProgressBarDownload.Value++;
            }
        }
        private void downloadPhoto(string i_PhotoPath, string i_PhotoName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(i_PhotoPath, i_PhotoName);
            }
        }
        internal static void CreateStrategy(eStrategyOptions i_StrategyOption)
        {
            ISortPhotosStrategy sortPhotosStrategy = null;

            switch (i_StrategyOption)
            {
                case eStrategyOptions.SortByCommentsAmountAscending:
                    sortPhotosStrategy = new SortByCommentsAmountAscending();
                    break;
                case eStrategyOptions.SortByCommentsAmountDescending:
                    sortPhotosStrategy = new SortByCommentsAmountDescending();
                    break;
                case eStrategyOptions.SortByCreationTimeAscending:
                    sortPhotosStrategy = new SortByCreationTimeAscending();
                    break;
                case eStrategyOptions.SortByCreationTimeDescending:
                    sortPhotosStrategy = new SortByCreationTimeDescending();
                    break;
            }

            s_DownloadAlbumInstance.m_SortPhotosStrategy = sortPhotosStrategy;
        }
    }
}
