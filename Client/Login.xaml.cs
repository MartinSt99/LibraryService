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

        public void loginUser(Kundenservice.AktienInfo.ServiceData status, Kundenservice.AktienInfo.ReturnedBooks rb)
        {
            Dispatcher.Invoke(delegate
            {

                if (status.Result)
                {
                    lbxLog.Items.Add(status.ErrorMessage + ":");
                    lbxLog.Items.Add(status.ErrorDetails);
                    lbxLog.Items.Add("Successfully logged in.. waiting for redirect...");
                    if (rb.hasReturned)
                    {
                        MessageBox.Show("Bücher wurden zurückgebracht, bei interesse beachten Sie ihre Wunschliste.");
                        MessageBox.Show("Bücher: " + rb.Books);
                    }
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        Dispatcher.Invoke(() =>
                        {
                            var main = new MainWindow();
                            main.user = txtUser.Text;
                            main.checkAdmin();
                            main.Show();
                            Close();
                        });
                    });
                }
                else if (!status.Result)
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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Kundenservice.AktienInfo.ServiceData x = await proxy.getLoginAsync(txtUser.Text, txtPwd.Text);

            if (x.Result)
            {
                lbxLog.Items.Add("Successfully logged in.. waiting for redirect...");
            }

            lbxLog.Items.Add("Sent credentials to server, please be pacient...");
        
            
        }
    }
}