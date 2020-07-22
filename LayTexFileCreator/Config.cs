using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LayTexFileCreator
{
    public class Config
    {
        // Config Values

        public Brush BACKGROUND_COLOR { get; set; }
        public Brush CONTROL_COLOR { get; set; }
        public Brush GUI_COLOR { get; set; }
        public Brush ACCENT_COLOR { get; set; }

        public Brush GROUPBOX_COLOR { get; set; }

        public string RAW_TEX_LOCATION { get; set; }
        public string DEFAULT_DIRECTORY_LOCATION { get; set; }
        public string DEFAULT_PAGE_LOCATION { get; set; }
        public string RAW_CHAPTER_LOCATION { get; set; }
        public string COMPILED_MANUAL_LOCATION { get; set; }

        public string CREATED_BY { get; set; }
        public List<string> BOOK_TITLE { get; set; }
        public string COVER_MESSAGE { get; set; }
        public string COVER_LOGO_LOCATION { get; set; }
        public string IMAGE_GRAPHICS_PATH { get; set; }
        public string PAGE_TOP_LEFT_TEXT { get; set; }
        public bool SHOW_TABLE_OF_CONTENTS { get; set; }
        public bool SHOW_LIST_OF_FIGURES { get; set; }
        public bool SHOW_LIST_OF_TABLES { get; set; }
        public FontFamily FONT_FAMILY { get; set; }
        public double FONT_SCALER { get; set; }
        public Brush TEXT_COLOR { get; set; }
        public int FONT_SIZE { get; set; }
        public int BUTTON_HEIGHT { get; set; }

        public List<string> FONTS { get; set; }
        public List<string> COLORS { get; set; }


        public bool CREATE_START_MENU_ENTRY { get; set; }
        public bool CREATE_TASK_BAR_ENTRY { get; set; }
        public bool CREATE_QUICK_LAUNCH_ENTRY { get; set; }
        public string GIT_FILE_SAVE_URL { get; set; }

        public Config() {
            // Program Colors
            BrushConverter bc = new BrushConverter();  
            BACKGROUND_COLOR = (Brush)bc.ConvertFrom("#2F2F2F");
            CONTROL_COLOR = Brushes.DimGray;
            GUI_COLOR = (Brush)bc.ConvertFrom("#F44336");
            ACCENT_COLOR = Brushes.Red;
            TEXT_COLOR = Brushes.White;
            GROUPBOX_COLOR = (Brush)bc.ConvertFrom("#3b3a3a");
            // Storage locations
            DEFAULT_DIRECTORY_LOCATION = "C:/StonetownKarateManual";
            RAW_TEX_LOCATION = DEFAULT_DIRECTORY_LOCATION + "/PreCompile/BookBackups";
            DEFAULT_PAGE_LOCATION = DEFAULT_DIRECTORY_LOCATION + "/PreCompile/PageBackups";
            RAW_CHAPTER_LOCATION = DEFAULT_DIRECTORY_LOCATION + "/PreCompile/ChapterBackups";
            COMPILED_MANUAL_LOCATION = DEFAULT_DIRECTORY_LOCATION + "/CompiledBookBackups";
            // Book Cover Presets
            CREATED_BY = "STONETOWN KARATE CENTRE";
            BOOK_TITLE = new List<string>
            {
                "STONETOWN KARATE",
                "OPERATIONS MANUAL"
            };
            COVER_MESSAGE = "A Comprehensive Collection of Procedures for the Successful Operation of a Stonetown Karate Centre Dojo";
            COVER_LOGO_LOCATION = "logo";
            // General Book Presets
            PAGE_TOP_LEFT_TEXT = "Stonetown Karate Centre Manual";
            IMAGE_GRAPHICS_PATH = "./images";
            SHOW_TABLE_OF_CONTENTS = true;
            SHOW_LIST_OF_FIGURES = false;
            SHOW_LIST_OF_TABLES = false;
            FONT_FAMILY = new FontFamily("Times New Roman");
            FONT_SCALER = 1;
            // Gui Presets
            FONT_SIZE = 12;
            BUTTON_HEIGHT = 30;
            COLORS = new List<string> {
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
            FONTS = new List<string> {
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
        }
    }
}