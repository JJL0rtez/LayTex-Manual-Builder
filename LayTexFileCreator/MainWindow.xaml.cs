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
using System.Windows.Documents;

namespace LayTexFileCreator {
	public partial class MainWindow : Window {
		// Config / Settings
		Config config = new Config();

		List<Element> elements = new List<Element>();
		List<Page> pages = new List<Page>();
		List<Chapter> chapters = new List<Chapter>();

		// ** NOTE ** lSBtn == lastSelectedButton 

		// Sorting elements vars
		List<Element> sortedElements = new List<Element>();
		List<Button> lSBtn = new List<Button>(),
		lSBtnSorted = new List<Button>();
		int lSBtnBefore = 0,
		lSBtnAfter = 0;
		Button removeElementBtn = new Button(),
		moveUpBtn = new Button(),
		moveDownBtn = new Button(),
		removeAllElementBtn = new Button(),
		addAllElementBtn = new Button(),
		addSelectedElement = new Button();
		ListBox sVBefore = new ListBox(),
		sVAfter = new ListBox();
		// Sorting chapter vars
		List<Page> sortedPages = new List<Page>();
		List<Button> lSBtnChapter = new List<Button>(),
		lSBtnSortedChapter = new List<Button>();
		int lSBtnBeforeChapter = 0,
		lSBtnAfterChapter = 0;
		Button removeChapterBtn = new Button(),
		moveUpBtnChapter = new Button(),
		moveDownBtnChapter = new Button(),
		removeAllChapterBtn = new Button(),
		addAllChapterBtn = new Button(),
		addSelectedChapter = new Button();
		ListBox sVBeforeChapter = new ListBox(),
		sVAfterChapter = new ListBox();
		// Sorting page vars
		List<Chapter> sortedChapters = new List<Chapter>();
		List<Button> lSBtnBook = new List<Button>(),
		lSBtnSortedBook = new List<Button>();
		int lSBtnBeforeBook = 0,
		lSBtnAfterBook = 0;
		Button removeBookBtn = new Button(),
		moveUpBtnBook = new Button(),
		moveDownBtnBook = new Button(),
		removeAllBookBtn = new Button(),
		addAllBookBtn = new Button(),
		addSelectedBook = new Button();
		ListBox sVBeforeBook = new ListBox(),
		sVAfterBook = new ListBox();

		Page page = new Page();
		Book book = new Book();
		Chapter chapter = new Chapter();
		Window popup = new Window();
		Grid popupGrid = new Grid();

		// Create page editor elements
		TextBox title = new TextBox();
		TextBox body = new TextBox();
		Grid grid = new Grid();
		List<Image> iconImage = new List<Image>();

		Label titleLabel = new Label(),
		bodyLabel = new Label(),
		textSizeLabel = new Label();
		Button deleteBtn = new Button(),
		addBtn = new Button(),
		boldBtn = new Button(),
		italicBtn = new Button(),
		smallCapsBtn = new Button(),
		LinkBtn = new Button(),
		leftAllignBtn = new Button(),
		rightAllignBtn = new Button(),
		centerAllignBtn = new Button(),
		closeBtn = new Button(),
		textColorBtn = new Button(),
		underLinebtn = new Button();
		string allign = "left";
		ComboBox textSizeCB = new ComboBox(),
		textColorCB = new ComboBox();

		int selectedId = 0;
		//string currentTitle = "", currentBody = "";

		// List item
		List<TextBox> listTextBox = new List<TextBox>();
		ListBox listBox = new ListBox();

		// Figure
		Image image = new Image();
		Button uploadButton = new Button();
		RadioButton smallImage = new RadioButton(),
		mediumImage = new RadioButton(),
		largeImage = new RadioButton();
		string selectedFile = "";
		//   int tmpImageSize = 1;

		// Table
		List<List<TextBox>> tableData = new List<List<TextBox>>();
		List<List<String>> tableStringData = new List<List<String>>();
		Grid tableGrid = new Grid();
		Button addColumnBtn = new Button(),
		addRowBtn = new Button(),
		removeColumnBtn = new Button(),
		removeRowBtn = new Button();
		ListBox tableArea = new ListBox();
		ComboBox listTypeButton = new ComboBox();
		Label listTypeLabel = new Label();

		// Save Dialog
		Label popupTitle = new Label();
		Button saveButton = new Button(),
		cancelButton = new Button();
		TextBox popupTextbox = new TextBox();

		// Initial window grid
		// Grid initialWindowGrid = new Grid();

		public MainWindow() {
			// Test program before launching to prevent crashes and corruption
			Installer installer = new Installer();
			if (!installer.CheckFileStructure())
			{
				MessageBox.Show("Error in file structure.\nNow running program repair.");
				if (installer.Install())
				{
					MessageBox.Show("Program repair sucessful!\nPlease re-open program.");
				}
				else
				{
					MessageBox.Show("Program repair failed!.\nPlease contact developer.");
				}
				Exit();
			}
			// Initlize program
			InitlizePageEditor();

			//this.Mouse.
		}

		private void InitlizePageEditor() {
			//InitlizePageEditor();
			InitializeComponent();
			elements = new List<Element>();
			InitialSetup();
			addBtn.Content = "Add Element";
			body.Text = "";
			title.Text = "";
			InitlizeParagraph("-1");
		}

