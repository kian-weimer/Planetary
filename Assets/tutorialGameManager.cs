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

    private void FixedUpdate()
    {
        if(tutorialCamera.orthographicSize <= 60)
        {
            tutorialCamera.orthographicSize += Time.deltaTime*3;
        }
        else if(tutorialCamera.gameObject.activeSelf)
        {
            if(messageText.activeSelf == false)
            {
                messageText.SetActive(true);
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                tutorialCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
                messageText.SetActive(false);
            }
        }
    }
}
