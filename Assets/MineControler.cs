using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineControler: MonoBehaviour
{
    public static int maxMines = 10;
    public static int mineAmount = 0;
    void Awake()
    {
        mineAmount = 2;
    }
}
