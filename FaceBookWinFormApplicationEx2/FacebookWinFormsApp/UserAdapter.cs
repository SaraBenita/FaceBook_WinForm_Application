using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class UserAdapter : IUserAdapter
    {
        private User m_LoggedInUser;
        internal UserAdapter(User i_LoggedInUser)
        {
            m_LoggedInUser = i_LoggedInUser;
        }
        public FacebookObjectCollection<Group> GroupsLoded => m_LoggedInUser.Groups;

        public FacebookObjectCollection<Post> PostsLoded => m_LoggedInUser.Posts;

        public FacebookObjectCollection<Album> AlbumsLoded => m_LoggedInUser.Albums;

        public FacebookObjectCollection<Page> PagesLoded => m_LoggedInUser.LikedPages;

        public string UserName => m_LoggedInUser.Name;

        public string ProfilePictureUrl => m_LoggedInUser.PictureNormalURL;

        public string Email => m_LoggedInUser.Email;

        public string Birthday => m_LoggedInUser.Birthday;

        public string Gender => m_LoggedInUser.Gender.ToString();
        internal DateTime? GetUpdateTime()
        {
           return m_LoggedInUser.UpdateTime;
        }
    }
}
