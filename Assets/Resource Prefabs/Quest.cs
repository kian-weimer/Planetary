using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string promptText;
    public string questType; // enemy, planet, resource
    public int requirementCount; //1, 2, 3
    public List<GameObject> icons;
    public List<string> itemNames;
    public List<int> quantities;

    public string rewardText;
    public int moneyReward = 0;
    public int expReward = 0;
    public string upgradeReward = "";
    public string recipeReward = "";


}
