using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Xml;

namespace DuckHunter.Controllers
{
    public class FileController
    {
        public static readonly string FOLDER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter";
        public static readonly string IMAGE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter" + "\\images";
        public static readonly string SAVE_FILE = "\\HighScores.xml";
        public static void EditHighScoresXmlDocument(string Username, string Score)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FOLDER_PATH + SAVE_FILE);

                XmlElement root = xdoc.DocumentElement;

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");
                Debug.WriteLine(nodes.Count);

                XmlElement savedPlayer = xdoc.CreateElement("Player");
                XmlElement savedPosition = xdoc.CreateElement("Position");
                savedPosition.InnerText = $"{nodes.Count + 1}";
                XmlElement savedUsername = xdoc.CreateElement("Username");
                savedUsername.InnerText = $"{Username}";
                XmlElement savedScore = xdoc.CreateElement("Score");
                savedScore.InnerText = $"{Score}";
                XmlElement savedImage_location = xdoc.CreateElement("Image_location");
                savedImage_location.InnerText = $"{IMAGE_PATH}\\{Username}.png";
                savedPlayer.AppendChild(savedPosition);
                savedPlayer.AppendChild(savedUsername);
                savedPlayer.AppendChild(savedScore);
                savedPlayer.AppendChild(savedImage_location);

                if (nodes.Count < 5)
                {
                    //add
                    root.AppendChild(savedPlayer);
                    //sort
                }
                else
                {
                    //find lowest
                    XmlNode oldPlayer = null;
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode score = node.SelectSingleNode("Score");
                        XmlNode position = node.SelectSingleNode("Position");
                        if (int.Parse(savedScore.InnerText) > int.Parse(score.InnerText))
                        {
                            savedScore.InnerText = score.InnerText;
                            savedPosition.InnerText = position.InnerText;
                            oldPlayer = node;
                        }
                    }
                    if (oldPlayer != null)
                    {
                        root.ReplaceChild(savedPlayer, oldPlayer);

                    }

                }
                xdoc.Save(FOLDER_PATH + SAVE_FILE);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }

        public static void CreateDirectories()
        {
            try
            {
                if (!Directory.Exists(FOLDER_PATH))
                {
                    Directory.CreateDirectory(FOLDER_PATH);
                    Debug.WriteLine("Creating `DuckHunter` dir in %appdata%");
                }
                if (!Directory.Exists(IMAGE_PATH))
                {
                    Directory.CreateDirectory(IMAGE_PATH);
                    Debug.WriteLine("Creating `images` dir in %appdata% DuckHunter");
                }
                if (!File.Exists(FOLDER_PATH + SAVE_FILE))
                {
                    using (File.Create(FOLDER_PATH + SAVE_FILE))
                    {
                        Debug.WriteLine("Creating `HighScores.xml` file");
                    }
                    XmlTextWriter textWriter = new XmlTextWriter(FOLDER_PATH + SAVE_FILE, null);
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
                Debug.WriteLine(e);
            }
        }
    }
}
