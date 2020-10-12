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
    public List<Planet> homePlanets;
    public HomePlanet homePlanet; 
    public planetGenerator PG;

    public Planet rockPlanet;

    public HomeInfo info; // TO BE IMPLEMENTED LATER

    public Camera main;
    public Camera planetView;
    public int currentViewingPlanet;
    // Start is called before the first frame update
    void Awake()
    {

        // Randomly generate home planets
        for (int i = 0; i < numberOfStartingHomePlanets; i++)
        {
            int numberOfHomeRings = (int) Math.Floor((float)(PG.homeOffset - sunOffset) / planetRingSeperation);

            Vector2 pos = (((float)i % numberOfHomeRings) * planetRingSeperation + sunOffset) * PG.PositionGenerator(i);
            // Vector2 gridPosition = PG.GetGridPosition(pos); // calculate the grid position that this planet falls in
            PlanetInfo info = new PlanetInfo(pos.x, pos.y, i, 100, 0, true);

            Planet planet = Instantiate(rockPlanet);
            planet.inHomeSystem = true;
            planet.Initialize(info);
            planet.gameObject.AddComponent<HomePlanet>();
            homePlanets.Add(planet);
        }
}
    public void IncreasePlanetView(int changeValue)
    {
        Planet planet = homePlanets[currentViewingPlanet];

        if (changeValue < -planet.GetComponent<Planet>().rarity)
        {
            changeValue = numberOfStartingHomePlanets + changeValue - planet.GetComponent<Planet>().rarity;
        }
        
        currentViewingPlanet = (planet.GetComponent<Planet>().rarity + changeValue) % numberOfStartingHomePlanets;
        Planet neighborPlanet = homePlanets[(planet.GetComponent<Planet>().rarity + changeValue) % numberOfStartingHomePlanets];
        planetView.transform.position = new Vector3(neighborPlanet.transform.position.x, neighborPlanet.transform.position.y, -10);
    }

    public void ChangePlanetView(int index)
    {
        currentViewingPlanet = index;
        Planet neighborPlanet = homePlanets[index];
        planetView.transform.position = new Vector3(neighborPlanet.transform.position.x, neighborPlanet.transform.position.y, -10);
    }
   

    // Update is called once per frame
    void UpdatePlanetHud()
    {
        
    }
}
