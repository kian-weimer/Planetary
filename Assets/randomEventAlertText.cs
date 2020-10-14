using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class randomEventAlertText : MonoBehaviour
{
    public float timeBetweenFlash = 1;
    public float timeToShow = 2;
    private float totalTime;
    private bool isFlashing;
    private int numberOfTimesFlashed = 0;
    public int maxNumberOfFlashes = 5;
    // Start is called before the first frame update
    void Start()
    {
        canvas mainCanvas = FindObjectOfType<canvas>();
        totalTime = timeBetweenFlash + timeToShow;
        transform.parent = mainCanvas.transform;
        transform.position = new Vector3(mainCanvas.GetComponent<RectTransform>().rect.width / 2, mainCanvas.GetComponent<RectTransform>().rect.height/2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        totalTime -= Time.deltaTime;
        if(totalTime <= timeBetweenFlash)
        {
            Color colorOfText = gameObject.GetComponent<Text>().color;  //  makes a new color zm
            colorOfText.a = 0.0f;
            gameObject.GetComponent<Text>().color = colorOfText;
        }
        if(totalTime <= 0)
        {
            numberOfTimesFlashed++;
            Color colorOfText = gameObject.GetComponent<Text>().color;  //  makes a new color zm
            colorOfText.a = 1;
            gameObject.GetComponent<Text>().color = colorOfText;
            totalTime = timeBetweenFlash + timeToShow;
        }

        if(numberOfTimesFlashed == maxNumberOfFlashes)
        {
            Destroy(gameObject);
        }
    }
}
