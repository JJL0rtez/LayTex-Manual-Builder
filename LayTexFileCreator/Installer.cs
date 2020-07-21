using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayTexFileCreator
{
    public class Installer
    {

        public bool Install()
        {
			Config config = new Config();
			try
			{
				// Create file structure
				if (!Directory.Exists("C:\\StonetownKarateManual"))
				{
					Directory.CreateDirectory("C:\\StonetownKarateManual");
				}
				if (!Directory.Exists(config.RAW_CHAPTER_LOCATION))
				{
					Directory.CreateDirectory(config.RAW_CHAPTER_LOCATION);
				}
				if (!Directory.Exists(config.RAW_TEX_LOCATION))
				{
					Directory.CreateDirectory(config.RAW_TEX_LOCATION);
				}
				if (!Directory.Exists(config.COMPILED_MANUAL_LOCATION))
				{
					Directory.CreateDirectory(config.COMPILED_MANUAL_LOCATION);
				}
				if (!Directory.Exists(config.DEFAULT_PAGE_LOCATION))
				{
					Directory.CreateDirectory(config.DEFAULT_PAGE_LOCATION);
				}
				if (!Directory.Exists("C:\\StonetownKarateManual\\bin"))
				{
					Directory.CreateDirectory("C:\\StonetownKarateManual\\bin");
				}
				if (!Directory.Exists("C:\\StonetownKarateManual\\bin\\images"))
				{
					Directory.CreateDirectory("C:\\StonetownKarateManual\\bin\\images");
				}
				// Copy Files
				// Images
				Copy(".\\images", "C:\\StonetownKarateManual\\bin\\images");
				// Pdfs
				if (!File.Exists("C:\\StonetownKarateManual\\bin\\tutorialDocument.pdf"))
				{
					//var tutorialDoc = new DirectoryInfo("..\\images\\tutorialDocument.pdf");
					File.Copy(".\\images\\tutorialDocument.pdf","C:\\StonetownKarateManual\\bin\\tutorialDocument.pdf");
				}
				// .bat files
				if (!File.Exists("C:\\StonetownKarateManual\\Save Files.bat"))
				{
					File.Copy(".\\images\\Save Files.bat" , "C:\\StonetownKarateManual\\Save Files.bat");
				}
				if (!File.Exists("C:\\StonetownKarateManual\\Update Files.bat"))
				{
					File.Copy(".\\images\\Update Files.bat" , "C:\\StonetownKarateManual\\Update Files.bat");
				}
				// Return outcome
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			// Check file structure to ensure program is setup correctly
			if (CheckFileStructure())
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		private static void Copy(string sourceDirectory, string targetDirectory)
		{
			var diSource = new DirectoryInfo(sourceDirectory);
			var diTarget = new DirectoryInfo(targetDirectory);

			CopyAll(diSource, diTarget);
		}


		private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}

		public bool CheckFileStructure()
		{
			Config config = new Config();
			if(!Directory.Exists("C:\\StonetownKarateManual")							||
				!Directory.Exists(config.RAW_CHAPTER_LOCATION)							||
				!Directory.Exists(config.RAW_TEX_LOCATION)								||
				!Directory.Exists(config.COMPILED_MANUAL_LOCATION)						||
				!Directory.Exists(config.DEFAULT_PAGE_LOCATION)							||
				!Directory.Exists("C:\\StonetownKarateManual\\bin")						||
				!Directory.Exists("C:\\StonetownKarateManual\\bin\\images")				||
				!File.Exists("C:\\StonetownKarateManual\\bin\\tutorialDocument.pdf")			||
				!File.Exists("C:\\StonetownKarateManual\\Save Files.bat")				||
				!File.Exists("C:\\StonetownKarateManual\\Update Files.bat")) 
			{

				return false;
			}
			else
			{
				return true;
			}
		}
	}
}