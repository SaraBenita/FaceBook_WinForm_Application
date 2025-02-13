using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public class SystemManager
    {
        private LoginResult m_LoginResult;
        private IUserAdapter m_UserCashingProxy;
        internal void LoginAndInit()
        {
            m_LoginResult = FacebookService.Login("3825089201092000",
                "email",
                "public_profile",
                "user_age_range",
                "user_birthday",
                "user_gender",
                "user_hometown",
                "user_likes",
                "user_link",
                "user_location",
                "user_photos",
                "user_posts",
                "user_videos");
            
            if (string.IsNullOrEmpty(m_LoginResult.AccessToken))
            {
                m_LoginResult = null;
                throw new Exception("Login Fail");
            }

            if (string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
            {
                m_UserCashingProxy = new UserCashingProxy(m_LoginResult.LoggedInUser);
            }
        }
        internal void ClearLoginResutlForLogout()
        {
            m_LoginResult = null;
        }
        internal FacebookObjectCollection<Group> GetGroupsList()
        {
            return m_UserCashingProxy.GroupsLoded;
        }
        internal FacebookObjectCollection<Post> GetPostsList()
        {
            return m_UserCashingProxy.PostsLoded;
        }
        internal FacebookObjectCollection<Album> GetAlbumsList()
        {
            return m_UserCashingProxy.AlbumsLoded;
        }
        internal FacebookObjectCollection<Page> GetPagesList()
        {
            return m_UserCashingProxy.PagesLoded;
        }
        internal string GetUserName()
        {
            return m_UserCashingProxy.UserName;
        }
        internal string GetProfilePicture()
        {
            return m_UserCashingProxy.ProfilePictureUrl;
        }
        internal string GetUserEmail()
        {
            return m_UserCashingProxy.Email;
        }
        internal string GetUserBirthday()
        {
            return m_UserCashingProxy.Birthday;
        }
        internal string GetUserGender()
        {
            return m_UserCashingProxy.Gender.ToString();
        }
    }
}
