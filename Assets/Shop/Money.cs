using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    // Start is called before the first frame update
    public void addMoney(int moneyToAdd)
    {
        int currentMoney = int.Parse(GetComponent<Text>().text);
        currentMoney += moneyToAdd;
        GetComponent<Text>().text = currentMoney.ToString();

    }
    public bool removeMoney(int moneyToRemove)
    {
        int currentMoney = int.Parse(GetComponent<Text>().text);
        int potentialMoney = currentMoney -= moneyToRemove;

        if (potentialMoney >= 0)
        {
            GetComponent<Text>().text = potentialMoney.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }
}
