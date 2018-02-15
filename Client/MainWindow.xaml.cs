using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.ServiceReference1;
using Kundenservice;
using Newtonsoft.Json;
using System.Web;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, IAktienInfoCallback
    {
        string connStr = "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=Linkstart1;convert zero datetime=True;convert zero datetime=True";
        private string password = "Linkstart1";
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
                gText.PreviewKeyDown += EnterClicked;
                this.WindowState = WindowState.Maximized;
                menuAdmin.Visibility = Visibility.Hidden;
            });

        }

        public void checkAdmin()
        {
            if (user == "MartinSt")
            {
                menuAdmin.Visibility = Visibility.Visible;
            }
        }

        void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    Book tempBook = new Book();
                    var isbn = gText.Text.Split('F')[1];
                    var body = GetDocumentContents("https://www.googleapis.com/books/v1/volumes?q=" + isbn +
                                                   "+isbn&key=AIzaSyCj1CyB6GBbejRkSD2sV9XAqcS7QzeVHE8&country=AT");
                    dynamic stuff = JsonConvert.DeserializeObject(body.ToString());
                    List<Book> blist = new List<Book>();
                    foreach(var book in stuff.items)
                    {


                        var title = book.volumeInfo.title.ToString();
                        var authors = "";
                        foreach(var author in book.volumeInfo.authors)
                        {
                            authors += author + " ";
                        }

                        tempBook = new Book();
                        tempBook.Author = authors;
                        try
                        {
                            tempBook.ean = book.volumeInfo.industryIdentifiers[1].identifier.ToString();

                        }
                        catch(Exception exception)
                        {
                         //   MessageBox.Show("Error parsing ean");

                        }
                        try
                        {
                            tempBook.Title = book.volumeInfo.title.ToString();

                        }
                        catch(Exception exception)
                        {
                         //   MessageBox.Show("Error parsing title");

                        }
                        try
                        {
                            tempBook.description = book.volumeInfo.description.ToString();

                        }
                        catch(Exception exception)
                        {
                          //  MessageBox.Show("Error parsing description");

                        }
                        try
                        {
                            tempBook.pageCount = book.volumeInfo.pageCount.ToString();

                        }
                        catch(Exception exception)
                        {
                           // MessageBox.Show("Error parsing description");

                        }
                        try
                        {
                            tempBook.averageRating = book.volumeInfo.averageRating.ToString();


                        }
                        catch(Exception exception)
                        {
                          //  MessageBox.Show("error parsing averagerating");

                        }
                        try
                        {
                            tempBook.imageLinks = book.volumeInfo.imageLinks.smallThumbnail.ToString();

                        }
                        catch(Exception exception)
                        {
                            //MessageBox.Show("Error parsing imagelink");

                        }
                        try
                        {
                            tempBook.publishedDate = new DateTime(book.volumeInfo.publishedDate.ToString(), 1, 1, 0, 0, 0).ToString();
                        }
                        catch(Exception exception)
                        {
                            //MessageBox.Show("Error parsing PublishedDate");

                        }
                        try
                        {
                            tempBook.language = book.volumeInfo.language;

                        }
                        catch(Exception exception)
                        {
                          //  MessageBox.Show("Error parsing language");

                        }
                        Dispatcher.Invoke(() =>
                        {
                            blist.Add(tempBook);


                        });
                    }
                    //endregion
                    Dispatcher.Invoke(() =>
                    {
                        bGrid.ItemsSource = null;
                        bGrid.ItemsSource = blist;
                        bGrid.Items.Refresh();


                        e.Handled = true;
                    });
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Google API has no result for this :-(");
                    Console.WriteLine(exception);
                }

            }

        }

        public void ShowImage()
        {
            Book tempBook = new Book();
            var ctr = 1;
            var imageLink = "";
            foreach(var item in bGrid.SelectedCells)
            {
                var obj = (item.Column.GetCellContent(item.Item) as TextBlock).Text;
                if(ctr == 12)
                {                    imageLink = obj;
                }
                Console.WriteLine(obj.ToString() + " " + ctr);
                ctr++;
            }
            var image = new Image();
            try
            {
                wrapPanel1.Children.Clear();
                var fullFilePath = imageLink;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;
                wrapPanel1.Children.Clear();
                wrapPanel1.Children.Add(image);
            }
            catch(Exception exception)
            {
            }
        }

        private string GetDocumentContents(string url)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                return json;
            }

        }

        public void loginUser(int stat)
        {
        }

        public void loadBooks(DataSet ds)
        {
            Dispatcher.Invoke(delegate
            {
                var book = new Books {books = ds};
                book.Show();
                book.user = user;
                book.loadBooks(ds);
                //var usrmgmt = new BookManagement {books = ds};
                //usrmgmt.Show();
                //usrmgmt.user = user;
                //usrmgmt.loadBooks(ds);
            });
        }

        public void loadRegisteredBooks(DataSet ds)
        {

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
                //
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

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            proxy.getUsers(user);
        }

        private void BookManagement_Click(object sender, RoutedEventArgs e)
        {
            proxy.getBooks(user);
        }

        private void gText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            int ctr = 1;
            //AddBook
            var b = new Book();
            try
            {
                foreach(var item in bGrid.SelectedCells)
                {
                    var obj = (item.Column.GetCellContent(item.Item) as TextBlock).Text;
                    if(ctr == 1)
                    {
                        b.Author = obj;
                    }
                    else if(ctr == 2)
                    {
                        b.Title = obj;
                    }
                    else if(ctr == 10)
                    {
                        b.language = obj;
                    }
                    ctr++;
                }
                AddBookToBookList(b);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void AddBookToBookList(Book b)
        {
            try
            {
                Console.WriteLine("Insert Book into booklist");
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `books`(`ean`, `title`, `author`,`isAvailable`) VALUES ('" + b.ean + "','" + b.Title + "','" + b.Author + "',1)", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                
            }
            MessageBox.Show("Buch wurde hinzugefügt!");

        }

        private void bGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowImage();
        }

        private void LendingManagement_OnClick(object sender, RoutedEventArgs e)
        {
            myLendings s = new myLendings();
            s.user = user;
            s.setUser();
            s.ShowDialog();
            
        }
    }
}