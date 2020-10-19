using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameItem : MonoBehaviour
{
    static bool EndGameReached = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!EndGameReached)
        {
            FindObjectOfType<canvas>().transform.Find("EndGamePrompt").gameObject.SetActive(true);
            EndGameReached = true;
        }
    }
}
