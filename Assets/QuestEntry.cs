using System.Text.RegularExpressions;
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

    public GameObject tracking1;
    public GameObject tracking2;
    public GameObject tracking3;
    public Quest quest;

    public BroadcastMessage BM;

    public bool tracking = false;
    public static int bossCounter = 0;

    [HideInInspector]
    public GameObject boss; // only contains a reference if the boss has been created

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
        
        this.quest = quest;
        this.position = position;
        transform.Find("QuestText").GetComponent<Text>().text = quest.promptText;
        transform.Find("RewardText").GetComponent<Text>().text = quest.rewardText;
        requiredCount = quest.requirementCount;
        questType = quest.questType;

        if (questType == "boss")
        {
            planetGenerator PG = FindObjectOfType<planetGenerator>();
            bossCounter++;
            Vector2 pos = ((15*bossCounter + 10) * PG.spacialResolution + PG.homeOffset) * PG.PositionGenerator(10 * bossCounter);
            boss = Instantiate(quest.icons[0]);
            boss.transform.position = pos;
        }

        moneyReward = quest.moneyReward;
        expReward = quest.expReward;
        upgradeReward = quest.upgradeReward;
        recipeReward = quest.recipeReward;


        if(quest.requirementCount == 999)
        {
            return;
        }

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

    public void track()
    {
        tracking = true;
        if (requiredCount >= 1)
        {
            tracking1.transform.Find("Image").GetComponent<Image>().sprite = quest.icons[0].GetComponent<SpriteRenderer>().sprite;
            tracking1.transform.Find("Image").GetComponent<Image>().color = quest.icons[0].GetComponent<SpriteRenderer>().color;

            tracking1.transform.Find("Quantity").GetComponent<Text>().text = 0 + "/" + quest.quantities[0];
            tracking1.transform.Find("Image").GetComponent<Image>().enabled = true;
            if (requiredCount >= 2)
            {
                tracking2.transform.Find("Image").GetComponent<Image>().sprite = quest.icons[1].GetComponent<SpriteRenderer>().sprite;
                tracking2.transform.Find("Image").GetComponent<Image>().color = quest.icons[1].GetComponent<SpriteRenderer>().color;

                tracking2.transform.Find("Quantity").GetComponent<Text>().text = 0 + "/" + quest.quantities[1];
                tracking2.transform.Find("Image").GetComponent<Image>().enabled = true;
                if (requiredCount >= 3)
                {
                    tracking3.transform.Find("Image").GetComponent<Image>().sprite = quest.icons[2].GetComponent<SpriteRenderer>().sprite;
                    tracking3.transform.Find("Image").GetComponent<Image>().color = quest.icons[2].GetComponent<SpriteRenderer>().color;

                    tracking3.transform.Find("Quantity").GetComponent<Text>().text = 0 + "/" + quest.quantities[2];
                    tracking3.transform.Find("Image").GetComponent<Image>().enabled = true;
                }
            }
            updateTracking();
        }
    }

    public void stopTracking()
    {
        if (!tracking) { return; }
        tracking = false;
        tracking1.transform.Find("Image").GetComponent<Image>().enabled = false;
        tracking1.transform.Find("Quantity").GetComponent<Text>().text = "";
        tracking2.transform.Find("Image").GetComponent<Image>().enabled = false;
        tracking2.transform.Find("Quantity").GetComponent<Text>().text = "";
        tracking3.transform.Find("Image").GetComponent<Image>().enabled = false;
        tracking3.transform.Find("Quantity").GetComponent<Text>().text = "";
    }

    public void updateTracking()
    {
        if (!tracking) { return; }
        if (quest.requirementCount == 1)
        {
            tracking1.transform.Find("Quantity").GetComponent<Text>().text =
                transform.Find("QuantityWithImage2").Find("Quantity").GetComponent<Text>().text;
            tracking2.transform.Find("Quantity").GetComponent<Text>().text =
                transform.Find("QuantityWithImage1").Find("Quantity").GetComponent<Text>().text;
            tracking3.transform.Find("Quantity").GetComponent<Text>().text =
                 transform.Find("QuantityWithImage3").Find("Quantity").GetComponent<Text>().text;
        }
        else
        {
            tracking1.transform.Find("Quantity").GetComponent<Text>().text =
                transform.Find("QuantityWithImage1").Find("Quantity").GetComponent<Text>().text;
            tracking2.transform.Find("Quantity").GetComponent<Text>().text =
                transform.Find("QuantityWithImage2").Find("Quantity").GetComponent<Text>().text;
            tracking3.transform.Find("Quantity").GetComponent<Text>().text =
                 transform.Find("QuantityWithImage3").Find("Quantity").GetComponent<Text>().text;
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
                if (planetName.ToUpper().Contains(transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().desiredResource.ToUpper()))
                {
                    transform.Find("QuantityWithImage" + i).Find("QuestSlot").GetComponent<QuestSlot>().increaseCount();
                }
            }
        }
    }

    public void taskComplete()
    {
        completionCount++;
        if (completionCount == requiredCount)
        {
            questCompleted();
            stopTracking();
        }
    }

    public void questCompleted()
    {
        FindObjectOfType<AudioManager>().PlayIfNotPlaying("QuestCompleted");
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
        transform.parent.parent.parent.GetComponent<QuestSystem>().removeQuest(quest);
    }

    public void RecipeReward()
    {
        foreach (QuestAlminacReward entry in transform.parent.parent.parent.GetComponent<QuestSystem>().almanacRewards)
        {
            if (entry.name == recipeReward)
            {
                if (transform.parent.parent.parent.GetComponent<QuestSystem>().alminac.
                    AddEntry(entry.combo.planet.GetComponent<SpriteRenderer>().sprite, 
                    entry.combo.item1.name, entry.combo.item2.name, entry.combo.item3.name))
                {
                    BM.Broadcast(Regex.Replace(entry.combo.planet.name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ") +
                    " combination unlocked in the almanac!");
                }
                else
                {
                    BM.Broadcast(Regex.Replace(entry.combo.planet.name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ") +
                    " already unlocked adding additional xp instead");
                    player.addExpPoints(50);
                }

                break;
            }
        }
    }

    public void UpgradeReward()
    {
        switch (upgradeReward)
        {
            case "gas":
                player.maxGas += 50;
                break;
            case "health":
                player.maxHealth += 50;
                break;
            case "trackingWeapon":
                player.AddWeapon(Instantiate(transform.parent.parent.parent.GetComponent<QuestSystem>().trackingWeapon));
                break;
            case "deleteRegen":
                player.deletionGasRegeneration = true;
                BM.Broadcast("You will now gain gas when deleting resources from your inventory!");
                break;

            case "tutorial quest one":
                FindObjectOfType<TutorialResourceCollection>().waitingOnRock = false;
                FindObjectOfType<TutorialResourceCollection>().timeBeforeNextText = .1f;
                break;

            case "tutorial quest two":
                FindObjectOfType<TutorialResourceCollection>().createdBasePlanet();
                FindObjectOfType<TutorialResourceCollection>().timeBeforeNextText = .1f;
                break;

            case "tutorial quest three":
                FindObjectOfType<TutorialResourceCollection>().createdComboPlanet();
                FindObjectOfType<TutorialResourceCollection>().timeBeforeNextText = .1f;
                break;

            case "tutorial quest four":
                FindObjectOfType<TutorialResourceCollection>().hasKilledEnemy = true;
                FindObjectOfType<TutorialResourceCollection>().timeBeforeNextText = .1f;
                break;
        }

    }

    public void clearRewards()
    {
        moneyReward = 0;
        expReward = 0;
        upgradeReward = "";
        recipeReward = "";
    }
}
