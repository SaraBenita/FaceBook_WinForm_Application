using System;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormLogin : Form
    {
        private readonly SystemManager r_SystemManager;

        public FormLogin()
        {
            InitializeComponent();
            r_SystemManager = new SystemManager();
            FacebookService.s_CollectionLimit = 150;
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                r_SystemManager.LoginAndInit();
                initialMainForm();
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
        internal SystemManager SystemManager
        {
            get
            {
                return r_SystemManager;
            }
        }
    }
}
