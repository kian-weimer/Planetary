using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialGameManager : MonoBehaviour
{
    public GameObject messageText;
    public Camera playerCamera;
    public Camera tutorialCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera.gameObject.SetActive(false);
        tutorialCamera.gameObject.SetActive(true);
    }
}
