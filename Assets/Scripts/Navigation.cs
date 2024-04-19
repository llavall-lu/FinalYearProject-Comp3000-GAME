using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Navigation : MonoBehaviour
{
    public GameObject HomeScreen;
    public GameObject SettingsScreen;


    public void Navigate(string location)
    {
        HomeScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        switch (location)
        {
            case "home":
                HomeScreen.SetActive(true);
                break;
            case "settings":
                SettingsScreen.SetActive(true);
                break;
        }
    }
}
