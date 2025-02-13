using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal sealed class GuessTheMomentManager
    {
        private Photo m_CurrentRandomPhoto;
        private static GuessTheMomentManager s_GuessTheMomentInstance = null;
        private static readonly object sr_DoubleCheckLock = new object();
        private readonly Random r_Random = new Random();

        private GuessTheMomentManager(){}      
        public static GuessTheMomentManager GuessTheMomentInstance
        {
            get
            {
                if (s_GuessTheMomentInstance == null)
                {
                    lock (sr_DoubleCheckLock)
                    {
                        if (s_GuessTheMomentInstance == null)
                        {
                            s_GuessTheMomentInstance = new GuessTheMomentManager();
                        }
                    }
                }

                return s_GuessTheMomentInstance;
            }
        }
        internal bool IsGuessDateRight(string i_DateGuess)
        {
            return i_DateGuess == getCurrentRandomPhotoDate();
        }
        private string getCurrentRandomPhotoDate()
        {
            string[] photoDateTime;

            photoDateTime = m_CurrentRandomPhoto.CreatedTime.ToString().Split(' ');
            return photoDateTime[0];
        }
        internal string GetCurrentRandomPhotoUrl()
        {
           return m_CurrentRandomPhoto.PictureNormalURL;
        }
        internal void GetRandomPhotoFromRandomAlbum(FacebookObjectCollection<Album> i_AlbumsList)
        {           
            int randomAlbumIndex, randomPhotoIndex;

            do
            {
                randomAlbumIndex = r_Random.Next(i_AlbumsList.Count);
            } while (i_AlbumsList[randomAlbumIndex].Count == 0);

            randomPhotoIndex = r_Random.Next(i_AlbumsList[randomAlbumIndex].Photos.Count);
            m_CurrentRandomPhoto = i_AlbumsList[randomAlbumIndex].Photos[randomPhotoIndex];          
        }
        internal List<string> InitializeGuessDatesList()
        {
            DateTime startDate = new DateTime(2010, 1, 1);
            DateTime endDate = DateTime.Today;
            int daysDifference = (endDate - startDate).Days;
            List<string> datesList = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                int randomDays = r_Random.Next(0, daysDifference);
                string randomDate = startDate.AddDays(randomDays).ToString("dd/MM/yyyy");
                datesList.Add(randomDate);
            }

            datesList.Add(getCurrentRandomPhotoDate());
            datesList = datesList.OrderBy(x => r_Random.Next()).ToList();

            return datesList;
        }
    }
}
