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

namespace HE_Fleche
{
    /// <summary>
    /// Interaction logic for NewModule.xaml
    /// </summary>
    public partial class NewModule : Window
    {
        public NewModule()
        {
            InitializeComponent();
        }

        private void txtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            txtBox.Text = "";
        }
    }
}
