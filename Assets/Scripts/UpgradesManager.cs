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
    

    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;

  
    
    public string[] clickUpgradeName;

    
    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMulti;
    public double[] clickUpgradesBasePower;
    
    
    public void StartUpgradeManager()
    {
        Methods.UpgradeCheck(Controller.instance.gameData.clickUpgradeLevel, length:4);
        
        clickUpgradeName = new[] { "Click Power +1", "Click Power +5", "Click Power +10", "Click Power +25" };
        
        clickUpgradeBaseCost = new double[] { 10, 50, 100, 1000 };
        clickUpgradeCostMulti = new double[] { 1.25, 1.35, 1.55, 2 };
        clickUpgradesBasePower = new double[] { 1, 5, 10, 25 };

        for (int i = 0; i < Controller.instance.gameData.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            clickUpgrades.Add(upgrade);
        }

        clickUpgradesScroll.normalizedPosition = new Vector2(x: 0, y: 0);
        UpdateClickUI();
    }

    public void UpdateClickUI(int UpgradeID = -1)
    {
        if (UpgradeID == -1)
            for (int i = 0; i < clickUpgrades.Count; i++) UpdateUI(i);
        else UpdateUI(UpgradeID);
        
        void UpdateUI(int ID)
        {
            clickUpgrades[ID].LevelText.text = Controller.instance.gameData.clickUpgradeLevel[ID].ToString();
            clickUpgrades[ID].CostText.text = $"Cost {ClickUpgradeCost(ID):F2} Currency";
            clickUpgrades[ID].NameText.text = clickUpgradeName[ID];
        }
    }

    public double ClickUpgradeCost(int UpgradeID) => clickUpgradeBaseCost[UpgradeID] * 
                                         Math.Pow(clickUpgradeCostMulti[UpgradeID], Controller.instance.gameData.clickUpgradeLevel[UpgradeID]);
    //public double Cost() => clickUpgradeBaseCost * (double)clickUpgradeCostMulti * Controller.instance.gameData.clickUpgradeLevel;
    
    public void BuyUpgrade(int UpgradeID)
    {
        if (Controller.instance.gameData.currency >= ClickUpgradeCost(UpgradeID))
        {
            Controller.instance.gameData.currency -= ClickUpgradeCost(UpgradeID);
            Controller.instance.gameData.clickUpgradeLevel[UpgradeID] += 1;
        }
        UpdateClickUI(UpgradeID);
    }
}
