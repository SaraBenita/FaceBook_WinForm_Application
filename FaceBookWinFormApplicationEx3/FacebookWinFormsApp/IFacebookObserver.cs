using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal interface IFacebookObserver
    {
        void UpdateLoginStatus(bool i_isLogin);
    }
}
