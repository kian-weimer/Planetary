using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public int questFrequency;
    float lastQuestCreationTime;
    public int currentQuestCount = 1;

    public List<Quest> quests;
    public List<Quest> completedQuests;
    public List<QuestAlminacReward> almanacRewards;

    public int questIndex = 1;
    public QuestEntry questEntry1;
    public QuestEntry questEntry2;
    public QuestEntry questEntry3;

    public GameObject background;
    public Alminac alminac;
    public GameObject trackingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        lastQuestCreationTime = -1 * questFrequency;// Time.time;
        int i = 1;
        foreach (Quest quest in quests)
        {
            quest.ID = i;
            i++;
        }
        //questEntry1.set(1, quests[0]);
    }

    public void Save()
    {
        List<int> completedQuestIDs = new List<int>();
        foreach (Quest quest in completedQuests)
        {
            completedQuestIDs.Add(quest.ID);
        }
        FileStream fs = new FileStream("savedQuestData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, completedQuestIDs);
        fs.Close();
    }

    public void Load()
    {
        List<int> completedQuestIDs;
        using (Stream stream = File.Open("savedQuestData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            completedQuestIDs = (List<int>)bformatter.Deserialize(stream);
        }

        QuestEntry tempQuestEntry = new QuestEntry(); // used to apply quest upgrades
        tempQuestEntry.player = FindObjectOfType<Player>();
        tempQuestEntry.BM = FindObjectOfType<GameManager>().BM;

        // fill quest GUI
        addQuest();
        addQuest();
        addQuest();

        // complete each saved quest
        foreach (int ID in completedQuestIDs)
        {
            Quest quest = new Quest();

            // find quest by ID
            foreach (Quest uncompletedQuest in quests)
            {
                if (uncompletedQuest.ID == ID)
                {
                    quest = uncompletedQuest;
                }
            }

            //quests.Remove(quest); // remove quest from list

            // apply quest upgrade if not gas or health
            if (quest.upgradeReward.Length > 0 && quest.upgradeReward != "gas" && quest.upgradeReward != "health")
            {
                tempQuestEntry.upgradeReward = quest.upgradeReward;
                tempQuestEntry.UpgradeReward();
            }

            // remove quest from UI if it is present
            if (questEntry1.quest == quest)
            {
                questEntry1.clearRewards(); // remove rewards (should be saved by other means)
                questEntry1.questCompleted(); // mark as complete
                addQuest(); // add new quest to GUI
            }
            else if (questEntry2.quest == quest)
            {
                questEntry2.clearRewards();
                questEntry2.questCompleted();
                addQuest(); // add new quest to GUI
            }
            else if (questEntry3.quest == quest)
            {
                questEntry3.clearRewards();
                questEntry3.questCompleted();
                addQuest(); // add new quest to GUI
            }

        }
    }

    public void Dissappear()
    {
        background.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    public void Appear()
    {
        background.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastQuestCreationTime > questFrequency && currentQuestCount < 3 && questIndex < quests.Count)
        {
            addQuest();
            lastQuestCreationTime = Time.time;
        }
    }

    public void addQuest()
    {
        if (questEntry1.requiredCount == 0)
        {
            currentQuestCount++;
            questEntry1.set(1, quests[questIndex]);
            questIndex++;
        }
        else if (questEntry2.requiredCount == 0)
        {
            currentQuestCount++;
            questEntry2.set(2, quests[questIndex]);
            questIndex++;
        }
        else if (questEntry3.requiredCount == 0)
        {
            currentQuestCount++;
            questEntry3.set(3, quests[questIndex]);
            questIndex++;
        }
    }

    public void removeQuest(Quest quest)
    {
        currentQuestCount--;
        completedQuests.Add(quest);
    }

    public void updateQuestsEnemy(string name)
    {
        if (questEntry1.questType == "enemy" || questEntry1.questType == "boss")
        {
            questEntry1.enemyKilled(name);
        }
        if (questEntry2.questType == "enemy" || questEntry2.questType == "boss")
        {
            questEntry2.enemyKilled(name);
        }
        if (questEntry3.questType == "enemy" || questEntry3.questType == "boss")
        {
            questEntry3.enemyKilled(name);
        }
    }

    public void updateQuestsPlanet(string name)
    {
        if (questEntry1.questType == "planet")
        {
            questEntry1.planetCreated(name);
        }
        if (questEntry2.questType == "planet")
        {
            questEntry2.planetCreated(name);
        }
        if (questEntry3.questType == "planet")
        {
            questEntry3.planetCreated(name);
        }
    }
}
