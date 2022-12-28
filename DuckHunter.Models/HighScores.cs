using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DuckHunter.Models
{
    public class HighScores
    {
        public string Position { get; set; }
        public string Username { get; set; }
        public string Score { get; set; }
        public string ImageSource   { get; set; }

        public HighScores(string position, string username, string score, string imageSource)
        {
            Position = position;
            Username = username;
            Score = score;
            ImageSource = imageSource;
        }
    }
}
