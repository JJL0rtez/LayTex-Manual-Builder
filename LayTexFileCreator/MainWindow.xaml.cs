﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
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
        // Create wpf elements
        TextBox title = new TextBox(), body = new TextBox();
        Grid grid = new Grid();
        Label titleLabel = new Label(), bodyLabel = new Label();
        Button deleteBtn = new Button(), addBtn = new Button();
        List<TextBox> listTextBox = new List<TextBox>();
        int selectedId = 0;
        string currentTitle = "", currentBody = "";
        // Figure
        Image image = new Image();
        Button uploadButton = new Button();
        RadioButton smallImage = new RadioButton();
        RadioButton mediumImage = new RadioButton();
        RadioButton largeImage = new RadioButton();
        // Table
        List<List<TextBox>> tableData = new List<List<TextBox>>();
        List<List<String>> tableStringData = new List<List<String>>();
        Grid tableGrid = new Grid();
        Button addColumnBtn = new Button(), addRowBtn = new Button(),
            removeColumnBtn = new Button(), removeRowBtn = new Button();
        
        public MainWindow()
        {
            InitializeComponent();
            elements = new List<Element>();
            InitialSetup();
        }
        private void AddParagraph_Click(object sender, RoutedEventArgs e)
        {
            
            addBtn.Content = "Add Element";
            body.Text = "";
            title.Text = "";
            InitlizeParagraph("-1");
        }
        private void UpdateBody(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(body.Text + " << --  Body");
            if (body.Text != "")
            {
                currentBody = body.Text;
            }
        }
        private void UpdateTitle(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(title.Text + " << --  TITLE");
            if (title.Text != "")
            {
                currentTitle = title.Text;
                if (selectedId == -1)
                {
                    elements[elements.Count()-1].SetTitle(currentTitle);

                }
                else
                {
                    elements[selectedId].SetTitle(currentTitle);

                }
            }

        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            UpdateElementList();  
        }
        private void PushItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("SaveFiles.bat");
        }
        private void CompileItem_Click(object sender, RoutedEventArgs e)
        {
        }
        private void OpenRefItem_Click(object sender, RoutedEventArgs e){
            string link = "http://www.icl.utk.edu/~mgates3/docs/latex.pdf";
            Process.Start(link);
        }
        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("UpdateFiles.bat");
        }
        private void AddFigure_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            elements.Add(new Element());
            InitlizeFigure(elements.Count() - 1);
        }
        
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (elements.Count() != 0)
            {
               // elements.RemoveAt(id);
            }
            else
            {
                grid.Children.Clear();
                sv.Content = null;
            }
        }
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            elements.Add(new Element());
            addBtn.Content = "Add Element";
            listTextBox.Clear();
            listTextBox.Add(new TextBox());
            InitlizeList("-1");
        }
        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            //reset page
            elements.Clear();
            UpdateElementList();
        }
        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                File.ReadAllLines(openFileDialog.FileName);
            }
         //       txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void SelectChapterFolder_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OpenChapterMode_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OpenPageMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindow = new Window();
            subWindow.Show();
            subWindow.Height = 500;
            subWindow.Width = 800;
            subWindow.Name = "Chapter_Mode";
            // Draw GroupBox and Grid
            GroupBox groupBox = new GroupBox();
            Grid grid = new Grid(), gridMain = new Grid();
            groupBox.Width = subWindow.Width - 10;
            groupBox.Height = subWindow.Height - 10;
            groupBox.VerticalAlignment = VerticalAlignment.Center;
            groupBox.HorizontalAlignment = HorizontalAlignment.Center;
            groupBox.Header = "Page Order Selector";
            //groupBox.Background = Brushes.Blue;
            //groupBox.Margin = new Thickness(5);
            //groupBox.Content = grid;


            //grid.Width = groupBox.Width - 10;
            //grid.Height = groupBox.Height - 10;
            //grid.VerticalAlignment = VerticalAlignment.Center;
            //grid.HorizontalAlignment = HorizontalAlignment.Center;
            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            //grid.Margin = new Thickness(5);
            // Add selector controls
            ScrollViewer scrollViewerBefore = new ScrollViewer();
            ScrollViewer scrollViewerAfter = new ScrollViewer();
            List<Button> beforeList = new List<Button>();
            List<Button> afterList = new List<Button>();
            Menu menu = new Menu();
            menu.Margin = new Thickness(5, 20, 0, 0);
            MenuItem saveMenuItem = new MenuItem();
            saveMenuItem.Header = " Save";
            MenuItem loadMenuItem = new MenuItem();
            loadMenuItem.Header = "Load";
            MenuItem pageView = new MenuItem();
            pageView.Header = " View Page Mode";
            MenuItem chapterView = new MenuItem();
            chapterView.Header = " View Chapter Mode";
            MenuItem file = new MenuItem();
            file.Header = " File";
            MenuItem page = new MenuItem();
            page.Header = " Page View";
            MenuItem chapter = new MenuItem();
            chapter.Header = " Chapter View";

            //menu.Background = Brushes.Red;
            menu.Items.Insert(0, file);
            menu.Items.Insert(1, page);
            menu.Items.Insert(2, chapter);
            file.Items.Insert(0, saveMenuItem);
            file.Items.Insert(1, loadMenuItem);
            page.Items.Insert(0, pageView);
            chapter.Items.Insert(0, chapterView);

            gridMain.Children.Add(menu);
           // gridMain.Children.Add(groupBox);
            subWindow.Content = gridMain;
        }
        private void UpdateElementList() {
            Grid gridE = new Grid();
            Grid elementGrid = new Grid();
            RowDefinition row;
            //remoove old item
            //if (elements.Count() > selectedId && selectedId != -1)
           // {
           //     elements.RemoveAt(selectedId);
           // }
            var titleB = new Button();
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            // If the data was updated then re add it to the element list
            if (currentTitle != "" && currentBody != "")
            {

                // elements.Add(new Element(currentTitle, currentBody));

                grid.Children.Clear();
                sv.Content = null;
            }

            // now redraw with new items
            int cId = 0;
            elementGrid.ShowGridLines = true;
            foreach (Element element in elements)
            {
                row = new RowDefinition {   Height = new GridLength(25, GridUnitType.Auto)  };
                elementGrid.RowDefinitions.Add(row);
            }

            foreach (Element element in elements)
            {
                titleB = new Button();
                title.Name = "Id" + cId.ToString();
                if (element.GetData().Count() > 1)
                {
                    titleB.Content = " (List) " + element.GetTitle();
                }
                else
                {
                    titleB.Content = " (Paragraph) " + element.GetTitle();
                }
                titleB.Tag = cId;
                titleB.Click += Element_click;
                Grid.SetRow(titleB, cId);
                elementGrid.Children.Add(titleB);
                cId++;
            }
            gridE.Width = 155;
            gridE.Height = 290;
            //gridE.Background = Brushes.Pink;
            gridE.Children.Add(elementGrid);
            elementSV.Content = gridE;
        }
        private void Element_click(object sender, RoutedEventArgs e)
        {
            addBtn.Content = "Update Element";
            Button button = (Button)sender;
            //Console.WriteLine(button.Tag + "<--------------");
            if (Int32.Parse(button.Tag.ToString()) <= elements.Count() - 1) {
                if (elements.ElementAt(int.Parse(button.Tag.ToString())).GetData().Count()  <= 1 ){
                    if (button.Content.ToString().Contains("  (List)  "))
                    {
                        InitlizeList(button.Tag.ToString());
                    }
                    else
                    {
                        InitlizeParagraph(button.Tag.ToString());
                    }
                }
                return;
            }
            InitlizeList(button.Tag.ToString());
        }
        private void AddGridRows(int numRows)
        {
            RowDefinition row;
            grid.RowDefinitions.Clear();

            while (numRows > 0)
            {
                row = new RowDefinition();
                numRows--;
                row.Height = new GridLength(25, GridUnitType.Auto);
                grid.RowDefinitions.Add(row);
            }
        }
        private void AddGridColoums(int numCol)
        {
            //RowDefinition row;
            //grid.RowDefinitions.Clear();

            //while (numRows > 0)
            //{
            //    row = new RowDefinition();
            //    numRows--;
            //    row.Height = new GridLength(25, GridUnitType.Auto);
            //    grid.RowDefinitions.Add(row);
            //}
        }
        private void InitlizeList(string id)
        {

            //initlize listS
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            sv.Content = null;
            int idNum = Int32.Parse(id);
            selectedId = idNum;
            //needsAdded = true;


            TextBox tmp = new TextBox();

            for (int i = 0; i < listTextBox.Count(); i++)
            {
                tmp = listTextBox.ElementAt(i);
                tmp.Name = "listItemTextBox" + i;
                if (selectedId != -1 && elements[selectedId].GetData().Count() > i)
                {
                    tmp.Text = elements[selectedId].GetData()[i];
                }
                else
                {
                    tmp.Text = "";
                }
                
                tmp.Width = sv.Width - 45;
                tmp.Height = 20;
                tmp.Background = Brushes.AntiqueWhite;
                tmp.Foreground = Brushes.Navy;
                tmp.HorizontalAlignment = HorizontalAlignment.Left;
                tmp.TextChanged += UpdatelistText;
                tmp.SpellCheck.IsEnabled = true;
                tmp.TextWrapping = 0;
                tmp.Tag = i;
            }

            // Grid
            grid.Width = sv.Width - 25;
            if (listTextBox.Count() > 10)
            {
                grid.Height = sv.Height - 5 + listTextBox.Count() - 10 * 20;
            }
            else
            {
                grid.Height = sv.Height - 5;
            }

            if (elements.Count() != 0)
            {
                AddGridRows(listTextBox.Count() + 5);
            }
            else
            {
                AddGridRows(5);
            }

            grid.Children.Clear();
            // Add elements to the grid
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(bodyLabel, 2);
            grid.Children.Add(bodyLabel);

            // Add grid to the app
            sv.Content = grid;
            int j = 3;
            if (idNum != -1)
            {
                title.Text = elements.ElementAt(idNum).GetTitle();
                foreach(TextBox text in listTextBox)
                {
                    Grid.SetRow(text, j);
                    grid.Children.Add(text);
                    j++;
                }
            }
            else
            {
                Grid.SetRow(listTextBox.ElementAt(0), j);
                grid.Children.Add(listTextBox.ElementAt(0));
                j++;
            }
            Grid.SetRow(addBtn, j);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, j);
            grid.Children.Add(deleteBtn);
        }
        private void UpdatelistText(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            //update data in elements


            if (textBox.Text != "" && int.Parse(textBox.Tag.ToString()) == listTextBox.Count()-1)
            {
                listTextBox.Add(new TextBox());
               // elements.Add(new Element());
                InitlizeList(selectedId.ToString());
                int tag = int.Parse(textBox.Tag.ToString());
                elements[tag].SetData(tag, textBox.Text);
            }

            //updata graphicial display
            
        }
        private void InitlizeParagraph(string id)
        {
            int idNum = Int32.Parse(id);
            selectedId = idNum;
           
            //Initlize and setup paragraph

            grid.Children.Clear();
            sv.Content = null;


            AddGridRows(5);

            // Add elements to the grid
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(bodyLabel, 2);
            grid.Children.Add(bodyLabel);
            Grid.SetRow(body, 3);
            grid.Children.Add(body);
            Grid.SetRow(addBtn, 4);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, 4);
            grid.Children.Add(deleteBtn);

            // Add grid to the app
            while (grid.RowDefinitions.Count() > 6)
            {
                grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count() - 1);
            }
            sv.Content = grid;
            if (idNum != -1) {
                title.Text = elements.ElementAt(idNum).GetTitle();
                body.Text = elements.ElementAt(idNum).GetData().ElementAt(0);
            }
        }
        private void InitialSetup()
        {
            // Initlize wpf elements
            // Grid
            grid.Width = sv.Width - 25;
            grid.Height = sv.Height - 5;
            grid.Background = Brushes.FloralWhite;
            grid.HorizontalAlignment = HorizontalAlignment.Left;

            // Text Fields
            title.Name = "title";
            title.Text = "";
            title.Width = sv.Width / 2;
            title.Height = 20;
            title.Background = Brushes.AntiqueWhite;
            title.Foreground = Brushes.Navy;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;

            body.Name = "body";
            body.Text = "";
            body.Width = sv.Width - 35;
            body.Height = sv.Height - 110;
            body.Background = Brushes.AntiqueWhite;
            body.Foreground = Brushes.Navy;
            body.Margin = new Thickness(3, 0, 5, 0);
            body.TextChanged += UpdateBody;
            body.SpellCheck.IsEnabled = true;
            body.TextWrapping = 0;
            body.HorizontalAlignment = HorizontalAlignment.Left;
            // Buttons
            deleteBtn.Name = "delete_btn";
            deleteBtn.Content = "Delete";
            deleteBtn.Height = 25;
            deleteBtn.Width = 75;
            deleteBtn.FontSize = 14;
            deleteBtn.Margin = new Thickness(5, 5, 10, 5);
            deleteBtn.FontWeight = FontWeights.Bold;
            deleteBtn.Foreground = Brushes.Black;
            deleteBtn.HorizontalAlignment = HorizontalAlignment.Right;
            deleteBtn.Click += DeleteClick;

            addBtn.Name = "add_btn";
            addBtn.Content = "Add Element";
            addBtn.Height = 25;
            addBtn.Width = 95;
            addBtn.FontSize = 14;
            addBtn.Margin = new Thickness(5, 5, 10, 5);
            addBtn.FontWeight = FontWeights.Bold;
            addBtn.Foreground = Brushes.Black;
            addBtn.HorizontalAlignment = HorizontalAlignment.Left;
            addBtn.Click += AddClick;

            // Labels
            titleLabel.Name = "titleLabel";
            titleLabel.Content = "Title";
            titleLabel.Width = 35;
            titleLabel.Height = 25;
            titleLabel.FontWeight = FontWeights.Bold;
            titleLabel.Background = Brushes.FloralWhite;
            titleLabel.FontSize = 12;
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;

            bodyLabel.Name = "listLabel";
            bodyLabel.Content = "List Items";
            bodyLabel.Width = 65;
            bodyLabel.Height = 25;
            bodyLabel.FontWeight = FontWeights.Bold;
            bodyLabel.Background = Brushes.FloralWhite;
            bodyLabel.FontSize = 12;
            bodyLabel.HorizontalAlignment = HorizontalAlignment.Left;

           // addColumnBtn,removeColumnBtn,addRowBtn,removeRowBtn
            removeRowBtn.Width = 100;
            removeRowBtn.Content = "Remove Row";
            removeRowBtn.Click += RemoveRowBtn_Click;  //AddGridColoums;
            removeRowBtn.Height = 25;
            removeRowBtn.HorizontalAlignment = HorizontalAlignment.Right;
            removeRowBtn.Margin = new Thickness(0, 0, 10, 0);

            removeColumnBtn.Width = 100;
            removeColumnBtn.Content = "Remove Column";
            removeColumnBtn.Click += RemoveColumnBtn_Click; //AddGridColoums;
            removeColumnBtn.Height = 25;
            removeColumnBtn.Margin = new Thickness(130, 0, 0, 0);
            removeColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

            addRowBtn.Width = 100;
            addRowBtn.Content = "Add Row";
            addRowBtn.Click += AddRowBtn_Click;  //AddGridColoums;
            addRowBtn.Height = 25;
            addRowBtn.Margin = new Thickness(0, 0, 130, 0);
            addRowBtn.HorizontalAlignment = HorizontalAlignment.Right;

            addColumnBtn.Width = 100;
            addColumnBtn.Content = "Add Column";
            addColumnBtn.Click += AddColumnBtn_Click;  //AddGridColoums;
            addColumnBtn.Height = 25;
            addColumnBtn.Margin = new Thickness(10, 0, 0, 0);
            addColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

        }

        private void AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            tableGrid.RowDefinitions.Add(row);
        }

        private void RemoveColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tableGrid.ColumnDefinitions.Count() > 0)
            {
                tableGrid.ColumnDefinitions.RemoveAt(tableGrid.ColumnDefinitions.Count() - 1);
            };
        }

        private void RemoveRowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tableGrid.RowDefinitions.Count() > 0)
            {
                tableGrid.RowDefinitions.RemoveAt(tableGrid.RowDefinitions.Count() - 1);
            };
        }
        private void AddTable_Click(object sender, RoutedEventArgs e)
        {
            elements.Add(new Element());
            InitlizeTable(elements.Count());
        }

        
        private void AddColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(25, GridUnitType.Auto);
            tableGrid.ColumnDefinitions.Add(column);
        }
     
        private void InitlizeFigure(int id)
        {
            //Steps
            // Clear other Controls
            //int idNum = Int32.Parse(id);
            selectedId = id;
            grid.Children.Clear();
            sv.Content = null;
            // Reset Image, image button, Size selection radio buttons
            titleLabel.Content = "Figure subtext";
            titleLabel.Width = 100;
            title = new TextBox();
            title.Name = "title";
            title.Text = "";
            title.Width = sv.Width / 2;
            title.Height = 20;
            title.Background = Brushes.AntiqueWhite;
            title.Foreground = Brushes.Navy;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;
            image = new Image();
            image.Width = 225;
            image.Height = 225;
            image.Stretch = Stretch.Uniform;
            image.HorizontalAlignment = HorizontalAlignment.Right;
            image.VerticalAlignment = VerticalAlignment.Center;
            smallImage = new RadioButton();
            smallImage.Content = "Small";
            smallImage.IsChecked = false;
            smallImage.GroupName = "imageSize";
            smallImage.Tag = 0;
            smallImage.Click += updateImageSize;
            smallImage.HorizontalAlignment = HorizontalAlignment.Left;
            mediumImage = new RadioButton();
            mediumImage.Content = "Medium";
            mediumImage.IsChecked = true;
            mediumImage.GroupName = "imageSize";
            mediumImage.Tag = 1;
            mediumImage.Click += updateImageSize;
            mediumImage.HorizontalAlignment = HorizontalAlignment.Center;
            largeImage = new RadioButton();
            largeImage.Content = "Large";
            largeImage.IsChecked = false;
            largeImage.GroupName = "imageSize";
            largeImage.Tag = 2;
            largeImage.Click += updateImageSize;
            largeImage.HorizontalAlignment = HorizontalAlignment.Right;
            uploadButton = new Button();
            uploadButton.Width = 50;
            uploadButton.Height = 50;
            uploadButton.Click += OpenImageUploadDialog;
            uploadButton.Content = "Upload";
            //uploadButton.Background = Brushes.DarkCyan;
            uploadButton.HorizontalAlignment = HorizontalAlignment.Left;
            // Add rows and coloums
            AddGridRows(5);
            //AddGridColoums(5);
            //Add Controls to grid
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(smallImage, 2);
            grid.Children.Add(smallImage);
            Grid.SetRow(mediumImage, 2);
            grid.Children.Add(mediumImage);
            Grid.SetRow(largeImage, 2);
            grid.Children.Add(largeImage);
            Grid.SetRow(image, 3);
            grid.Children.Add(image);
            Grid.SetRow(uploadButton, 3);
            grid.Children.Add(uploadButton);
            Grid.SetRow(addBtn, 4);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, 4);
            grid.Children.Add(deleteBtn);
            //add grid to gui
            sv.Content = grid;
            //add data to figure if avalible

        }

        private void OpenImageUploadDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = "",
                Filter = "Image files (*.png)|*.png",
                Title = "Open an Image",
                DefaultExt = ".png.jpeg.ico.gif"
            };
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                elements[selectedId].setImagLoc(openFileDialog.FileName);
                Uri uri = new Uri(openFileDialog.FileName);
                image.Source = new BitmapImage(uri);

            }
        }

        private void updateImageSize(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            elements[selectedId].SetImageSize(int.Parse(button.Tag.ToString()));
        }

        private void InitlizeTable(int id)                                              
        {
            //Steps
            // Clear other Controls
            //int idNum = Int32.Parse(id);
            selectedId = id;
            grid.Children.Clear();
            sv.Content = null;
            tableGrid = new Grid();
            tableData = new List<List<TextBox>>();
            // Reset Image, image button, Size selection radio buttons
            titleLabel.Content = "Table Title";
            titleLabel.Width = 100;
            title = new TextBox();
            title.Name = "title";
            title.Text = "";
            title.Width = sv.Width / 2;
            title.Height = 20;
            title.Background = Brushes.AntiqueWhite;
            title.Foreground = Brushes.Navy;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;

            int totalRows = tableData.Count();
            int totalColoums = 0;
            if (totalRows > 0)
            {
                totalColoums = tableData[0].Count();
            }
            tableGrid =  AddTableGrid(totalRows, totalColoums);
            // Now that grid is there add all the textboxs to the grid and content 
            for (int rows = 0; rows < totalRows; rows++)
            {
                for (int columns = 0; columns < totalColoums; columns++)
                {
                    tableData[rows][columns].Text = tableStringData[rows][columns];
                    //textBox.Text = "";
                    tableData[rows][columns].Width = tableGrid.Width-20;
                    tableData[rows][columns].Height = 20;
                }
            }
            
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(addColumnBtn, 2);
            grid.Children.Add(addColumnBtn);
            Grid.SetRow(addRowBtn, 2);
            grid.Children.Add(addRowBtn);
            Grid.SetRow(removeRowBtn, 2);
            grid.Children.Add(removeRowBtn);
            Grid.SetRow(removeColumnBtn, 2);
            grid.Children.Add(removeColumnBtn);
            Grid.SetRow(tableGrid, 3);
            grid.Children.Add(tableGrid);
            Grid.SetRow(addBtn, 4);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, 4);
            grid.Children.Add(deleteBtn);

            sv.Content = grid;


        }


        private Grid AddTableGrid(int totalRows, int totalColoums)
        {
            Grid tGrid = new Grid();
            tGrid.Width = totalColoums * 100;
            tGrid.Height = totalRows * 25;
            tGrid.Background = Brushes.Pink;
            RowDefinition row;
            ColumnDefinition column;
            tGrid.ColumnDefinitions.Clear();
            tGrid.RowDefinitions.Clear();

            while (totalRows > 0)
            {
                row = new RowDefinition();
                totalRows--;
                row.Height = new GridLength(25, GridUnitType.Auto);
                tGrid.RowDefinitions.Add(row);
            }
            while (totalColoums > 0)
            {
                column = new ColumnDefinition();
                totalColoums--;
                column.Width = new GridLength(25, GridUnitType.Auto);
                tGrid.ColumnDefinitions.Add(column);
            }
            return tGrid;
        }
    }
}
