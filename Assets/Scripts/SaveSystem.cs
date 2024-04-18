using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Directory = System.IO.Directory;
using File = UnityEngine.Windows.File;

public class SaveSystem : MonoBehaviour
{
    private const string FileType = ".txt";
    private static string SavePath => Application.persistentDataPath + "/Saves/";
    private static string BackupSavePath => Application.persistentDataPath + "/Backups/";
    private static int SaveCount;
    
    public static void SaveData<T>(T data, string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackupSavePath);
        
        if (SaveCount % 5 == 0) Save(BackupSavePath);
        Save(SavePath);

        SaveCount++;

        void Save(string path)
        {
            using (StreamWriter writer = new StreamWriter(path: path + fileName + FileType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                formatter.Serialize(memoryStream, data);
                string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
                writer.WriteLine(dataToSave);
            }
        }
    }

    public static T LoadData<T>(string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackupSavePath);

        bool backUpNeeded = false;
        T dataToReturn;
        
        Load(SavePath);
        if (backUpNeeded) Load(BackupSavePath);

        return dataToReturn;

        void Load(string path)
        {
            using (StreamReader reader = new StreamReader(path: path + fileName + FileType)) 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string dataToLoad = reader.ReadToEnd();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToLoad));
                try
                {
                    dataToReturn = (T)formatter.Deserialize(memoryStream);
                }
                catch
                {
                    backUpNeeded = true;
                    dataToReturn = default;
                }
            }
        }
    }

    public static bool SaveExists(string FileName) => File.Exists(path: SavePath + FileName + FileType) ||
                                                      File.Exists(path: BackupSavePath + FileName + FileType);
}
