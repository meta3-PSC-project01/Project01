using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveLoad
{
    public int gameTime = default;
    public int[] savePoint = new int[2];
    //public int leaf;
    //public string[] equipItems = new string[5];
    //public string[] activeItems = new string[5];
    //public int activeItemsX;
    //public string[] durationItems = new string[5];
    //public int durationItemsX;

    public SaveLoad(int gameTime_, int[] savePoint_)
    {
        gameTime = gameTime_;
        savePoint[0] = savePoint_[0];
        savePoint[1] = savePoint_[1];
        //leaf = leaf_;
        //equipItems = equipItems_;
        //activeItems = activeItems_;
        //activeItemsX = activeItemsX_;
        //durationItems = durationItems_;
        //durationItemsX = durationItemsX_;
    }

    //public SaveLoad(int leaf_, int time_, string[] equipItems_, string[] activeItems_, int activeItemsX_, string[] durationItems_,
    //    int durationItemsX_, int[] startingPoint_)
}
