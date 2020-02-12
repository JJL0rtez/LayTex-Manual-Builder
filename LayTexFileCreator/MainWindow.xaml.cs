using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LayTexFileCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Element> elements;
        int id = 0;

        string currentTitle = "",
               currentBody = "";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void addParagraph_Click(object sender, RoutedEventArgs e)
        {

            if(currentTitle != "" || currentBody != "")
            {

            }
            // Create wpf elements
            var title = new TextBox();
            var body = new TextBox();
            var grid = new Grid();
            var titleLabel = new Label();
            var bodyLabel = new Label();
            var deleteBtn = new Button();

            // Initlize wpf elements
            
            // Text Fields
            title.Name = "title";
            title.Text = "";
            title.Width = sv.Width/2;
            title.Height = 20;
            title.Background = Brushes.AntiqueWhite;
            title.Foreground = Brushes.Navy;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Binding myBinding = new Binding("currentTitle");
            myBinding.Source = currentTitle;
            title.SetBinding(TextBlock.TextProperty, myBinding);





            body.Name = "body";
            body.Text = "";
            body.Width = sv.Width - 23;
            body.Height = sv.Height - 110;
            body.Background = Brushes.AntiqueWhite;
            body.Foreground = Brushes.Navy;
            body.Margin = new Thickness(3, 0, 5, 0);
            body.HorizontalAlignment = HorizontalAlignment.Left;

            // Grid
            grid.Width = sv.Width-15;
            grid.Height = sv.Height-5;
            grid.Background = Brushes.FloralWhite;
            //grid.ShowGridLines = true;
               

            RowDefinition row1 = new RowDefinition(),
                          row2 = new RowDefinition(),
                          row3 = new RowDefinition(),
                          row4 = new RowDefinition(),
                          row5 = new RowDefinition();

            row1.Height = new GridLength(25, GridUnitType.Auto);
            grid.RowDefinitions.Add(row1);
            row2.Height = new GridLength(25, GridUnitType.Auto);
            grid.RowDefinitions.Add(row2);
            row3.Height = new GridLength(25, GridUnitType.Auto);
            grid.RowDefinitions.Add(row3);
            row4.Height = new GridLength(25, GridUnitType.Auto);
            grid.RowDefinitions.Add(row4);
            row4.Height = new GridLength(25, GridUnitType.Auto);
            grid.RowDefinitions.Add(row5);

            // Labels
            titleLabel.Name = "titleLabel";
            titleLabel.Content = "Title";
            titleLabel.Width = 35;
            titleLabel.Height = 25;
            titleLabel.FontWeight = FontWeights.Bold;
            titleLabel.Background = Brushes.FloralWhite;
            titleLabel.FontSize = 12;
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;

            bodyLabel.Name = "bodyLabel";
            bodyLabel.Content = "Body";
            bodyLabel.Width = 40;
            bodyLabel.Height = 25;
            bodyLabel.FontWeight = FontWeights.Bold;
            bodyLabel.Background = Brushes.FloralWhite;
            bodyLabel.FontSize = 12;
            bodyLabel.HorizontalAlignment = HorizontalAlignment.Left;

            // Button
            deleteBtn.Name = "delete_btn";
            deleteBtn.Content = "Delete";
            deleteBtn.Height = 25;
            deleteBtn.Width = 75;
            deleteBtn.FontSize = 14;
            deleteBtn.Margin = new Thickness(5, 5, 10, 5);
            deleteBtn.FontWeight = FontWeights.ExtraBlack;
            deleteBtn.Foreground = Brushes.Black;
            deleteBtn.HorizontalAlignment = HorizontalAlignment.Right;

            // Add elements to the grid
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(bodyLabel, 2);
            grid.Children.Add(bodyLabel);
            Grid.SetRow(body, 3);
            grid.Children.Add(body);
            Grid.SetRow(deleteBtn, 4);
            grid.Children.Add(deleteBtn);
            // Add grid to the app
            sv.Content = grid;


        }

        private void addList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var file = File.ReadAllLines(openFileDialog.FileName);
            }
         //       txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void UpdateElementList() {
            Grid grid = new Grid();
            Grid elementGrid = new Grid();
            RowDefinition row;
            int i = 0;

            var title = new Label();

            foreach (Element element in elements)
            {
                row = new RowDefinition();
                row.Height = new GridLength(25, GridUnitType.Auto);
                grid.RowDefinitions.Add(row);
                title.Content = element.getTitle();
                Grid.SetRow(title, i);
                elementGrid.Children.Add(title);
                i++;
            }
        }
    }
}
