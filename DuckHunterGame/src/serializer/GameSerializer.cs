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
            string fileName = "/DuckHunterGameSaveData.json";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullPath = path+ fileName;
            string jsonString = JsonSerializer.Serialize(game);
            File.WriteAllText(fullPath, jsonString);
        }

        public Game LoadGame(Game game)
        {
            string fileName = "/DuckHunterGameSaveData.json";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullPath = path + fileName;
            string jsonString = File.ReadAllText(fullPath);

            game = JsonSerializer.Deserialize<Game>(jsonString)!;
            return game;
        }
    }
}
