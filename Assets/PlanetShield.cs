using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShield : MonoBehaviour
{
    public void damage(float damage)
    {
        transform.parent.GetComponent<HomePlanet>().damageShield(damage);
    }
}
