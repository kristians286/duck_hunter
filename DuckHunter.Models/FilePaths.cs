using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunter.Models
{
    public class FilePaths
    {
        public static readonly string FOLDER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter";
        public static readonly string IMAGE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter" + "\\images";
        public static readonly string SAVE_FILE = "\\HighScores.xml";
    }
}