		private void OpenSettingsPage() {
			Window window = new Window {
				Height = 768,
				Width = 700,
				Background = config.BACKGROUND_COLOR,
				Name = "Settings",
				Title = "Settings",
				ResizeMode = ResizeMode.NoResize
			};

			Grid grid = new Grid(); //, tmpGrid = new Grid();
			List<Grid> grids = new List<Grid>();
			ListBox listBox = new ListBox {
				Width = 650,
				Height = window.Height - 100,
				//Background = Brushes.Pink
				Background = config.BACKGROUND_COLOR
			};
			RowDefinition row;
			Label parentLocationLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Parent Location",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox parentLocationTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.DEFAULT_DIRECTORY_LOCATION,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(parentLocationLabel, 0);
			grids[grids.Count() - 1].Children.Add(parentLocationLabel);
			Grid.SetRow(parentLocationTextBox, 1);
			grids[grids.Count() - 1].Children.Add(parentLocationTextBox);

			Label CreatedByLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Created by",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox CreatedByTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.CREATED_BY,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(CreatedByLabel, 0);
			grids[grids.Count() - 1].Children.Add(CreatedByLabel);
			Grid.SetRow(CreatedByTextBox, 1);
			grids[grids.Count() - 1].Children.Add(CreatedByTextBox);
			Label BookTitleLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Book Title",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox BookTitleTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.CREATED_BY,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(BookTitleLabel, 0);
			grids[grids.Count() - 1].Children.Add(BookTitleLabel);
			Grid.SetRow(BookTitleTextBox, 1);
			grids[grids.Count() - 1].Children.Add(BookTitleTextBox);
			Label CoverMessageLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Book Cover Message",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox CoverMessageTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.COVER_MESSAGE,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(CoverMessageLabel, 0);
			grids[grids.Count() - 1].Children.Add(CoverMessageLabel);
			Grid.SetRow(CoverMessageTextBox, 1);
			grids[grids.Count() - 1].Children.Add(CoverMessageTextBox);
			Label CoverLogoLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Book Cover Logo Location",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox CoverLogoTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.COVER_LOGO_LOCATION,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(CoverLogoLabel, 0);
			grids[grids.Count() - 1].Children.Add(CoverLogoLabel);
			Grid.SetRow(CoverLogoTextBox, 1);
			grids[grids.Count() - 1].Children.Add(CoverLogoTextBox);
			Label ImageGraphicsPathLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Image Graphics Path",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox ImageGraphicsPathTextBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.IMAGE_GRAPHICS_PATH,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(ImageGraphicsPathLabel, 0);
			grids[grids.Count() - 1].Children.Add(ImageGraphicsPathLabel);
			Grid.SetRow(ImageGraphicsPathTextBox, 1);
			grids[grids.Count() - 1].Children.Add(ImageGraphicsPathTextBox);
			Label PageHeaderTextLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Page Header Text",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			TextBox PageHeaderTexttBox = new TextBox {
				Width = listBox.Width - 10,
				Height = config.BUTTON_HEIGHT,
				Text = config.PAGE_TOP_LEFT_TEXT,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(PageHeaderTextLabel, 0);
			grids[grids.Count() - 1].Children.Add(PageHeaderTextLabel);
			Grid.SetRow(PageHeaderTexttBox, 1);
			grids[grids.Count() - 1].Children.Add(PageHeaderTexttBox);
			Label ShowTableOfContextLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Show table of context?",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			Button ShowTableOfContextTextBox = new Button {
				Width = listBox.Width - 20,
				Height = config.BUTTON_HEIGHT,
				Content = config.CREATED_BY,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			if (config.SHOW_TABLE_OF_CONTENTS) {
				ShowTableOfContextTextBox.Content = "Show";
			} else {
				ShowTableOfContextTextBox.Content = "Hide";
			}
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(ShowTableOfContextLabel, 0);
			grids[grids.Count() - 1].Children.Add(ShowTableOfContextLabel);
			Grid.SetRow(ShowTableOfContextTextBox, 1);
			grids[grids.Count() - 1].Children.Add(ShowTableOfContextTextBox);
			Label ShowListOfFiguresLabel = new Label {

				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Show list of figures?",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			Button ShowListOfFiguresButton = new Button {
				Width = listBox.Width - 20,
				Height = config.BUTTON_HEIGHT,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			if (config.SHOW_LIST_OF_FIGURES) {
				ShowListOfFiguresButton.Content = "Show";
			} else {
				ShowListOfFiguresButton.Content = "Hide";
			}
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(ShowListOfFiguresLabel, 0);
			grids[grids.Count() - 1].Children.Add(ShowListOfFiguresLabel);
			Grid.SetRow(ShowListOfFiguresButton, 1);
			grids[grids.Count() - 1].Children.Add(ShowListOfFiguresButton);
			Label ShowListOfTablesLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Show list of Tables?",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			Button ShowListOfTablesButton = new Button {
				Width = listBox.Width - 20,
				Height = config.BUTTON_HEIGHT,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray
			};
			if (config.SHOW_LIST_OF_TABLES) {
				ShowListOfTablesButton.Content = "Show";
			} else {
				ShowListOfTablesButton.Content = "Hide";
			}
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(ShowListOfTablesLabel, 0);
			grids[grids.Count() - 1].Children.Add(ShowListOfTablesLabel);
			Grid.SetRow(ShowListOfTablesButton, 1);
			grids[grids.Count() - 1].Children.Add(ShowListOfTablesButton);

			Label FontFamilyLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Book Font",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			List<string> fonts = new List<string> {
				"Aharoni",
				"Andalus",
				"AngsanaUPC",
				"Angsana New",
				"Arabic Transparent",
				"Arial",
				"Arial Black",
				"Batang",
				"Browallia New",
				"Comic Sans MS",
				"CordiaUPC",
				"Tunga",
				"Verdana",
				"Vrinda",
				"Webdings",
				"Wingdings",
				"Traditional Arabic",
				"Trebuchet MS",
				"Tahoma",
				"Cordia New",
				"Courier New",
				"David",
				"DFKai-SB",
				"DilleniaUPC",
				"Estrangelo Edessa",
				"EucrosiaUPC",
				"Fixed Miriam Transparent",
				"Franklin Gothic",
				"FrankRuehl",
				"FreesiaUPC",
				"Gautami",
				"Georgia",
				"Gulim",
				"Impact",
				"IrisUPC",
				"JasmineUPC",
				"KaiTi",
				"Kartika",
				"KodchiangUPC",
				"Latha",
				"Levenim MT",
				"LilyUPC",
				"Lucida Console",
				"Lucida Sans",
				"Lucida Sans Unicode",
				"Mangal",
				"Marlett",
				"PMingLiU",
				"Miriam",
				"Miriam Fixed",
				"MS Gothic",
				"MS Mincho",
				"MV Boli",
				"Narkisim",
				"Palatino Linotype",
				"PMingLiU-ExtB",
				"Raavi",
				"Rod",
				"Shruti",
				"SimHei",
				"Simplified Arabic Fixed",
				"Simplified Arabic Fixed",
				"SimSun-ExtB",
				"Sylfaen",
				"Symbol",
				"Times New Roman"
			};
			ComboBox FontFamilyComboBox = new ComboBox {
				Width = listBox.Width - 20,
				Height = config.BUTTON_HEIGHT,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray,
				SelectedIndex = fonts.Count() - 1
			};

			ComboBoxItem comboBoxItem;
			foreach (string font in fonts) {
				comboBoxItem = new ComboBoxItem {
					Content = font
				};
				FontFamilyComboBox.Items.Add(comboBoxItem);
			}
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(FontFamilyLabel, 0);
			grids[grids.Count() - 1].Children.Add(FontFamilyLabel);
			Grid.SetRow(FontFamilyComboBox, 1);
			grids[grids.Count() - 1].Children.Add(FontFamilyComboBox);

			Label TextColorLabel = new Label {
				Width = 150,
				HorizontalAlignment = HorizontalAlignment.Left,
				Height = config.BUTTON_HEIGHT,
				Content = "Book Title Color",
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White
			};
			ComboBox TextColorComboBox = new ComboBox {
				Width = listBox.Width - 20,
				Height = config.BUTTON_HEIGHT,
				FontSize = config.FONT_SIZE,
				Foreground = Brushes.White,
				Background = Brushes.DimGray,
				SelectedIndex = 13

			};
			List<string> colors = new List<string> {
				"black",
				"blue",
				"brown",
				"cyan",
				"darkgray",
				"gray",
				"green",
				"lime",
				"magenta",
				"olive",
				"orange",
				"pink",
				"purple",
				"red",
				"teal",
				"violet",
				"white",
				"yellow"
			};
			foreach (string color in colors) {
				comboBoxItem = new ComboBoxItem {
					Content = color
				};
				TextColorComboBox.Items.Add(comboBoxItem);
			}
			grids.Add(new Grid());
			grids[grids.Count() - 1].RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grids[grids.Count() - 1].RowDefinitions.Add(row);
			Grid.SetRow(TextColorLabel, 0);
			grids[grids.Count() - 1].Children.Add(TextColorLabel);
			Grid.SetRow(TextColorComboBox, 1);
			grids[grids.Count() - 1].Children.Add(TextColorComboBox);
			Button saveSettings = new Button {
				Height = config.BUTTON_HEIGHT,
				Width = 150,
				Content = "Save",
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(10)
			};
			Button cancelSettings = new Button {
				Height = config.BUTTON_HEIGHT,
				Width = 150,
				Content = "Cancel",
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(10)
			};

			grid.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(row);

			listBox.Items.Clear();
			foreach (Grid g in grids) {
				listBox.Items.Add(g);
			}

			Grid.SetRow(listBox, 0);
			grid.Children.Add(listBox);
			Grid.SetRow(saveSettings, 1);
			grid.Children.Add(saveSettings);
			Grid.SetRow(cancelSettings, 1);
			grid.Children.Add(cancelSettings);

			window.Content = grid;
			window.Show();
		}

		//private void OpenInitialWindow()
		//{
		//    Window initialWindow = new Window
		//    {
		//        //initialWindow.U
		//        Content = initialWindowGrid,
		//        Width = 800,
		//        Height = 600
		//    };
		//    initialWindow.Show();
		//    initialWindow.Title = "Stonetown Karate Manual Editor";

		//    initialWindowGrid.Width = initialWindow.Width;
		//    initialWindowGrid.Height = initialWindow.Height;
		//}

