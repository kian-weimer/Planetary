using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public float waitTime = 0.15f;
    public Color color;
    public bool useColor;

    public void start()
    {
        StartCoroutine(startFlash(waitTime));
    }

    IEnumerator startFlash(float duration)
    {
        Color storedColor = gameObject.GetComponent<SpriteRenderer>().color;
        if (!useColor)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = color;

        }
        float start = Time.time;
        float time = duration;
        //yield on a new YieldInstruction that waits for 5 seconds.
        while (Time.time - start < waitTime)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().color = storedColor;
    }
}
