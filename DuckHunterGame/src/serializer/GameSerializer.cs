using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using DuckHunterGame.src.models;

namespace DuckHunterGame.src.serializer
{
    internal class GameSerializer
    {
        public void SaveGame(Game game)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = "/DuckHunterGameSaveData.json";
            string fullPath = path+ fileName;
            string jsonString = JsonSerializer.Serialize(game);
            File.WriteAllText(fullPath, jsonString);
        }
    }
}
