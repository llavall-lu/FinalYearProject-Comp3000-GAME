using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameData
{
    public double currency;

    public List<int> clickUpgradeLevel;
    
    
    public GameData()
    {
        currency = 0;
        
        clickUpgradeLevel = new int[4].ToList();
        //clickUpgradeLevel = Methods.CreateList<double>(capacity: 4);
        
    }
}