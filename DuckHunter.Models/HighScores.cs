using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunter.Models
{
    public class HighScores : IDataErrorInfo
    {
        private string _username;
        private bool _imageSet;

        public string Username;
        public bool ImageSet;
        public string Error
        {
            get
            {
                return "";
            }
        }

        public string this[string name]
        {
            get
            {
                string result = null;
                if (Username == "")
                {
                    result += "Username cant be blank \n";
                }
                if (ImageSet)
                {
                    result += "Please select image \n";
                }

                return result;
            }
        }
    }
}
