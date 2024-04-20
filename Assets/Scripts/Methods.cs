using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
}
