using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DuckHunter.Models;
using System.Diagnostics;

namespace DuckHunter.Controllers
{
    public class GameSerializer
    {
        public void SaveGame(Game game)
        {
            try
            {
                string fileName = "\\DuckHunterGameSaveData.xml";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullPath = path + fileName;

                XmlSerializer serializer = new XmlSerializer(typeof(Game));
                FileStream file = File.Create(fullPath);

                serializer.Serialize(file, game);
                file.Close();

            } catch (Exception e)
            { 
                Debug.WriteLine(e);
            }

            
        }
        
        public Game LoadGame()
        {
            Game game = new Game();
            try
            {
                string fileName = "\\DuckHunterGameSaveData.xml";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullPath = path + fileName;

                XmlSerializer serializer = new XmlSerializer(typeof(Game));


                StreamReader file = new StreamReader(fullPath);

                game = (Game)serializer.Deserialize(file);


                file.Close();

            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return game;
        }
    }
}
