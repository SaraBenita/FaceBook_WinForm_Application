using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class UserCashingProxy 
    {
        private User m_LoggedInUser;
        private DateTime m_TimeAlbumsLoaded = new DateTime();
        private FacebookObjectCollection<Album> m_AlbumsLoded;
        private DateTime m_TimeGroupsLoaded = new DateTime();
        private FacebookObjectCollection<Group> m_GroupsLoded;
        private DateTime m_TimePagesLoaded = new DateTime();
        private FacebookObjectCollection<Page> m_PagesLoded;
        private DateTime m_TimePostsLoaded = new DateTime();
        private FacebookObjectCollection<Post> m_PostsLoded;
        private DateTime m_TimeUserProfileInfoLoaded = new DateTime();
        private string m_UserName;
        private string m_Email;
        private string m_Gender;
        private string m_ProfilePictureUrl;
        private string m_Birthday;

        internal UserCashingProxy(User i_LoggedInUser) 
        {
            m_LoggedInUser = i_LoggedInUser;
        }      
        private void updateCacheUserProfileInfo()
        {
            m_UserName = m_LoggedInUser.Name;
            m_Email = m_LoggedInUser.Email;
            m_Gender = m_LoggedInUser.Gender.ToString();
            m_ProfilePictureUrl = m_LoggedInUser.PictureNormalURL;
            m_Birthday = m_LoggedInUser.Birthday;
        }
        internal FacebookObjectCollection<Group> GroupsLoded
        {
            get
            {
                if(m_LoggedInUser.UpdateTime >= m_TimeGroupsLoaded)
                {
                    m_GroupsLoded = m_LoggedInUser.Groups;
                    m_TimeGroupsLoaded = DateTime.Now;
                }

                return m_GroupsLoded;
            }
        }
        internal FacebookObjectCollection<Post> PostsLoded
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimePostsLoaded)
                {
                    m_PostsLoded = m_LoggedInUser.Posts;
                    m_TimePostsLoaded = DateTime.Now;
                }

                return m_PostsLoded;
            }
        }
        internal FacebookObjectCollection<Album> AlbumsLoded
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeAlbumsLoaded)
                {
                    m_AlbumsLoded = m_LoggedInUser.Albums;
                    m_TimeAlbumsLoaded = DateTime.Now;
                }

                return m_AlbumsLoded;
            }
        }
        internal FacebookObjectCollection<Page> PagesLoded
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimePagesLoaded)
                {
                    m_PagesLoded = m_LoggedInUser.LikedPages;
                    m_TimePagesLoaded = DateTime.Now;
                }

                return m_PagesLoded;
            }        
        }
        internal string UserName
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeUserProfileInfoLoaded)
                {
                    updateCacheUserProfileInfo();
                    m_TimeUserProfileInfoLoaded = DateTime.Now;
                }

                return m_UserName;
            }
        }
        internal string ProfilePictureUrl
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeUserProfileInfoLoaded)
                {
                    updateCacheUserProfileInfo();
                    m_TimeUserProfileInfoLoaded = DateTime.Now;
                }

                return m_ProfilePictureUrl;
            }      
        }
        internal string Email
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeUserProfileInfoLoaded)
                {
                    updateCacheUserProfileInfo();
                    m_TimeUserProfileInfoLoaded = DateTime.Now;
                }

                return m_Email;
            }
        }
        internal string Birthday
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeUserProfileInfoLoaded)
                {
                    updateCacheUserProfileInfo();
                    m_TimeUserProfileInfoLoaded = DateTime.Now;
                }

                return m_Birthday;
            }
        }
        internal string Gender
        {
            get
            {
                if (m_LoggedInUser.UpdateTime >= m_TimeUserProfileInfoLoaded)
                {
                    updateCacheUserProfileInfo();
                    m_TimeUserProfileInfoLoaded = DateTime.Now;
                }

                return m_Gender;
            }
        }
    }
}
