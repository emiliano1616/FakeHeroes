using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Grid;
//using System.Linq;

namespace Assets.Scripts.Managers
{
    public static class Serialization
    {

        //TODO Should I saved the players position? I think I should...
        public static void SaveLevel(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
                saveName = "default_level_name";

            var saveFile = new SaveLevelFile();

            //Get all nodes;

            var grid = GridBase.GetInstance;
            saveFile.SizeX = grid.SizeX;
            saveFile.SizeY = grid.SizeY;
            saveFile.ScaleXY = grid.ScaleXY;

            saveFile.SavedNodes = GetSaveableNodes(grid);
            saveFile.SavedHeros = GetSaveableHeros(GameManager.GetInstance.Heroes);                                                          

            var saveLocation = GetSaveLocation();
            saveLocation += saveName;

            IFormatter formater = new BinaryFormatter();
            using (Stream stream = new FileStream(saveLocation, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formater.Serialize(stream, saveFile);
                stream.Close();
            }

            Debug.Log(saveName + " saved");

        }

        private static List<SaveableHero> GetSaveableHeros(List<UnitController> heroes)
        {
            var list = new List<SaveableHero>();
            foreach(var hero in heroes)
            {
                list.Add(hero.ToSaveable);
            }
            return list;
        }

        public static SaveLevelFile LoadLevel(string loadName)
        {

            SaveLevelFile saveFile = null;

            string targetName = GetSaveLocation() + loadName;

            if (!File.Exists(targetName))
            {
                Debug.Log($"cant find level {loadName}");
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                using (var stream = new FileStream(targetName, FileMode.Open))
                {
                    saveFile = (SaveLevelFile)formatter.Deserialize(stream);
                    stream.Close();
                }

            }

            return saveFile;

        }

        public static List<SaveableNode> GetSaveableNodes(GridBase grid)
        {
            var ret = new List<SaveableNode>();
            foreach (var x in grid.Grid)
            {
                ret.Add(x.ToSaveable);
            }

            return ret;
        }

        private static string GetSaveLocation()
        {
            string saveLocation = Application.streamingAssetsPath + "/Levels/";
            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation;
        }

    }

    [Serializable]
    public class SaveLevelFile
    {
        public int SizeX;
        public int SizeY;
        public float ScaleXY;
        public List<SaveableNode> SavedNodes;
        public List<SaveableHero> SavedHeros { get; set; }
    }

    [Serializable]
    public class SaveableNode
    {
        public int X, Y;
        public bool IsWalkable;
    }

    [Serializable]
    public class SaveableHero
    {
        public int X, Y;
        public float ActionPoints { get; set; }
        public int HeroIndex;
        public int HeroPlayer { get; set; }
    }
}
