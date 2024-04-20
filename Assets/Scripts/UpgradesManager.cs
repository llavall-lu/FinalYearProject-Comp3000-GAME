using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager upgradeManager;
    private void Awake() => upgradeManager = this;

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
    public string[] upgradeTitles; 
    public string[] upgradePopupText; 

    public double[] clickUpgradeBaseCost;
    public double[] clickUpgradeCostMulti;
    public double[] clickUpgradesBasePower;
    public double[] clickUpgradesUnlocked;
    private bool[] clickUpgradesAppeared;

    public double[] productionUpgradeBaseCost;
    public double[] productionUpgradeCostMulti;
    public double[] productionUpgradesBasePower;
    public double[] productionUpgradesUnlocked;
    private bool[] productionUpgradesAppeared;

    public GameObject upgradePopupPrefab; 

    public void StartUpgradeManager()
    {
        Methods.UpgradeCheck(Controller.controller.gameData.clickUpgradeLevel, length: 4);

        clickUpgradeName = new[] { "Infect Device +1", "Infect Device +5", "Infect Device +10", "Infect Device +20" };
        
        
        productionUpgradeName = new[]
        {
            "+1 Infected Devices/s",
            "+3 Infected Devices/s",
            "+7 Infected Devices/s",
            "+15 Infected Devices/s",
            "+30 Infected Devices/s" 
        };

        
        upgradeTitles = new[] 
        {
            "Trojan",
            "Worm",
            "Rootkit",
            "Fileless Malware",
            "Botnet" 
        };
        
    
        upgradePopupText = new[]
        {
            "This upgrade introduces a Trojan, a type of malware that masquerades as legitimate software. Trojans can steal sensitive information or provide attackers unauthorized access to the infected system.",
            "This upgrade introduces a Worm, a self-replicating malware that spreads through networks by exploiting vulnerabilities in computer systems.",
            "This upgrade introduces a Rootkit, a type of malware designed to conceal itself and other malicious software on a system, granting attackers privileged access.",
            "This upgrade introduces a Fileless Malware, operating in system memory without leaving traces on the hard drive, often used for stealthy attacks like data theft.",
            "This upgrade introduces a Botnet, a network of infected computers controlled by attackers, commonly utilized for large-scale attacks and data theft."
        };


        clickUpgradeBaseCost = new double[] { 10, 50, 150, 300 };
        clickUpgradeCostMulti = new double[] { 1.25, 1.35, 1.5, 1.7 };
        clickUpgradesBasePower = new double[] { 1, 5, 10, 20 };
        clickUpgradesUnlocked = new double[] { 0, 25, 50, 1000 }; // Thresholds to unlock upgrades
        clickUpgradesAppeared = new bool[clickUpgradeName.Length];

        productionUpgradeBaseCost = new double[] { 25, 100, 500, 2000, 10000 }; 
        productionUpgradeCostMulti = new double[] { 1.5, 1.75, 2.2, 2.8, 3.5 }; 
        productionUpgradesBasePower = new double[] { 1, 3, 7, 15, 30 }; 
        productionUpgradesUnlocked = new double[] { 250, 750, 2000, 5000, 20000 }; // Thresholds to unlock upgrades
        productionUpgradesAppeared = new bool[productionUpgradeName.Length];
    
        // Instantiate click upgrades
        for (int i = 0; i < Controller.controller.gameData.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            upgrade.gameObject.SetActive(false);
            clickUpgrades.Add(upgrade);
        }

        // Instantiate production upgrades
        for (int i = 0; i < Controller.controller.gameData.productionUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradesPrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i;
            upgrade.gameObject.SetActive(false);
            productionUpgrades.Add(upgrade);
        }

        clickUpgradesScroll.normalizedPosition = new Vector2(x: 0, y: 0);
        productionUpgradesScroll.normalizedPosition = new Vector2(x: 0, y: 0);
        UpdateUpgradeUI("click");
        UpdateUpgradeUI(type: "production");
    }

    public void Update()
    {   
        UpdateUpgradesAppearance(clickUpgradesUnlocked, clickUpgradesAppeared, clickUpgrades, showCustomText: false);  
        UpdateUpgradesAppearance(productionUpgradesUnlocked, productionUpgradesAppeared, productionUpgrades, showCustomText: true);
    }

    private void UpdateUpgradesAppearance(double[] thresholds, bool[] appearedArray, List<Upgrades> upgrades, bool showCustomText)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (!appearedArray[i] && Controller.controller.gameData.currency >= thresholds[i])
            {
                appearedArray[i] = true;
                upgrades[i].gameObject.SetActive(true);
               
                if (showCustomText && upgrades == productionUpgrades) 
                {
                    string customText = upgradePopupText[i];
                    
                    ShowUpgradePopup(upgrades[i].transform.position, customText);
                }
            }
        }
    }

    private void ShowUpgradePopup(Vector3 position, string customText)
    {
        
        GameObject popupsPanel = GameObject.Find("PopupsPanel");

        RectTransform canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, position, Camera.main, out localPos);
    
        // Instantiate the popup as a child of the Homescreen GameObject
        GameObject popup = Instantiate(upgradePopupPrefab, popupsPanel.transform);
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchoredPosition = localPos;

        UpgradePopup upgradePopup = popup.GetComponent<UpgradePopup>();
        upgradePopup.SetCustomText(customText); 
    
        upgradePopup.DestroyPopup(15f); 
        popup.transform.localPosition = new Vector2(0, 0);
    }


    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        switch (type)
        {
            case "click":
                if (upgradeID == -1)
                    for (int i = 0; i < clickUpgrades.Count; i++) UpdateUI(clickUpgrades, Controller.controller.gameData.clickUpgradeLevel, clickUpgradeName, i);
                else UpdateUI(clickUpgrades, Controller.controller.gameData.clickUpgradeLevel, clickUpgradeName, upgradeID);
                break;
            case "production":
                if (upgradeID == -1)
                    for (int i = 0; i < productionUpgrades.Count; i++) UpdateUI(productionUpgrades, Controller.controller.gameData.productionUpgradeLevel, productionUpgradeName, i);
                else UpdateUI(productionUpgrades, Controller.controller.gameData.productionUpgradeLevel, productionUpgradeName, upgradeID);
                break;
        }
        void UpdateUI(List<Upgrades> upgrades, List<int> upgradeLevels, string[] upgradeNames, int ID)
        {
            upgrades[ID].LevelText.text = upgradeLevels[ID].ToString();
            upgrades[ID].CostText.text = $"Req {UpgradeCost(type, ID):F0} Infected Devices";
            upgrades[ID].NameText.text = upgradeNames[ID];
            if (upgrades == productionUpgrades) 
                upgrades[ID].TitleText.text = upgradeTitles[ID];
        }
    }

    public double UpgradeCost(string type, int upgradeID)
    {
        var data = Controller.controller.gameData; // to help shorten code length

        switch (type)
        {
            case "click":
                return clickUpgradeBaseCost[upgradeID] *
                       Math.Pow(clickUpgradeCostMulti[upgradeID], data.clickUpgradeLevel[upgradeID]);
            case "production":
                return productionUpgradeBaseCost[upgradeID] *
                       Math.Pow(productionUpgradeCostMulti[upgradeID], data.productionUpgradeLevel[upgradeID]);
        }

        return 0;
    }

    public void BuyUpgrade(string type, int upgradeID)
    {
        switch (type)
        {
            case "click": Buy(Controller.controller.gameData.clickUpgradeLevel);
                break;
            case "production": Buy(Controller.controller.gameData.productionUpgradeLevel);
                break;
        }
        void Buy(List<int> upgradeLevels)
        {
            if (Controller.controller.gameData.currency >= UpgradeCost(type, upgradeID))
            {
                Controller.controller.gameData.currency -= UpgradeCost(type, upgradeID);
                upgradeLevels[upgradeID] += 1;
            }
            UpdateUpgradeUI(type, upgradeID);
        }
    }

    public void BuyMax()
    {
        var data = Controller.controller.gameData;

        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            List<int> clickUpgradeLevelList = new List<int> { data.clickUpgradeLevel[i] };
            Methods.BuyMax(ref data.currency, clickUpgradeBaseCost[i], (float)clickUpgradeCostMulti[i], ref clickUpgradeLevelList, 0);
            data.clickUpgradeLevel[i] = clickUpgradeLevelList[0];
        }

        for (int i = 0; i < data.productionUpgradeLevel.Count; i++)
        {
            List<int> productionUpgradeLevelList = new List<int> { data.productionUpgradeLevel[i] };
            Methods.BuyMax(ref data.currency, productionUpgradeBaseCost[i], (float)productionUpgradeCostMulti[i], ref productionUpgradeLevelList, 0);
            data.productionUpgradeLevel[i] = productionUpgradeLevelList[0];
        }

        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");
    }
}
