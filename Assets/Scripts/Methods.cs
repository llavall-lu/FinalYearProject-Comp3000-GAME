using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public static class Methods
{

    public static int Notation;
    public static string Notate(this double number)
    {
        switch (Notation)
        {
            case 0:
                return "Standard";
            case 1:
                return "Scientific";
        }
        return "";
    }
    
    public static List<T> CreateList<T>(int capacity) => Enumerable.Repeat(default(T), capacity).ToList();

    public static void UpgradeCheck<T>(List<T> list, int length) where T : new()
    {
        try
        {
            //if (list.Count == 0) list = CreateList<T>(length);
            if (list.Count == 0) list = new T[length].ToList();
            while(list.Count < length) list.Add(item: new T());
        }
        catch
        {
            list = new T[length].ToList();
        }
    }

    public static double Cost(double baseCost, float costMult, int level) =>
        baseCost * Math.Pow(costMult, level);
    public static void BuyMax(ref double currency, double baseCost, float costMult, ref int level)
    {
        int n = (int)Math.Floor(Math.Log(currency * (costMult - 1) / Cost(baseCost, costMult, level) + 1, costMult));
        double cost = Cost(baseCost, costMult, level) * ((Math.Pow(costMult, n) - 1) / (costMult - 1));
        
        if (currency < cost) return;
        currency -= cost;
        level += n;
    }
    public static void BuyMax(ref double currency, double baseCost, float costMult, ref List<int> level, int i)
    {
        int n = (int)Math.Floor(Math.Log(currency * (costMult - 1) / Cost(baseCost, costMult, level[i]) + 1, costMult));
        double cost = Cost(baseCost, costMult, level[i]) * ((Math.Pow(costMult, n) - 1) / (costMult - 1));
        
        if (currency < cost) return;
        currency -= cost;
        level[i] += n;
    }
}
