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
using System.Windows.Media.Imaging;

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
                gText.PreviewKeyDown += EnterClicked;
                
            });
           
        }
        void EnterClicked(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                try
                {
                    var isbn = gText.Text.Split('F')[1];
                    var body = GetDocumentContents("https://www.googleapis.com/books/v1/volumes?q=" + isbn + "+isbn&key=AIzaSyCj1CyB6GBbejRkSD2sV9XAqcS7QzeVHE8&country=AT");
                    dynamic stuff = JsonConvert.DeserializeObject(body.ToString());
                    var title = stuff.items[0].volumeInfo.title.ToString();
                    var authors = "";
                    foreach (var author in stuff.items[0].volumeInfo.authors)
                    {
                        authors += author + " ";
                    }
                    List<Book> blist = new List<Book>(1);
                    Book tempBook = new Book();
                    tempBook.Author = authors;
                    try
                    {
                        tempBook.Title = stuff.items[0].volumeInfo.title.ToString();

                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.Title = stuff.items[0].volumeInfo.title.ToString();

                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                    try
                    {
                        tempBook.description = stuff.items[0].volumeInfo.description.ToString();

                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.pageCount = stuff.items[0].volumeInfo.pageCount.ToString();

                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.averageRating = stuff.items[0].volumeInfo.averageRating.ToString();


                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.imageLinks = stuff.items[0].volumeInfo.imageLinks.smallThumbnail.ToString();

                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.publishedDate = stuff.items[0].volumeInfo.publishedDate.ToString();

                    }
                    catch (Exception exception)
                    {
                    }
                    try
                    {
                        tempBook.language = stuff.items[0].volumeInfo.lanugage;

                    }
                    catch (Exception exception)
                    {
                    }
                    blist.Add(tempBook);
                    bGrid.ItemsSource = null;
                    bGrid.ItemsSource = blist;
                    bGrid.Items.Refresh();
                    var image = new Image();
                    try
                    {
                        wrapPanel1.Children.Clear();
                        var fullFilePath = tempBook.imageLinks;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                        bitmap.EndInit();

                        image.Source = bitmap;
                        wrapPanel1.Children.Clear();
                        wrapPanel1.Children.Add(image);
                    }
                    catch (Exception exception)
                    {
                    }
                  
                    e.Handled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Google API has no result for this :-(");
                    Console.WriteLine(exception);
                }
             
            }
            
        }

        private string GetDocumentContents(string url)
        {
            using(WebClient wc = new WebClient())
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
                var usrmgmt = new BookManagement {books = ds};
                usrmgmt.Show();
                usrmgmt.user = user;
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
    }
}