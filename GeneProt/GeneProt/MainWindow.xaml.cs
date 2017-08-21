using System;
using GeneProt;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace WpfApplication
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Search search = new Search();
            this.NavigateTo(search);
        }

        public void NavigateTo(object o)
        {
            Frame1.Navigate(o);
        }

        private void Button_search(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            this.NavigateTo(search);
        }

        private void Button_export(object sender, RoutedEventArgs e)
        {
            Export export = new Export();
            this.NavigateTo(export);
        }
        
    }
}