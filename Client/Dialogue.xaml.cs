using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für Dialogue.xaml
    /// </summary>
    public partial class Dialogue : Window
    {
        private string AnswerText;
        public Dialogue()
        {
            InitializeComponent();
        }

        public void setQuestionText(string questiontext)
        {
            lblAsk.Content = questiontext;
        }
        public string returnText()
        {
           return txtResult.Text;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
