using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;

namespace LayTexFileCreator
{
    public partial class MainWindow : Window
    {
        // Config / Settings
        Config config = new Config();

        List<Element> elements = new List<Element>();
        List<Page> pages = new List<Page>();
        List<Chapter> chapters = new List<Chapter>();

        // ** NOTE ** lSBtn == lastSelectedButton 

        // Sorting elements vars
        List<Element> sortedElements = new List<Element>();
        List<Button> lSBtn = new List<Button>(), lSBtnSorted = new List<Button>();
        int lSBtnBefore = 0, lSBtnAfter = 0;
        Button removeElementBtn = new Button(), moveUpBtn = new Button(),
            moveDownBtn = new Button(), removeAllElementBtn = new Button(),
            addAllElementBtn = new Button(), addSelectedElement = new Button();
        ListBox sVBefore = new ListBox(), sVAfter = new ListBox();
        // Sorting elements vars
        List<Page> sortedPages = new List<Page>();
        List<Button> lSBtnChapter = new List<Button>(), lSBtnSortedChapter = new List<Button>();
        int lSBtnBeforeChapter = 0, lSBtnAfterChapter = 0;
        Button removeChapterBtn = new Button(), moveUpBtnChapter = new Button(),
            moveDownBtnChapter = new Button(), removeAllChapterBtn = new Button(),
            addAllChapterBtn = new Button(), addSelectedChapter = new Button();
        ListBox sVBeforeChapter = new ListBox(), sVAfterChapter = new ListBox();
        // Sorting elements vars
        List<Chapter> sortedChapters = new List<Chapter>();
        List<Button> lSBtnBook = new List<Button>(), lSBtnSortedBook = new List<Button>();
        int lSBtnBeforeBook = 0, lSBtnAfterBook = 0;
        Button removeBookBtn = new Button(), moveUpBtnBook = new Button(),
            moveDownBtnBook = new Button(), removeAllBookBtn = new Button(),
            addAllBookBtn = new Button(), addSelectedBook = new Button();
        ListBox sVBeforeBook= new ListBox(), sVAfterBook = new ListBox();

        Page page = new Page();
        Book book = new Book();
        Chapter chapter = new Chapter();
        Window popup = new Window();
        Grid popupGrid = new Grid();

        // Create wpf elements
        TextBox body = new TextBox(), title = new TextBox();
        Grid grid = new Grid();
        Label titleLabel = new Label(), bodyLabel = new Label();
        Button deleteBtn = new Button(), addBtn = new Button();
        int selectedId = 0;
        //string currentTitle = "", currentBody = "";

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
        int tmpImageSize = 1;

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

        // Initial window grid
        Grid initialWindowGrid = new Grid();
        

        public MainWindow()
        {
            //OpenInitialWindow();
            InitlizePageEditor();
            //this.Mouse.
        }

        private void InitlizePageEditor()
        {
            //InitlizePageEditor();
            InitializeComponent();
            elements = new List<Element>();
            InitialSetup();
            addBtn.Content = "Add Element";
            body.Text = "";
            title.Text = "";
            InitlizeParagraph("-1");
        }

        private void OpenInitialWindow()
        {
            Window initialWindow = new Window();
            initialWindow.Content = initialWindowGrid;
            initialWindow.Width = 800;
            initialWindow.Height = 600;
            initialWindow.Show();
            initialWindow.Title = "Stonetown Karate Manual Editor";

            initialWindowGrid.Width = initialWindow.Width;
            initialWindowGrid.Height = initialWindow.Height;
        }

