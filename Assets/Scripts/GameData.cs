using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[Serializable]
public class GameData
{
    public double currency;

    public List<int> clickUpgradeLevel;
    public List<int> productionUpgradeLevel;

    public int notation;
    
    public GameData()
    {
        currency = 0;
        
        clickUpgradeLevel = new int[4].ToList();
        //clickUpgradeLevel = Methods.CreateList<double>(capacity: 4);
        productionUpgradeLevel = new int[5].ToList();

        notation = 0;
    }
}