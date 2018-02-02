using System;
using System.Data;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Client.ServiceReference1;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window, IAktienInfoCallback
    {
        public InstanceContext context;
        public AktienInfoClient proxy;

        public Login()
        {
            InitializeComponent();
            context = new InstanceContext(this);
            proxy = new AktienInfoClient(context);
            lbxLog.Items.Add("Initialized System..");
            lbxLog.Items.Add("----------------------------------");
           
        
            
        }

        public void loginUser(int status)
        {
            Dispatcher.Invoke(delegate
            {
                if (status == 1)
                {
                    lbxLog.Items.Add("Successfully logged in.. waiting for redirect...");
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        Dispatcher.Invoke(() =>
                        {
                            var main = new MainWindow();
                            main.user = txtUser.Text;
                            main.Show();
                            Close();
                        });
                    });
                }
                else if (status == 0)
                {
                    lbxLog.Items.Add("login not successfull.. waiting for retry...");
                    lbxLog.Items.Add("----------------------------------");
                    Task.Run(() => { Thread.Sleep(2000); });

                    txtUser.IsEnabled = true;
                    txtPwd.IsEnabled = true;
                    btnLogin.IsEnabled = true;
                }
            });
        }

        public void UpdateUsers(DataSet ds)
        {
        }

        public void BookUpdate(string ticker, double preis, DataSet ds)
        {
        }

        public void loadBooks(DataSet ds)
        {
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            proxy.getLogin(txtUser.Text, txtPwd.Text);
            lbxLog.Items.Add("Sent credentials to server, please be pacient...");
        
            
        }
    }
}