using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.UI;
using Directory = System.IO.Directory;
using File = UnityEngine.Windows.File;

public class SaveSystem : MonoBehaviour
{

    public TMP_InputField ImportField;
    public TMP_InputField ExportField;
    public WebGLNativeInputField ImportFieldWebGL;
    public WebGLNativeInputField ExportFieldWebGL;

    public Image CopyButton;
    public Image PasteButton;
    public Image CopyButtonWebGL;
    public Image PasteButtonWebGL;

    public TMP_Text CopyButtonText;
    public TMP_Text PasteButtonText;
    public TMP_Text CopyButtonTextWebGL;
    public TMP_Text PasteButtonTextWebGL;

    public GameObject SaveSystemObject;
    public GameObject SaveSystemObjectWebGL;
    
    private const string FileType = ".txt";
    private const string FilePath = "PlayerData_KyberConquest";
    private static string SavePath => Application.persistentDataPath + "/Saves/";
    private static string BackupSavePath => Application.persistentDataPath + "/Backups/";
    private static int SaveCount;

    private void Start()
    {
        #if UNITY_WEBGL
            SaveSystemObject.SetActive(false);
            SaveSystemObjectWebGL.SetActive(true);
        #else
            SaveSystemObject.SetActive(true);
            SaveSystemObjectWebGL.SetActive(false);
        #endif
    }

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
                writer.Close();
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

    public void Import()
    {
        Directory.CreateDirectory(SavePath);

        using (StreamWriter writer = new StreamWriter(SavePath + FilePath + FileType))
        {
            #if UNITY_WEBGL
            writer.WriteLine(ImportFieldWebGL.text);
            #else
            writer.WriteLine(ImportField.text);
            #endif
            
            
            writer.Close();
        }

        Controller.instance.gameData = SaveExists(FileName: FilePath)
            ? SaveSystem.LoadData<GameData>(fileName: FilePath)
            : new GameData();
    }

    public void Export()
    {
        Controller.instance.Save();
        Directory.CreateDirectory(SavePath);

        using (StreamReader reader = new StreamReader(path: SavePath + FilePath + FileType))
        {
            
            #if UNITY_WEBGL
            ExportFieldWebGL.text = reader.ReadToEnd();
            #else
            ExportField.text = reader.ReadToEnd();
            #endif
            reader.Close();
        }
    }

    public void Copy()
    {
        if (ExportField.text == "") return;
        
        #if UNITY_WEBGL
            GUIUtility.systemCopyBuffer = ExportField.text;
            CopyButton.color = Color.gray;
            CopyButtonText.text = "Copied";
        #else
            GUIUtility.systemCopyBuffer = ExportField.text;
            CopyButton.color = Color.gray;
            CopyButtonText.text = "Copied";
        #endif
        
        StartCoroutine(CopyPasteButtonNormal());
    }
    public void Paste()
    {
        #if UNITY_WEBGL
            GUIUtility.systemCopyBuffer = ExportField.text;
            CopyButton.color = Color.gray;
            CopyButtonText.text = "Copied";
        #else
            GUIUtility.systemCopyBuffer = ExportField.text;
            CopyButton.color = Color.gray;
            CopyButtonText.text = "Copied";
        #endif
        StartCoroutine(CopyPasteButtonNormal());
    }

    public void Clear(string type)
    {
        if (type == "Export")
        {
            ExportField.text = "";
            ExportFieldWebGL.text = "";
            return;
        }
        ImportField.text = "";
        ImportFieldWebGL.text = "";
    }

    public IEnumerator CopyPasteButtonNormal()
    {
        yield return new WaitForSeconds(2f);
        CopyButton.color = new Color(93, 32, 181);
        CopyButtonText.text = "Copy";
        PasteButton.color = new Color(93, 32, 181);
        PasteButtonText.text = "Paste";
        CopyButtonWebGL.color = new Color(93, 32, 181);
        CopyButtonTextWebGL.text = "Copy";
        PasteButtonWebGL.color = new Color(93, 32, 181);
        PasteButtonTextWebGL.text = "Paste";
    } 
}
