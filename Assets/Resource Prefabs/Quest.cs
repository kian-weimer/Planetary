using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int ID;
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

    public static bool operator ==(Quest lhs, Quest rhs)
    {
        return lhs.promptText == rhs.promptText && lhs.questType == rhs.questType && lhs.requirementCount == rhs.requirementCount && lhs.rewardText == rhs.rewardText;
    }

    public static bool operator !=(Quest lhs, Quest rhs)
    {
        return lhs.promptText != rhs.promptText || lhs.questType != rhs.questType || lhs.requirementCount != rhs.requirementCount || lhs.rewardText != rhs.rewardText;
    }
}
