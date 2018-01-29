using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window, ServiceReference1.IAktienInfoCallback
    {
        public ServiceReference1.AktienInfoClient proxy;
        public InstanceContext context;
        public Login()
        {
            InitializeComponent();
            context = new InstanceContext(this);
            proxy = new ServiceReference1.AktienInfoClient(context);
            lbxLog.Items.Add("Initialized System..");
            lbxLog.Items.Add("----------------------------------");


        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
          proxy.getLogin(txtUser.Text, txtPwd.Text);
            lbxLog.Items.Add("Sent credentials to server, please be pacient...");
        }

        public void loginUser(int status)
        {

            Dispatcher.Invoke((Action) delegate
            {
                if (status == 1)
                {
                    lbxLog.Items.Add("Successfully logged in.. waiting for redirect...");
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        Dispatcher.Invoke(() =>
                        {
MainWindow main = new MainWindow();
                        main.user = txtUser.Text;
                        main.Show();
                        this.Hide();
                        });
                        
                    });
                   
                }
                else if (status == 0)
                {
                    lbxLog.Items.Add("login not successfull.. waiting for retry...");
                    lbxLog.Items.Add("----------------------------------");
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                    });

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
    }
}
