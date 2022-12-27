﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Xml;
using DuckHunter.Models;
using System.Xml.Linq;

namespace DuckHunter.Controllers
{
    public class FileController
    {
        public static void EditHighScoresXmlDocument(string Username, string Score)
        {
            try
            {
                
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);

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
                savedImage_location.InnerText = $"{FilePaths.IMAGE_PATH}\\{Username}.png";
                savedPlayer.AppendChild(savedPosition);
                savedPlayer.AppendChild(savedUsername);
                savedPlayer.AppendChild(savedScore);
                savedPlayer.AppendChild(savedImage_location);

                if (nodes.Count < 5)
                {
                    root.AppendChild(savedPlayer);

                } else
                {

                
                    //find lowest
                    XmlNode oldPlayer = null;
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode username = node.SelectSingleNode("Username");
                        XmlNode score = node.SelectSingleNode("Score");
                        XmlNode position = node.SelectSingleNode("Position");
                        XmlNode imageLocation = node.SelectSingleNode("Image_location");
                        if (int.Parse(savedScore.InnerText) > int.Parse(score.InnerText) ||
                            (username.InnerText == savedUsername.InnerText) && int.Parse(savedScore.InnerText) > int.Parse(score.InnerText))
                        {
                            savedPosition.InnerText = position.InnerText;
                            oldPlayer = node;
                            break;
                        }
                    }
                    if (oldPlayer != null)
                    {
                        root.ReplaceChild(savedPlayer, oldPlayer);

                    }
                }


                xdoc.Save(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);
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
                Debug.WriteLine(e);
            }
        }
    }
}