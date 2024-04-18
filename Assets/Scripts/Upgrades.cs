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

    public void BuyClickUpgrades() => UpgradesManager.instance.BuyUpgrade(type:"click", UpgradeID);
    public void BuyProductionUpgrades() => UpgradesManager.instance.BuyUpgrade(type:"production", UpgradeID);
}
