using System;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormLogin : Form, IFacebookObserver
    {
        private readonly SystemManager r_SystemManager;

        public FormLogin()
        {
            InitializeComponent();
            r_SystemManager = new SystemManager();
            r_SystemManager.AddObserver(this);
            FacebookService.s_CollectionLimit = 150;

        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                r_SystemManager.LoginAndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void initialMainForm()
        {
            this.Hide();
            Form formMain = FormFactory.createFormBasedOnType(FormFactory.eFormType.FormMain);
            formMain.Show();
        }
        public void UpdateLoginStatus(bool i_IsLogin)
        {
            if(i_IsLogin)
            {
                initialMainForm();
            }
        }
        internal SystemManager SystemManager
        {
            get
            {
                return r_SystemManager;
            }
        }
    }
}
