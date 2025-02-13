using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    internal static class FormFactory
    {
        public enum eFormType
        {
            FormLogin,
            FormMain
        }
        public static Form createFormBasedOnType(eFormType i_FormType)
        {
            Form newForm = null;

            switch (i_FormType)
            {
                case eFormType.FormLogin:
                    newForm = new FormLogin();
                    break;
                case eFormType.FormMain:
                    FormLogin loginForm = Application.OpenForms.
                    OfType<FormLogin>().FirstOrDefault();
                    if(loginForm!=null)
                    {
                        newForm = new FormMain(loginForm.SystemManager);
                    }
                    break;
            }

            return newForm;
        }
    }
}
