using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LayTexFileCreator
{
    class LaTex
    {
        Config config = new Config();
		
		private List<string> doParagraph(bool isSection, Element element)
		{
			List<string> data = new List<string>();
			try
			{
				data.Add("");
			if (isSection)
			{
				data.Add("\\section{" + element.GetTitle() + "}");
				
			}
			else
			{
				data.Add("\\subsection{" + element.GetTitle() + "}");
			}
			data.Add(element.GetBody());
			data.Add("");
			} 
			catch (Exception ex)
			{
				data = new List<string>();
				Console.WriteLine(ex.Message);
			}
			return data;
		}
		private List<string> DoList(bool isSection, Element element)
		{
			List<string> data = new List<string>();
			try
			{
				data.Add("");
			if (isSection)
			{
				data.Add("\\section{" + element.GetTitle() + "}");

			}
			else
			{
				data.Add("\\subsection{" + element.GetTitle() + "}");
			}
			data.Add("\\begin{enumerate}");
				foreach (string listItem in element.GetListItems())
				{
					if (listItem != "")
					{
						data.Add("  \\item " + listItem);
					}
				}
			data.Add("\\end{enumerate}");
			data.Add("");
		    } 
			catch (Exception ex)
			{
				data = new List<string>();
				Console.WriteLine(ex.Message);
			}
			return data;
		}
		private List<string> DoFigure(bool isSection, Element element)
		{
			List<string> data = new List<string>();
			try
			{
				if (!File.Exists(element.GetImageLocation()))
				{
					File.Copy(element.GetImageLocation(), config.IMAGE_GRAPHICS_PATH);
				}
			}
			catch(IOException ioEx)
			{
				Console.WriteLine(ioEx.Message);
			}
			try
			{
				string tmp = Path.GetFileName(element.GetImageLocation());
				tmp.Replace(".png", "");
				tmp.Replace(".jpeg", "");
				data.Add("\\begin{figure}[!htb]");
				data.Add("\\centering");
				if (element.GetImageSize() == 0)
				{
					data.Add("\\includegraphics[width = 4cm, height = 3.44cm]{" + Path.GetFileName(element.GetImageLocation()) + "} ");
				}
				else if (element.GetImageSize() == 2)
				{
					data.Add("\\includegraphics[width = 8cm, height = 6.88cm]{" + Path.GetFileName(element.GetImageLocation()) + "} ");
				}
				else
				{
					data.Add("\\includegraphics[width = 6cm, height = 4.66cm]{" + Path.GetFileName(element.GetImageLocation()) + "} ");

				}
				data.Add("\\caption{" + element.GetTitle() + "}");
				data.Add("\\end{figure}");
			}
			catch(Exception ex)
			{
				data = new List<string>();
				Console.WriteLine(ex.Message);
			}
			return data;
		}
		private List<List<String>> CompileChapter(Book book)
		{
			List<List<String>> data = new List<List<string>>();
			int tmp = 0;
			bool isSection = true;
			if (book != null && book.GetChapters() != null) {
				foreach (Chapter chapter in book.GetChapters())
				{
					data.Add(new List<string>());
					data[tmp].Add("\\chapter{" + chapter.GetChapterName() + "}");
					data[tmp].Add("");
					foreach (Page page in chapter.GetPages())
					{
						foreach (Element element in page.GetElements())
						{
							if (element.GetElementType() == "Paragraph")
							{
								data[tmp].AddRange(doParagraph(isSection, element));
							}
							else if (element.GetElementType() == "List")
							{
								data[tmp].AddRange(DoList(isSection, element));
							}
							else if (element.GetElementType() == "Figure")
							{
								data[tmp].AddRange(DoFigure(isSection, element));
							}
						}
					}
				}

				tmp++;
			}
			return data;
		}

        public void CompileBook(Book book)
        {
			// Vetify file structure
			if (!System.IO.Directory.Exists("C:\\StonetownKarateManual"))
			{
				System.IO.Directory.CreateDirectory("C:\\StonetownKarateManual");
			}
			if (!System.IO.Directory.Exists(config.RAW_CHAPTER_LOCATION))
			{
				System.IO.Directory.CreateDirectory(config.RAW_CHAPTER_LOCATION);
			}
			if (!System.IO.Directory.Exists(config.RAW_TEX_LOCATION))
			{
				System.IO.Directory.CreateDirectory(config.RAW_TEX_LOCATION);
			}
			if (!System.IO.Directory.Exists(config.COMPILED_MANUAL_LOCATION))
			{
				System.IO.Directory.CreateDirectory(config.COMPILED_MANUAL_LOCATION);
			}
			if (!System.IO.Directory.Exists(config.DEFAULT_PAGE_LOCATION))
			{
				System.IO.Directory.CreateDirectory(config.DEFAULT_PAGE_LOCATION);
			}
			if (!System.IO.Directory.Exists("C:\\StonetownKarateManual\\bin"))
			{
				System.IO.Directory.CreateDirectory("C:\\StonetownKarateManual\\bin");
			}
			if (!System.IO.Directory.Exists("C:\\StonetownKarateManual\\bin\\images"))
			{
				System.IO.Directory.CreateDirectory("C:\\StonetownKarateManual\\bin\\images");
			}
			string compiledFile = config.RAW_TEX_LOCATION + "/" + DateTime.Now.ToString("yy_MM_dd_h_mm_ss_tt_") + "CompiledBook.tex",
				line = "";
			if (!System.IO.File.Exists(compiledFile))
			{
				using (FileStream fs = File.Create(compiledFile))
				{
					fs.Close();
				}
			}


			// The file.WriteLine(""); does nothing for the compiler but will make the tex more readiable for trouble shooting problems
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(compiledFile))
			{
				file.WriteLine("\\documentclass{report}");
				file.WriteLine("\\usepackage[utf8]{inputenc}");
				file.WriteLine("\\usepackage[english]{ babel}");
				file.WriteLine("\\usepackage{fancyhdr}");
				file.WriteLine("\\usepackage{lastpage}");
				file.WriteLine("\\usepackage{xcolor}");
				file.WriteLine("\\usepackage{blindtext}");
				file.WriteLine("\\usepackage{graphicx}");
				file.WriteLine("\\graphicspath{ {" + config.IMAGE_GRAPHICS_PATH + "/} }");
				file.WriteLine("");
				file.WriteLine("\\pagestyle{fancy}");
				file.WriteLine("\\fancyhf{ }");
				file.WriteLine("\\fancyhead[LE, LO]{\\textcolor{ red} {\\textbf{" + config.PAGE_TOP_LEFT_TEXT + "}}}");
				file.WriteLine("\\fancyhead[RE, RO]{\\textbf{ Updated:} \\today}");
				file.WriteLine("\\fancyfoot[CE, CO]{\\textcolor{ red} {\\textbf{\\leftmark}}}");
				file.WriteLine("\\fancyfoot[LE, LO]{Page \\thepage \\hspace{1pt} of \\pageref{LastPage}}");
				file.WriteLine("");
				file.WriteLine("\\renewcommand{\\headrulewidth}{2pt}");
				file.WriteLine("\\renewcommand{\\footrulewidth}{1pt}");
				file.WriteLine("");
				file.WriteLine("\\begin{document}");
				file.WriteLine("\\begin{titlepage}");
				file.WriteLine("\\centering");
				file.WriteLine("\\scshape");
				file.WriteLine("\\includegraphics[width = 7cm, height = 6.02cm]{" + config.COVER_LOGO_LOCATION + "}");
				file.WriteLine("\\vspace *{\\baselineskip}");
				file.WriteLine("");
				file.WriteLine("\\rule{\\textwidth}{1.6pt}\\vspace*{-\\baselineskip}\\vspace *{2pt} % Thick horizontal rule");
				file.WriteLine("\\rule{\\textwidth}{0.4pt} % Thin horizontal rule");
				file.WriteLine("");
				file.WriteLine("\\vspace{0.75\\baselineskip} % Whitespace above the title");
				line = "{\\LARGE";
				foreach(string titleLine in config.BOOK_TITLE)
				{
					line += " " + titleLine + "\\\\";
				}
				line += "}";
				file.WriteLine(line);
				file.WriteLine("");
				file.WriteLine("\\vspace{0.75\\baselineskip} % Whitespace below the title");
				file.WriteLine("");
				file.WriteLine("\\rule{\\textwidth}{1.6pt}\\vspace *{-\\baselineskip}\\vspace *{2pt} % Thick horizontal rule");
				file.WriteLine("\\rule{\\textwidth}{0.4pt} % Thin horizontal rule");
				file.WriteLine("");
				file.WriteLine("\\vspace{2\\baselineskip} % Whitespace after the title block");
				file.WriteLine("\\vspace{2\\baselineskip}");
				file.WriteLine("");
				file.WriteLine("\\textcolor{red}{" + config.COVER_MESSAGE + "}");
				file.WriteLine("");
				file.WriteLine("\\vspace *{3\\baselineskip}");
				file.WriteLine("");
				file.WriteLine("Created By");
				file.WriteLine("");
				file.WriteLine("\\vspace{0.5\\baselineskip}");
				file.WriteLine("{\\scshape\\Large Stonetown Karate Centre}");
				file.WriteLine("\\vspace{0.5\\baselineskip}");
				file.WriteLine("");
				file.WriteLine("\\end{titlepage}");
				file.WriteLine("\\tableofcontents");

				List<List<String>> chapterData = CompileChapter(book);
				String tmpStr = "";

				foreach(List<String> chapter in chapterData)
				{
					foreach(string pageLine in chapter)
					{
						tmpStr = Regex.Replace(pageLine, @"[^\u0000-\u007F]+", string.Empty);
						//tmpStr = tmpStr.Replace(" \\ ", "\\textbackslash ");
						tmpStr = tmpStr.Replace("&", "\\& ");
						tmpStr = tmpStr.Replace("$", "\\$ ");
						//tmpStr = tmpStr.Replace("{", "\\{ ");
						//tmpStr = tmpStr.Replace("}", "\\} ");
						tmpStr = tmpStr.Replace(">", "\\textgreater ");
						tmpStr = tmpStr.Replace("<", "\\textless ");
						tmpStr = tmpStr.Replace("\u200B", "");
						file.WriteLine(tmpStr);
					}
				}
				file.WriteLine("");
				if (config.SHOW_LIST_OF_FIGURES)
				{
					file.WriteLine("\\listoffigures");
				}
				if (config.SHOW_LIST_OF_TABLES)
				{
					file.WriteLine("\\listoftables");
				}
				file.WriteLine("\\end{document}");
				file.Close();

				MessageBox.Show("Book has been compiled and saved.");
			}
        }


    }
}
