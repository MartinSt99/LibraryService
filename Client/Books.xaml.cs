using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.ServiceReference1;
using Kundenservice;
using MySql.Data.MySqlClient;

namespace Client
{
    /// <summary>
    /// Interaktionslogik für Books.xaml
    /// </summary>
    public partial class Books : Window
    {
        public string connStr;
        public DataSet books;
        public DataSet tempBooks;
        private readonly OrderBooks x = new OrderBooks();
        private List<Book> bl = new List<Book>();
        public InstanceContext context;
        public AktienInfoClient proxy;
        public string user;
        public Books()
        {
            Dispatcher.Invoke(() =>
            {
                InitializeComponent();
                context = new InstanceContext(this);
                proxy = new AktienInfoClient(context);
                txtContent.PreviewKeyDown += EnterClickedName;
                txtAuthor.PreviewKeyDown += EnterClickedAuthor;
                this.WindowState = WindowState.Maximized;
                string password = "Linkstart1";
                connStr = "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=" + password + ";convert zero datetime=True;convert zero datetime=True";
                tempBooks = books;
            });
        }

        public void loadBooks(DataSet ds)
        {
            dataGrid.ItemsSource = books.Tables["LoadBooks"].DefaultView;
            loadBookList();
        }

        
            public void loadBookList()
            {
                Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        Console.WriteLine("Send Books");
                        MySqlConnection conn = new MySqlConnection(connStr);
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT id,author,title,existsSince FROM books", conn);
                        MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds, "loadBookList");
                        tempBooks = ds;
                        dataGrid.ItemsSource = ds.Tables["loadBookList"].DefaultView;
                        dataGrid.Columns[0].Header = "ID";
                        dataGrid.Columns[1].Header = "Author";
                        dataGrid.Columns[2].Header = "Buchname";
                        dataGrid.Columns[3].Header = "Hinzugefügt am";
                        dataGrid.ItemsSource = ds.Tables["loadBookList"].DefaultView;

                    });

                });

            }


        void EnterClickedAuthor(object sender, KeyEventArgs e)
        {
            
                if(e.Key == Key.Return)
            {
                DataView dv;
                dv = new DataView(tempBooks.Tables[0], "author like '*" + txtAuthor.Text + "*'", "author Desc", DataViewRowState.CurrentRows);
                dataGrid.ItemsSource = dv;

            }
        }

        void EnterClickedName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DataView dv; dv = new DataView(tempBooks.Tables[0], "title like '*" + txtContent.Text + "*'","title Desc", DataViewRowState.CurrentRows);
                dataGrid.ItemsSource = dv;
                
            }
        }
    }
}
