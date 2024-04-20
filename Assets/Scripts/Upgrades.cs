using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour
{
    
    public int UpgradeID;
    public Image UpgradeButton;
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text CostText;
    public TMP_Text TitleText;

    public void BuyClickUpgrades() => UpgradesManager.upgradeManager.BuyUpgrade(type:"click", UpgradeID);
    public void BuyProductionUpgrades() => UpgradesManager.upgradeManager.BuyUpgrade(type:"production", UpgradeID);
}
