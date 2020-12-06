using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] // A LIE. Just used to trik unity into displaying it in the inspector
public class ProductionItem
{
    public rsrce resource;
    public int amountProduced;
    public float frequency; // seconds
}
