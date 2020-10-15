using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject planetHUD;
    // Start is called before the first frame update
    void Awake()
    {

        // Randomly generate home planets
        for (int i = 0; i < numberOfStartingHomePlanets; i++)
        {
            int numberOfHomeRings = (int) Math.Floor((float)(PG.homeOffset - sunOffset) / planetRingSeperation);

            Vector2 pos = (((float)i % numberOfHomeRings) * planetRingSeperation + sunOffset) * PG.PositionGenerator(i);
            // Vector2 gridPosition = PG.GetGridPosition(pos); // calculate the grid position that this planet falls in
            PlanetInfo info = new PlanetInfo(pos.x, pos.y, i, rockPlanet.maxHealth, 0, true);

            Planet planet = Instantiate(rockPlanet);
            planet.inHomeSystem = true;
            planet.Initialize(info);
            planet.gameObject.AddComponent<HomePlanet>();
            planet.gameObject.GetComponent<HomePlanet>().name = "PLANET" + i;
            planet.transform.parent = gameObject.transform;
            homePlanets.Add(planet);
        }
}
    public void IncreasePlanetView(int changeValue)
    {
        Planet planet = homePlanets[currentViewingPlanet];

        if (changeValue < 0 && currentViewingPlanet == 0)
        {
            changeValue = numberOfStartingHomePlanets - 1;
        }
        
        currentViewingPlanet = (planet.GetComponent<Planet>().rarity + changeValue) % numberOfStartingHomePlanets;

        while (homePlanets[currentViewingPlanet] == null)
        {
            if (changeValue > 0)
            {
                changeValue++;
            }
            else
            {
                changeValue--;
            }
            currentViewingPlanet = (planet.GetComponent<Planet>().rarity + changeValue) % numberOfStartingHomePlanets;
        }
        Debug.Log(currentViewingPlanet);
        Planet neighborPlanet = homePlanets[currentViewingPlanet];
        planetView.transform.position = new Vector3(neighborPlanet.transform.position.x, neighborPlanet.transform.position.y, -10);

        UpdatePlanetHud();
    }

    public void ChangePlanetView(int index)
    {
        currentViewingPlanet = index;
        Planet neighborPlanet = homePlanets[currentViewingPlanet];
        planetView.transform.position = new Vector3(neighborPlanet.transform.position.x, neighborPlanet.transform.position.y, -10);
        UpdatePlanetHud();
    }

    public GameObject getCurrentViewingPlanet()
    {
        return homePlanets[currentViewingPlanet].gameObject;
    }

    public void ChangePlanetName(Text text)
    {
        homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name = text.text;
    }
   

    public void UpdatePlanetHud()
    {
        Transform HUD = FindObjectOfType<canvas>().transform.Find("PlanetHUD");

        // changeName don't question it, due to listener on input...
        HUD.Find("PlanetName").GetComponent<InputField>().text = homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name;
        homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name = HUD.Find("PlanetName").GetComponent<InputField>().text;

        // change health bar
        HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale =
            new Vector3(homePlanets[currentViewingPlanet].health / (float) homePlanets[currentViewingPlanet].maxHealth,
            HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale.y,
            HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale.z);

        // change health number
        HUD.Find("Health Bar").Find("NumeratedHealth").GetComponent<Text>().text = homePlanets[currentViewingPlanet].health + "/" + homePlanets[currentViewingPlanet].maxHealth;

        homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().UpdateUI();


    }
}
