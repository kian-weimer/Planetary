﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlanetResource
{
    public GameObject resource = null;
    public int quantity = 0;
    public void Set(GameObject resource, int quantity)
    {
        this.resource = resource;
        this.quantity = quantity;
    }

    public GameObject getResouce()
    {
        return resource;
    }

}
