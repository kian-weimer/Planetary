using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject player;
    public GameObject home;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Mathf.Abs(player.GetComponent<Rigidbody2D>().rotation) % 360;
        bool positive = player.GetComponent<Rigidbody2D>().rotation > 0;
        Vector3 relativePos = player.transform.position.normalized;

        // the second argument, upwards, defaults to Vector3.up
        //float homeRotation = Mathf.Abs((Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles.x)) % 360;
        float homeRotation = Mathf.Rad2Deg * (Mathf.Atan2(relativePos.x, relativePos.y)) + 180;

        if (rotation <= 180 && positive)
        {
            transform.GetChild(0).localPosition = new Vector3(rotation * 10, 0, 0);
            home.transform.localPosition = new Vector3((360 - homeRotation) * -10, 0, 0);

        }
        else if (rotation > 180 && !positive)
        {
            transform.GetChild(0).localPosition = new Vector3((360 - rotation) * 10, 0, 0);
            home.transform.localPosition = new Vector3((360 - homeRotation) * -10, 0, 0);
           
        }
        else if (rotation <= 180 && !positive)
        {
            transform.GetChild(0).localPosition = new Vector3(rotation * -10, 0, 0);
            home.transform.localPosition = new Vector3(homeRotation * 10, 0, 0); 
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3((360 - rotation) * -10, 0, 0);
            //home.transform.localPosition = new Vector3(homeRotation * 10, 0, 0);
            home.transform.localPosition = new Vector3(homeRotation * 10, 0, 0);

        }
    }
}
