using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
using System.Xml;
using System.Xml.Serialization;

namespace LayTexFileCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Element> elements;
        Page page = new Page();
        Window popup = new Window();
        Grid popupGrid = new Grid();

        // Create wpf elements
        TextBox title = new TextBox(), body = new TextBox();
        Grid grid = new Grid();
        Label titleLabel = new Label(), bodyLabel = new Label();
        Button deleteBtn = new Button(), addBtn = new Button();
        int selectedId = 0;
        string currentTitle = "", currentBody = "";

        // List item
        List<TextBox> listTextBox = new List<TextBox>();
        ListBox listBox = new ListBox();

        // Figure
        Image image = new Image();
        Button uploadButton = new Button();
        RadioButton smallImage = new RadioButton();
        RadioButton mediumImage = new RadioButton();
        RadioButton largeImage = new RadioButton();
        string selectedFile = "";
        
        // Table
        List<List<TextBox>> tableData = new List<List<TextBox>>();
        List<List<String>> tableStringData = new List<List<String>>();
        Grid tableGrid = new Grid();
        Button addColumnBtn = new Button(), addRowBtn = new Button(), removeColumnBtn = new Button(), removeRowBtn = new Button();
        ListBox tableArea = new ListBox();

        // Save Dialog
        Label popupTitle = new Label(); 
        Button saveButton = new Button(), cancelButton = new Button();
        TextBox popupTextbox = new TextBox();


        public MainWindow()
        {
            InitializeComponent();
            elements = new List<Element>();
            InitialSetup();
            addBtn.Content = "Add Element";
            body.Text = "";
            title.Text = "";
            InitlizeParagraph("-1");
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
            if (body.Text != "")
            {
                currentBody = body.Text;
            }
        }
        private void UpdateTitle(object sender, TextChangedEventArgs e)
        {
            if (title.Text != "")
            {
                currentTitle = title.Text;
            }
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string[] strlist = button.Tag.ToString().Split(',');
            SaveData(strlist[0],int.Parse(strlist[1]));
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
            //elements.Add(new Element());
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
            //elements.Add(new Element());
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
            InitlizeParagraph("-1");
        }
        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            if (page.GetDateCreated() == null)
            {
                page.SetDateCreated(DateTime.Now.ToString("h:mm:ss tt"));
            }
            page.SetdateEdited(DateTime.Now.ToString("h:mm:ss tt"));
            page.SetElements(elements);
            string name = "";
            popup = new Window();
            popup.Width = 300;
            popup.Height = 130;
            popup.Background = Brushes.LightGray;
            
            


            popupTitle.Content = "Page Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 5, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width-35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20,5,0,0);
            saveButton.Width = 50;
            saveButton.Height = 25;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8,5, 0, 0);
            saveButton.Click += SaveElements_Click;
            cancelButton.Width = 50;
            cancelButton.Height = 25;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0,5,27,0);
            cancelButton.Click += CancelPopup_Click;


            popupGrid.Children.Clear();
            popupGrid.Width = popup.Width;
            popupGrid.Height = popup.Height;

            RowDefinition row;
            popupGrid.RowDefinitions.Clear();
            row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            popupGrid.RowDefinitions.Add(row);

            Grid.SetRow(popupTitle, 0);
            popupGrid.Children.Add(popupTitle);
            Grid.SetRow(popupTextbox, 1);
            popupGrid.Children.Add(popupTextbox);
            Grid.SetRow(saveButton, 2);
            popupGrid.Children.Add(saveButton);
            Grid.SetRow(cancelButton, 2);
            popupGrid.Children.Add(cancelButton);

            popup.Content = popupGrid;

            popup.Show();
            page.Setname(name);
           
        }

        private void CancelPopup_Click(object sender, RoutedEventArgs e)
        {
            popup.Close();
        }

        private void SaveElements_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //saveFileDialog = new SaveFileDialog();
                if (popupTextbox.Text != "")
                {
                    page.Setname(popupTextbox.Text);
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xml file|*.xml";
                    saveFileDialog.Title = "Save a page data File";
                    saveFileDialog.FileName = popupTextbox.Text + ".xml";

                    saveFileDialog.ShowDialog();
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlSerializer serializer = new XmlSerializer(typeof(Page));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, page);
                        stream.Position = 0;
                        xmlDocument.Load(stream);
                        xmlDocument.Save(saveFileDialog.FileName);
                    }
                    popup.Close();
                    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            groupBox.Width = subWindow.Width - 20;
            groupBox.Height = subWindow.Height - 60;
            groupBox.VerticalAlignment = VerticalAlignment.Center;
            groupBox.HorizontalAlignment = HorizontalAlignment.Center;
            groupBox.Header = "Page Order Selector";
            //groupBox.Background = Brushes.Blue;
            //groupBox.Margin = new Thickness(5);
            //groupBox.Content = grid;


            grid.Width = groupBox.Width - 10;
            grid.Height = groupBox.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;

            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            //grid.Margin = new Thickness(5);
            // Add selector controls
            ScrollViewer scrollViewerBefore = new ScrollViewer();
            scrollViewerBefore.Height = subWindow.Height - 100;
            scrollViewerBefore.Width = subWindow.Width / 2 - 30;
            scrollViewerBefore.HorizontalAlignment = HorizontalAlignment.Left;
            scrollViewerBefore.VerticalAlignment = VerticalAlignment.Center;
            scrollViewerBefore.Background = Brushes.AntiqueWhite;
            scrollViewerBefore.Margin = new Thickness(1);
            ScrollViewer scrollViewerAfter = new ScrollViewer();
            scrollViewerAfter.Height = subWindow.Height - 100;
            scrollViewerAfter.Width = subWindow.Width / 2 - 30;
            scrollViewerAfter.HorizontalAlignment = HorizontalAlignment.Right;
            scrollViewerAfter.VerticalAlignment = VerticalAlignment.Center;
            scrollViewerAfter.Background = Brushes.AntiqueWhite;
            scrollViewerAfter.Margin = new Thickness(1);

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

            RowDefinition row;
            gridMain.RowDefinitions.Clear();
            row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            gridMain.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(25, GridUnitType.Auto);
            gridMain.RowDefinitions.Add(row);

            Grid.SetRow(menu, 0);
            gridMain.Children.Add(menu);
            Grid.SetRow(groupBox, 1);
            gridMain.Children.Add(groupBox);

            groupBox.Content = grid;

            grid.Children.Add(scrollViewerBefore);
            grid.Children.Add(scrollViewerAfter);
            // gridMain.Children.Add(groupBox);
            subWindow.Content = gridMain;
        }
        private void UpdateElementList() {
            Button elementNameBttn = new Button();
            int i = 0;
            elementSV.Items.Clear();
            foreach (Element element in elements)
            {
                elementNameBttn = new Button();
                elementNameBttn.Tag = element.getElementType() + "," + i;
                elementNameBttn.Click += Element_click;
                elementNameBttn.Content =  "(" + element.getElementType() + ") " +  element.GetTitle();
                elementNameBttn.Width = elementSV.Width - 10;
                i++;
                elementSV.Items.Add(elementNameBttn);
            }
        }
        private void Element_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            addBtn.Content = "Update Element";
            string[] strlist = button.Tag.ToString().Split(',');
            if (strlist[0] == "Paragraph")
            {
                InitlizeParagraph(strlist[1]);
            }
            else if (strlist[0] == "List")
            {
                InitlizeList(strlist[1]);
            }
            else if (strlist[0] == "Figure")
            {
                InitlizeFigure(int.Parse(strlist[1]));
            }
            else
                InitlizeTable(int.Parse(strlist[1]));
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
        private void InitlizeList(string id)
        {
            addBtn.Tag = "List," + id;
            deleteBtn.Tag = "List," + id;
            titleLabel.Content = "List Title";
           // title.Text = "";
            //initlize listS
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            sv.Content = null;
            int idNum = Int32.Parse(id);
            selectedId = idNum;
            //needsAdded = true;

            listBox.Width = grid.Width - 10;

            // Set the tag for add and delete


            TextBox tmp = new TextBox();
            listBox.Items.Clear();
            for (int i = 0; i < listTextBox.Count(); i++)
            {
                tmp = new TextBox();
                tmp = listTextBox.ElementAt(i);
                tmp.Name = "listItemTextBox" + i;
                
                tmp.Width = sv.Width - 50;
                tmp.Height = 20;
                tmp.Background = Brushes.AntiqueWhite;
                tmp.Foreground = Brushes.Navy;
                tmp.HorizontalAlignment = HorizontalAlignment.Left;
                tmp.TextChanged += UpdatelistText;
                tmp.SpellCheck.IsEnabled = true;
                tmp.TextWrapping = 0;
                tmp.Tag = i;
                listBox.Items.Add(tmp);
            }

            grid.Children.Clear();
            AddGridRows(5);
            // Add elements to the grid
            Grid.SetRow(titleLabel, 0);
            grid.Children.Add(titleLabel);
            Grid.SetRow(title, 1);
            grid.Children.Add(title);
            Grid.SetRow(bodyLabel, 2);
            grid.Children.Add(bodyLabel);
            Grid.SetRow(listBox, 3);
            grid.Children.Add(listBox);

            // Add grid to the app
            sv.Content = grid;


            Grid.SetRow(addBtn, 4);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, 5);
            grid.Children.Add(deleteBtn);

            if (idNum != -1)
            {
                title.Text = elements.ElementAt(idNum).GetTitle();
               // body.Text = elements.ElementAt(idNum).GetBody();
            }
        }
        private void UpdatelistText(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            //update data in elements


            if (textBox.Text != "" && int.Parse(textBox.Tag.ToString()) == listTextBox.Count()-1)
            {
                listTextBox.Add(new TextBox());
                InitlizeList(selectedId.ToString());
                int tag = int.Parse(textBox.Tag.ToString());
            }
        }
        private void InitlizeParagraph(string id)
        {
            addBtn.Tag = "Paragraph," + id;
            deleteBtn.Tag = "Paragraph," + id;
            //Initlize and setup paragraph
            grid.Children.Clear();
            sv.Content = null;
            AddGridRows(5);
            int idNum = Int32.Parse(id);
            selectedId = idNum;
            titleLabel.Content = "Paragraph Title";

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
                body.Text = elements.ElementAt(idNum).GetBody();
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
            addBtn.Width = 115;
            addBtn.FontSize = 14;
            addBtn.Margin = new Thickness(5, 5, 10, 5);
            addBtn.FontWeight = FontWeights.Bold;
            addBtn.Foreground = Brushes.Black;
            addBtn.HorizontalAlignment = HorizontalAlignment.Left;
            addBtn.Click += AddClick;

            // Labels
            titleLabel.Name = "titleLabel";
            titleLabel.Content = "Title";
            titleLabel.Width = 135;
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
            removeRowBtn.Margin = new Thickness(0, 10, 11, 0);

            removeColumnBtn.Width = 100;
            removeColumnBtn.Content = "Remove Column";
            removeColumnBtn.Click += RemoveColumnBtn_Click; //AddGridColoums;
            removeColumnBtn.Height = 25;
            removeColumnBtn.Margin = new Thickness(130, 10, 0, 0);
            removeColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

            addRowBtn.Width = 100;
            addRowBtn.Content = "Add Row";
            addRowBtn.Click += AddRowBtn_Click;  //AddGridColoums;
            addRowBtn.Height = 25;
            addRowBtn.Margin = new Thickness(0, 10, 135, 0);
            addRowBtn.HorizontalAlignment = HorizontalAlignment.Right;

            addColumnBtn.Width = 100;
            addColumnBtn.Content = "Add Column";
            addColumnBtn.Click += AddColumnBtn_Click;  //AddGridColoums;
            addColumnBtn.Height = 25;
            addColumnBtn.Margin = new Thickness(5, 10, 0, 0);
            addColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

            //
            listBox.Height = 200;
            listBox.Background = Brushes.FloralWhite;
            listBox.Margin = new Thickness(5);
            listBox.HorizontalAlignment = HorizontalAlignment.Center;


            tableArea.Height = 200;
            tableArea.Background = Brushes.White;
            tableArea.Margin = new Thickness(5);
            tableArea.HorizontalAlignment = HorizontalAlignment.Left;
            tableArea.Width = sv.Width - 40;
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
            //elements.Add(new Element());
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
            addBtn.Tag = "Figure," + id;
            deleteBtn.Tag = "Figure," + id;
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
            smallImage.Click += UpdateImageSize;
            smallImage.HorizontalAlignment = HorizontalAlignment.Left;
            mediumImage = new RadioButton();
            mediumImage.Content = "Medium";
            mediumImage.IsChecked = true;
            mediumImage.GroupName = "imageSize";
            mediumImage.Tag = 1;
            mediumImage.Click += UpdateImageSize;
            mediumImage.HorizontalAlignment = HorizontalAlignment.Center;
            largeImage = new RadioButton();
            largeImage.Content = "Large";
            largeImage.IsChecked = false;
            largeImage.GroupName = "imageSize";
            largeImage.Tag = 2;
            largeImage.Click += UpdateImageSize;
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
            if(selectedId != -1)
            {
                title.Text = elements[selectedId].GetTitle();
                switch (elements[selectedId].GetImageSize())
                {
                    case 0:
                        smallImage.IsChecked = true;
                        largeImage.IsChecked = false;
                        mediumImage.IsChecked = false;
                        break;
                    case 2:
                        largeImage.IsChecked = true;
                        smallImage.IsChecked = false;
                        mediumImage.IsChecked = false;
                        break;
                    default:
                        mediumImage.IsChecked = true;
                        smallImage.IsChecked = false;
                        largeImage.IsChecked = false;
                        break;
                }

            }
        }
        private void OpenImageUploadDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = "",
                Filter = "Image files (*.png*, *.jpeg)|*.png;*.jpeg",
                Title = "Open an Image",
                DefaultExt = ".png.jpeg.ico.gif"
            };
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                selectedFile = openFileDialog.FileName;
                Uri uri = new Uri(openFileDialog.FileName);
                image.Source = new BitmapImage(uri);

            }
        }
        private void UpdateImageSize(object sender, RoutedEventArgs e)
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

            int totalRows = tableData.Count(), totalColoums = 0, tmpInt = 0;
            if (totalRows > 0)
            {
                totalColoums = tableData[0].Count();
            }
            List<Grid> tmpGrid = new List<Grid>();
            while (tmpInt < totalRows)
            {
                tmpInt++;
                tmpGrid.Add(AddTableGrid(totalColoums));
            }

            TextBox tmpText;
            // Now that grid is there add all the textboxs to the grid and content
            for (int rows = 0; rows < totalRows; rows++)
            {
                for (int columns = 0; columns < totalColoums; columns++)
                {
                    tmpText = new TextBox();
                    tmpText.Width = 50;
                    tmpText.Text = tableStringData[rows][columns];
                    Grid.SetColumn(tmpText, columns);
                    tmpGrid[columns].Children.Add(tmpText);
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
            Grid.SetRow(tableArea, 3);
            grid.Children.Add(tableArea);
            Grid.SetRow(addBtn, 4);
            grid.Children.Add(addBtn);
            Grid.SetRow(deleteBtn, 4);
            grid.Children.Add(deleteBtn);

            sv.Content = grid;


        }
        private Grid AddTableGrid(int totalColoums)
        {   
            Grid tGrid = new Grid();
            tGrid.Width = totalColoums * 100;
            tGrid.Height = 25;
            tGrid.Background = Brushes.Pink;
            ColumnDefinition column;
            tGrid.ColumnDefinitions.Clear();

            while (totalColoums > 0)
            {
                column = new ColumnDefinition();
                totalColoums--;
                column.Width = new GridLength(25, GridUnitType.Auto);
                tGrid.ColumnDefinitions.Add(column);
            }
            return tGrid;
        }
        private bool SaveData(string type, int id)
        {
            try
            {
                if (title.Text != "") {
                // If id is -1 add an element to elements and change id to that new id
                if (id < 0)
                {
                    elements.Add(new Element());
                    id = elements.Count() - 1;
                }
                // Get title and reset title

                elements[id].SetTitle(title.Text);
                // Set type to element
                elements[id].setElementType(type);
                    // Get type specific data
                    if (type == "Table")
                    {
                        tableStringData = new List<List<String>>();
                        foreach (List<TextBox> data in tableData)
                        {
                            tableStringData.Add(new List<string>());
                            foreach (TextBox textBox in data)
                            {
                                tableStringData.Last().Add(textBox.Text);
                            }
                        }
                    }
                    else if (type == "Paragraph")
                    {
                        elements[id].setBody(body.Text);
                    }
                    else if (type == "List")
                    {
                        List<string> listItems = new List<string>();
                        foreach (TextBox text in listTextBox)
                        {
                            listItems.Add(text.Text);
                        }
                        elements[id].setListItems(listItems);
                    }
                    else if (type == "Figure")
                    {
                        if (smallImage.IsChecked == true)
                        {
                            elements[id].SetImageSize(0);
                        }
                        else if (mediumImage.IsChecked == true)
                        {
                            elements[id].SetImageSize(1);
                        }
                        else
                        {
                            elements[id].SetImageSize(2);
                        }
                        elements[id].setImagLoc(selectedFile);

                    }   
                }
                // Update element list

                // Return true
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
