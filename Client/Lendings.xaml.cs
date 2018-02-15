using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using MySql.Data.MySqlClient;

namespace Client
{
    /// <summary>
    /// Interaktionslogik für Lendings.xaml
    /// </summary>
    public partial class Lendings : Window
    {
        private DataSet ds;
        private DataSet temp;
        string connStr = "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=Linkstart1;convert zero datetime=True;convert zero datetime=True";
        public Lendings()
        {
            InitializeComponent();
            loadLendingList();
            txtAuthor.PreviewKeyDown += EnterClickedAuthor;
            txtAuthor.PreviewKeyDown += EnterClickedTitle;

            txtAuthor.PreviewKeyDown += EnterClickedName;

        }
        void EnterClickedTitle(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Return)
            {
                DataView dv;
                dv = new DataView(temp.Tables[0], "Titel like '*" + txtAuthor.Text + "*'", "Titel Desc", DataViewRowState.CurrentRows);
                dGrid.ItemsSource = dv;

            }
        }

        void EnterClickedName(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Return)
            {
                DataView dv;
                dv = new DataView(temp.Tables[0], "Benutzer like '*" + txtAuthor.Text + "*'", "Benutzer Desc", DataViewRowState.CurrentRows);
                dGrid.ItemsSource = dv;

            }
        }

        void EnterClickedAuthor(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Return)
            {
                DataView dv;
                dv = new DataView(temp.Tables[0], "Author like '*" + txtAuthor.Text + "*'", "Author Desc", DataViewRowState.CurrentRows);
                dGrid.ItemsSource = dv;

            }
        }
        public void loadLendingList()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine("Send Books");
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT uid as Benutzer, title as Titel , author as Autor, duration as Dauer, comment as Kommentar, lendedAt as 'Ausgeborgt am' FROM `lending` join books on books.id = lending.bid", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadLendingList");
                    dGrid.ItemsSource = ds.Tables["LoadLendingList"].DefaultView;
                    temp = ds;
                });

            });

        }
    }
}
