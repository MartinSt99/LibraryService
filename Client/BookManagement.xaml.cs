using System;
using System.Data;
using System.ServiceModel;
using System.Windows;
using Client.ServiceReference1;
using Kundenservice;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Client.ServiceReference2;
using MySql.Data.MySqlClient;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für BookManagement.xaml
    /// </summary>
    public partial class BookManagement : Window, IBookUpdateCallback, IBookWishlistCallback, IOrderServiceCallback
    {
        public DataSet books;
        public InstanceContext context;
        public AktienInfoClient proxy;
        public string user;
        public OrderServiceClient client;
        string password = "Linkstart1";

        private string connStr;
        public BookManagement()
        { 
           connStr = "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password + ";convert zero datetime=True;convert zero datetime=True";

            InitializeComponent();
            context = new InstanceContext(this);
            client = new OrderServiceClient(context);
        }

       
       
    
        
        public void loadBooks(DataSet ds)
        {

            dataGrid.ItemsSource = books.Tables["LoadBooks"].DefaultView;
            loadWishlist();
        }

        public void loadWishlist()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine("Send Books");
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM wishlist", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadWishlist");
                    dataGridWishlist.ItemsSource = ds.Tables["LoadWishlist"].DefaultView;

                });
               
            });
           
        }

        public void loginUser(int status)
        {
        }

        public void UpdateUsers(DataSet ds)
        {
        }

        public void BookUpdate(string x, double test, DataSet ds)
        {
        }

        private void btnAddWishlist_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var outtext = "";
                Book b = new Book();
                var ctr = 0;
                try
                {


                    foreach(var item in dataGrid.SelectedCells)
                    {
                        var obj = (item.Column.GetCellContent(item.Item) as TextBlock).Text;
                        if(ctr == 1)
                        {
                            b.ean = obj;
                        }
                        else if(ctr == 2)
                        {
                            b.Title = obj;
                        }
                        else if(ctr == 3)
                        {
                            b.Author = obj;
                        }
                        else if(ctr == 4)
                        {
                            b.isAvailable = int.Parse(obj);
                        }
                        else if(ctr == 5)
                        {
                            b.lastUpdated = obj;
                        }
                        else if(ctr == 6)
                        {
                            b.existsSince = Convert.ToDateTime(obj);
                        }
                        ctr++;
                    }
                    Console.WriteLine("Test");
                    //MessageBox.Show(b.Author + " " + b.Title);
                    wishlistAddBook(b, user);
                    loadWishlist();
                }
                catch(Exception exception)
                {
                }
            });
            


        }

        public void wishlistAddBook(Book b,string user)
        {
            try
            {
                Console.WriteLine("Insert Book into wishlist");
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `wishlist`(`uid`, `bid`) VALUES ('" + user + "','" + b.Title + "')", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Bereits in der Wunschliste enthalten!");
            }
            


        }
        public void AddBook()
        {
            MessageBox.Show("Buch hinzugefügt");
        }

        public void DelBook()
        {
            throw new NotImplementedException();
        }

        public void FindBook()
        {
            throw new NotImplementedException();
        }

        private void delBookFromWishlist(Book b, string user)
        {
            try
            {
                Console.WriteLine("Delete Book from wishlist");
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `wishlist` WHERE `uid` = '" + user + "' AND `bid` = '" + b.Title + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Buch ist nichtmehr in der Wunschliste! Bitte öffne das Fenster neu.");
            }
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

            var outtext = "";
            Book b = new Book();
            var ctr = 0;
            try
            {
                foreach(var item in dataGridWishlist.SelectedCells)
                {
                    var obj = (item.Column.GetCellContent(item.Item) as TextBlock).Text;
                    if(ctr == 1)
                    {
                        b.ean = obj;
                    }
                    else if(ctr == 2)
                    {
                        b.Title = obj;
                    }
                    else if(ctr == 3)
                    {
                        b.Author = obj;
                    }
                    else if(ctr == 4)
                    {
                        b.isAvailable = int.Parse(obj);
                    }
                    else if(ctr == 5)
                    {
                        b.lastUpdated = obj;
                    }
                    else if(ctr == 6)
                    {
                        b.existsSince = Convert.ToDateTime(obj);
                    }
                    ctr++;
                }
                Console.WriteLine("Test");
                //MessageBox.Show(b.Author + " " + b.Title);
                delBookFromWishlist(b, user);
                loadWishlist();
            }
            catch(Exception exception)
            {
            }

        }
    }
}