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

namespace Client
{
    /// <summary>
    /// Interaktionslogik für BookManagement.xaml
    /// </summary>
    public partial class BookManagement : Window
    {
        public DataSet books;
        public BookManagement()
        {
            InitializeComponent();
        }

        public void getBooks(DataSet ds)
        {
            dataGrid.ItemsSource = books.Tables["LoadBooks"].DefaultView;
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
    }

}
