using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Controller;
using static Methods;

public class Settings : MonoBehaviour
{
    public static Settings settings;
    private void Awake() =>  settings = this;

    public string[] NotationNames;

    public TMP_Text[] SettingsText;
    
    public void StartSettings()
    {
        NotationNames = new[] { "standard, scientific" };
        Notation = controller.gameData.notation;
        SyncSettings();
    }

    public void ChangeSetting(string settingName)
    {
        
        switch (settingName)
        {
            case "Notation":
                controller.gameData.notation++;
                if (controller.gameData.notation > NotationNames.Length - 1) controller.gameData.notation = 0;
                Notation = controller.gameData.notation;
                break;
        }
        SyncSettings(settingName);
    }

    public void SyncSettings(string settingName = "")
    {


        if (settingName == string.Empty)
        {
            SettingsText[0].text = $"Notation:\n{NotationNames[Notation]}";
            return;
        }
        
        switch (settingName)
        {
            case "Notation":
                SettingsText[0].text = $"Notation:\n{NotationNames[Notation]}";
                break;
        }
    }
}
