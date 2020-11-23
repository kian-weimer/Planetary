using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenEventStartToken : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Inventory>().transform.GetComponentInChildren<OxygenSlot>(true).startEvent();
        Destroy(gameObject);
    }
}
