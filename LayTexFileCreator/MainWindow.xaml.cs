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


namespace LayTexFileCreator
{
    public partial class MainWindow : Window
    {
        // Config / Settings
        Config config = new Config();

        List<Element> elements;
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
        ListBox sVBefore, sVAfter;
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
        TextBox title = new TextBox(), body = new TextBox();
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

        //
        

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
            //if (body.Text != "")
            //{
            //    currentBody = body.Text;
            //}
        }
        private void UpdateTitle(object sender, TextChangedEventArgs e)
        {
            //  if (title.Text != "")
            //   {
            //currentTitle = title.Text;
            //  }
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string[] strlist = btn.Tag.ToString().Split(',');
            SaveData(strlist[0], int.Parse(strlist[1]));
            UpdateElementList();
            addBtn.Content = "Update Element";
        }
        private void PushItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("SaveFiles.bat");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void CompileItem_Click(object sender, RoutedEventArgs e)
        {
            LaTex laTex = new LaTex();
            book.SetChapters(sortedChapters);
            laTex.CompileBook(book);
        }
        private void OpenRefItem_Click(object sender, RoutedEventArgs e)
        {
            string link = "http://www.icl.utk.edu/~mgates3/docs/latex.pdf";
            Process.Start(link);
        }
        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("UpdateFiles.bat");
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
                Width = 300,
                Height = 130,
                Background = Brushes.LightGray
            };

            popupTitle.Content = "Page Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 5, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 5, 0, 0);
            saveButton.Width = 50;
            saveButton.Height = 25;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 5, 0, 0);
            saveButton.Click += SaveElements_Click;
            cancelButton.Width = 50;
            cancelButton.Height = 25;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 5, 27, 0);
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
                Width = 300,
                Height = 130,
                Background = Brushes.LightGray
            };

            popupTitle.Content = "Page Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 5, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 5, 0, 0);
            saveButton.Width = 50;
            saveButton.Height = 25;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 5, 0, 0);
            saveButton.Click += SaveElements_Click;
            cancelButton.Width = 50;
            cancelButton.Height = 25;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 5, 27, 0);
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
            Button last;
            int tmpId = 0;
            lSBtn = new List<Button>();
            foreach (Element el in elements)
            {

                last = new Button
                {
                    Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                    Tag = tmpId,
                    Width = sVBefore.Width - 20,
                    Height = 25
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
                b.Background = Brushes.White;
            }
            lSBtn[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
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
            //       txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void OpenBookMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindowBook = new Window();
            subWindowBook.Show();
            subWindowBook.Height = 500;
            subWindowBook.Width = 800;
            subWindowBook.Title = "Book_Mode";
            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();
            GroupBox groupBoxMain = new GroupBox();
            groupBoxMain.Width = subWindowBook.Width - 40;
            groupBoxMain.Height = subWindowBook.Height - 70;
            groupBoxMain.VerticalAlignment = VerticalAlignment.Center;
            groupBoxMain.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxMain.Header = "Book Order Selector";
            groupBoxMain.Margin = new Thickness(5);
            GroupBox groupBoxBefore = new GroupBox();
            groupBoxBefore.Width = subWindowBook.Width / 2 - 40;
            groupBoxBefore.Height = subWindowBook.Height - 160;
            groupBoxBefore.VerticalAlignment = VerticalAlignment.Center;
            groupBoxBefore.HorizontalAlignment = HorizontalAlignment.Left;
            groupBoxBefore.Header = "Non-Sorted View";
            groupBoxBefore.Margin = new Thickness(5);
            GroupBox groupBoxAfter = new GroupBox();
            groupBoxAfter.Width = subWindowBook.Width / 2 - 40;
            groupBoxAfter.Height = subWindowBook.Height - 160;
            groupBoxAfter.VerticalAlignment = VerticalAlignment.Center;
            groupBoxAfter.HorizontalAlignment = HorizontalAlignment.Right;
            groupBoxAfter.Header = "Sorted View";
            groupBoxAfter.Margin = new Thickness(5);
            GroupBox groupBoxControls = new GroupBox();
            groupBoxControls.Width = subWindowBook.Width - 60;
            groupBoxControls.Height = 55;
            groupBoxControls.VerticalAlignment = VerticalAlignment.Center;
            groupBoxControls.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxControls.Header = "Order Controls";
            groupBoxControls.Margin = new Thickness(5, 5, 5, 0);

            grid.Width = groupBoxMain.Width - 10;
            grid.Height = groupBoxMain.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;

            gridMain.Width = subWindowBook.Width;
            gridMain.Height = subWindowBook.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;

            // Add selector controls
            sVBeforeBook = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindowBook.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
            };
            sVAfterBook = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindowBook.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
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
                Header = " Load"
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
            Grid.SetRow(groupBoxMain, 1);
            gridMain.Children.Add(groupBoxMain);

            groupBoxMain.Content = grid;

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
            moveUpBtnBook.Width = 75;
            moveUpBtnBook.Height = 25;
            moveUpBtnBook.Content = "Move Up";
            moveUpBtnBook.Click += MoveUpBook_Click;
            moveUpBtnBook.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtnBook.Margin = new Thickness(10, 2, 10, 0);

            moveDownBtnBook.Width = 75;
            moveDownBtnBook.Height = 25;
            moveDownBtnBook.Content = "Move Down";
            moveDownBtnBook.Click += MoveDownBook_Click;
            moveDownBtnBook.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtnBook.Margin = new Thickness(10, 2, 90, 0);

            removeBookBtn.Width = 75;
            removeBookBtn.Height = 25;
            removeBookBtn.Content = "Remove One";
            removeBookBtn.Click += RemoveBook_Click;
            removeBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeBookBtn.Margin = new Thickness(10, 2, 10, 0);

            removeAllBookBtn.Width = 75;
            removeAllBookBtn.Height = 25;
            removeAllBookBtn.Content = "Remove All";
            removeAllBookBtn.Click += RemoveAllBook_Click;
            removeAllBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllBookBtn.Margin = new Thickness(10, 2, 10, 0);

            addSelectedBook.Width = 75;
            addSelectedBook.Height = 25;
            addSelectedBook.Content = "Add One";
            addSelectedBook.Click += AddBook_Click;
            addSelectedBook.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedBook.Margin = new Thickness(90, 2, 10, 0);

            addAllBookBtn.Width = 75;
            addAllBookBtn.Height = 25;
            addAllBookBtn.Content = "Add All";
            addAllBookBtn.Click += AddAllBook_Click;
            addAllBookBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllBookBtn.Margin = new Thickness(10, 2, 10, 0);

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
            subWindowBook.Content = gridMain;
        }
        private void OpenItemSortedBook_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION = "\\PreCompile\\ChapterBackups";
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
                    Width = sVBeforeBook.Width - 20,
                    Height = 25
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
                b.Background = Brushes.White;
            }
            lSBtnBook[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
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
                Width = 300,
                Height = 130,
                Background = Brushes.LightGray
            };

            popupTitle.Content = "Chapter Title";
            popupTitle.HorizontalAlignment = HorizontalAlignment.Center;
            popupTitle.Margin = new Thickness(-20, 5, 0, 0);
            popupTextbox.Text = "";
            popupTextbox.Width = popup.Width - 35;
            popupTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            popupTextbox.Margin = new Thickness(-20, 5, 0, 0);
            saveButton.Width = 50;
            saveButton.Height = 25;
            saveButton.Content = "Save";
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Margin = new Thickness(8, 5, 0, 0);
            saveButton.Click += SaveChapter_Click;
            cancelButton.Width = 50;
            cancelButton.Height = 25;
            cancelButton.Content = "Cancel";
            cancelButton.HorizontalAlignment = HorizontalAlignment.Right;
            cancelButton.Margin = new Thickness(0, 5, 27, 0);
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
                        Width = sVBeforeBook.Width - 20,
                        Height = 25
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
                        Width = sVBeforeBook.Width - 20,
                        Height = 25
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
                    Width = sVBeforeBook.Width - 20,
                    Height = 25
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
                    Width = sVBeforeBook.Width - 20,
                    Height = 25
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
                b.Background = Brushes.White;
            }
            lSBtnSortedBook[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
            lSBtnAfterBook = int.Parse(btn.Tag.ToString());
        }
        private void OpenChapterMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindowChapter = new Window();
            subWindowChapter.Show();
            subWindowChapter.Height = 500;
            subWindowChapter.Width = 800;
            subWindowChapter.Title = "Chapter_Mode";
            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();
            GroupBox groupBoxMain = new GroupBox();
            groupBoxMain.Width = subWindowChapter.Width - 40;
            groupBoxMain.Height = subWindowChapter.Height - 70;
            groupBoxMain.VerticalAlignment = VerticalAlignment.Center;
            groupBoxMain.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxMain.Header = "Chapter Order Selector";
            groupBoxMain.Margin = new Thickness(5);
            GroupBox groupBoxBefore = new GroupBox();
            groupBoxBefore.Width = subWindowChapter.Width / 2 - 40;
            groupBoxBefore.Height = subWindowChapter.Height - 160;
            groupBoxBefore.VerticalAlignment = VerticalAlignment.Center;
            groupBoxBefore.HorizontalAlignment = HorizontalAlignment.Left;
            groupBoxBefore.Header = "Non-Sorted View";
            groupBoxBefore.Margin = new Thickness(5);
            GroupBox groupBoxAfter = new GroupBox();
            groupBoxAfter.Width = subWindowChapter.Width / 2 - 40;
            groupBoxAfter.Height = subWindowChapter.Height - 160;
            groupBoxAfter.VerticalAlignment = VerticalAlignment.Center;
            groupBoxAfter.HorizontalAlignment = HorizontalAlignment.Right;
            groupBoxAfter.Header = "Sorted View";
            groupBoxAfter.Margin = new Thickness(5);
            GroupBox groupBoxControls = new GroupBox();
            groupBoxControls.Width = subWindowChapter.Width - 60;
            groupBoxControls.Height = 55;
            groupBoxControls.VerticalAlignment = VerticalAlignment.Center;
            groupBoxControls.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxControls.Header = "Order Controls";
            groupBoxControls.Margin = new Thickness(5, 5, 5, 0);

            grid.Width = groupBoxMain.Width - 10;
            grid.Height = groupBoxMain.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;

            gridMain.Width = subWindowChapter.Width;
            gridMain.Height = subWindowChapter.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;

            // Add selector controls
            sVBeforeChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindowChapter.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
            };
            sVAfterChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindowChapter.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
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
                Header = " Load"
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
            Grid.SetRow(groupBoxMain, 1);
            gridMain.Children.Add(groupBoxMain);

            groupBoxMain.Content = grid;

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
            moveUpBtnChapter.Width = 75;
            moveUpBtnChapter.Height = 25;
            moveUpBtnChapter.Content = "Move Up";
            moveUpBtnChapter.Click += MoveUpChapter_Click;
            moveUpBtnChapter.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtnChapter.Margin = new Thickness(10, 2, 10, 0);

            moveDownBtnChapter.Width = 75;
            moveDownBtnChapter.Height = 25;
            moveDownBtnChapter.Content = "Move Down";
            moveDownBtnChapter.Click += MoveDownChapter_Click;
            moveDownBtnChapter.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtnChapter.Margin = new Thickness(10, 2, 90, 0);

            removeChapterBtn.Width = 75;
            removeChapterBtn.Height = 25;
            removeChapterBtn.Content = "Remove One";
            removeChapterBtn.Click += RemoveChapter_Click;
            removeChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeChapterBtn.Margin = new Thickness(10, 2, 10, 0);

            removeAllChapterBtn.Width = 75;
            removeAllChapterBtn.Height = 25;
            removeAllChapterBtn.Content = "Remove All";
            removeAllChapterBtn.Click += RemoveAllChapter_Click;
            removeAllChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllChapterBtn.Margin = new Thickness(10, 2, 10, 0);

            addSelectedChapter.Width = 75;
            addSelectedChapter.Height = 25;
            addSelectedChapter.Content = "Add One";
            addSelectedChapter.Click += AddChapter_Click;
            addSelectedChapter.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedChapter.Margin = new Thickness(90, 2, 10, 0);

            addAllChapterBtn.Width = 75;
            addAllChapterBtn.Height = 25;
            addAllChapterBtn.Content = "Add All";
            addAllChapterBtn.Click += AddAllChapter_Click;
            addAllChapterBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllChapterBtn.Margin = new Thickness(10, 2, 10, 0);

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
            subWindowChapter.Content = gridMain;
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
                        Width = sVBeforeChapter.Width - 20,
                        Height = 25
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
                        Width = sVBeforeChapter.Width - 20,
                        Height = 25
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
                    Width = sVBeforeChapter.Width - 20,
                    Height = 25
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
                    Width = sVBeforeChapter.Width - 20,
                    Height = 25
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
                b.Background = Brushes.White;
            }
            lSBtnSortedChapter[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
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



        private void OpenItemSortedChapter_Click(object sender, RoutedEventArgs e)
        {
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
                    Width = sVBeforeChapter.Width - 20,
                    Height = 25
                };
                //pages.Add(page);

                last.Click += SetBeforeSelectionChapter_Click;
                lSBtnChapter.Add(last);
                sVBeforeChapter.Items.Add(last);
                tmpId++;
            }

        }
        private void SetBeforeSelectionChapter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Button b in lSBtnChapter)
            {
                b.Background = Brushes.White;
            }
            lSBtnChapter[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
            lSBtnBeforeChapter = int.Parse(btn.Tag.ToString());
        }
        private void OpenPageMode_Click(object sender, RoutedEventArgs e)
        {
            // First open new page
            Window subWindow = new Window();
            subWindow.Show();
            subWindow.Height = 500;
            subWindow.Width = 800;
            subWindow.Title = "Page_Mode";
            // Draw GroupBox and Grid
            Grid grid = new Grid(), gridMain = new Grid(), controlsGrid = new Grid();
            GroupBox groupBoxMain = new GroupBox();
            groupBoxMain.Width = subWindow.Width - 40;
            groupBoxMain.Height = subWindow.Height - 70;
            groupBoxMain.VerticalAlignment = VerticalAlignment.Center;
            groupBoxMain.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxMain.Header = "Page Order Selector";
            groupBoxMain.Margin = new Thickness(5);
            GroupBox groupBoxBefore = new GroupBox();
            groupBoxBefore.Width = subWindow.Width / 2 - 40;
            groupBoxBefore.Height = subWindow.Height - 160;
            groupBoxBefore.VerticalAlignment = VerticalAlignment.Center;
            groupBoxBefore.HorizontalAlignment = HorizontalAlignment.Left;
            groupBoxBefore.Header = "Non-Sorted View";
            groupBoxBefore.Margin = new Thickness(5);
            GroupBox groupBoxAfter = new GroupBox();
            groupBoxAfter.Width = subWindow.Width / 2 - 40;
            groupBoxAfter.Height = subWindow.Height - 160;
            groupBoxAfter.VerticalAlignment = VerticalAlignment.Center;
            groupBoxAfter.HorizontalAlignment = HorizontalAlignment.Right;
            groupBoxAfter.Header = "Sorted View";
            groupBoxAfter.Margin = new Thickness(5);
            GroupBox groupBoxControls = new GroupBox();
            groupBoxControls.Width = subWindow.Width - 60;
            groupBoxControls.Height = 55;
            groupBoxControls.VerticalAlignment = VerticalAlignment.Center;
            groupBoxControls.HorizontalAlignment = HorizontalAlignment.Center;
            groupBoxControls.Header = "Order Controls";
            groupBoxControls.Margin = new Thickness(5, 5, 5, 0);
            //groupBox.Background = Brushes.Blue;
            //groupBox.Margin = new Thickness(5);
            //groupBox.Content = grid;


            grid.Width = groupBoxMain.Width - 10;
            grid.Height = groupBoxMain.Height - 10;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;

            controlsGrid.Width = groupBoxControls.Width - 10;
            controlsGrid.Height = groupBoxControls.Height - 10;
            controlsGrid.VerticalAlignment = VerticalAlignment.Center;
            controlsGrid.HorizontalAlignment = HorizontalAlignment.Center;

            gridMain.Width = subWindow.Width;
            gridMain.Height = subWindow.Height;
            gridMain.VerticalAlignment = VerticalAlignment.Center;
            gridMain.HorizontalAlignment = HorizontalAlignment.Center;
            //grid.Margin = new Thickness(5);
            // Add selector controls
            sVBeforeChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
            };
            sVAfterChapter = new ListBox
            {
                Height = groupBoxAfter.Height - 30,
                Width = subWindow.Width / 2 - 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.AntiqueWhite,
                Margin = new Thickness(1)
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
                Header = " Load"
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
            Grid.SetRow(groupBoxMain, 1);
            gridMain.Children.Add(groupBoxMain);

            groupBoxMain.Content = grid;

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
            moveUpBtn.Width = 75;
            moveUpBtn.Height = 25;
            moveUpBtn.Content = "Move Up";
            moveUpBtn.Click += MoveElementUp_Click;
            moveUpBtn.HorizontalAlignment = HorizontalAlignment.Center;
            moveUpBtn.Margin = new Thickness(10, 2, 10, 0);

            moveDownBtn.Width = 75;
            moveDownBtn.Height = 25;
            moveDownBtn.Content = "Move Down";
            moveDownBtn.Click += MoveElementDown_Click;
            moveDownBtn.HorizontalAlignment = HorizontalAlignment.Center;
            moveDownBtn.Margin = new Thickness(10, 2, 90, 0);

            removeElementBtn.Width = 75;
            removeElementBtn.Height = 25;
            removeElementBtn.Content = "Remove One";
            removeElementBtn.Click += RemoveElement_Click;
            removeElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeElementBtn.Margin = new Thickness(10, 2, 10, 0);

            removeAllElementBtn.Width = 75;
            removeAllElementBtn.Height = 25;
            removeAllElementBtn.Content = "Remove All";
            removeAllElementBtn.Click += RemoveAllElement_Click;
            removeAllElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            removeAllElementBtn.Margin = new Thickness(10, 2, 10, 0);

            addSelectedElement.Width = 75;
            addSelectedElement.Height = 25;
            addSelectedElement.Content = "Add One";
            addSelectedElement.Click += AddElement_Click;
            addSelectedElement.HorizontalAlignment = HorizontalAlignment.Center;
            addSelectedElement.Margin = new Thickness(90, 2, 10, 0);

            addAllElementBtn.Width = 75;
            addAllElementBtn.Height = 25;
            addAllElementBtn.Content = "Add All";
            addAllElementBtn.Click += AddAllElement_Click;
            addAllElementBtn.HorizontalAlignment = HorizontalAlignment.Center;
            addAllElementBtn.Margin = new Thickness(10, 2, 10, 0);

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
                elementNameBttn.Width = elementSV.Width - 10;
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

            sv.Content = null;
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

            //removeElementBtn = new Button(), moveUpBtn = new Button(),
            //moveDownBtn = new Button(), removeAllElementBtn = new Button(),
            //addAllElementBtn = new Button(), addSelectedElement = new Button();
        }
        private void AddAllElement_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfter.Items.Clear();
            lSBtnSorted.Clear();
            sortedElements.Clear();
            // Tmp btn and counting var
            Button last;
            int tmpId = 0;
            // Then add all elements
            foreach (Element el in elements)
            {
                last = new Button
                {
                    Content = "(" + el.GetElementType() + ") " + el.GetTitle(),
                    Tag = tmpId,
                    Width = sVBefore.Width - 20,
                    Height = 25
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
                b.Background = Brushes.White;
            }
            lSBtnSorted[int.Parse(btn.Tag.ToString())].Background = config.ACCENT_COLOR;
            lSBtnAfter = int.Parse(btn.Tag.ToString());
        }
        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            if (lSBtnBefore != -1)
            {
                Button last = new Button
                {
                    Content = "(" + elements[lSBtnBefore].GetElementType() + ") " + elements[lSBtnBefore].GetTitle(),
                    Tag = lSBtnSorted.Count(),
                    Width = sVBefore.Width - 20,
                    Height = 25
                };
                sortedElements.Add(elements[lSBtnBefore]);
                last.Click += SetAfterSelection_Click;
                lSBtnSorted.Add(last);
                sVAfter.Items.Add(last);
                lSBtnBefore = -1;
            }
        }
        private void RemoveAllElement_Click(object sender, RoutedEventArgs e)
        {
            // First clear all elements 
            sVAfter.Items.Clear();
            lSBtnSorted.Clear();
            sortedElements.Clear();
            lSBtnAfter = -1;
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
                        Width = sVBefore.Width - 20,
                        Height = 25
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
                        Width = sVBefore.Width - 20,
                        Height = 25
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
            sv.Content = null;
            // Reset Image, image btn, Size selection radio btns
            titleLabel.Content = "Figure subtext";
            titleLabel.Width = 100;
            title = new TextBox
            {
                Name = "title",
                Text = "",
                Width = sv.Width / 2,
                Height = 20,
                Background = Brushes.AntiqueWhite,
                Foreground = Brushes.Navy,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            title.TextChanged += UpdateTitle;
            title.SpellCheck.IsEnabled = true;
            title.TextWrapping = 0;
            image = new Image
            {
                Width = 225,
                Height = 225,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            smallImage = new RadioButton
            {
                Content = "Small",
                IsChecked = false,
                GroupName = "imageSize",
                Tag = 0,
                Margin = new Thickness(20, 5, 0, 0)
            };
            smallImage.Click += UpdateImageSize;
            smallImage.HorizontalAlignment = HorizontalAlignment.Left;
            mediumImage = new RadioButton
            {
                Content = "Medium",
                IsChecked = true,
                GroupName = "imageSize",
                Tag = 1,
                Margin = new Thickness(0, 5, 0, 0)
            };
            mediumImage.Click += UpdateImageSize;
            mediumImage.HorizontalAlignment = HorizontalAlignment.Center;
            largeImage = new RadioButton
            {
                Content = "Large",
                IsChecked = false,
                GroupName = "imageSize",
                Tag = 2,
                Margin = new Thickness(0, 5, 20, 0)
            };
            largeImage.Click += UpdateImageSize;
            largeImage.HorizontalAlignment = HorizontalAlignment.Right;
            uploadButton = new Button
            {
                Width = 50,
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
            sv.Content = grid;
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
            elements[selectedId].SetImageSize(int.Parse(btn.Tag.ToString()));
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
            // Reset Image, image btn, Size selection radio btns
            titleLabel.Content = "Table Title";
            titleLabel.Width = 100;
            title = new TextBox
            {
                Name = "title",
                Text = "",
                Width = sv.Width / 2,
                Height = 20,
                Background = Brushes.AntiqueWhite,
                Foreground = Brushes.Navy,
                HorizontalAlignment = HorizontalAlignment.Left
            };
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

            sv.Content = grid;


        }
        private Grid AddTableGrid(int totalColoums)
        {
            Grid tGrid = new Grid
            {
                Width = totalColoums * 100,
                Height = 25,
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
