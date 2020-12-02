using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    private bool infiniteMoney = false;
    private void Start()
    {
        loadDifficlutySettings(); // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
        if (infiniteMoney)
        {
            GetComponent<Text>().text = "∞";
            GetComponent<Text>().fontSize = 90;

        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("MoneyAmount", int.Parse(GetComponent<Text>().text));
    }
    public void Load()
    {
        int moneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        GetComponent<Text>().text = moneyAmount.ToString();
    }


    public void loadDifficlutySettings() // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
    {
        if (PlayerPrefs.HasKey("infiniteMoney"))
        {
            infiniteMoney = PlayerPrefs.GetInt("infiniteMoney") == 1;
        }
    }
    public void addMoney(int moneyToAdd)
    {
        if (infiniteMoney) { return; }
        int currentMoney = int.Parse(GetComponent<Text>().text);
        currentMoney += moneyToAdd;
        GetComponent<Text>().text = currentMoney.ToString();

    }
    public bool removeMoney(int moneyToRemove)
    {
        if (infiniteMoney) { return true; }
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
