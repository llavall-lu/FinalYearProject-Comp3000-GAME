using TMPro;
using UnityEngine;

public class UpgradePopup : MonoBehaviour
{
    public TMP_Text upgradeNameText; 
    
    public void SetCustomText(string text)
    {
        upgradeNameText.text = text;
    }

    // Function to destroy the pop-up after a certain duration
    public void DestroyPopup(float delay)
    {
        Destroy(gameObject, delay);
    }
}