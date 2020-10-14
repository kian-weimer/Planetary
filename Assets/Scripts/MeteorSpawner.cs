using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    private RectTransform rt;
    public int numberOfAsteroids;
    public float distanceAway;
    public float timeBetweenWaves = 10.0f;

    public float timeForMeteorWave;

    public GameObject meteor;

    public bool isSpawningMeteors;
    public int numberOfWaves;
    private int numberOfWavesOccured = 0;
    private float degree;

    void Awake()
    {
        isSpawningMeteors = false;
       // timeForMeteorWave = timeBetweenWaves;

        rt = GetComponent<RectTransform>();
        degree = Random.Range(0, 360);

        float x = Mathf.Cos(degree * Mathf.PI / 180) * distanceAway;
        float y = Mathf.Sin(degree * Mathf.PI / 180) * distanceAway;

        rt.transform.position = new Vector2(x, y) + FindObjectOfType<Player>().GetComponent<Rigidbody2D>().position;

        //instantiate your dot in the bounds of that recttransform

        rt.rotation = Quaternion.Euler(new Vector3(0, 0, degree + 90 - 180)); //Random.Range(0, 360) - 90 the other angle
    }
    private void Update()
    {
        timeForMeteorWave -= Time.deltaTime;

        if (timeForMeteorWave <= 0.0f && numberOfWaves - numberOfWavesOccured != 0)
        {
            rt.rotation = Quaternion.Euler(new Vector3(0, 0, 0)); //Random.Range(0, 360) - 90 the other angle
            timeForMeteorWave = timeBetweenWaves;
            numberOfWavesOccured++;
            for (int i = 0; i < numberOfAsteroids; i++)
            {
                Instantiate(meteor, new Vector3(Random.Range(rt.rect.x, rt.rect.x + rt.rect.width),
                      Random.Range(rt.rect.y, rt.rect.y + rt.rect.height), 0) + rt.transform.position, Quaternion.identity).transform.parent = gameObject.transform;
            }
            rt.rotation = Quaternion.Euler(new Vector3(0, 0, degree + 90 - 180)); //Random.Range(0, 360) - 90 the other angle
        }

        //Todo Fix it so it waits to destroy
        if(numberOfWavesOccured == numberOfWaves)
        {
            Destroy(gameObject);
        }
    }
}