        /*
         * Method Name: AddParagraph_Click
         * Method Description: Runs when the "Paragraph" button on the page editor is clicked. Calls 
         *                     on the Initlize paragraph method with a parameter of -1 in order to
         *                     start a new paragraph activity.
         */
        private void AddParagraph_Click(object sender, RoutedEventArgs e)
        {

            addBtn.Content = "Add Element";
            body.Text = "";
            title.Text = "";
            InitlizeParagraph("-1");
        }
        /*
         * Method Name: AddParagraph_Click
         * Method Description: Triggered by the "Add element" button this method calls SaveData to 
         *                     save the current element and UpdateElementList to update the element
         *                     list.
         */
        private void AddClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string[] strlist = btn.Tag.ToString().Split(',');
            SaveData(strlist[0], int.Parse(strlist[1]));
            UpdateElementList();
            addBtn.Content = "Update Element";
        }
        /*
         * Method Name: PushItem_Click
         * Method Description: Triggered by Git-->Push this method calls the file SaveFiles.bat as a process.
         */
        private void PushItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("../SaveFiles.bat");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /*
         * Method Name: CompileItem_Click
         * Method Description: Triggered by Compile-->Compile to Book this method calls the CompileBook method from LayTex.cs 
         */
        private void CompileItem_Click(object sender, RoutedEventArgs e)
        {
            LaTex laTex = new LaTex();
            book.SetChapters(sortedChapters);
            laTex.CompileBook(book);
        }
        /*
         * Method Name: OpenRefItem_Click
         * Method Description: Triggered by Help-->Reference this method opens a LayTex reference Document
         */
        private void OpenRefItem_Click(object sender, RoutedEventArgs e)
        {
            string link = "http://www.icl.utk.edu/~mgates3/docs/latex.pdf";
            Process.Start(link);
        }
        /*
         * Method Name: UpdateItem_Click
         * Method Description: Triggered by Git-->Update this method calls the file UpdateFiles.bat as a process.
         */
        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("../UpdateFiles.bat");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddFigure_Click(object sender, RoutedEventArgs e)
        {
            //Button btn = (Button)sender;
            //elements.Add(new Element());
            addBtn.Content = "Add Element";
            InitlizeFigure(-1);
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string[] strlist = btn.Tag.ToString().Split(',');
            if (int.Parse(strlist[1]) >= 0 && int.Parse(strlist[1]) < elements.Count())
            {
                elements.RemoveAt(int.Parse(strlist[1]));
                UpdateElementList();
            }
            InitlizeParagraph("-1");
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
            title.Text = "";
            body.Text = "";
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
            popup = new Window
            {
                Width = 500,
                Height = 160,
                Background = config.BACKGROUND_COLOR,
                FontSize = 14
            };

            popupTitle.Content = "Page Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Foreground = Brushes.White;
            popupTextbox.FontSize = 14;
            saveButton.Width = 120;
            saveButton.Height = config.BUTTON_HEIGHT;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 10, 0, 0);
            saveButton.Click += SaveElements_Click;
            cancelButton.Width = 120;
            cancelButton.Height = config.BUTTON_HEIGHT;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 10, 27, 0);
            cancelButton.Click += CancelPopup_Click;


            popupGrid.Children.Clear();
            popupGrid.Width = popup.Width;
            popupGrid.Height = popup.Height;

            RowDefinition row;
            popupGrid.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
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
        private void SaveItemSorted_Click(object sender, RoutedEventArgs e)
        {
            if (page.GetDateCreated() == null)
            {
                page.SetDateCreated(DateTime.Now.ToString("h:mm:ss tt"));
            }
            page.SetdateEdited(DateTime.Now.ToString("h:mm:ss tt"));
            page.SetElements(sortedElements);
            string name = "";
            popup = new Window
            {
                Width = 500,
                Height = 160,
                Background = config.BACKGROUND_COLOR,
                FontSize = 14
            };

            popupTitle.Content = "Page Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Foreground = Brushes.White;
            popupTextbox.FontSize = 14;
            saveButton.Width = 120;
            saveButton.Height = config.BUTTON_HEIGHT;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 10, 0, 0);
            saveButton.Click += SaveElements_Click;
            cancelButton.Width = 120;
            cancelButton.Height = config.BUTTON_HEIGHT;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 10, 27, 0);
            cancelButton.Click += CancelPopup_Click;


            popupGrid.Children.Clear();
            popupGrid.Width = popup.Width;
            popupGrid.Height = popup.Height;

            RowDefinition row;
            popupGrid.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
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
        private void OpenItemSorted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Page));
                    System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName);
                    page = (Page)reader.Deserialize(file);
                    file.Close();
                    elements = page.GetElements();
                    UpdateElementList();
                    InitlizeParagraph("-1");
                    title.Text = "";
                    body.Text = "";
                    InitlizeBeforeListBox();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void InitlizeBeforeListBox()
        {
            Button last = new Button();
            int tmpId = 0;
            lSBtn = new List<Button>();
            foreach (Element el in elements)
            {

                last = new Button
                {
                    Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                last.Click += SetBeforeSelection_Click;
                lSBtn.Add(last);
                sVBefore.Items.Add(last);
                tmpId++;
            }
        }
        private void SetBeforeSelection_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtn)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtn[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnBefore = int.Parse(btn.Tag.ToString());
        }
        private void CancelPopup_Click(object sender, RoutedEventArgs e)
        {
            popup.Close();
        }
        private void SaveElements_Click(object sender, RoutedEventArgs e)
        {
            DoDialogSavePage();
        }
        private void DoDialogSavePage()
        {
            try
            {
                //saveFileDialog = new SaveFileDialog();
                if (popupTextbox.Text != "")
                {
                    page.Setname(popupTextbox.Text);
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Xml file|*.xml",
                        Title = "Save a page data File",
                        FileName = popupTextbox.Text + ".xml"
                    };

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Page));
                    System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName);
                    page = (Page)reader.Deserialize(file);
                    file.Close();
                    elements = page.GetElements();
                    UpdateElementList();
                    InitlizeParagraph("-1");
                    title.Text = "";
                    body.Text = "";

                    //File.ReadAllLines(openFileDialog.FileName);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //       txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void OpenBookMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindow = new Window();
            subWindow.Show();
            subWindow.Height = 766;
            subWindow.Width = 1366;
            subWindow.Title = "Book Mode";
            subWindow.ResizeMode = ResizeMode.CanMinimize;
            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();

            GroupBox groupBoxBefore = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 190,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Header = "Non-Sorted View",
                Margin = new Thickness(10),
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxAfter = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 190,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Header = "Sorted View",
                Margin = new Thickness(10),
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxControls = new GroupBox
            {
                Width = subWindow.Width - 40,
                Height = 80,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Header = "Order Controls",
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red,
               
            };

            grid.Width = subWindow.Width - 10;
            grid.Height = subWindow.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Background = config.BACKGROUND_COLOR;
            

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;
            controlsGrid.Background = config.BACKGROUND_COLOR;

            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            gridMain.Background = config.BACKGROUND_COLOR;

            // Add selector controls
            sVBeforeBook = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
               // Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),
            };
            sVAfterBook = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ///Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),
            };

            Menu menu = new Menu
            {
                Margin = new Thickness(5, 20, 0, 0)
            };
            MenuItem file = new MenuItem
            {
                Header = " File"
            };
            MenuItem saveMenuItem = new MenuItem
            {
                Header = " Save"
            };
            saveMenuItem.Click += SaveItemSortedBook_Click;
            MenuItem loadMenuItem = new MenuItem
            {
                Header = " Open"
            };
            loadMenuItem.Click += OpenItemSortedBook_Click;
            MenuItem compile = new MenuItem
            {
                Header = " Compile"
            };
            MenuItem compileFile = new MenuItem
            {
                Header = " Compile to Book"
            };
            compileFile.Click += CompileItem_Click;

            //menu.Background = Brushes.Red;
            menu.Items.Insert(0, file);
            menu.Items.Insert(1, compile);
            file.Items.Insert(0, saveMenuItem);
            file.Items.Insert(1, loadMenuItem);
            compile.Items.Insert(0, compileFile);

            RowDefinition row;
            gridMain.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);

            Grid.SetRow(menu, 0);
            gridMain.Children.Add(menu);
            Grid.SetRow(grid, 1);
            gridMain.Children.Add(grid);

            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);


            Grid.SetRow(groupBoxControls, 0);
            grid.Children.Add(groupBoxControls);
            Grid.SetRow(groupBoxBefore, 1);
            grid.Children.Add(groupBoxBefore);
            Grid.SetRow(groupBoxAfter, 1);
            grid.Children.Add(groupBoxAfter);

            ColumnDefinition column;
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);

            moveUpBtnBook = new Button();
            moveDownBtnBook = new Button();
            removeBookBtn = new Button();
            removeAllBookBtn = new Button();
            addSelectedBook = new Button();
            addAllBookBtn = new Button();

            //Sorted view
            moveUpBtnBook.Width = 120;
            moveUpBtnBook.Height = config.BUTTON_HEIGHT;
            moveUpBtnBook.Content = "Move Up";
            moveUpBtnBook.Click += MoveUpBook_Click;
            moveUpBtnBook.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtnBook.Margin = new Thickness(10, -5, 10, -5);

            moveDownBtnBook.Width = 120;
            moveDownBtnBook.Height = config.BUTTON_HEIGHT;
            moveDownBtnBook.Content = "Move Down";
            moveDownBtnBook.Click += MoveDownBook_Click;
            moveDownBtnBook.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtnBook.Margin = new Thickness(10, -5, 130, -5);

            removeBookBtn.Width = 120;
            removeBookBtn.Height = config.BUTTON_HEIGHT;
            removeBookBtn.Content = "Remove One";
            removeBookBtn.Click += RemoveBook_Click;
            removeBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeBookBtn.Margin = new Thickness(130, -5, 10, -5);

            removeAllBookBtn.Width = 120;
            removeAllBookBtn.Height = config.BUTTON_HEIGHT;
            removeAllBookBtn.Content = "Remove All";
            removeAllBookBtn.Click += RemoveAllBook_Click;
            removeAllBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllBookBtn.Margin = new Thickness(10, -5, 130, -5);

            addSelectedBook.Width = 120;
            addSelectedBook.Height = config.BUTTON_HEIGHT;
            addSelectedBook.Content = "Add One";
            addSelectedBook.Click += AddBook_Click;
            addSelectedBook.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedBook.Margin = new Thickness(125, -5, 10, -5);

            addAllBookBtn.Width = 120;
            addAllBookBtn.Height = config.BUTTON_HEIGHT;
            addAllBookBtn.Content = "Add All";
            addAllBookBtn.Click += AddAllBook_Click;
            addAllBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllBookBtn.Margin = new Thickness(10, -5, 10, -5);

            Grid.SetColumn(moveUpBtnBook, 0);
            controlsGrid.Children.Add(moveUpBtnBook);
            Grid.SetColumn(moveDownBtnBook, 1);
            controlsGrid.Children.Add(moveDownBtnBook);
            Grid.SetColumn(removeBookBtn, 2);
            controlsGrid.Children.Add(removeBookBtn);
            Grid.SetColumn(removeAllBookBtn, 3);
            controlsGrid.Children.Add(removeAllBookBtn);
            Grid.SetColumn(addSelectedBook, 4);
            controlsGrid.Children.Add(addSelectedBook);
            Grid.SetColumn(addAllBookBtn, 5);
            controlsGrid.Children.Add(addAllBookBtn);

            groupBoxControls.Content = controlsGrid;
            groupBoxBefore.Content = sVBeforeBook;
            groupBoxAfter.Content = sVAfterBook;
            subWindow.Content = gridMain;
        }
        private void OpenItemSortedBook_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION + "\\PreCompile\\ChapterBackups";
            dialog.IsFolderPicker = true;
            chapters.Clear();
            List<string> files = new List<string>();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName, "*.xml", SearchOption.AllDirectories).ToList();
            }
            foreach (string file in files)
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Chapter));
                System.IO.StreamReader file1 = new System.IO.StreamReader(file);
                chapters.Add((Chapter)reader.Deserialize(file1));

                file1.Close();
            }
            // First clear all elements 
            sVBeforeBook.Items.Clear();
            lSBtnBook.Clear();

            // Tmp btn and counting var
            Button last;
            int tmpId = 0;
            // Then add all elements
            foreach (Chapter chapter in chapters)
            {
                last = new Button
                {
                    Content = chapter.GetChapterName(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                //pages.Add(page);

                last.Click += SetBeforeSelectionBook_Click;
                lSBtnBook.Add(last);
                sVBeforeBook.Items.Add(last);
                tmpId++;
            }
        }
        private void SetBeforeSelectionBook_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnBook)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtnBook[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnBeforeBook = int.Parse(btn.Tag.ToString()); ;
        }
        private void SaveItemSortedChapter_Click(object sender, RoutedEventArgs e)
        {
            if (chapter.GetDateCreated() == null)
            {
                chapter.SetDateCreated(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
            }
            chapter.SetDateEdited(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
            chapter.SetPages(sortedPages);
            string name = "";
            popup = new Window
            {
                Width = 500,
                Height = 160,
                Background = config.BACKGROUND_COLOR,
                FontSize = 14
            };

            popupTitle.Content = "Chapter Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 10, 0, 0);
            popupTextbox.Foreground = Brushes.White;
            popupTextbox.FontSize = 14;
            saveButton.Width = 120;
            saveButton.Height = config.BUTTON_HEIGHT;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 10, 0, 0);
            saveButton.Click += SaveChapter_Click;
            cancelButton.Width = 120;
            cancelButton.Height = config.BUTTON_HEIGHT;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 10, 27, 0);
            cancelButton.Click += CancelPopup_Click;


            popupGrid.Children.Clear();
            popupGrid.Width = popup.Width;
            popupGrid.Height = popup.Height;

            RowDefinition row;
            popupGrid.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            popupGrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
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
            //page.Setname(name);
        }
        private void SaveChapter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (popupTextbox.Text != "")
                {
                    
                    chapter.SetChapterName(popupTextbox.Text);
                    chapter.SetPages(pages);
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Xml file|*.xml",
                        Title = "Save a page data File",
                        FileName = popupTextbox.Text + "_sorted.xml"
                    };

                    saveFileDialog.ShowDialog();
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlSerializer serializer = new XmlSerializer(typeof(Chapter));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, chapter);
                        stream.Position = 0;
                        xmlDocument.Load(stream);
                        xmlDocument.Save(saveFileDialog.FileName);
                    }
                    popup.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void MoveUpBook_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnAfterBook >= 0 && lSBtnAfterBook > 0)
            {
                sVAfterBook.Items.Clear();
                lSBtnSortedBook.Clear();
                sortedChapters = SwapBook(lSBtnAfterBook, lSBtnAfterBook - 1);
                Button last;
                int tmpId = 0;
                foreach (Chapter chapter in sortedChapters)
                {
                    last = new Button
                    {
                        Content = chapter.GetChapterName(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelectionBook_Click;
                    lSBtnSortedBook.Add(last);
                    sVAfterBook.Items.Add(last);
                    tmpId++;
                }
                lSBtnAfterBook = -1;
            }
        }
        private List<Page> SwapChapter(int a, int b)
        {
            Page tmp = sortedPages[a];
            sortedPages[a] = sortedPages[b];
            sortedPages[b] = tmp;
            return sortedPages;
        }
        private List<Chapter> SwapBook(int a, int b)
        {
            Chapter tmp = sortedChapters[a];
            sortedChapters[a] = sortedChapters[b];
            sortedChapters[b] = tmp;
            return sortedChapters;
        }
        private void MoveDownBook_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnAfterBook >= 0 && lSBtnAfterBook + 1 < sortedChapters.Count())
            {
                sVAfterBook.Items.Clear();
                lSBtnSortedBook.Clear();
                sortedChapters = SwapBook(lSBtnAfterBook, lSBtnAfterBook + 1);
                Button last;
                int tmpId = 0;
                foreach (Chapter chapter in sortedChapters)
                {
                    last = new Button
                    {
                        Content = chapter.GetChapterName(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelectionBook_Click;
                    lSBtnSortedBook.Add(last);
                    sVAfterBook.Items.Add(last);
                    tmpId++;
                }
                lSBtnAfterBook = -1;
            }
        }
        private void RemoveBook_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            if (lSBtnAfterBook != -1 && lSBtnAfterBook < sortedChapters.Count())
            {
                sVAfterBook.Items.RemoveAt(lSBtnAfterBook);
                lSBtnSortedBook.RemoveAt(lSBtnAfterBook);
                sortedChapters.RemoveAt(lSBtnAfterBook);
                lSBtnAfterBook = -1;
            }
            // if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
            sVAfterBook.Items.Clear();
            int tmpId = 0;
            foreach (Button btn in lSBtnSortedBook)
            {
                btn.Tag = tmpId;
                sVAfterBook.Items.Add(btn);
                tmpId++;
            }
        }
        private void RemoveAllBook_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfterBook.Items.Clear();
            lSBtnSortedBook.Clear();
            sortedChapters.Clear();
            lSBtnAfterBook = -1;
        }
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnBeforeBook != -1)
            {
                Button last = new Button
                {
                    Content = chapters[lSBtnBeforeBook].GetChapterName(),
                    Tag = lSBtnSortedBook.Count(),
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sortedChapters.Add(chapters[lSBtnBeforeBook]);
                last.Click += SetAfterSelectionBook_Click;
                lSBtnSortedBook.Add(last);
                sVAfterBook.Items.Add(last);
                lSBtnBeforeBook = -1;
            }
        }
        private void AddAllBook_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfterBook.Items.Clear();
            lSBtnSortedBook.Clear();
            sortedChapters.Clear();
            // Tmp btn and counting var
            Button last;
            int tmpId = 0;
            // Then add all elements
            foreach (Chapter chapter in chapters)
            {
                last = new Button
                {
                    Content = chapter.GetChapterName(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sortedChapters.Add(chapter);

                last.Click += SetAfterSelectionBook_Click;
                lSBtnSortedBook.Add(last);
                sVAfterBook.Items.Add(last);
                tmpId++;
            }
        }
        private void SetAfterSelectionBook_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnSortedBook)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtnSortedBook[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnAfterBook = int.Parse(btn.Tag.ToString());
        }
        private void OpenChapterMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindow = new Window();
            subWindow.Show();
            subWindow.Height = 766;
            subWindow.Width = 1366;
            subWindow.Title = "Chapter Mode";
            subWindow.ResizeMode = ResizeMode.CanMinimize;
            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();

            GroupBox groupBoxBefore = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 190,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Header = "Non-Sorted View",
                Margin = new Thickness(10),
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxAfter = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 190,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Header = "Sorted View",
                FontSize = 14,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(10),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxControls = new GroupBox
            {
                Width = subWindow.Width - 40,
                Height = 80,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Header = "Order Controls",
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };

            grid.Width = subWindow.Width - 10;
            grid.Height = subWindow.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Background = config.BACKGROUND_COLOR;

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;
            controlsGrid.Background = config.BACKGROUND_COLOR;

            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            gridMain.Background = config.BACKGROUND_COLOR;

            // Add selector controls
            sVBeforeChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 50,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
               // Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),
            };
            sVAfterChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 50,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                //Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),
            };

            Menu menu = new Menu
            {
                Margin = new Thickness(5, 20, 0, 0)
            };
            MenuItem file = new MenuItem
            {
                Header = " File"
            };
            MenuItem saveMenuItem = new MenuItem
            {
                Header = " Save"
            };
            saveMenuItem.Click += SaveItemSortedChapter_Click;
            MenuItem loadMenuItem = new MenuItem
            {
                Header = " Open"
            };
            loadMenuItem.Click += OpenItemSortedChapter_Click;

            //menu.Background = Brushes.Red;
            menu.Items.Insert(0, file);
            file.Items.Insert(0, saveMenuItem);
            file.Items.Insert(1, loadMenuItem);


            RowDefinition row;
            gridMain.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);

            Grid.SetRow(menu, 0);
            gridMain.Children.Add(menu);
            Grid.SetRow(grid, 1);
            gridMain.Children.Add(grid);


            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);


            Grid.SetRow(groupBoxControls, 0);
            grid.Children.Add(groupBoxControls);
            Grid.SetRow(groupBoxBefore, 1);
            grid.Children.Add(groupBoxBefore);
            Grid.SetRow(groupBoxAfter, 1);
            grid.Children.Add(groupBoxAfter);

            ColumnDefinition column;
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);

            moveUpBtnChapter = new Button();
            moveDownBtnChapter = new Button();
            removeChapterBtn = new Button();
            removeAllChapterBtn = new Button();
            addSelectedChapter = new Button();
            addAllChapterBtn = new Button();

            //Sorted view
            moveUpBtnChapter.Width = 120;
            moveUpBtnChapter.Height = config.BUTTON_HEIGHT;
            moveUpBtnChapter.Content = "Move Up";
            moveUpBtnChapter.Click += MoveUpChapter_Click;
            moveUpBtnChapter.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtnChapter.Margin = new Thickness(10, -5, 10, -5);

            moveDownBtnChapter.Width = 120;
            moveDownBtnChapter.Height = config.BUTTON_HEIGHT;
            moveDownBtnChapter.Content = "Move Down";
            moveDownBtnChapter.Click += MoveDownChapter_Click;
            moveDownBtnChapter.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtnChapter.Margin = new Thickness(10, -5, 130, -5);

            removeChapterBtn.Width = 120;
            removeChapterBtn.Height = config.BUTTON_HEIGHT;
            removeChapterBtn.Content = "Remove One";
            removeChapterBtn.Click += RemoveChapter_Click;
            removeChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeChapterBtn.Margin = new Thickness(130, -5, 10, -5);

            removeAllChapterBtn.Width = 120;
            removeAllChapterBtn.Height = config.BUTTON_HEIGHT;
            removeAllChapterBtn.Content = "Remove All";
            removeAllChapterBtn.Click += RemoveAllChapter_Click;
            removeAllChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllChapterBtn.Margin = new Thickness(10, -5, 130, -5);

            addSelectedChapter.Width = 120;
            addSelectedChapter.Height = config.BUTTON_HEIGHT;
            addSelectedChapter.Content = "Add One";
            addSelectedChapter.Click += AddChapter_Click;
            addSelectedChapter.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedChapter.Margin = new Thickness(125, -5, 10, -5);

            addAllChapterBtn.Width = 120;
            addAllChapterBtn.Height = config.BUTTON_HEIGHT;
            addAllChapterBtn.Content = "Add All";
            addAllChapterBtn.Click += AddAllChapter_Click;
            addAllChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllChapterBtn.Margin = new Thickness(10, -5, 10, -5);

            Grid.SetColumn(moveUpBtnChapter, 0);
            controlsGrid.Children.Add(moveUpBtnChapter);
            Grid.SetColumn(moveDownBtnChapter, 1);
            controlsGrid.Children.Add(moveDownBtnChapter);
            Grid.SetColumn(removeChapterBtn, 2);
            controlsGrid.Children.Add(removeChapterBtn);
            Grid.SetColumn(removeAllChapterBtn, 3);
            controlsGrid.Children.Add(removeAllChapterBtn);
            Grid.SetColumn(addSelectedChapter, 4);
            controlsGrid.Children.Add(addSelectedChapter);
            Grid.SetColumn(addAllChapterBtn, 5);
            controlsGrid.Children.Add(addAllChapterBtn);

            groupBoxControls.Content = controlsGrid;
            groupBoxBefore.Content = sVBeforeChapter;
            groupBoxAfter.Content = sVAfterChapter;
            subWindow.Content = gridMain;
        }
        private void MoveUpChapter_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnAfterChapter >= 0 && lSBtnAfterChapter > 0)
            {
                sVAfterChapter.Items.Clear();
                lSBtnSortedChapter.Clear();
                sortedPages = SwapChapter(lSBtnAfterChapter, lSBtnAfterChapter - 1);
                Button last;
                int tmpId = 0;
                foreach (Page page in sortedPages)
                {
                    last = new Button
                    {
                        Content = page.Getname(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelectionChapter_Click;
                    lSBtnSortedChapter.Add(last);
                    sVAfterChapter.Items.Add(last);
                    tmpId++;
                }
            }
        }
        private void MoveDownChapter_Click(object sender, RoutedEventArgs e)
        {
          if (lSBtnAfterChapter >= 0 && lSBtnAfterChapter + 1 < sortedPages.Count())
            {
                sVAfterChapter.Items.Clear();
                lSBtnSortedChapter.Clear();
                sortedPages = SwapChapter(lSBtnAfterChapter, lSBtnAfterChapter +  1);
                Button last;
                int tmpId = 0;
                foreach (Page page in sortedPages)
                {
                    last = new Button
                    {
                        Content = page.Getname(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelectionChapter_Click;
                    lSBtnSortedChapter.Add(last);
                    sVAfterChapter.Items.Add(last);
                    tmpId++;
                }
            }
        }
        private void RemoveChapter_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            if (lSBtnAfterChapter != -1 && lSBtnAfterChapter < sortedPages.Count())
            {
                sVAfterChapter.Items.RemoveAt(lSBtnAfterChapter);
                lSBtnSortedChapter.RemoveAt(lSBtnAfterChapter);
                sortedPages.RemoveAt(lSBtnAfterChapter);
                lSBtnAfterChapter = -1;
            }
            // if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
            sVAfterChapter.Items.Clear();
            int tmpId = 0;
            foreach (Button btn in lSBtnSortedChapter)
            {
                btn.Tag = tmpId;
                sVAfterChapter.Items.Add(btn);
                tmpId++;
            }
        }
        private void RemoveAllChapter_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfterChapter.Items.Clear();
            lSBtnSortedChapter.Clear();
            sortedPages.Clear();
            lSBtnAfterChapter = -1;
        }
        private void AddChapter_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnBeforeChapter != -1)
            {
                Button last = new Button
                {
                    Content = pages[lSBtnBeforeChapter].Getname(),
                    Tag = lSBtnSortedBook.Count(),
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sortedPages.Add(pages[lSBtnBeforeChapter]);
                last.Click += SetAfterSelectionBook_Click;
                lSBtnSortedChapter.Add(last);
                sVAfterChapter.Items.Add(last);
                lSBtnBeforeChapter = -1;
            }
        }
        private void AddAllChapter_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfterChapter.Items.Clear();
            lSBtnSortedChapter.Clear();
            sortedPages.Clear();
            // Tmp btn and counting var
            Button last;
            int tmpId = 0;
            // Then add all elements
            foreach (Page page in pages)
            {
                last = new Button
                {
                    Content = page.Getname(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sortedPages.Add(page);

                last.Click += SetAfterSelectionChapter_Click;
                lSBtnSortedChapter.Add(last);
                sVAfterChapter.Items.Add(last);
                tmpId++;
            }
        }
        private void SetAfterSelectionChapter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnSortedChapter)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtnSortedChapter[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnAfterChapter = int.Parse(btn.Tag.ToString());
        }
        private void SaveItemSortedBook_Click(object sender, RoutedEventArgs e)
        {
            //if (chapter.GetDateCreated() == null)
            //{
            //    chapter.SetDateCreated(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
            //}
            //chapter.SetDateEdited(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
            //chapter.SetPages(sortedPages);
            try
            {
                book.SetChapters(chapters);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION + config.DEFAULT_DIRECTORY_LOCATION,
                    Filter = "Xml file|*.xml",
                    Title = "Save a page data File",
                    FileName = DateTime.Now.ToString("yyyy_MM_dd_h_mm_ss_tt") + "_backup.xml"
                };

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
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void OpenItemSortedChapter_Click(object sender, RoutedEventArgs e)
        {
            try {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION ;
            dialog.IsFolderPicker = true;
            List<string> files = new List<string>();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName, "*.xml" ,SearchOption.AllDirectories).ToList();
            }
            foreach(string file in files)
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Page));
                System.IO.StreamReader file1 = new System.IO.StreamReader(file);
                pages.Add((Page)reader.Deserialize(file1));
                
                file1.Close();
            }
            // First clear all elements 
            sVBeforeChapter.Items.Clear();
            lSBtnChapter.Clear();
            
            // Tmp btn and counting var
            Button last;
            int tmpId = 0;
            // Then add all elements
            foreach (Page page in pages)
            {
                last = new Button
                {
                    Content = page.Getname(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                //pages.Add(page);

                last.Click += SetBeforeSelectionChapter_Click;
                lSBtnChapter.Add(last);
                sVBeforeChapter.Items.Add(last);
                tmpId++;
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void SetBeforeSelectionChapter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnChapter)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtnChapter[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnBeforeChapter = int.Parse(btn.Tag.ToString());
        }
        private void OpenPageMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindow = new Window();
            subWindow.Show();
            subWindow.Height = 766;
            subWindow.Width = 1366;
            subWindow.Title = "Page Mode";
            subWindow.ResizeMode = ResizeMode.CanMinimize;
           // subWindow.ResizeMode = ResizeMode.NoResize;

            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();

            GroupBox groupBoxBefore = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 230,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Header = "Non-Sorted View",
                Margin = new Thickness(10),
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxAfter = new GroupBox
            {
                Width = subWindow.Width / 2 - 25,
                Height = subWindow.Height - 230,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Header = "Sorted View",
                FontSize = 14,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(10),
                BorderBrush = Brushes.Red
            };
            GroupBox groupBoxControls = new GroupBox
            {
                Width = subWindow.Width - 40,
                Height = 80,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Header = "Order Controls",
                FontSize = 14,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Red
            };
            //groupBox.Background = Brushes.Blue;
            //groupBox.Margin = new Thickness(5);
            //groupBox.Content = grid;


            grid.Width = subWindow.Width - 10;
            grid.Height = subWindow.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Background = config.BACKGROUND_COLOR;

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;
            controlsGrid.Background = config.BACKGROUND_COLOR;

            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            gridMain.Background = config.BACKGROUND_COLOR;

            //grid.Margin = new Thickness(5);
            // Add selector controls
            sVBeforeChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),

            };
            sVAfterChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = config.CONTROL_COLOR,
                Margin = new Thickness(1),
            };

            Menu menu = new Menu
            {
                Margin = new Thickness(5, 20, 0, 0)
            };
            MenuItem file = new MenuItem
            {
                Header = " File"
            };
            MenuItem saveMenuItem = new MenuItem
            {
                Header = " Save"
            };
            saveMenuItem.Click += SaveItemSorted_Click;
            MenuItem loadMenuItem = new MenuItem
            {
                Header = " Open"
            };
            loadMenuItem.Click += OpenItemSorted_Click;

            //menu.Background = Brushes.Red;
            menu.Items.Insert(0, file);
            file.Items.Insert(0, saveMenuItem);
            file.Items.Insert(1, loadMenuItem);


            RowDefinition row;
            gridMain.RowDefinitions.Clear();
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            gridMain.RowDefinitions.Add(row);

            Grid.SetRow(menu, 0);
            gridMain.Children.Add(menu);
            Grid.SetRow(grid, 1);
            gridMain.Children.Add(grid);

            //groupBoxMain.Content = grid;

            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row);


            Grid.SetRow(groupBoxControls, 0);
            grid.Children.Add(groupBoxControls);
            Grid.SetRow(groupBoxBefore, 1);
            grid.Children.Add(groupBoxBefore);
            Grid.SetRow(groupBoxAfter, 1);
            grid.Children.Add(groupBoxAfter);

            ColumnDefinition column;
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
            controlsGrid.ColumnDefinitions.Add(column);
 

            moveUpBtn = new Button();
            moveDownBtn = new Button();
            removeElementBtn = new Button();
            removeAllElementBtn = new Button();
            addSelectedElement = new Button();
            addAllElementBtn = new Button();

            //Sorted view
            moveUpBtn.Width = 120;
            moveUpBtn.Height = config.BUTTON_HEIGHT;
            moveUpBtn.Content = "Move Up";
            moveUpBtn.Click += MoveElementUp_Click;
            moveUpBtn.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtn.Margin = new Thickness(10, -5, 10, -5);

            moveDownBtn.Width = 120;
            moveDownBtn.Height = config.BUTTON_HEIGHT;
            moveDownBtn.Content = "Move Down";
            moveDownBtn.Click += MoveElementDown_Click;
            moveDownBtn.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtn.Margin = new Thickness(10, -5, 130, -5);

            removeElementBtn.Width = 120;
            removeElementBtn.Height = config.BUTTON_HEIGHT;
            removeElementBtn.Content = "Remove One";
            removeElementBtn.Click += RemoveElement_Click;
            removeElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeElementBtn.Margin = new Thickness(130, -5, 10, -5);

            removeAllElementBtn.Width = 120;
            removeAllElementBtn.Height = config.BUTTON_HEIGHT;
            removeAllElementBtn.Content = "Remove All";
            removeAllElementBtn.Click += RemoveAllElement_Click;
            removeAllElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllElementBtn.Margin = new Thickness(10, -5, 130, -5);

            addSelectedElement.Width = 120;
            addSelectedElement.Height = config.BUTTON_HEIGHT;
            addSelectedElement.Content = "Add One";
            addSelectedElement.Click += AddElement_Click;
            addSelectedElement.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedElement.Margin = new Thickness(125, -5, 10, -5);

            addAllElementBtn.Width = 120;
            addAllElementBtn.Height = config.BUTTON_HEIGHT;
            addAllElementBtn.Content = "Add All";
            addAllElementBtn.Click += AddAllElement_Click;
            addAllElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllElementBtn.Margin = new Thickness(10, -5, 10, -5);

            Grid.SetColumn(moveUpBtn, 0);
            controlsGrid.Children.Add(moveUpBtn);
            Grid.SetColumn(moveDownBtn, 1);
            controlsGrid.Children.Add(moveDownBtn);
            Grid.SetColumn(removeElementBtn, 2);
            controlsGrid.Children.Add(removeElementBtn);
            Grid.SetColumn(removeAllElementBtn, 3);
            controlsGrid.Children.Add(removeAllElementBtn);
            Grid.SetColumn(addSelectedElement, 4);
            controlsGrid.Children.Add(addSelectedElement);
            Grid.SetColumn(addAllElementBtn, 5);
            controlsGrid.Children.Add(addAllElementBtn);

            groupBoxControls.Content = controlsGrid;
            groupBoxBefore.Content = sVBefore;
            groupBoxAfter.Content = sVAfter;


            // gridMain.Children.Add(groupBox);
            subWindow.Content = gridMain;

            // If page was open populate before box with that data
            InitlizeBeforeListBox();
        }
        private void UpdateElementList()
        {
            Button elementNameBttn = new Button();
            int i = 0;
            elementSV.Items.Clear();
            foreach (Element element in elements)
            {
                elementNameBttn = new Button
                {
                    Tag = element.GetElementType() + "," + i
                };
                elementNameBttn.Click += Element_click;
                elementNameBttn.Content = "(" + element.GetElementType() + ") " + element.GetTitle();
                elementNameBttn.Width = elementSV.Width - 20;
                i++;
                elementSV.Items.Add(elementNameBttn);
            }
        }
        private void Element_click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            addBtn.Content = "Update Element";
            string[] strlist = btn.Tag.ToString().Split(',');
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
            if (id == "-1" && listTextBox.Count() < 2)
            {
                title.Text = "";
            }
            // title.Text = "";
            //initlize listS
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            sv.Children.Clear();// = null;
            int idNum = Int32.Parse(id);
            selectedId = idNum;
            //needsAdded = true;

            listBox.Width = grid.Width - 10;

            // Set the tag for add and delete


            TextBox tmp = new TextBox();
            listBox.Items.Clear();
            if (idNum != -1)
            {
                while (listTextBox.Count() < elements[idNum].GetListItems().Count())
                {
                    listTextBox.Add(new TextBox());
                }
            }

           // listBox.Margin

            for (int i = 0; i < listTextBox.Count(); i++)
            {
                tmp = new TextBox();
                tmp = listTextBox.ElementAt(i);
                if (idNum != -1 && i < elements[idNum].GetListItems().Count())
                {
                    tmp.Text = elements[idNum].GetListItems()[i];
                }
                tmp.Name = "listItemTextBox" + i;

                tmp.Width = sv.Width - 50;
                tmp.Height = 30;
                //tmp.FontSize = 12
                tmp.Background = config.CONTROL_COLOR;
                tmp.Foreground = Brushes.White;
                tmp.HorizontalAlignment = HorizontalAlignment.Center;
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
            sv.Children.Add(grid);


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


            if (textBox.Text != "" && int.Parse(textBox.Tag.ToString()) == listTextBox.Count() - 1)
            {
                listTextBox.Add(new TextBox());
                InitlizeList(selectedId.ToString());
                //int tag = int.Parse(textBox.Tag.ToString());
            }
        }
        private void InitlizeParagraph(string id)
        {
            addBtn.Tag = "Paragraph," + id;
            deleteBtn.Tag = "Paragraph," + id;
            //Initlize and setup paragraph
            grid.Children.Clear();
            sv.Children.Clear();
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
            sv.Children.Add(grid);
            if (idNum != -1)
            {
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
            grid.HorizontalAlignment = HorizontalAlignment.Left;

            // Text Fields
            title.Name = "title";
            title.Text = "";
            title.Width = sv.Width - 40;
            title.Height = 30;
            title.FontSize = 14;
            title.Background = Brushes.DimGray;
            title.Foreground = Brushes.White;
            title.ToolTip = "Type your section title here";
            title.HorizontalAlignment = HorizontalAlignment.Center;
           // title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;
            title.Margin = new Thickness(5);
            title.CaretBrush = Brushes.White;

            body.Name = "body";
            body.Text = "";
            body.Height = sv.Height - 150;
            body.Width = sv.Width - 40;
            body.FontSize = 14;
            body.Background = Brushes.DimGray;
            body.Foreground = Brushes.White;
            body.Margin = new Thickness(5);
         //   body.TextChanged += UpdateBody;
            body.SpellCheck.IsEnabled = true;
            body.TextWrapping = 0;
            body.HorizontalAlignment = HorizontalAlignment.Center;
            body.ToolTip = "Type your section body here";
            body.CaretBrush = Brushes.White;
            // Buttons
            deleteBtn.Name = "delete_btn";
            deleteBtn.Content = "Delete";
            deleteBtn.Height = 30;
            deleteBtn.Width = 150;
            deleteBtn.FontSize = 14;
            deleteBtn.Margin = new Thickness(5, 5, 5, 5);
            deleteBtn.FontWeight = FontWeights.Bold;
            deleteBtn.Foreground = Brushes.White;
            deleteBtn.HorizontalAlignment = HorizontalAlignment.Right;
            deleteBtn.Click += DeleteClick;

            addBtn.Name = "add_btn";
            addBtn.Content = "Add Element";
            addBtn.Height = 30;
            addBtn.Width = 150;
            addBtn.FontSize = 14;
            addBtn.Margin = new Thickness(7, 5, 5, 5);
            addBtn.FontWeight = FontWeights.Bold;
            addBtn.Foreground = Brushes.White;
            addBtn.HorizontalAlignment = HorizontalAlignment.Left;
            addBtn.Click += AddClick;

            // Labels
            titleLabel.Name = "titleLabel";
            titleLabel.Content = "Title";
            titleLabel.Width = 135;
            titleLabel.Height = 30;
            titleLabel.FontWeight = FontWeights.SemiBold;
            titleLabel.Foreground = Brushes.White;
            titleLabel.FontSize = 14;
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;

            bodyLabel.Name = "listLabel";
            bodyLabel.Content = "List Items";
            bodyLabel.Width = 135;
            bodyLabel.Height = 30;
            bodyLabel.FontWeight = FontWeights.SemiBold;
            bodyLabel.Foreground = Brushes.White;
            bodyLabel.FontSize = 14;
            bodyLabel.HorizontalAlignment = HorizontalAlignment.Left;

            // addColumnBtn,removeColumnBtn,addRowBtn,removeRowBtn
            removeRowBtn.Width = 100;
            removeRowBtn.Content = "Remove Row";
            removeRowBtn.Click += RemoveRowBtn_Click;  //AddGridColoums;
            removeRowBtn.Height = 30;
            removeRowBtn.HorizontalAlignment = HorizontalAlignment.Right;
            removeRowBtn.Margin = new Thickness(0, 10, 11, 0);

            removeColumnBtn.Width = 100;
            removeColumnBtn.Content = "Remove Column";
            removeColumnBtn.Click += RemoveColumnBtn_Click; //AddGridColoums;
            removeColumnBtn.Height = 30;
            removeColumnBtn.Margin = new Thickness(130, 10, 0, 0);
            removeColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

            addRowBtn.Width = 100;
            addRowBtn.Content = "Add Row";
            addRowBtn.Click += AddRowBtn_Click;  //AddGridColoums;
            addRowBtn.Height = 30;
            addRowBtn.Margin = new Thickness(0, 10, 135, 0);
            addRowBtn.HorizontalAlignment = HorizontalAlignment.Right;

            addColumnBtn.Width = 100;
            addColumnBtn.Content = "Add Column";
            addColumnBtn.Click += AddColumnBtn_Click;  //AddGridColoums;
            addColumnBtn.Height = 30;
            addColumnBtn.Margin = new Thickness(5, 10, 0, 0);
            addColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

            //
            listBox.Height = 450;
            listBox.Background = Brushes.DarkGray;
            listBox.Margin = new Thickness(5);
            listBox.HorizontalAlignment = HorizontalAlignment.Center;
           // listBox.Padding = new Thickness(1);

            tableArea.Height = 200;
            tableArea.Background = Brushes.White;
            tableArea.Margin = new Thickness(5);
            tableArea.HorizontalAlignment = HorizontalAlignment.Left;
            tableArea.Width = sv.Width - 40;

            //removeElementBtn = new Button(), moveUpBtn = new Button(),
            //moveDownBtn = new Button(), removeAllElementBtn = new Button(),
            //addAllElementBtn = new Button(), addSelectedElement = new Button();
        }
        private void AddAllElement_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            if (sVAfter != null)
            {
                sVAfter.Items.Clear();
            }
            lSBtnSorted.Clear();
            sortedElements.Clear();
            // Tmp btn and counting var
            Button last = new Button();
            int tmpId = 0;
            // Then add all elements
            foreach (Element el in elements)
            {
                last = new Button
                {
                    Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                    Tag = tmpId,
                    FontSize = config.FONT_SIZE,
                    Width = 600,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sortedElements.Add(el);

                last.Click += SetAfterSelection_Click;
                lSBtnSorted.Add(last);
                sVAfter.Items.Add(last);
                tmpId++;
            }
        }
        private void SetAfterSelection_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnSorted)
            {
                b.Background = config.GUI_COLOR;
            }
            lSBtnSorted[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
            lSBtnAfter = int.Parse(btn.Tag.ToString());
        }
        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lSBtnBefore != -1)
                {
                    Button last = new Button
                    {
                        Content = "(" + elements[lSBtnBefore].GetElementType() + ") " + elements[lSBtnBefore].GetTitle(),
                        Tag = lSBtnSorted.Count(),
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    sortedElements.Add(elements[lSBtnBefore]);
                    last.Click += SetAfterSelection_Click;
                    lSBtnSorted.Add(last);
                    sVAfter.Items.Add(last);
                    lSBtnBefore = -1;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RemoveAllElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First clear all elements 
                sVAfter.Items.Clear();
                lSBtnSorted.Clear();
                sortedElements.Clear();
                lSBtnAfter = -1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RemoveElement_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            if (lSBtnAfter != -1 && lSBtnAfter < sortedElements.Count())
            {
                sVAfter.Items.RemoveAt(lSBtnAfter);
                lSBtnSorted.RemoveAt(lSBtnAfter);
                sortedElements.RemoveAt(lSBtnAfter);
                lSBtnAfter = -1;
            }
            // if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
            sVAfter.Items.Clear();
            int tmpId = 0;
            foreach (Button btn in lSBtnSorted)
            {
                btn.Tag = tmpId;
                sVAfter.Items.Add(btn);
                tmpId++;
            }

        }
        private List<Element> Swap(int indexA, int indexB)
        {
            Element tmp = sortedElements[indexA];
            sortedElements[indexA] = sortedElements[indexB];
            sortedElements[indexB] = tmp;
            return sortedElements;
        }
        private void MoveElementUp_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnAfter >= 0 && lSBtnAfter < sortedElements.Count())
            {
                sVAfter.Items.Clear();
                lSBtnSorted.Clear();
                sortedElements = Swap(lSBtnAfter, lSBtnAfter - 1);
                Button last;
                int tmpId = 0;
                foreach (Element el in sortedElements)
                {
                    last = new Button
                    {
                        Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelection_Click;
                    lSBtnSorted.Add(last);
                    sVAfter.Items.Add(last);
                    tmpId++;
                }
            }
        }
        private void MoveElementDown_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnAfter >= 0 && lSBtnAfter + 1 < sortedElements.Count())
            {
                sVAfter.Items.Clear();
                lSBtnSorted.Clear();
                sortedElements = Swap(lSBtnAfter, lSBtnAfter + 1);
                Button last;
                int tmpId = 0;
                foreach (Element el in sortedElements)
                {
                    last = new Button
                    {
                        Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                        Tag = tmpId,
                        FontSize = config.FONT_SIZE,
                        Width = 600,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    last.Click += SetAfterSelection_Click;
                    lSBtnSorted.Add(last);
                    sVAfter.Items.Add(last);
                    tmpId++;
                }

            }
        }
        private void AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            RowDefinition row = new RowDefinition
            {
                Height = new GridLength(25, GridUnitType.Auto)
            };
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
            ColumnDefinition column = new ColumnDefinition
            {
                Width = new GridLength(25, GridUnitType.Auto)
            };
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
            sv.Children.Clear();
            // Reset Image, image btn, Size selection radio btns
            titleLabel.Content = "Figure subtext";
            titleLabel.Width = sv.Width;
            title = new TextBox
            {
                Name = "title",
                Text = "",
                Width = sv.Width - 40,
                Height = 30,
                Background = config.CONTROL_COLOR,
                Foreground = config.TEXT_COLOR,
                FontSize = config.FONT_SIZE,
                HorizontalAlignment = HorizontalAlignment.Left
            };
           // title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;
            image = new Image
            {
                Width = 460,
                Height = 460,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            smallImage = new RadioButton
            {
                Content = "Small",
                IsChecked = false,
                GroupName = "imageSize",
                FontSize = config.FONT_SIZE + 2,
                Tag = 0,
                Margin = new Thickness(0, 10, 0, 0),
                Background = config.ACCENT_COLOR,
                Foreground = config.TEXT_COLOR,
                
            };
            smallImage.Click += UpdateImageSize;
            smallImage.HorizontalAlignment = HorizontalAlignment.Left;
            mediumImage = new RadioButton
            {
                Content = "Medium",
                IsChecked = true,
                GroupName = "imageSize",
                Tag = 1,
                Margin = new Thickness(0, 10, 0, 0),
                Background = config.ACCENT_COLOR,
                Foreground = config.TEXT_COLOR,
            };
            mediumImage.Click += UpdateImageSize;
            mediumImage.HorizontalAlignment = HorizontalAlignment.Center;
            largeImage = new RadioButton
            {
                Content = "Large",
                IsChecked = false,
                GroupName = "imageSize",
                Tag = 2,
                Margin = new Thickness(0, 10, 0, 0),
                Background = config.ACCENT_COLOR,
                Foreground = config.TEXT_COLOR,
            };
            largeImage.Click += UpdateImageSize;
            largeImage.HorizontalAlignment = HorizontalAlignment.Right;
            uploadButton = new Button
            {
                Width = 150,
                Height = 50
            };
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
            sv.Children.Add(grid);
            //add data to figure if avalible
            if (selectedId != -1)
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
                Uri uri = new Uri(elements[selectedId].GetImageLocation());
                image.Source = new BitmapImage(uri);

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
            RadioButton btn = (RadioButton)sender;
            tmpImageSize =  int.Parse(btn.Tag.ToString());
        }
        private void InitlizeTable(int id)
        {
            //Steps
            // Clear other Controls
            //int idNum = Int32.Parse(id);
            selectedId = id;
            grid.Children.Clear();
            sv.Children.Clear();
            tableGrid = new Grid();
            tableData = new List<List<TextBox>>();
            // Reset Image, image btn, Size selection radio btns
            titleLabel.Content = "Table Title";
            titleLabel.Width = 100;
            title = new TextBox
            {
                Name = "title",
                Text = "",
                Width = sv.Width / 2,
                Height = 20,
                Background = config.CONTROL_COLOR,
                Foreground = Brushes.Navy,
                HorizontalAlignment = HorizontalAlignment.Left
            };
         //   title.TextChanged += UpdateTitle;
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
                    tmpText = new TextBox
                    {
                        Width = 50,
                        Text = tableStringData[rows][columns]
                    };
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

            sv.Children.Add(grid);


        }
        private Grid AddTableGrid(int totalColoums)
        {
            Grid tGrid = new Grid
            {
                Width = totalColoums * 100,
                Height = 30,
                Background = Brushes.Pink
            };
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
                //selectedId = id;
                //Steps

                if (title.Text != "")
                {
                    // If id is -1 add an element to elements and change id to that new id
                    if (id < 0)
                    {
                        elements.Add(new Element());
                        id = elements.Count() - 1;
                    }
                    addBtn.Tag = type + "," + id;
                    deleteBtn.Tag = type + "," + id;
                    // Get title and reset title

                    elements[id].SetTitle(title.Text);

                    // Set type to element
                    elements[id].SetElementType(type);
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
                        elements[id].SetBody(body.Text);
                    }
                    else if (type == "List")
                    {
                        List<string> listItems = new List<string>();
                        foreach (TextBox text in listTextBox)
                        {
                            listItems.Add(text.Text);
                        }
                        elements[id].SetListItems(listItems);
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
                        if (selectedFile != "")
                        {
                            elements[id].SetImagLoc(selectedFile);
                        }

                    }
                }
                // Update element list

                // Return true
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
