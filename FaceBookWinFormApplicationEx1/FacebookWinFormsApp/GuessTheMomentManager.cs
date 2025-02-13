using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class GuessTheMomentManager
    {
        public string CurrentRandomPhotoDate { get; set; }
        private readonly Random r_Random;
        public GuessTheMomentManager()
        {
            r_Random = new Random();
        }
        internal Photo GetRandomPhotoFromRandomAlbum(User i_LoggedInUser)
        {
            Photo RandomPhoto;
            string[] photoDateTime;
            int randomAlbumIndex, randomPhotoIndex;

            do
            {
                randomAlbumIndex = r_Random.Next(i_LoggedInUser.Albums.Count);
            } while (i_LoggedInUser.Albums[randomAlbumIndex].Count == 0);

            randomPhotoIndex = r_Random.Next(i_LoggedInUser.Albums[randomAlbumIndex].Photos.Count);         
            photoDateTime = i_LoggedInUser
                .Albums[randomAlbumIndex].Photos[randomPhotoIndex].CreatedTime.ToString().Split(' ');
            CurrentRandomPhotoDate = photoDateTime[0];
            RandomPhoto = i_LoggedInUser.Albums[randomAlbumIndex].Photos[randomPhotoIndex];

            return RandomPhoto;
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

            datesList.Add(CurrentRandomPhotoDate);
            datesList = datesList.OrderBy(x => r_Random.Next()).ToList();

            return datesList;
        }
    }
}
