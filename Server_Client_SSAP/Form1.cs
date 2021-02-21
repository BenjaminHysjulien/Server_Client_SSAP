using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.NetworkInformation;
using System.Threading;
using Server_Client_SSAP.ServiceReference1;
using Server_Client_SSAP.ServiceReference2;

namespace Server_Client_SSAP
{
    public partial class Form1 : Form
    {
        List<string> listBox_Profiles = new List<string>();
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timer2;
        bool is_alive = false;
        bool IpFlag = true;
        private bool timer2Flag = false;

        //Form1 Interactive Objecst 
        string IPAddress;
        String userName;
        string password;
        bool play_again = false;
        UriBuilder local_uri = new UriBuilder();

        private bool URL_Only_Flag = true;

        //List for metadata. Will be needed for added functionality. 
        List<string> MetaData_List;

        //Timer 
        System.Timers.Timer UpdateTime = new System.Timers.Timer(1000);

        //Create Media Profiles through service references 
        ServiceReference1.Media2Client media_client;
        ServiceReference1.MediaProfile[] profiles;
        DeviceClient Device_Client;
        double DeviceTimeOffset = 0;

        //Custom URI variable 
        UriBuilder deviceUri;
        private bool check_flag2;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            record();
        }

        private System.DateTime GetDeviceTime(DeviceClient client)
        {
            // Should compare recieved timestamp with local machine.  If out of sync, authentication may fail
            SystemDateTime dt = client.GetSystemDateAndTime();
            System.DateTime deviceTime = new System.DateTime(dt.UTCDateTime.Date.Year, dt.UTCDateTime.Date.Month, dt.UTCDateTime.Date.Day, dt.UTCDateTime.Time.Hour, dt.UTCDateTime.Time.Minute, dt.UTCDateTime.Time.Second);
            double offset = (deviceTime - System.DateTime.UtcNow).TotalSeconds;
            return deviceTime;
        }
        private void SetupSystem()
        {
            try
            {

                System.ServiceModel.Channels.CustomBinding binding;
                Device_Client = CustomBindingsAndClients.GetClient(IPAddress, 80, 0, out binding, out deviceUri, userName, password);
                Device.Service[] service = Device_Client.GetServices(false);
                //Check if they contain media and that we have made contact TODO wrap in a try catch block
                Device.Service checkk_media = service.FirstOrDefault(s => s.Namespace == "");
                if (checkk_media != null)
                {
                    media_client = new Media2Client(binding, new EndpointAddress(deviceUri.ToString()));
                    media_client.ClientCredentials.HttpDigest.ClientCredential.UserName = userName;
                    media_client.ClientCredentials.HttpDigest.ClientCredential.Password = password;
                    media_client.ClientCredentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

                    //Get camera Profiles. 
                    try
                    {
                        profiles = media_client.GetProfiles(null, null);
                    }
                    catch
                    {
                        //Do something 
                    }
                    if (profiles != null)
                    {
                        foreach (var p in profiles)
                        {
                            listBox_Profiles.Add(p.Name);
                            //Confirmed: Profiles listed in box are a match to profiles setup on camera. 
                        }
                    }
                }
            }
            catch
            {
                string message = "You did not enter a valid IP address, username, or password?";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            }
        }
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
        public void Start_Timer(object sender, EventArgs e)
        {
            IPAddress = txtbox_IPAddress.Text;
            is_alive = PingHost(IPAddress);
            if (!is_alive)
            {
                IpFlag = false;
            }
            if (IpFlag == false && is_alive == true && timer2Flag == false)
            {
                timer2 = new System.Windows.Forms.Timer();
                timer2.Tick += new EventHandler(Start_Timer2);
                timer2.Interval = 18000;
                timer2.Start();
                timer2Flag = true;
            }
            if (IpFlag == true && is_alive == true && timer2 != null)
            {
                timer2.Dispose();
            }

        }
        private void StopVideo_Click(object sender, EventArgs e)
        {
            //Stop Video Play
            //Clear Meta data
            //Clear URI Info Box. 
            TxtBox_URI_Info.Text = "";
            if (false)
            {
                timer.Dispose();
                timer2.Dispose();
            }
        }
        public void Timer_Setup()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Start_Timer);
            timer.Interval = 1000;
            timer.Start();
        }

        private void Start_Timer2(object sender, EventArgs e)
        {
            SetupSystem();
            IpFlag = true;
            timer2Flag = false;
        }

        private void check_flag()
        {
            if (!is_alive)
            {
                check_flag2 = true;
            }
        }
        private void record()
        {
            if (profiles != null || URL_Only_Flag == true)
            {

                if (txtbox_IPAddress.Text == "192.168.1.65") //First unit 
                {

                    if (play_again == false)
                    {
                        Timer_Setup();
                        //ToDO: Build list box to allow user to switch between profiles. Just past main profile for now. 
                        local_uri.Host = "192.168.1.65";
                        local_uri.Port = 554; //Change to port 80 for modbus 
                        TxtBox_URI_Info.Text = local_uri.ToString();
                        local_uri.Password = "Update";
                        local_uri.UserName = "Update";

                        //bool check = true;
                        //_mp is of type LibVlcSharp.Shared.MediaPlayer
                    }
                }
                else if (txtbox_IPAddress.Text == "192.168.1.64") //Second Unit 
                {

                    if (play_again == false)
                    {
                        Timer_Setup();
                        local_uri = new UriBuilder("http://192.168.1.65");
                        //ToDO: Build list box to allow user to switch between profiles. Just past main profile for now. 
                        local_uri.Port = 554; //Change to port 80 for modbus 
                        TxtBox_URI_Info.Text = local_uri.ToString();
                        local_uri.Password = "Update";
                        local_uri.UserName = "Update";
                        play_again = true;
                    }
                }

                //List full URI info. 


                //Past it to VideoView and start playing video. 
                //  bool check = Uri.IsWellFormedUriString(local_uri.Uri.ToString(), UriKind.RelativeOrAbsolute);
                //bool check = true;

                if (false)
                {
                    //DO SOMETHING 

                    if (URL_Only_Flag == false)
                    {
                        //check = false;
                        System.Threading.Thread.Sleep(1000);
                        if (false)
                        {
                            UpdateInfoBox();
                        }
                    }
                }
                else
                {
                    //TODO Put in popup that informs them that the URI was not formattd correctly. 
                    //check = false;
                }
            }
            else
            {
                //TO DO: Add popup to indicate a selection was not made on the user profile list. (This should be changed to a try catch block) 
                //Check if there are valid profiles. 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress = txtbox_IPAddress.Text;
            userName = TxtBox_UserName.Text;
            password = TxtBox_Password.Text;
            if (URL_Only_Flag == false)
            {
                SetupSystem();
            }
            else
            {
                System.Threading.Thread.Sleep(5000);
                //DO SOMETHING 
            }
        }
        private void UpdateTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!lblTimeLocal.IsDisposed)
                Invoke((Action)(() => lblTimeLocal.Text = string.Format("Local Time: {0:s}", System.DateTime.Now)));
            if (!lblTimeUtc.IsDisposed)
                Invoke((Action)(() => lblTimeUtc.Text = string.Format("UTC Time:  {0:s}", System.DateTime.UtcNow)));
        }

    }
}