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
    /// Interaktionslogik für myLendings.xaml
    /// </summary>
    public partial class myLendings : Window
    {
        private DataSet ds;
        private DataSet temp;
        public string user;
        string connStr = "server=165.227.160.225;user=root2;database=libraryservice;port=3306;password=Linkstart1;convert zero datetime=True;convert zero datetime=True";

        public myLendings()
        {
            InitializeComponent();
            InitializeComponent();
            loadLendingList();
            txtAuthor.PreviewKeyDown += EnterClickedAuthor;
            txtBuch.PreviewKeyDown += EnterClickedTitle;
          

        }

        public void setUser()
        {
            txtSchueler.Text = user;
            txtSchueler.IsEnabled = false;
        }

    
    void EnterClickedTitle(object sender, KeyEventArgs e)
    {

        if(e.Key == Key.Return)
        {
            DataView dv;
            dv = new DataView(temp.Tables[0], "Titel like '*" + txtBuch.Text + "*'", "Titel Desc", DataViewRowState.CurrentRows);
            dGrid.ItemsSource = dv;

        }
    }

       

    void EnterClickedAuthor(object sender, KeyEventArgs e)
    {

        if(e.Key == Key.Return)
        {
            DataView dv;
            dv = new DataView(temp.Tables[0], "Author like '*" + txtAuthor.Text + "*'", "Author Desc", DataViewRowState.CurrentRows);
                dGrid.ItemsSource = null;
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
                MySqlCommand cmd = new MySqlCommand("SELECT uid as Benutzer, title as Titel , author as Author, duration as Dauer, comment as Kommentar, lendedAt as 'Ausgeborgt am' FROM `lending` join books on books.id = lending.bid where lending.uid = '" + user + "'", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadLendingList");
                dGrid.ItemsSource = ds.Tables["LoadLendingList"].DefaultView;
                temp = ds;
                dGrid.Columns[0].Visibility = Visibility.Hidden;
            });

        });

    }

        private void txtAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataView dv;
            dv = new DataView(temp.Tables[0], "Author like '*" + txtAuthor.Text + "*'", "Author Desc", DataViewRowState.CurrentRows);
            dGrid.ItemsSource = null;
            dGrid.ItemsSource = dv;
        }

        private void txtBuch_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataView dv;
            dv = new DataView(temp.Tables[0], "Titel like '*" + txtBuch.Text + "*'", "Titel Desc", DataViewRowState.CurrentRows);
            dGrid.ItemsSource = null;
            dGrid.ItemsSource = dv;

        }
    }
}
