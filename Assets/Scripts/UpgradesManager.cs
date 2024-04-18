using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    
    
    public static UpgradesManager instance;
    private void Awake() => instance = this;


    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;

    public List<Upgrades> productionUpgrades;
    public Upgrades productionUpgradesPrefab;

    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;

    public ScrollRect productionUpgradesScroll;
    public Transform productionUpgradesPanel;
    
    public string[] clickUpgradeName;
    public string[] productionUpgradeName;
    
    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMulti;
    public double[] clickUpgradesBasePower;

    public double[] productionUpgradeBaseCost;
    public double[] productionUpgradeCostMulti;
    public double[] productionUpgradesBasePower;
    
    public void StartUpgradeManager()
    {
        Methods.UpgradeCheck(Controller.instance.gameData.clickUpgradeLevel, length:4);
        
        clickUpgradeName = new[] { "Click Power +1", "Click Power +5", "Click Power +10", "Click Power +25" };
        productionUpgradeName = new[]
        {
            "+1 Currency/s",
            "+2 Currency/s",
            "+5 Currency/s",
            "+25 Currency/s"
        };
        
        clickUpgradeBaseCost = new double[] { 10, 50, 100, 1000 };
        clickUpgradeCostMulti = new double[] { 1.25, 1.35, 1.55, 2 };
        clickUpgradesBasePower = new double[] { 1, 5, 10, 25 };
        
        productionUpgradeBaseCost = new double[] { 25, 100, 1000, 10000 };
        productionUpgradeCostMulti = new double[] { 1.5, 1.75, 2, 3 };
        productionUpgradesBasePower = new double[] { 1, 2, 10, 100 };

        for (int i = 0; i < Controller.instance.gameData.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            clickUpgrades.Add(upgrade);
        }

        for (int i = 0; i < Controller.instance.gameData.productionUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradesPrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i;
            productionUpgrades.Add(upgrade);
        }

        clickUpgradesScroll.normalizedPosition = new Vector2(x: 0, y: 0);
        productionUpgradesScroll.normalizedPosition = new Vector2(x: 0, y: 0);
        UpdateUI("click");
        UpdateUI(type:"production");
    }

    public void UpdateUI(string type, int UpgradeID = -1)
    {

        switch (type)
        {
            case "click":
                if (UpgradeID == -1)
                    for (int i = 0; i < clickUpgrades.Count; i++) UpdateUI(clickUpgrades, Controller.instance.gameData.clickUpgradeLevel, clickUpgradeName, i);
                else UpdateUI(clickUpgrades, Controller.instance.gameData.clickUpgradeLevel, clickUpgradeName, UpgradeID);
                break;
            case "production":
                if (UpgradeID == -1)
                    for (int i = 0; i < productionUpgrades.Count; i++) UpdateUI(productionUpgrades, Controller.instance.gameData.productionUpgradeLevel, productionUpgradeName, i);
                else UpdateUI(productionUpgrades, Controller.instance.gameData.productionUpgradeLevel, productionUpgradeName, UpgradeID);
                break;
        }
        void UpdateUI(List<Upgrades> upgrades, List<int> upgradeLevels, string[] upgradeNames, int ID)
        {
            upgrades[ID].LevelText.text = upgradeLevels[ID].ToString();
            upgrades[ID].CostText.text = $"Cost {UpgradeCost(type, ID):F2} Currency";
            upgrades[ID].NameText.text = upgradeNames[ID];
        }
    }

    public double UpgradeCost(string type, int UpgradeID)
    {
        var data = Controller.instance.gameData; // to help shorten code length

        switch (type)
        {
            case "click":
                return clickUpgradeBaseCost[UpgradeID] *
                       Math.Pow(clickUpgradeCostMulti[UpgradeID], data.clickUpgradeLevel[UpgradeID]);
            case "production":
                return productionUpgradeBaseCost[UpgradeID] *
                       Math.Pow(productionUpgradeCostMulti[UpgradeID], data.productionUpgradeLevel[UpgradeID]);
        }

        return 0;
    }
    //public double Cost() => clickUpgradeBaseCost * (double)clickUpgradeCostMulti * Controller.instance.gameData.clickUpgradeLevel;
    
    public void BuyUpgrade(string type, int UpgradeID)
    {
        
        switch (type)
        {
            case "click": Buy(Controller.instance.gameData.clickUpgradeLevel);
                break;
            case "production": Buy(Controller.instance.gameData.productionUpgradeLevel);
                break;
        }
        void Buy(List<int> upgradeLevels)
        {
            if (Controller.instance.gameData.currency >= UpgradeCost(type, UpgradeID))
            {
                Controller.instance.gameData.currency -= UpgradeCost(type, UpgradeID);
                upgradeLevels[UpgradeID] += 1;
            }
            UpdateUI(type, UpgradeID);
        }
    }
}
