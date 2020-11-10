using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntry : MonoBehaviour
{
    public int completionCount;
    public int requiredCount;
    public Money playerMoney;
    public Player player;
    public string questType;


    public int moneyReward;
    public int expReward;
    public string upgradeReward;
    public string recipeReward;
    public int position;

    void Start()
    {
        requiredCount = 0;
        transform.Find("QuestText").GetComponent<Text>().text = "";
        transform.Find("RewardText").GetComponent<Text>().text = "";
        for (int i = 1; i < 4; i++)
        {
            transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().sprite = null;
            transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().enabled = false;
            transform.Find("QuantityWithImage" + i).Find("Quantity").GetComponent<Text>().text = "";
            transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredResource = "";
            transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredQuantity = 0;
            transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().quantity = 0;
        }
    }

    public void set(int position, Quest quest )
    {
        this.position = position;
        transform.Find("QuestText").GetComponent<Text>().text = quest.promptText;
        transform.Find("RewardText").GetComponent<Text>().text = quest.rewardText;
        requiredCount = quest.requirementCount;
        questType = quest.questType;

        moneyReward = quest.moneyReward;
        expReward = quest.expReward;
        upgradeReward = quest.upgradeReward;
        recipeReward = quest.recipeReward;


        for (int i = 1; i < 4; i++)
        {
            transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().sprite = null ;
            transform.Find("QuantityWithImage" + i).Find("Quantity").GetComponent<Text>().text = "";
        }

        if (quest.requirementCount == 1)
        {
            transform.Find("QuantityWithImage2").Find("Image").GetComponent<Image>().sprite = quest.icons[0].GetComponent<SpriteRenderer>().sprite;
            transform.Find("QuantityWithImage2").Find("Image").GetComponent<Image>().color = quest.icons[0].GetComponent<SpriteRenderer>().color;

            transform.Find("QuantityWithImage2").Find("Quantity").GetComponent<Text>().text = 0 + "/" + quest.quantities[0];
            transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().desiredResource = quest.itemNames[0];
            transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().desiredQuantity = quest.quantities[0];
            transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().quantity = 0;
            transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().select = quest.questType;
            transform.Find("QuantityWithImage2").Find("Image").GetComponent<Image>().enabled = true;

        }
        else
        {
            for (int i = 1; i < quest.requirementCount + 1; i++)
            {
                transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().sprite = quest.icons[i - 1].GetComponent<SpriteRenderer>().sprite;
                transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().color = quest.icons[i - 1].GetComponent<SpriteRenderer>().color;
                transform.Find("QuantityWithImage" + i).Find("Quantity").GetComponent<Text>().text = 0 + "/" + quest.quantities[i - 1];
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredResource = quest.itemNames[i - 1];
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredQuantity = quest.quantities[i - 1];
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().quantity = 0;
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().select = quest.questType;
                transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().enabled = true;

            }
        }
    }

    public void enemyKilled(string enemyName)
    {
        if (requiredCount == 1)
        {
            if (enemyName.ToUpper().Contains(
                transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().desiredResource.ToUpper()))
            {
                transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().increaseCount();
            }
        }
        else
        {
            for (int i = 1; i < requiredCount + 1; i++)
            {
                if (enemyName.ToUpper().Contains(
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredResource.ToUpper()))
                {
                    transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().increaseCount();
                }
            }
        }
    }

    public void planetCreated(string planetName)
    {
        if (requiredCount == 1)
        {
            if (planetName.ToUpper().Contains(
                transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().desiredResource.ToUpper()))
            {
                transform.Find("QuantityWithImage2").Find("QuestSlot").GetComponent<QuestSlot>().increaseCount();
            }
        }
        else
        {
            for (int i = 1; i < requiredCount + 1; i++)
            {
                if (planetName.ToUpper().Contains(
                transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredResource.ToUpper()))
                {
                    transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().increaseCount();
                }
            }
        }
    }

    public void taskComplete()
    {
        completionCount++;
        Debug.Log("taskComplete");
        if (completionCount == requiredCount)
        {
            questCompleted();
        }
    }

    public void questCompleted()
    {
        playerMoney.addMoney(moneyReward);
        player.addExpPoints(expReward);
        if (upgradeReward.Length > 0)
        {
            UpgradeReward();
        }
        if (recipeReward.Length > 0)
        {
            RecipeReward();
        }


        transform.Find("QuestText").GetComponent<Text>().text = "";
        transform.Find("RewardText").GetComponent<Text>().text = "";
        for (int i = 1; i < 4; i++)
        {
            transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().sprite = null;
            transform.Find("QuantityWithImage" + i).Find("Image").GetComponent<Image>().enabled = false;
            transform.Find("QuantityWithImage" + i).Find("Quantity").GetComponent<Text>().text = "";
        }

        requiredCount = 0;
        completionCount = 0;
        transform.parent.parent.parent.GetComponent<QuestSystem>().removeQuest(position);
    }

    public void UpgradeReward()
    {

    }

    public void RecipeReward()
    {

    }


}
