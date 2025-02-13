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
        public User LoggedInUser { get; set; }
        private LoginResult m_LoginResult; 
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
                LoggedInUser = m_LoginResult.LoggedInUser;
            }
        }
        internal void ClearLoginResutlForLogout()
        {
            m_LoginResult = null;
        }
    }
}
