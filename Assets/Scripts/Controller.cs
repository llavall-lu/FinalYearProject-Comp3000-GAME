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

    public double ClickPower()
    {
        double total = 1;
        for (int i = 0; i < gameData.clickUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.instance.clickUpgradesBasePower[i] * gameData.clickUpgradeLevel[i];
        }

        return total;
    }
    
    private void Start() 
        {
            gameData = new GameData();
            UpgradesManager.instance.StartUpgradeManager();
        }
    private void Update()
        {
            currencyText.text = gameData.currency + " Currency";
            currencyClickPowerText.text = "+" + ClickPower() + " Currency";
            
        } 
    public void AddCurrency()
        {
            gameData.currency += ClickPower();
        }
}

