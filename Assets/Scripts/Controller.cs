using System;
using UnityEngine;
using TMPro;
using static Settings;
using static UpgradesManager;

public class Controller : MonoBehaviour
{

    public static Controller controller; 
    private void Awake() => controller = this;
    
    public GameData gameData;
    
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text currencyClickPowerText;
    [SerializeField] private TMP_Text currencyPerSecond;

    public double ClickPower()
    {
        double total = 1;
        for (int i = 0; i < gameData.clickUpgradeLevel.Count; i++)
        {
            total += upgradeManager.clickUpgradesBasePower[i] * gameData.clickUpgradeLevel[i];
        }

        return total * PrestigeManager.prestigeManager.PrestigeEffect();
    }
    
    public double CurrencyPerSecond()
    {
        double total = 0;
        for (int i = 0; i < gameData.productionUpgradeLevel.Count; i++)
        {
            total += upgradeManager.productionUpgradesBasePower[i] * gameData.productionUpgradeLevel[i];
        }

        return total * PrestigeManager.prestigeManager.PrestigeEffect();
    }

    private const string dataFileName = "PlayerData_KyberConquest";
    public void Start()
    {
        gameData = SaveSystem.SaveExists(FileName: dataFileName)
            ? SaveSystem.LoadData<GameData>(fileName: dataFileName)
            : new GameData();
        
            upgradeManager.StartUpgradeManager();
            settings.StartSettings();
        }


    public float SaveTime;
    private void Update()
        {
            currencyText.text = $"{gameData.currency:F1} Infected Devices";
            currencyClickPowerText.text = $"+{ClickPower():F2} Infected Devices";
            currencyPerSecond.text = $"{CurrencyPerSecond():F2}/s";
            // using interpolation to clean up messy code

            Controller.controller.gameData.currency += CurrencyPerSecond() * Time.deltaTime;
            
            SaveTime += Time.deltaTime * (1 / Time.timeScale);
            if (SaveTime >= 15)
            {
                Save();
                SaveTime = 0;
            }

        }

    public void Save()
    {
        SaveSystem.SaveData(gameData, dataFileName);
    }
    
    public void AddCurrency()
        {
            gameData.currency += ClickPower();
        }
}

