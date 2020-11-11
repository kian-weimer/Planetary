using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public int questFrequency;
    float lastQuestCreationTime;
    public int currentQuestCount = 1;

    public List<Quest> quests;
    public List<QuestAlminacReward> almanacRewards;

    public int questIndex = 1;
    public QuestEntry questEntry1;
    public QuestEntry questEntry2;
    public QuestEntry questEntry3;

    public GameObject background;
    public Alminac alminac;


    // Start is called before the first frame update
    void Start()
    {
        lastQuestCreationTime = -1 * questFrequency;// Time.time;
        //questEntry1.set(1, quests[0]);
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
            questEntry1.set(1, quests[questIndex]);
            questIndex++;
        }
        else if (questEntry2.requiredCount == 0)
        {
            questEntry2.set(2, quests[questIndex]);
            questIndex++;
        }
        else if (questEntry3.requiredCount == 0)
        {
            questEntry3.set(3, quests[questIndex]);
            questIndex++;
        }
    }

    public void removeQuest(int questSlot)
    {

    }

    public void updateQuestsEnemy(string name)
    {
        if (questEntry1.questType == "enemy")
        {
            questEntry1.enemyKilled(name);
        }
        else if (questEntry2.questType == "enemy")
        {
            questEntry2.enemyKilled(name);
        }
        else if (questEntry3.questType == "enemy")
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
        else if (questEntry2.questType == "planet")
        {
            questEntry2.planetCreated(name);
        }
        else if (questEntry3.questType == "planet")
        {
            questEntry3.planetCreated(name);
        }
    }
}
