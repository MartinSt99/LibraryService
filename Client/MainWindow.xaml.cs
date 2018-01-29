using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Data;



namespace Client
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceReference1.IAktienInfoCallback
    {
        public string user;
        public ServiceReference1.AktienInfoClient proxy;
        public InstanceContext context;
        private readonly Kundenservice.OrderBooks x = new Kundenservice.OrderBooks();
        private List<Kundenservice.Book> bl = new List<Kundenservice.Book>();
        
        public MainWindow()
        {
            Dispatcher.Invoke(() =>
            {
                InitializeComponent();
                context = new InstanceContext(this);
                proxy = new ServiceReference1.AktienInfoClient(context);
                //proxy.connect("test");
            });
         
            

        }

        public void loginUser(int stat)
        {
            
        }

        public void loadBooks(DataSet ds)
        {
            Dispatcher.Invoke((Action)delegate
            {
                BookManagement usrmgmt = new BookManagement();
                usrmgmt.books = ds;

                //usrmgmt.users
                usrmgmt.Show();
                usrmgmt.getBooks(ds);

            });
        }

        public void UpdateUsers(DataSet ds)
        {
            Dispatcher.Invoke((Action)delegate
            {
                userManagement usrmgmt = new userManagement();
                usrmgmt.users = ds;

                //usrmgmt.users
                usrmgmt.Show();
                usrmgmt.UpdateUsers(ds);

            });

            
        }

        public void BookUpdate(string ticker, double preis, DataSet ds)
        {

           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var temp = new Kundenservice.Book(aText.Text, tText.Text, pdate.SelectedDate.Value, Guid.NewGuid());

            x.addBook(temp);
            var z = x.getwishList();

            bGrid.ItemsSource = z;
            bGrid.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var temp = x.getBook(new Guid(gText.Text));
            bGrid.ItemsSource = null;
            var templist = new List<Kundenservice.Book>();
            templist.Add(temp);
            bGrid.ItemsSource = templist;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempbook = (Kundenservice.Book)bGrid.SelectedItem;
                bGrid.ItemsSource = null;
                x.delBook(new Guid(tempbook.ID.ToString()));
                bGrid.ItemsSource = x.getwishList();
            }
            catch (Exception exception)
            {
            }
          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            bGrid.ItemsSource = x.getwishList();
        }

        
        private void getPrice_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_Click_4(object sender, RoutedEventArgs e)
        {
            proxy.getUsers("me");
        }

        private void btnLoadBooks_Click(object sender, RoutedEventArgs e)
        {
            proxy.getBooks("me");

        }
    }
}