using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetHUD : MonoBehaviour
{
    public PlayerController player;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        player.speed = 0;
        audioManager.Stop("Thrust");
    }
}
