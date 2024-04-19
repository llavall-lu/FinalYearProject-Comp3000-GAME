using System;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{

    public static Controller instance; 
    private void Awake() => instance = this;
    
    public GameData gameData;
    
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text currencyClickPowerText;
    [SerializeField] private TMP_Text currencyPerSecond;

    public double ClickPower()
    {
        double total = 1;
        for (int i = 0; i < gameData.clickUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.instance.clickUpgradesBasePower[i] * gameData.clickUpgradeLevel[i];
        }

        return total;
    }
    
    public double CurrencyPerSecond()
    {
        double total = 0;
        for (int i = 0; i < gameData.productionUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.instance.productionUpgradesBasePower[i] * gameData.productionUpgradeLevel[i];
        }

        return total;
    }

    private const string dataFileName = "PlayerData_KyberConquest";
    public void Start()
    {
        gameData = SaveSystem.SaveExists(FileName: dataFileName)
            ? SaveSystem.LoadData<GameData>(fileName: dataFileName)
            : new GameData();
        
            UpgradesManager.instance.StartUpgradeManager();
        }


    public float SaveTime;
    private void Update()
        {
            currencyText.text = $"{gameData.currency:F2} Currency";
            currencyClickPowerText.text = $"+{ClickPower()} Currency";
            currencyPerSecond.text = $"{CurrencyPerSecond():F2}/s";
            // using interpolation to clean up messy code

            Controller.instance.gameData.currency += CurrencyPerSecond() * Time.deltaTime;
            
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