		/*
         * Method Name: AddParagraph_Click
         * Method Description: Runs when the "Paragraph" button on the page editor is clicked. Calls 
         *                     on the Initlize paragraph method with a parameter of -1 in order to
         *                     start a new paragraph activity.
         */
		private void AddParagraph_Click(object sender, RoutedEventArgs e) {

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
		private void AddClick(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			string[] strlist = btn.Tag.ToString().Split(',');
			SaveData(strlist[0], int.Parse(strlist[1]));
			UpdateElementList();
			addBtn.Content = "Update Element";
		}
		private void OpenSettingsPage_Click(object sender, RoutedEventArgs e) {
			OpenSettingsPage();
		}
		/*
         * Method Name: PushItem_Click
         * Method Description: Triggered by Git-->Push this method calls the file SaveFiles.bat as a process.
         */
		private void PushItem_Click(object sender, RoutedEventArgs e) {
			try {
				Process.Start("C:\\StonetownKarateManual\\Save Files.bat");
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		/*
         * Method Name: CompileItem_Click
         * Method Description: Triggered by Compile-->Compile to Book this method calls the CompileBook method from LayTex.cs 
         */
		private void CompileItem_Click(object sender, RoutedEventArgs e) {
			LaTex laTex = new LaTex();
			book.SetChapters(sortedChapters);
			laTex.CompileBook(book);
		}
		/*
         * Method Name: OpenRefItem_Click
         * Method Description: Triggered by Help-->Reference this method opens a LayTex reference Document
         */
		private void OpenRefItem_Click(object sender, RoutedEventArgs e) {
			string link = "http://www.icl.utk.edu/~mgates3/docs/latex.pdf";
			Process.Start(link);
		}
		/*
         * Method Name: UpdateItem_Click
         * Method Description: Triggered by Git-->Update this method calls the file UpdateFiles.bat as a process.
         */
		private void UpdateItem_Click(object sender, RoutedEventArgs e) {
			try {
				Process.Start("C:\\StonetownKarateManual\\Update Files.bat");
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void AddFigure_Click(object sender, RoutedEventArgs e) {
			//Button btn = (Button)sender;
			//elements.Add(new Element());
			addBtn.Content = "Add Element";
			InitlizeFigure(-1);
		}
		private void DeleteClick(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			string[] strlist = btn.Tag.ToString().Split(',');
			if (int.Parse(strlist[1]) >= 0 && int.Parse(strlist[1]) < elements.Count()) {
				elements.RemoveAt(int.Parse(strlist[1]));
				UpdateElementList();
			}
			InitlizeParagraph("-1");
		}
		private void AddList_Click(object sender, RoutedEventArgs e) {
			//elements.Add(new Element());
			addBtn.Content = "Add Element";
			listTextBox.Clear();
			listTextBox.Add(new TextBox());
			InitlizeList("-1");
		}
		private void NewItem_Click(object sender, RoutedEventArgs e) {
			//reset page
			elements.Clear();
			UpdateElementList();
			InitlizeParagraph("-1");
			title.Text = "";
			body.Text = "";
		}
		private void SaveItem_Click(object sender, RoutedEventArgs e) {
			if (page.GetDateCreated() == null) {
				page.SetDateCreated(DateTime.Now.ToString("h:mm:ss tt"));
			}
			page.SetdateEdited(DateTime.Now.ToString("h:mm:ss tt"));
			page.SetElements(elements);
			string name = "";
			popup = new Window {
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
			saveButton = new Button {
				Width = 120,
				Height = config.BUTTON_HEIGHT,
				Content = "Save",
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(8, 10, 0, 0)
			};
			saveButton.Click += SaveElements_Click;
			cancelButton = new Button {
				Width = 120,
				Height = config.BUTTON_HEIGHT,
				Content = "Cancel",
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0, 10, 27, 0)
			};
			cancelButton.Click += CancelPopup_Click;

			popupGrid.Children.Clear();
			popupGrid.Width = popup.Width;
			popupGrid.Height = popup.Height;

			RowDefinition row;
			popupGrid.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
		private void SaveItemSorted_Click(object sender, RoutedEventArgs e) {
			if (page.GetDateCreated() == null) {
				page.SetDateCreated(DateTime.Now.ToString("h:mm:ss tt"));
			}
			page.SetdateEdited(DateTime.Now.ToString("h:mm:ss tt"));
			page.SetElements(sortedElements);
			string name = "";
			popup = new Window {
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
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
		private void OpenItemSorted_Click(object sender, RoutedEventArgs e) {
			try {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				if (openFileDialog.ShowDialog() == true) {
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
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void OpenTutorialDocument_Click(object sender, RoutedEventArgs e) {
			//tutorialDocument.pdf
			System.Diagnostics.Process.Start(config.DEFAULT_DIRECTORY_LOCATION + "/bin/tutorialDocument.pdf");
		}
		private void InitlizeBeforeListBox() {
			Button last = new Button();
			int tmpId = 0;
			lSBtn = new List<Button>();
			foreach (Element el in elements) {

				last = new Button {
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
		private void SetBeforeSelection_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtn) {
				b.Background = config.GUI_COLOR;
			}
			lSBtn[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnBefore = int.Parse(btn.Tag.ToString());
		}
		private void CancelPopup_Click(object sender, RoutedEventArgs e) {
			popup.Close();
		}
		private void SaveElements_Click(object sender, RoutedEventArgs e) {
			DoDialogSavePage();
		}
		private void DoDialogSavePage() {
			try {
				//saveFileDialog = new SaveFileDialog();
				if (popupTextbox.Text != "") {
					popup.Close();
					page.Setname(popupTextbox.Text);
					SaveFileDialog saveFileDialog = new SaveFileDialog {
						Filter = "Xml file|*.xml",
						Title = "Save a page data File",
						FileName = popupTextbox.Text + ".xml"
					};

					saveFileDialog.ShowDialog();
					XmlDocument xmlDocument = new XmlDocument();
					XmlSerializer serializer = new XmlSerializer(typeof(Page));
					using (MemoryStream stream = new MemoryStream()) {
						serializer.Serialize(stream, page);
						stream.Position = 0;
						xmlDocument.Load(stream);
						xmlDocument.Save(saveFileDialog.FileName);

					}

				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void OpenItem_Click(object sender, RoutedEventArgs e) {
			try {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				if (openFileDialog.ShowDialog() == true) {
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
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			//       txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
		}
		private void ExitItem_Click(object sender, RoutedEventArgs e) {
			Exit();
		}
		private void OpenBookMode_Click(object sender, RoutedEventArgs e) {
			// First open new page
			Window subWindow = new Window();
			subWindow.Show();
			subWindow.Height = 766;
			subWindow.Width = 1366;
			subWindow.Title = "Book Mode";
			subWindow.ResizeMode = ResizeMode.CanMinimize;
			// Draw GroupBox and Grid
			Grid grid = new Grid(),
			gridMain = new Grid(),
			controlsGrid = new Grid();

			GroupBox groupBoxBefore = new GroupBox {
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
			GroupBox groupBoxAfter = new GroupBox {
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
			GroupBox groupBoxControls = new GroupBox {
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
			sVBeforeBook = new ListBox {
				Height = groupBoxAfter.Height - 55,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.BACKGROUND_COLOR,
				Margin = new Thickness(1),
			};
			sVAfterBook = new ListBox {
				Height = groupBoxAfter.Height - 55,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.BACKGROUND_COLOR,
				Margin = new Thickness(1),
			};

			Menu menu = new Menu {
				Margin = new Thickness(5, 20, 0, 0)
			};
			MenuItem file = new MenuItem {
				Header = " File"
			};
			MenuItem saveMenuItem = new MenuItem {
				Header = " Save"
			};
			saveMenuItem.Click += SaveItemSortedBook_Click;
			MenuItem loadMenuItem = new MenuItem {
				Header = " Open"
			};
			loadMenuItem.Click += OpenItemSortedBook_Click;
			MenuItem compile = new MenuItem {
				Header = " Compile"
			};
			MenuItem compileFile = new MenuItem {
				Header = " Compile to Book"
			};
			compileFile.Click += CompileItem_Click;
			MenuItem help = new MenuItem {
				Header = " Help"
			};
			MenuItem openHelpDoc = new MenuItem {
				Header = " Open Tutorials Document"
			};
			MenuItem gitItem = new MenuItem {
				Header = " Git"
			};
			MenuItem gitUpdate = new MenuItem {
				Header = " Update"
			};
			gitUpdate.Click += UpdateItem_Click;
			MenuItem gitPush = new MenuItem {
				Header = " Push"
			};
			gitPush.Click += PushItem_Click;
			gitItem.Items.Insert(0, gitPush);
			gitItem.Items.Insert(1, gitUpdate);
			openHelpDoc.Click += OpenTutorialDocument_Click;
			help.Items.Insert(0, openHelpDoc);
			menu.Items.Insert(0, file);
			menu.Items.Insert(1, gitItem);
			menu.Items.Insert(2, compile);
			menu.Items.Insert(3, help);
			file.Items.Insert(0, saveMenuItem);
			file.Items.Insert(1, loadMenuItem);
			compile.Items.Insert(0, compileFile);

			RowDefinition row;
			gridMain.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);

			Grid.SetRow(menu, 0);
			gridMain.Children.Add(menu);
			Grid.SetRow(grid, 1);
			gridMain.Children.Add(grid);

			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
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
		private void OpenItemSortedBook_Click(object sender, RoutedEventArgs e) {
			CommonOpenFileDialog dialog = new CommonOpenFileDialog {
				InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION + "\\PreCompile\\ChapterBackups",
				IsFolderPicker = true
			};
			chapters.Clear();
			List<string> files = new List<string>();
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
				files = Directory.GetFiles(dialog.FileName, "*.xml", SearchOption.AllDirectories).ToList();
			}
			foreach (string file in files) {
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
			foreach (Chapter chapter in chapters) {
				last = new Button {
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
		private void SetBeforeSelectionBook_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtnBook) {
				b.Background = config.GUI_COLOR;
			}
			lSBtnBook[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnBeforeBook = int.Parse(btn.Tag.ToString()); ;
		}
		private void SaveItemSortedChapter_Click(object sender, RoutedEventArgs e) {
			if (chapter.GetDateCreated() == null) {
				chapter.SetDateCreated(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
			}
			chapter.SetDateEdited(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
			chapter.SetPages(sortedPages);
			// string name;
			popup = new Window {
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
			saveButton = new Button {
				Width = 120,
				Height = config.BUTTON_HEIGHT,
				Content = "Save",
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(8, 10, 0, 0)
			};
			saveButton.Click += SaveChapter_Click;

			cancelButton = new Button {
				Width = 120,
				Height = config.BUTTON_HEIGHT,
				Content = "Cancel",
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0, 10, 27, 0)
			};
			cancelButton.Click += CancelPopup_Click;

			popupGrid.Children.Clear();
			popupGrid.Width = popup.Width;
			popupGrid.Height = popup.Height;

			RowDefinition row;
			popupGrid.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			popupGrid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
		private void SaveChapter_Click(object sender, RoutedEventArgs e) {
			try {

				if (popupTextbox.Text != "") {

					chapter.SetChapterName(popupTextbox.Text);
					chapter.SetPages(pages);
					popup.Close();
					popup = new Window();
					SaveFileDialog saveFileDialog = new SaveFileDialog {
						Filter = "Xml file|*.xml",
						Title = "Save a page data File",
						FileName = popupTextbox.Text + "_sorted.xml"
					};

					saveFileDialog.ShowDialog();
					XmlDocument xmlDocument = new XmlDocument();
					XmlSerializer serializer = new XmlSerializer(typeof(Chapter));
					using (MemoryStream stream = new MemoryStream()) {
						serializer.Serialize(stream, chapter);
						stream.Position = 0;
						xmlDocument.Load(stream);
						xmlDocument.Save(saveFileDialog.FileName);
					}

				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void MoveUpBook_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfterBook >= 0 && lSBtnAfterBook > 0) {
				sVAfterBook.Items.Clear();
				lSBtnSortedBook.Clear();
				sortedChapters = SwapBook(lSBtnAfterBook, lSBtnAfterBook - 1);
				Button last;
				int tmpId = 0;
				foreach (Chapter chapter in sortedChapters) {
					last = new Button {
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
		private List<Page> SwapChapter(int a, int b) {
			Page tmp = sortedPages[a];
			sortedPages[a] = sortedPages[b];
			sortedPages[b] = tmp;
			return sortedPages;
		}
		private List<Chapter> SwapBook(int a, int b) {
			Chapter tmp = sortedChapters[a];
			sortedChapters[a] = sortedChapters[b];
			sortedChapters[b] = tmp;
			return sortedChapters;
		}
		private void MoveDownBook_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfterBook >= 0 && lSBtnAfterBook + 1 < sortedChapters.Count()) {
				sVAfterBook.Items.Clear();
				lSBtnSortedBook.Clear();
				sortedChapters = SwapBook(lSBtnAfterBook, lSBtnAfterBook + 1);
				Button last;
				int tmpId = 0;
				foreach (Chapter chapter in sortedChapters) {
					last = new Button {
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
		private void RemoveBook_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			if (lSBtnAfterBook != -1 && lSBtnAfterBook < sortedChapters.Count()) {
				sVAfterBook.Items.RemoveAt(lSBtnAfterBook);
				lSBtnSortedBook.RemoveAt(lSBtnAfterBook);
				sortedChapters.RemoveAt(lSBtnAfterBook);
				lSBtnAfterBook = -1;
			}
			// if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
			sVAfterBook.Items.Clear();
			int tmpId = 0;
			foreach (Button btn in lSBtnSortedBook) {
				btn.Tag = tmpId;
				sVAfterBook.Items.Add(btn);
				tmpId++;
			}
		}
		private void RemoveAllBook_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			sVAfterBook.Items.Clear();
			lSBtnSortedBook.Clear();
			sortedChapters.Clear();
			lSBtnAfterBook = -1;
		}
		private void AddBook_Click(object sender, RoutedEventArgs e) {
			if (lSBtnBeforeBook != -1) {
				Button last = new Button {
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
		private void AddAllBook_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			sVAfterBook.Items.Clear();
			lSBtnSortedBook.Clear();
			sortedChapters.Clear();
			// Tmp btn and counting var
			Button last;
			int tmpId = 0;
			// Then add all elements
			foreach (Chapter chapter in chapters) {
				last = new Button {
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
		private void SetAfterSelectionBook_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtnSortedBook) {
				b.Background = config.GUI_COLOR;
			}
			lSBtnSortedBook[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnAfterBook = int.Parse(btn.Tag.ToString());
		}
		private void OpenChapterMode_Click(object sender, RoutedEventArgs e) {
			// First open new page
			Window subWindow = new Window();
			subWindow.Show();
			subWindow.Height = 766;
			subWindow.Width = 1366;
			subWindow.Title = "Chapter Mode";
			subWindow.ResizeMode = ResizeMode.CanMinimize;
			// Draw GroupBox and Grid
			Grid grid = new Grid(),
			gridMain = new Grid(),
			controlsGrid = new Grid();

			GroupBox groupBoxBefore = new GroupBox {
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
			GroupBox groupBoxAfter = new GroupBox {
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
			GroupBox groupBoxControls = new GroupBox {
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
			sVBeforeChapter = new ListBox {
				Height = groupBoxAfter.Height - 55,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.BACKGROUND_COLOR,
				Margin = new Thickness(1),
			};
			sVAfterChapter = new ListBox {
				Height = groupBoxAfter.Height - 55,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.BACKGROUND_COLOR,
				Margin = new Thickness(1),
			};

			Menu menu = new Menu {
				Margin = new Thickness(5, 20, 0, 0)
			};
			MenuItem file = new MenuItem {
				Header = " File"
			};
			MenuItem saveMenuItem = new MenuItem {
				Header = " Save"
			};
			saveMenuItem.Click += SaveItemSortedChapter_Click;
			MenuItem loadMenuItem = new MenuItem {
				Header = " Open"
			};
			loadMenuItem.Click += OpenItemSortedChapter_Click;
			MenuItem help = new MenuItem {
				Header = " Help"
			};
			MenuItem openHelpDoc = new MenuItem {
				Header = " Open Tutorials Document"
			};
			MenuItem gitItem = new MenuItem {
				Header = " Git"
			};
			MenuItem gitUpdate = new MenuItem {
				Header = " Update"
			};
			gitUpdate.Click += UpdateItem_Click;
			MenuItem gitPush = new MenuItem {
				Header = " Push"
			};
			gitPush.Click += PushItem_Click;
			gitItem.Items.Insert(0, gitPush);
			gitItem.Items.Insert(1, gitUpdate);
			openHelpDoc.Click += OpenTutorialDocument_Click;
			help.Items.Insert(0, openHelpDoc);
			menu.Items.Insert(0, file);
			menu.Items.Insert(1, gitItem);
			menu.Items.Insert(2, help);
			file.Items.Insert(0, saveMenuItem);
			file.Items.Insert(1, loadMenuItem);

			RowDefinition row;
			gridMain.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);

			Grid.SetRow(menu, 0);
			gridMain.Children.Add(menu);
			Grid.SetRow(grid, 1);
			gridMain.Children.Add(grid);

			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
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
		private void MoveUpChapter_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfterChapter >= 0 && lSBtnAfterChapter > 0) {
				sVAfterChapter.Items.Clear();
				lSBtnSortedChapter.Clear();
				sortedPages = SwapChapter(lSBtnAfterChapter, lSBtnAfterChapter - 1);
				Button last;
				int tmpId = 0;
				foreach (Page page in sortedPages) {
					last = new Button {
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
		private void MoveDownChapter_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfterChapter >= 0 && lSBtnAfterChapter + 1 < sortedPages.Count()) {
				sVAfterChapter.Items.Clear();
				lSBtnSortedChapter.Clear();
				sortedPages = SwapChapter(lSBtnAfterChapter, lSBtnAfterChapter + 1);
				Button last;
				int tmpId = 0;
				foreach (Page page in sortedPages) {
					last = new Button {
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
		private void RemoveChapter_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			if (lSBtnAfterChapter != -1 && lSBtnAfterChapter < sortedPages.Count()) {
				sVAfterChapter.Items.RemoveAt(lSBtnAfterChapter);
				lSBtnSortedChapter.RemoveAt(lSBtnAfterChapter);
				sortedPages.RemoveAt(lSBtnAfterChapter);
				lSBtnAfterChapter = -1;
			}
			// if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
			sVAfterChapter.Items.Clear();
			int tmpId = 0;
			foreach (Button btn in lSBtnSortedChapter) {
				btn.Tag = tmpId;
				sVAfterChapter.Items.Add(btn);
				tmpId++;
			}
		}
		private void RemoveAllChapter_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			sVAfterChapter.Items.Clear();
			lSBtnSortedChapter.Clear();
			sortedPages.Clear();
			lSBtnAfterChapter = -1;
		}
		private void AddChapter_Click(object sender, RoutedEventArgs e) {
			if (lSBtnBeforeChapter != -1) {
				Button last = new Button {
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
		private void AddAllChapter_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			sVAfterChapter.Items.Clear();
			lSBtnSortedChapter.Clear();
			sortedPages.Clear();
			// Tmp btn and counting var
			Button last;
			int tmpId = 0;
			// Then add all elements
			foreach (Page page in pages) {
				last = new Button {
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
		private void SetAfterSelectionChapter_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtnSortedChapter) {
				b.Background = config.GUI_COLOR;
			}
			lSBtnSortedChapter[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnAfterChapter = int.Parse(btn.Tag.ToString());
		}
		private void SaveItemSortedBook_Click(object sender, RoutedEventArgs e) {
			//if (chapter.GetDateCreated() == null)
			//{
			//    chapter.SetDateCreated(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
			//}
			//chapter.SetDateEdited(DateTime.Now.ToString("yyyy:MM:dd:h:mm:ss tt"));
			//chapter.SetPages(sortedPages);
			try {
				book.SetChapters(chapters);

				SaveFileDialog saveFileDialog = new SaveFileDialog {
					InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION + config.DEFAULT_DIRECTORY_LOCATION,
					Filter = "Xml file|*.xml",
					Title = "Save a page data File",
					FileName = DateTime.Now.ToString("yyyy_MM_dd_h_mm_ss_tt") + "_backup.xml"
				};

				saveFileDialog.ShowDialog();
				XmlDocument xmlDocument = new XmlDocument();
				XmlSerializer serializer = new XmlSerializer(typeof(Page));
				using (MemoryStream stream = new MemoryStream()) {
					serializer.Serialize(stream, page);
					stream.Position = 0;
					xmlDocument.Load(stream);
					xmlDocument.Save(saveFileDialog.FileName);
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void OpenItemSortedChapter_Click(object sender, RoutedEventArgs e) {
			try {
				CommonOpenFileDialog dialog = new CommonOpenFileDialog {
					InitialDirectory = config.DEFAULT_DIRECTORY_LOCATION,
					IsFolderPicker = true
				};
				List<string> files = new List<string>();
				if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
					files = Directory.GetFiles(dialog.FileName, "*.xml", SearchOption.AllDirectories).ToList();
				}
				foreach (string file in files) {
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
				foreach (Page page in pages) {
					last = new Button {
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
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void SetBeforeSelectionChapter_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtnChapter) {
				b.Background = config.GUI_COLOR;
			}
			lSBtnChapter[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnBeforeChapter = int.Parse(btn.Tag.ToString());
		}
		private void OpenPageMode_Click(object sender, RoutedEventArgs e) {
			// First open new page
			Window subWindow = new Window();
			subWindow.Show();
			subWindow.Height = 766;
			subWindow.Width = 1366;
			subWindow.Title = "Page Mode";
			subWindow.ResizeMode = ResizeMode.CanMinimize;
			// subWindow.ResizeMode = ResizeMode.NoResize;

			// Draw GroupBox and Grid
			Grid grid = new Grid(),
			gridMain = new Grid(),
			controlsGrid = new Grid();

			GroupBox groupBoxBefore = new GroupBox {
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
			GroupBox groupBoxAfter = new GroupBox {
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
			GroupBox groupBoxControls = new GroupBox {
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
			sVBeforeChapter = new ListBox {
				Height = groupBoxAfter.Height - 30,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.CONTROL_COLOR,
				Margin = new Thickness(1),

			};
			sVAfterChapter = new ListBox {
				Height = groupBoxAfter.Height - 30,
				Width = subWindow.Width / 2 - 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = config.CONTROL_COLOR,
				Margin = new Thickness(1),
			};

			Menu menu = new Menu {
				Margin = new Thickness(5, 20, 0, 0)
			};
			MenuItem file = new MenuItem {
				Header = " File"
			};
			MenuItem saveMenuItem = new MenuItem {
				Header = " Save"
			};
			saveMenuItem.Click += SaveItemSorted_Click;
			MenuItem loadMenuItem = new MenuItem {
				Header = " Open"
			};
			loadMenuItem.Click += OpenItemSorted_Click;
			MenuItem help = new MenuItem {
				Header = " Help"
			};
			MenuItem openHelpDoc = new MenuItem {
				Header = " Open Tutorials Document"
			};
			MenuItem gitItem = new MenuItem {
				Header = " Git"
			};
			MenuItem gitUpdate = new MenuItem {
				Header = " Update"
			};
			gitUpdate.Click += UpdateItem_Click;
			MenuItem gitPush = new MenuItem {
				Header = " Push"
			};
			gitItem.Items.Insert(0, gitPush);
			gitItem.Items.Insert(1, gitUpdate);
			gitPush.Click += PushItem_Click;
			openHelpDoc.Click += OpenTutorialDocument_Click;
			help.Items.Insert(0, openHelpDoc);
			menu.Items.Insert(0, file);
			menu.Items.Insert(1, gitItem);
			menu.Items.Insert(2, help);
			file.Items.Insert(0, saveMenuItem);
			file.Items.Insert(1, loadMenuItem);

			RowDefinition row;
			gridMain.RowDefinitions.Clear();
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);
			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			gridMain.RowDefinitions.Add(row);

			Grid.SetRow(menu, 0);
			gridMain.Children.Add(menu);
			Grid.SetRow(grid, 1);
			gridMain.Children.Add(grid);

			//groupBoxMain.Content = grid;

			row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			grid.RowDefinitions.Add(row);
			row = new RowDefinition {
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
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			controlsGrid.ColumnDefinitions.Add(column);
			column = new ColumnDefinition {
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
		private void UpdateElementList() {
			Button elementNameBttn = new Button();
			int i = 0;
			elementSV.Items.Clear();
			foreach (Element element in elements) {
				elementNameBttn = new Button {
					Tag = element.GetElementType() + "," + i
				};
				elementNameBttn.Click += Element_click;
				elementNameBttn.Content = "(" + element.GetElementType() + ") " + element.GetTitle();
				elementNameBttn.Width = elementSV.Width - 20;
				i++;
				elementSV.Items.Add(elementNameBttn);
			}
		}
		private void Element_click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			addBtn.Content = "Update Element";
			string[] strlist = btn.Tag.ToString().Split(',');
			if (strlist[0] == "Paragraph") {
				InitlizeParagraph(strlist[1]);
			} else if (strlist[0] == "List") {
				InitlizeList(strlist[1]);
			} else if (strlist[0] == "Figure") {
				InitlizeFigure(int.Parse(strlist[1]));
			} else InitlizeTable(int.Parse(strlist[1]));
		}
		private void AddGridRows(int numRows) {
			RowDefinition row;
			grid.RowDefinitions.Clear();

			while (numRows > 0) {
				row = new RowDefinition();
				numRows--;
				row.Height = new GridLength(25, GridUnitType.Auto);
				grid.RowDefinitions.Add(row);
			}
		}
		private void InitlizeList(string id) {
			addBtn.Tag = "List," + id;
			deleteBtn.Tag = "List," + id;
			titleLabel.Content = "List Title";
			if (id == "-1" && listTextBox.Count() < 2) {
				title.Text = "";
			}
			// title.Text = "";
			//initlize listS
			grid.Children.Clear();
			grid.RowDefinitions.Clear();

			sv.Children.Clear(); // = null;
			int idNum = Int32.Parse(id);
			selectedId = idNum;
			//needsAdded = true;

			listBox.Width = grid.Width - 10;

			// Set the tag for add and delete

			TextBox tmp = new TextBox();
			listBox.Items.Clear();
			if (idNum != -1) {
				while (listTextBox.Count() < elements[idNum].GetListItems().Count()) {
					listTextBox.Add(new TextBox());
				}
			}

			// listBox.Margin

			for (int i = 0; i < listTextBox.Count(); i++) {
				tmp = new TextBox();
				tmp = listTextBox.ElementAt(i);
				if (idNum != -1 && i < elements[idNum].GetListItems().Count()) {
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
			Grid.SetRow(listTypeLabel, 2);
			grid.Children.Add(listTypeLabel);
			Grid.SetRow(listTypeButton, 2);
			grid.Children.Add(listTypeButton);
			Grid.SetRow(listBox, 3);
			grid.Children.Add(listBox);

			// Add grid to the app
			sv.Children.Add(grid);

			Grid.SetRow(addBtn, 4);
			grid.Children.Add(addBtn);
			Grid.SetRow(deleteBtn, 5);
			grid.Children.Add(deleteBtn);

			if (idNum != -1) {
				title.Text = elements.ElementAt(idNum).GetTitle();
				// body.Text = elements.ElementAt(idNum).GetBody();
			}
		}
		private void UpdatelistText(object sender, TextChangedEventArgs e) {
			TextBox textBox = (TextBox)sender;
			//update data in elements

			if (textBox.Text != "" && int.Parse(textBox.Tag.ToString()) == listTextBox.Count() - 1) {
				listTextBox.Add(new TextBox());
				InitlizeList(selectedId.ToString());
				//int tag = int.Parse(textBox.Tag.ToString());
			}
		}
		private void InitlizeParagraph(string id) {
			addBtn.Tag = "Paragraph," + id;
			deleteBtn.Tag = "Paragraph," + id;
			//Initlize and setup paragraph
			grid.Children.Clear();
			sv.Children.Clear();
			AddGridRows(5);
			int idNum = Int32.Parse(id);
			selectedId = idNum;
			titleLabel.Content = "Paragraph Title";
			try {
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\spacing.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\bold.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\italic.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\leftalign.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\rightalign.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\centerAlign.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\text-height.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\link.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\textColor.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\underline.png"))
				});
				iconImage.Add(new Image {
					Source = new BitmapImage(new Uri(@"C:\\StonetownKarateManual\\bin\\images\\close.png"))
				});
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			};

			/** Depricated and moved to settings as a book wide setting **/
			//lineSpacingBtn.Content = iconImage[0];
			//lineSpacingBtn.Width = 35;
			//lineSpacingBtn.Height = 35;
			//lineSpacingBtn.HorizontalAlignment = HorizontalAlignment.Right;
			//lineSpacingBtn.Padding = new Thickness(3);
			//lineSpacingBtn.Margin = new Thickness(0, 0, 5, 0);
			//lineSpacingBtn.Click += SetLineSpacing_Click;
			try
			{
				boldBtn.Content = iconImage[1];
				boldBtn.Width = 35;
				boldBtn.Height = 35;
				boldBtn.HorizontalAlignment = HorizontalAlignment.Right;
				boldBtn.Padding = new Thickness(3);
				boldBtn.Margin = new Thickness(0, 0, 45, 0);
				boldBtn.Click += SetBold_Click;

				italicBtn.Content = iconImage[2];
				italicBtn.Width = 35;
				italicBtn.Height = 35;
				italicBtn.HorizontalAlignment = HorizontalAlignment.Right;
				italicBtn.Padding = new Thickness(3);
				italicBtn.Margin = new Thickness(0, 0, 85, 0);
				italicBtn.Click += SetItalic_Click;

				leftAllignBtn.Content = iconImage[3];
				leftAllignBtn.Width = 35;
				leftAllignBtn.Height = 35;
				leftAllignBtn.HorizontalAlignment = HorizontalAlignment.Right;
				leftAllignBtn.Padding = new Thickness(3);
				leftAllignBtn.Margin = new Thickness(0, 0, 205, 0);
				leftAllignBtn.Click += SetAllignLeft_Click;
				leftAllignBtn.Background = Brushes.DimGray;
				leftAllignBtn.BorderBrush = Brushes.DimGray;

				rightAllignBtn.Content = iconImage[4];
				rightAllignBtn.Width = 35;
				rightAllignBtn.Height = 35;
				rightAllignBtn.HorizontalAlignment = HorizontalAlignment.Right;
				rightAllignBtn.Padding = new Thickness(3);
				rightAllignBtn.Margin = new Thickness(0, 0, 125, 0);
				rightAllignBtn.Click += SetAllignRignt_Click;

				centerAllignBtn.Content = iconImage[5];
				centerAllignBtn.Width = 35;
				centerAllignBtn.Height = 35;
				centerAllignBtn.HorizontalAlignment = HorizontalAlignment.Right;
				centerAllignBtn.Padding = new Thickness(3);
				centerAllignBtn.Margin = new Thickness(0, 0, 165, 0);
				centerAllignBtn.Click += SetAllignCenter_Click;

				smallCapsBtn.Content = iconImage[6];
				smallCapsBtn.Width = 35;
				smallCapsBtn.Height = 35;
				smallCapsBtn.HorizontalAlignment = HorizontalAlignment.Right;
				smallCapsBtn.Padding = new Thickness(3);
				smallCapsBtn.Margin = new Thickness(0, 0, 285, 0);
				smallCapsBtn.Click += SetSmallCaps_Click;

				LinkBtn.Content = iconImage[7];
				LinkBtn.Width = 35;
				LinkBtn.Height = 35;
				LinkBtn.HorizontalAlignment = HorizontalAlignment.Right;
				LinkBtn.Padding = new Thickness(3);
				LinkBtn.Margin = new Thickness(0, 0, 245, 0);
				LinkBtn.Click += SetLink_Click;

				textColorBtn.Content = iconImage[8];
				textColorBtn.Width = 35;
				textColorBtn.Height = 35;
				textColorBtn.HorizontalAlignment = HorizontalAlignment.Right;
				textColorBtn.Padding = new Thickness(3);
				textColorBtn.Margin = new Thickness(0, 0, 325, 0);
				textColorBtn.Click += SetTextColor_Click;

				List<string> colors = new List<string> {
				"black",
				"blue",
				"brown",
				"cyan",
				"darkgray",
				"gray",
				"green",
				"lime",
				"magenta",
				"olive",
				"orange",
				"pink",
				"purple",
				"red",
				"teal",
				"violet",
				"white",
				"yellow"
			};
				textColorCB.ItemsSource = colors;
				textColorCB.Margin = new Thickness(0, 0, 405, 0);
				textColorCB.Padding = new Thickness(5);
				textColorCB.Width = 80;
				textColorCB.Foreground = Brushes.White;
				textColorCB.Background = Brushes.DimGray;
				textColorCB.FontSize = 14;
				textColorCB.HorizontalAlignment = HorizontalAlignment.Right;

				underLinebtn.Content = iconImage[9];
				underLinebtn.Width = 35;
				underLinebtn.Height = 35;
				underLinebtn.HorizontalAlignment = HorizontalAlignment.Right;
				underLinebtn.Padding = new Thickness(3);
				underLinebtn.Margin = new Thickness(0, 0, 5, 0);
				underLinebtn.Click += SetUnderline_Click;

				textSizeCB.SelectedIndex = 4;
				textSizeCB.Text = "normal";
				List<string> textSizes = new List<string> {
				"tiny",
				"scriptsize",
				"footnotesize",
				"small",
				"normal",
				"large",
				"Large",
				"LARGE",
				"huge",
				"Huge"
			};
				textSizeCB.ItemsSource = textSizes;
				textSizeCB.Margin = new Thickness(0, 0, 495, 0);
				textSizeCB.Padding = new Thickness(5);
				textSizeCB.Width = 80;
				textSizeCB.Foreground = Brushes.White;
				textSizeCB.Background = Brushes.DimGray;
				textSizeCB.FontSize = 14;
				textSizeCB.HorizontalAlignment = HorizontalAlignment.Right;
				textSizeCB.SelectionChanged += SetTextSize_Click;

				textSizeLabel.Content = "Text Size";
				textSizeLabel.Foreground = Brushes.White;
				textSizeLabel.HorizontalAlignment = HorizontalAlignment.Right;
				textSizeLabel.FontSize = 14;
				textSizeLabel.Margin = new Thickness(0, 0, 580, 0);
				textSizeLabel.VerticalAlignment = VerticalAlignment.Center;

				closeBtn.Content = iconImage[10];
				closeBtn.Width = 35;
				closeBtn.Height = 35;
				closeBtn.HorizontalAlignment = HorizontalAlignment.Right;
				closeBtn.Padding = new Thickness(3);
				closeBtn.Margin = new Thickness(0, 0, 365, 0);
				closeBtn.Click += SetClearFormatting_Click;

			// Add elements to the grid
			Grid.SetRow(titleLabel, 0);
			grid.Children.Add(titleLabel);
			Grid.SetRow(title, 1);
			grid.Children.Add(title);
			Grid.SetRow(closeBtn, 2);
			grid.Children.Add(closeBtn);
			Grid.SetRow(boldBtn, 2);
			grid.Children.Add(boldBtn);
			Grid.SetRow(italicBtn, 2);
			grid.Children.Add(italicBtn);
			Grid.SetRow(LinkBtn, 2);
			grid.Children.Add(LinkBtn);
			Grid.SetRow(textColorBtn, 2);
			grid.Children.Add(textColorBtn);
			Grid.SetRow(smallCapsBtn, 2);
			grid.Children.Add(smallCapsBtn);
			Grid.SetRow(centerAllignBtn, 2);
			grid.Children.Add(centerAllignBtn);
			Grid.SetRow(rightAllignBtn, 2);
			grid.Children.Add(rightAllignBtn);
			Grid.SetRow(leftAllignBtn, 2);
			grid.Children.Add(leftAllignBtn);
			Grid.SetRow(textSizeCB, 2);
			grid.Children.Add(textSizeCB);
			Grid.SetRow(bodyLabel, 2);
			grid.Children.Add(bodyLabel);
			Grid.SetRow(textSizeLabel, 2);
			grid.Children.Add(textSizeLabel);
			Grid.SetRow(underLinebtn, 2);
			grid.Children.Add(underLinebtn);
			Grid.SetRow(textColorCB, 2);
			grid.Children.Add(textColorCB);
			Grid.SetRow(body, 3);
			grid.Children.Add(body);
			Grid.SetRow(addBtn, 4);
			grid.Children.Add(addBtn);
			Grid.SetRow(deleteBtn, 4);
			grid.Children.Add(deleteBtn);

			// Add grid to the app
			while (grid.RowDefinitions.Count() > 6) {
				grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count() - 1);
			}
			sv.Children.Add(grid);
			if (idNum != -1) {
				title.Text = elements.ElementAt(idNum).GetTitle();
				body.Text = elements.ElementAt(idNum).GetBody();
				// body.Text = ;
			}
			}
			catch (ArgumentOutOfRangeException ex)
			{
				// This type of error will be triggered if files are missing from the install
				MessageBox.Show("Error in file structure.\nNow running program repair.");
				Installer installer = new Installer();
				if (installer.Install())
				{
					MessageBox.Show("Program repair sucessful!\nPlease re-open program.");
				}
				else
				{
					MessageBox.Show("Program repair failed!.\nPlease contact developer.");
				}
				Exit();
				
			}
		}

		private void Exit()
		{
			System.Windows.Application.Current.Shutdown();
		}

		private void SetClearFormatting_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					string tmpBody = body.Text,
					tmpString = "";
					int startIndex = 0,
					lastIndex = 0;
					List<string> testData = new List<string> {
					"textul",
					"textsc",
					"textbf",
					"tiny",
					"scriptsize",
					"footnotesize",
					"small",
					"normal",
					"large",
					"Large",
					"LARGE",
					"huge",
					"Huge",
					"textit"
				};

					for (; ; ) {
						if (tmpBody.Length > 0 && tmpBody[0] == '{') {
							tmpBody = tmpBody.Remove(0, 1);
						} else {
							startIndex = tmpBody.IndexOf("\\");
							if (startIndex == -1) {
								break;
							}
							lastIndex = tmpBody.IndexOf("{");
							tmpString = tmpBody.Substring(startIndex + 1, lastIndex - startIndex - 1);
							if (!testData.Contains(tmpString)) {
								lastIndex = tmpBody.IndexOf("}");
							}
							tmpBody = tmpBody.Remove(startIndex, lastIndex - startIndex + 1);
							tmpBody = tmpBody.Remove(tmpBody.Length - 1);
						}
					}
					body.Text = tmpBody;
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetTextSize_Click(object sender, SelectionChangedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\" + textSizeCB.SelectedItem.ToString() + "{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetAllignRignt_Click(object sender, RoutedEventArgs e) {
			rightAllignBtn.Background = Brushes.DimGray;
			rightAllignBtn.BorderBrush = Brushes.DimGray;
			leftAllignBtn.Background = config.GUI_COLOR;
			leftAllignBtn.BorderBrush = config.GUI_COLOR;
			centerAllignBtn.Background = config.GUI_COLOR;
			centerAllignBtn.BorderBrush = config.GUI_COLOR;
			allign = "right";
			body.TextAlignment = TextAlignment.Right;
		}

		private void SetAllignCenter_Click(object sender, RoutedEventArgs e) {
			rightAllignBtn.Background = config.GUI_COLOR;
			rightAllignBtn.BorderBrush = config.GUI_COLOR;
			leftAllignBtn.Background = config.GUI_COLOR;
			leftAllignBtn.BorderBrush = config.GUI_COLOR;
			centerAllignBtn.Background = Brushes.DimGray;
			centerAllignBtn.BorderBrush = Brushes.DimGray;
			allign = "center";
			body.TextAlignment = TextAlignment.Center;
		}

		private void SetAllignLeft_Click(object sender, RoutedEventArgs e) {
			rightAllignBtn.Background = config.GUI_COLOR;
			rightAllignBtn.BorderBrush = config.GUI_COLOR;
			leftAllignBtn.Background = Brushes.DimGray;
			leftAllignBtn.BorderBrush = Brushes.DimGray;
			centerAllignBtn.Background = config.GUI_COLOR;
			centerAllignBtn.BorderBrush = config.GUI_COLOR;
			allign = "left";
			body.TextAlignment = TextAlignment.Left;
		}

		private void SetTextColor_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\textcolor{" + textColorCB.SelectedItem.ToString() + "}{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetUnderline_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\textul{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetLink_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\href{" + body.SelectedText + "}{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetSmallCaps_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\textsc{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetItalic_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\textit{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void SetBold_Click(object sender, RoutedEventArgs e) {
			if (body.SelectionLength > 0) try {
					int pos = body.SelectionStart;
					string tmpText = "\\textbf{" + body.SelectedText + "}";
					body.Text = body.Text.Substring(0, pos) + tmpText + body.Text.Substring(pos + body.SelectionLength);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
		}

		private void InitialSetup() {
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
			body.Height = sv.Height - 170;
			body.Width = sv.Width - 40;
			body.FontSize = 14;
			body.Background = Brushes.DimGray;
			body.Foreground = Brushes.White;
			body.Margin = new Thickness(5);
			//   body.TextChanged += UpdateBody;
			body.SpellCheck.IsEnabled = true;
			body.TextWrapping = 0;
			// body.TextWrapping = 0;
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
			titleLabel.Width = 150;
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
			removeRowBtn.Click += RemoveRowBtn_Click; //AddGridColoums;
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
			addRowBtn.Click += AddRowBtn_Click; //AddGridColoums;
			addRowBtn.Height = 30;
			addRowBtn.Margin = new Thickness(0, 10, 135, 0);
			addRowBtn.HorizontalAlignment = HorizontalAlignment.Right;

			addColumnBtn.Width = 100;
			addColumnBtn.Content = "Add Column";
			addColumnBtn.Click += AddColumnBtn_Click; //AddGridColoums;
			addColumnBtn.Height = 30;
			addColumnBtn.Margin = new Thickness(5, 10, 0, 0);
			addColumnBtn.HorizontalAlignment = HorizontalAlignment.Left;

			listTypeLabel.HorizontalAlignment = HorizontalAlignment.Right;
			listTypeLabel.FontSize = 14;
			listTypeLabel.Content = "List Type";
			listTypeLabel.Foreground = Brushes.White;
			listTypeLabel.Margin = new Thickness(0, 0, 135, 0);
			listTypeLabel.FontWeight = FontWeights.SemiBold;

			listTypeButton.HorizontalAlignment = HorizontalAlignment.Right;
			listTypeButton.Width = 130;
			listTypeButton.Height = 30;
			List<string> listTypes = new List<string> {
				" Bullet",
				" Numbered",
				" Roman Numeral"
			};
			listTypeButton.ItemsSource = listTypes;
			listTypeButton.SelectedIndex = 1;
			listTypeButton.Margin = new Thickness(0, 0, 5, 0);
			listTypeButton.Foreground = Brushes.White;
			listTypeButton.Background = Brushes.DimGray;
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
		private void AddAllElement_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			if (sVAfter != null) {
				sVAfter.Items.Clear();
			}
			lSBtnSorted.Clear();
			sortedElements.Clear();
			// Tmp btn and counting var
			Button last = new Button();
			int tmpId = 0;
			// Then add all elements
			foreach (Element el in elements) {
				last = new Button {
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
		private void SetAfterSelection_Click(object sender, RoutedEventArgs e) {
			Button btn = (Button)sender;
			foreach (Button b in lSBtnSorted) {
				b.Background = config.GUI_COLOR;
			}
			lSBtnSorted[int.Parse(btn.Tag.ToString())].Background = Brushes.DimGray;
			lSBtnAfter = int.Parse(btn.Tag.ToString());
		}
		private void AddElement_Click(object sender, RoutedEventArgs e) {
			try {
				if (lSBtnBefore != -1) {
					Button last = new Button {
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
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void RemoveAllElement_Click(object sender, RoutedEventArgs e) {
			try {
				// First clear all elements 
				sVAfter.Items.Clear();
				lSBtnSorted.Clear();
				sortedElements.Clear();
				lSBtnAfter = -1;
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void RemoveElement_Click(object sender, RoutedEventArgs e) {
			// First clear all elements 
			if (lSBtnAfter != -1 && lSBtnAfter < sortedElements.Count()) {
				sVAfter.Items.RemoveAt(lSBtnAfter);
				lSBtnSorted.RemoveAt(lSBtnAfter);
				sortedElements.RemoveAt(lSBtnAfter);
				lSBtnAfter = -1;
			}
			// if item is Deleted then retag all items to prevent stack overflow due to incorrect list usage
			sVAfter.Items.Clear();
			int tmpId = 0;
			foreach (Button btn in lSBtnSorted) {
				btn.Tag = tmpId;
				sVAfter.Items.Add(btn);
				tmpId++;
			}

		}
		private List<Element> Swap(int indexA, int indexB) {
			Element tmp = sortedElements[indexA];
			sortedElements[indexA] = sortedElements[indexB];
			sortedElements[indexB] = tmp;
			return sortedElements;
		}
		private void MoveElementUp_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfter >= 0 && lSBtnAfter < sortedElements.Count()) {
				sVAfter.Items.Clear();
				lSBtnSorted.Clear();
				sortedElements = Swap(lSBtnAfter, lSBtnAfter - 1);
				Button last;
				int tmpId = 0;
				foreach (Element el in sortedElements) {
					last = new Button {
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
		private void MoveElementDown_Click(object sender, RoutedEventArgs e) {
			if (lSBtnAfter >= 0 && lSBtnAfter + 1 < sortedElements.Count()) {
				sVAfter.Items.Clear();
				lSBtnSorted.Clear();
				sortedElements = Swap(lSBtnAfter, lSBtnAfter + 1);
				Button last;
				int tmpId = 0;
				foreach (Element el in sortedElements) {
					last = new Button {
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
		private void AddRowBtn_Click(object sender, RoutedEventArgs e) {
			RowDefinition row = new RowDefinition {
				Height = new GridLength(25, GridUnitType.Auto)
			};
			tableGrid.RowDefinitions.Add(row);
		}
		private void RemoveColumnBtn_Click(object sender, RoutedEventArgs e) {
			if (tableGrid.ColumnDefinitions.Count() > 0) {
				tableGrid.ColumnDefinitions.RemoveAt(tableGrid.ColumnDefinitions.Count() - 1);
			};
		}
		private void RemoveRowBtn_Click(object sender, RoutedEventArgs e) {
			if (tableGrid.RowDefinitions.Count() > 0) {
				tableGrid.RowDefinitions.RemoveAt(tableGrid.RowDefinitions.Count() - 1);
			};
		}
		private void AddTable_Click(object sender, RoutedEventArgs e) {
			//elements.Add(new Element());
			InitlizeTable(elements.Count());
		}
		private void AddColumnBtn_Click(object sender, RoutedEventArgs e) {
			ColumnDefinition column = new ColumnDefinition {
				Width = new GridLength(25, GridUnitType.Auto)
			};
			tableGrid.ColumnDefinitions.Add(column);
		}
		private void InitlizeFigure(int id) {
			//Steps
			addBtn.Tag = "Figure," + id;
			deleteBtn.Tag = "Figure," + id;
			// Clear other Controls
			selectedId = id;
			grid.Children.Clear();
			sv.Children.Clear();
			// Reset Image, image btn, Size selection radio btns
			titleLabel.Content = "Figure subtext";
			titleLabel.Width = sv.Width;
			title = new TextBox {
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
			image = new Image {
				Width = 460,
				Height = 460,
				Stretch = Stretch.Uniform,
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			smallImage = new RadioButton {
				Content = "Small",
				IsChecked = false,
				GroupName = "imageSize",
				FontSize = config.FONT_SIZE + 2,
				Tag = 0,
				Margin = new Thickness(0, 10, 0, 0),
				Background = config.ACCENT_COLOR,
				Foreground = config.TEXT_COLOR,

			};
			//   smallImage.Click += UpdateImageSize;
			smallImage.HorizontalAlignment = HorizontalAlignment.Left;
			mediumImage = new RadioButton {
				Content = "Medium",
				IsChecked = true,
				GroupName = "imageSize",
				Tag = 1,
				Margin = new Thickness(0, 10, 0, 0),
				Background = config.ACCENT_COLOR,
				Foreground = config.TEXT_COLOR,
			};
			//   mediumImage.Click += UpdateImageSize;
			mediumImage.HorizontalAlignment = HorizontalAlignment.Center;
			largeImage = new RadioButton {
				Content = "Large",
				IsChecked = false,
				GroupName = "imageSize",
				Tag = 2,
				Margin = new Thickness(0, 10, 0, 0),
				Background = config.ACCENT_COLOR,
				Foreground = config.TEXT_COLOR,
			};
			//  largeImage.Click += UpdateImageSize;
			largeImage.HorizontalAlignment = HorizontalAlignment.Right;
			uploadButton = new Button {
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
			if (selectedId != -1) {
				title.Text = elements[selectedId].GetTitle();
				switch (elements[selectedId].GetImageSize()) {
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
		private void OpenImageUploadDialog(object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog() {
				FileName = "",
				Filter = "Image files (*.png*, *.jpeg)|*.png;*.jpeg",
				Title = "Open an Image",
				DefaultExt = ".png.jpeg.ico.gif"
			};
			Nullable<bool> result = openFileDialog.ShowDialog();
			if (result == true) {
				selectedFile = openFileDialog.FileName;
				Uri uri = new Uri(openFileDialog.FileName);
				image.Source = new BitmapImage(uri);

			}
		}
		//private void UpdateImageSize(object sender, RoutedEventArgs e)
		//{
		//    RadioButton btn = (RadioButton)sender;
		//    //tmpImageSize = int.Parse(btn.Tag.ToString());
		//}
		private void InitlizeTable(int id) {
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
			title = new TextBox {
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

			int totalRows = tableData.Count(),
			totalColoums = 0,
			tmpInt = 0;
			if (totalRows > 0) {
				totalColoums = tableData[0].Count();
			}
			List<Grid> tmpGrid = new List<Grid>();
			while (tmpInt < totalRows) {
				tmpInt++;
				tmpGrid.Add(AddTableGrid(totalColoums));
			}

			TextBox tmpText;
			// Now that grid is there add all the textboxs to the grid and content
			for (int rows = 0; rows < totalRows; rows++) {
				for (int columns = 0; columns < totalColoums; columns++) {
					tmpText = new TextBox {
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
		private Grid AddTableGrid(int totalColoums) {
			Grid tGrid = new Grid {
				Width = totalColoums * 100,
				Height = 30,
				Background = Brushes.Pink
			};
			ColumnDefinition column;
			tGrid.ColumnDefinitions.Clear();

			while (totalColoums > 0) {
				column = new ColumnDefinition();
				totalColoums--;
				column.Width = new GridLength(25, GridUnitType.Auto);
				tGrid.ColumnDefinitions.Add(column);
			}
			return tGrid;
		}
		private bool SaveData(string type, int id) {
			try {
				//selectedId = id;
				//Steps

				if (title.Text != "") {
					// If id is -1 add an element to elements and change id to that new id
					if (id < 0) {
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
					if (type == "Table") {
						tableStringData = new List<List<String>>();
						foreach (List<TextBox> data in tableData) {
							tableStringData.Add(new List<string>());
							foreach (TextBox textBox in data) {
								tableStringData.Last().Add(textBox.Text);
							}
						}

					} else if (type == "Paragraph") {

						string str = body.Text;
						elements[id].SetBody(str);
					} else if (type == "List") {
						List<string> listItems = new List<string>();
						foreach (TextBox text in listTextBox) {
							listItems.Add(text.Text);
						}
						elements[id].SetListItems(listItems);
					} else if (type == "Figure") {
						if (smallImage.IsChecked == true) {
							elements[id].SetImageSize(0);
						} else if (mediumImage.IsChecked == true) {
							elements[id].SetImageSize(1);
						} else {
							elements[id].SetImageSize(2);
						}
						if (selectedFile != "") {
							elements[id].SetImagLoc(selectedFile);
						}

					}
				}
				// Update element list

				// Return true
				return true;
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}