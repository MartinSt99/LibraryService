using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Windows;
using Client.ServiceReference1;
using Kundenservice;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAktienInfoCallback
    {
        private readonly OrderBooks x = new OrderBooks();
        private List<Book> bl = new List<Book>();
        public InstanceContext context;
        public AktienInfoClient proxy;
        public string user;

        public MainWindow()
        {
            Dispatcher.Invoke(() =>
            {
                InitializeComponent();
                context = new InstanceContext(this);
                proxy = new AktienInfoClient(context);
            });
        }

        public void loginUser(int stat)
        {
        }

        public void loadBooks(DataSet ds)
        {
            Dispatcher.Invoke(delegate
            {
                var usrmgmt = new BookManagement {books = ds};
                usrmgmt.Show();
                usrmgmt.loadBooks(ds);
            });
        }

        public void UpdateUsers(DataSet ds)
        {
            Dispatcher.Invoke(delegate
            {
                var usrmgmt = new userManagement {users = ds};
                usrmgmt.Show();
                usrmgmt.UpdateUsers(ds);
            });
        }

        public void BookUpdate(string ticker, double preis, DataSet ds)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var temp = new Book(aText.Text, tText.Text, pdate.SelectedDate.Value, "x");

            x.addBook(temp);
            var z = x.getwishList();

            bGrid.ItemsSource = z;
            bGrid.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var temp = x.getBook(new Guid(gText.Text));
            bGrid.ItemsSource = null;
            var templist = new List<Book>();
            templist.Add(temp);
            bGrid.ItemsSource = templist;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
            //    var tempbook = (Book) bGrid.SelectedItem;
            //    bGrid.ItemsSource = null;
            //    x.delBook(new Guid(tempbook.ID.ToString()));
            //    bGrid.ItemsSource = x.getwishList();
            //}
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