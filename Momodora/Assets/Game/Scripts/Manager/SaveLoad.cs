using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveLoad
{
    public int gameTime = default;
    public int[] savePoint = new int[2];

    public bool[] eventCheck = new bool[10];
    public int[] positionX = new int[10];
    public int[] positionY = new int[10];
    
    public int money = default;

    //public string[] equipItems = new string[5];
    //public string[] activeItems = new string[5];
    //public int activeItemsX;
    //public string[] durationItems = new string[5];
    //public int durationItemsX;

    public SaveLoad(int gameTime_, int[] savePoint_, bool[] eventCheck_, int[] positionX_, int[] positionY_, int money_)
    {
        gameTime = gameTime_;
        savePoint = savePoint_;
        eventCheck = eventCheck_;
        positionX = positionX_;
        positionY = positionY_;
        money = money_;

        //leaf = leaf_;
        //equipItems = equipItems_;
        //activeItems = activeItems_;
        //activeItemsX = activeItemsX_;
        //durationItems = durationItems_;
        //durationItemsX = durationItemsX_;
    }
}
