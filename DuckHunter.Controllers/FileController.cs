using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Xml;
using DuckHunter.Models;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace DuckHunter.Controllers
{
    public class FileController
    {
        public static void AddHighScore(HighScore newHighScore)
        {
            try
            {
                ObservableCollection<HighScore> highScoresList = GetHighScoresListFromXml();
                var existingUsername = false;
                foreach (HighScore highScore in highScoresList)
                {
                    if (highScore.Username != newHighScore.Username)
                    {
                        continue;
                    }
                    highScore.ImageSource = newHighScore.ImageSource;
                    if (int.Parse(highScore.Score) < int.Parse(newHighScore.Score))
                    {
                        highScore.Score = newHighScore.Score;
                    }
                    existingUsername = true;
                    break;
                }
                if (!existingUsername)
                {
                    highScoresList.Add(newHighScore);
                }
                

                highScoresList = new ObservableCollection<HighScore>(highScoresList.OrderByDescending(i => int.Parse(i.Score)).Take(5));

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);
                XmlElement root = xdoc.DocumentElement;
                root.RemoveAll();
                //XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");
                var i = 1;
                foreach (HighScore highScore in highScoresList)
                {
                    Debug.WriteLine(highScore.Username);
                    XmlElement Player = xdoc.CreateElement("Player");
                    Player.SetAttribute("Position",$"{i++}");
                    Player.SetAttribute("Username", $"{highScore.Username}");
                    Player.SetAttribute("Score", $"{highScore.Score}");
                    Player.SetAttribute("Image_location", $"{highScore.ImageSource}");
                    root.AppendChild(Player);
                }

                xdoc.Save(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }
        public static ObservableCollection<HighScore> GetHighScoresListFromXml()
        {
            var list = new ObservableCollection<HighScore>();
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");

                foreach (XmlElement el in nodes)
                {
                    string position = el.GetAttribute("Position");
                    string username = el.GetAttribute("Username");
                    string score = el.GetAttribute("Score");
                    string imageLocation = el.GetAttribute("Image_location");

                    list.Add(new HighScore(position, username, score, imageLocation));
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return list;
        }

        public static void CreateDirectories()
        {
            try
            {
                if (!Directory.Exists(FilePaths.FOLDER_PATH))
                {
                    Directory.CreateDirectory(FilePaths.FOLDER_PATH);
                    Debug.WriteLine("Creating `DuckHunter` dir in %appdata%");
                }
                if (!Directory.Exists(FilePaths.IMAGE_PATH))
                {
                    Directory.CreateDirectory(FilePaths.IMAGE_PATH);
                    Debug.WriteLine("Creating `images` dir in %appdata% DuckHunter");
                }
                if (!Directory.Exists(FilePaths.LOGS_PATH))
                {
                    Directory.CreateDirectory(FilePaths.LOGS_PATH);
                }
                if (!File.Exists(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE))
                {
                    using (File.Create(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE))
                    {
                        Debug.WriteLine("Creating `HighScores.xml` file");
                    }
                    XmlTextWriter textWriter = new XmlTextWriter(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE, null);
                    textWriter.Formatting = Formatting.Indented;
                    textWriter.WriteStartDocument();
                    textWriter.WriteStartElement("HighScores");
                    textWriter.WriteEndElement();
                    textWriter.WriteEndDocument();
                    textWriter.Close();
                }
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        public static void LogException(Exception exc)
        {

            string logfile = $"\\LOG-{DateTime.Now.Year}-{DateTime.Now.Day}-{DateTime.Now.Month}.log";

            try
            {
                string text = "";
                if (File.Exists(FilePaths.LOGS_PATH + logfile))
                {
                    text = File.ReadAllText(FilePaths.LOGS_PATH + logfile);
                }
                File.WriteAllText(FilePaths.LOGS_PATH + logfile, text + DateTime.Now + " \n " + exc.Message + "\n");

            }
            catch (Exception logExc)
            {
                Debug.WriteLine(logExc.Message);
            }
        }

        

        public static bool PlayerExistsInXml(string name)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");

                foreach (XmlElement el in nodes)
                {
                    if (el.GetAttribute("Username") == name)
                    {
                        return true;
                    }
                }
            } catch (Exception ex)
            { 
                LogException(ex);
            }
            return false;
        }

        public static string GetPlayerImageFromXml(string name)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");

                foreach (XmlElement el in nodes)
                {
                    if (el.GetAttribute("Username") == name)
                    {
                        return el.GetAttribute("Image_location");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return null;
        }
    }

}
