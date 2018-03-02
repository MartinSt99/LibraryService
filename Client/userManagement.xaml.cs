using System.Data;
using System.Windows;
using Client.ServiceReference1;

namespace Client
{
    /// <summary>
    ///     Interaktionslogik für userManagement.xaml
    /// </summary>
    public partial class userManagement : Window, IAktienInfoCallback
    {
        public DataSet users;

        public userManagement()
        {
            InitializeComponent();
        }

        public void loginUser(Kundenservice.AktienInfo.ServiceData x, Kundenservice.AktienInfo.ReturnedBooks rb)
        {
        }

        public void UpdateUsers(DataSet ds)
        {
            dataGrid.ItemsSource = users.Tables["LoadUsers"].DefaultView;
            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Benutzername";
            dataGrid.Columns[2].Header = "Vorname";
            dataGrid.Columns[3].Header = "Nachname";
            dataGrid.Columns[4].Visibility = Visibility.Hidden;
            dataGrid.Columns[5].Header = "Registriert seit";
            dataGrid.Columns[6].Header = "Verifiziert";
            dataGrid.Columns[7].Header = "Verizifziert Seit";
            dataGrid.Columns[8].Header = "Stadt";
            dataGrid.Columns[9].Header = "PLZ";
            dataGrid.Columns[10].Header = "Adresse";
            dataGrid.Columns[11].Header = "Klasse";
            dataGrid.Columns[12].Header = "Gesperrt";
        }

        public void BookUpdate(string x, double test, DataSet ds)
        {
        }

        public void loadBooks(DataSet ds)
        {
        }
    }
}