using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;

namespace CardChargesServerDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string strError = "";
            try
            {
                StartRemoteServer();
            }
            catch (Exception ex)
            {
                strError = "启动 Remoting 发生错误:" + ex.Message;
                MessageBox.Show(this, strError);
                return;
            }
        }

        IChannel m_serverChannel = null;
        void StartRemoteServer()
        {
            m_serverChannel = new IpcServerChannel("CardCenterChannel");
            RemotingConfiguration.ApplicationName = "CardCenterServer";

            //Register the server channel.
            ChannelServices.RegisterChannel(m_serverChannel, false);

            //Register this service type.
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(CardCenterServer),
                "CardCenterServer",
                WellKnownObjectMode.Singleton);
        }
    }
}
