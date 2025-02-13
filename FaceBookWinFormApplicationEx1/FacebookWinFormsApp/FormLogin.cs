using System;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormLogin : Form
    {
        private SystemManager m_SystemManager;
        public FormLogin()
        {
            InitializeComponent();
            m_SystemManager = new SystemManager();
            FacebookService.s_CollectionLimit = 150;
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                m_SystemManager.LoginAndInit();
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
            FormMain formMain = new FormMain(m_SystemManager);
            formMain.Show();
        }
    }
}
