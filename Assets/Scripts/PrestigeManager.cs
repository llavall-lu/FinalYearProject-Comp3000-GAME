using System;
using System.Linq;
using TMPro;
using UnityEngine;
using static Controller;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager prestigeManager;

    private void Awake() => prestigeManager = this;
  

    public TMP_Text PrestigeGainsText;
    public TMP_Text PrestigeCurrencyText;

    public GameObject PrestigeConfirmation;

    public double PrestigeGains() => Math.Sqrt(controller.gameData.currency / 1000);

    public double PrestigeEffect() => controller.gameData.information / 100 + 1;
 
    public void Update()
    {
        PrestigeGainsText.text = $"Prestige:\n+{PrestigeGains():f1} Information";
        PrestigeCurrencyText.text = $"{controller.gameData.information:F1} Information";
    }

    public void TogglePrestigeConfirm() => PrestigeConfirmation.SetActive(!PrestigeConfirmation.activeSelf);
    

    public void Prestige()
    {
        var data = controller.gameData;
        data.information += PrestigeGains();

        data.currency = 0;

        data.clickUpgradeLevel = new int[4].ToList();
        data.productionUpgradeLevel = new int[4].ToList();
        
        UpgradesManager.upgradeManager.UpdateUpgradeUI("click");
        UpgradesManager.upgradeManager.UpdateUpgradeUI("production");

        TogglePrestigeConfirm();
    }
}
