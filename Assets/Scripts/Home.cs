using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [Range(1, 10)]
    public int numberOfStartingHomePlanets = 5;
    public int planetRingSeperation = 10;
    public int sunOffset = 20;
    public planetGenerator PG;

    public Planet rockPlanet;

    public HomeInfo info; // TO BE IMPLEMENTED LATER
    // Start is called before the first frame update
    void Awake()
    {

        // Randomly generate home planets
        for (int i = 0; i < numberOfStartingHomePlanets; i++)
        {
            int numberOfHomeRings = (int) Math.Floor((float)(PG.homeOffset - sunOffset) / planetRingSeperation);
            Debug.Log(PG.homeOffset);

            Vector2 pos = (((float)i % numberOfHomeRings) * planetRingSeperation + sunOffset) * PG.PositionGenerator(i);
            // Vector2 gridPosition = PG.GetGridPosition(pos); // calculate the grid position that this planet falls in
            PlanetInfo info = new PlanetInfo(pos.x, pos.y, 0, 100, 0);

            Planet planet = Instantiate(rockPlanet);
            planet.Initialize(info);
            
        }

}

    // Update is called once per frame
    void Update()
    {
        
    }
}
