using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

// Y GRID POSITION REPORTS 1 VALUES GREATER THAN ACTUAL...
public class planetGenerator : MonoBehaviour
{
    // rows correspond to planet rarity
    // cols porrespond to individual planets
    public List<PlanetList> planetTypes;

    // a grid system (xpos, ypos) where each grid section is size gridResolutionxgridResolution
    // each grid position contains the info of every planet in that position
    public Dictionary<Vector2, ArrayList>  planetInfoList; // the info of each of the actual planets in the game

    // a list of all planet gameObjects currently spawned in
    // NOT INCLUDING HOME PLANETS
    public ArrayList planetsObjectsInGame;

    // distance from center before planets spawn in
    public int homeOffset = 1000;

    // smaller values mean less degrees of space between planets
    public float degreeResolution = 10; // the number of degrees each increment of the random degree creation makes // 0.5

    public float iterationalRotationalOffset = 2; // number of degrees that are offset after each ring // 0.1

    public int spacialResolution = 10; // the distance to increment by on each ring iteration

    public int frequency = 1; // the number of planets to be created on each layer

    public int frequencyIncrementor = 5; // how often the frequency increaces

    public int frequencyResolution = 1; // the amount to increment the frequency by

    public int rarityRingDistanceLimit = 20;

    // public float planetSizeVariance = 0.2f;

    public Map map;


    // Start is called before the first frame update
    void Awake()
    {
        /* creates a home circle
        var go1 = new GameObject { name = "HomeCircle" };
        go1.transform.position = new Vector2(0, 0);
        go1.transform.Rotate(90f, 0, 0);
        go1.DrawCircle(70, .25f);//radius, thickness
        */
        // Randomly generate all outside planets
        int numberOfRings = FindObjectOfType<GameManager>().numberOfRings;
        planetInfoList = new Dictionary<Vector2, ArrayList>();
        planetsObjectsInGame = new ArrayList();
        int maxRarity = 1; // the maximum rarity level allowed in a ring
        for (int i = 0; i < numberOfRings; i++)
        {
            if ((i+1)%rarityRingDistanceLimit == 0)
            {
                maxRarity += 1;
            }
            if (i%frequencyIncrementor == 0)
            {
                frequency += frequencyResolution;
            }
            for (int j = 0; j < frequency; j++)
            {
                int rarity = determinePlanetRarity(maxRarity); // find better way to implement
                int planet = UnityEngine.Random.Range(0, planetTypes[rarity].planets.Count);
                Vector2 pos = (i * spacialResolution + homeOffset)* PositionGenerator(i);
                Vector2 gridPosition = GetGridPosition(pos); // calculate the grid position that this planet falls in

                // max health must be manually entered into planet prefab
                PlanetInfo info = new PlanetInfo(pos.x, pos.y, rarity, planetTypes[rarity].planets[planet].maxHealth, planet, false);
                

                // A planet already exists in this grid position
                if (planetInfoList.ContainsKey(gridPosition))
                {
                    // check that that position isnt already filled
                    bool duplicate = false;
                    foreach (PlanetInfo storedInfo in planetInfoList[gridPosition])
                    {
                        //if (Math.Abs(storedInfo.position[0] - pos.x) < planetDistance && (storedInfo.position[1] - pos.y) < planetDistance)
                        // protects against exact overlaps
                        if (storedInfo.position[0] == pos.x && storedInfo.position[1] == pos.y)
                        {
                            duplicate = true;
                            //Debug.Log("PL");
                            break;
                        }

                    }

                    if (!duplicate)
                    {
                        planetInfoList[gridPosition].Add(info);
                    }
                }

                // This is the first planet in the grid position
                else
                {
                    planetInfoList[gridPosition] = new ArrayList();
                    planetInfoList[gridPosition].Add(info);
                }
            }
        }

        //InstantiateAllPlanets();
        // fill planetsInGame list
        // this contains a list of the info of all planets in the game
        //Instantiate(planetTypes[0], new Vector2(0, 0), Quaternion.identity);
    }

    public void Regenerate()
    {
        DestroyAllPlanets();
        planetInfoList.Clear();
        frequency = 1;

        int numberOfRings = FindObjectOfType<GameManager>().numberOfRings;
        int gridResolution = FindObjectOfType<GameManager>().gridResolution;

        planetInfoList = new Dictionary<Vector2, ArrayList>();
        planetsObjectsInGame = new ArrayList();

        int maxRarity = 1; // the maximum rarity level allowed in a ring

        for (int i = 0; i < numberOfRings; i++)
        {
            if ((i + 1) % rarityRingDistanceLimit == 0)
            {
                maxRarity += 1;
            }
            if (i % frequencyIncrementor == 0)
            {
                frequency += frequencyResolution;
            }
            for (int j = 0; j < frequency; j++)
            {
                int rarity = determinePlanetRarity(maxRarity); // find better way to implement
                int planet = UnityEngine.Random.Range(0, planetTypes[rarity].planets.Count);
                Vector2 pos = (i * spacialResolution + homeOffset) * PositionGenerator(i);
                // calculate the grid position that this planet falls in
                Vector2 gridPosition = new Vector2((float)Math.Floor(pos.x / gridResolution), (float)Math.Floor(pos.y / gridResolution));
                PlanetInfo info = new PlanetInfo(pos.x, pos.y, rarity, 100, planet, false);

                if (planetInfoList.ContainsKey(gridPosition))
                {
                    // check that that position isnt already filled
                    bool duplicate = false;
                    foreach (PlanetInfo storedInfo in planetInfoList[gridPosition])
                    {
                        if (storedInfo.position[0] == pos.x && storedInfo.position[1] == pos.y)
                        {
                            duplicate = true;
                            print("DUP");
                            break;
                        }
                    }

                    if (!duplicate)
                    {
                        planetInfoList[gridPosition].Add(info);
                    }
                }

                else
                {
                    planetInfoList[gridPosition] = new ArrayList();
                    planetInfoList[gridPosition].Add(info);
                }
            }
        }
    }

    public Vector2 GetGridPosition(Vector2 pos)
    {
        int gridResolution = FindObjectOfType<GameManager>().gridResolution;
        return new Vector2((float)Math.Floor(pos.x / gridResolution), (float)Math.Floor(pos.y / gridResolution));
    }

    public Vector2 PositionGenerator(int ringNumber)
    {
        float degrees = (UnityEngine.Random.Range(0, (int)(360f / degreeResolution)) * degreeResolution) + iterationalRotationalOffset * ringNumber; // may want ro reduce rondomness of this
        return new Vector2(Mathf.Cos((degrees * Mathf.PI) / 180), Mathf.Sin((degrees * Mathf.PI) / 180));
    }

    int determinePlanetRarity(int maxRarity)
    {
        // determin the planet rarity
        // must be a better way...
        int range = 0;

        int rarity = 0;
        foreach (PlanetList list in planetTypes)
        {
            if (rarity > maxRarity - 1)
            {
                break;
            }
            range += list.rarityLevel;
            rarity++;
        }

        int rarityValue = UnityEngine.Random.Range(0, range);
        range = 0;
        rarity = 0;

        foreach (PlanetList list in planetTypes)
        {
            range += list.rarityLevel;
            if (range > rarityValue || rarity == maxRarity - 1)
            {
                return rarity;
            }
            rarity++;
        }

        return 0;
    }

    // SHOULD NEVER BE USED!!!!
    void InstantiateAllPlanets()
    {
        foreach (KeyValuePair<Vector2, ArrayList> gridPosition in planetInfoList)
        {
            foreach (PlanetInfo planetInfo in gridPosition.Value)
            {
                Planet planet = Instantiate(planetTypes[planetInfo.rarity].planets[planetInfo.type]);
                planet.Initialize(planetInfo);
                planetsObjectsInGame.Add(planet.gameObject);
            }
        }
     
    }

    void DestroyAllPlanets()
    {
        foreach (GameObject planet in planetsObjectsInGame)
        {
            Destroy(planet);
        }
    }

    public void InstantiateGrid(Vector2 gridPosition)
    {
        if (!planetInfoList.ContainsKey(gridPosition)) { Debug.LogWarning("PlanetGenError"); return; }
        foreach (PlanetInfo planetInfo in planetInfoList[gridPosition])
        {
            // overlapping planets are simply ignored///////////////////////
            Vector2 pos = new Vector2(planetInfo.position[0], planetInfo.position[1]);
            Collider2D[] collidersHit = Physics2D.OverlapCircleAll(pos, 5 * planetTypes[planetInfo.rarity].planets[planetInfo.type].transform.localScale.x);
            if (collidersHit.Length > 1)
            {
                foreach (Collider2D collider in collidersHit)
                {
                    if (collider != null)
                    {
                        if (collider.gameObject.tag == "Planet" || collider.gameObject.tag == "Player")
                        {
                            Debug.Log("Overlap Detected for planet: PGPLANET LVL" + planetInfo.rarity);
                            pos = Vector2.zero;
                        }
                    }
                }
                if (pos == Vector2.zero)
                {
                    continue;
                }
            }
            ////////////////////////////////////////////////////////////////

            Planet planet = Instantiate(planetTypes[planetInfo.rarity].planets[planetInfo.type]);
            planet.Initialize(planetInfo); 
            if (planetInfo.discovered < 2)
            {
                if (planetInfo.rarity <= Map.vewableRarityLevel) {
                    planetInfo.discovered = 2;
                    map.addPlanetToMap(planet);
                }
                if (planetInfo.rarity > Map.vewableRarityLevel)
                {
                    planetInfo.discovered = 1;
                    map.addPlanetToMap(planet, false, Map.vewableRarityLevel);
                }
            }
            planetsObjectsInGame.Add(planet.gameObject);
        }
    }

    public void DestroyGrid(Vector2 gridPosition) // INEFFICIENT GARBAGE
    {
        GameObject[] planetsObjectsInGameTemp = new GameObject[planetsObjectsInGame.Count];
        planetsObjectsInGame.CopyTo(planetsObjectsInGameTemp);
        ArrayList planetsObjectsInGameArrayTemp = new ArrayList();
        planetsObjectsInGameArrayTemp.AddRange(planetsObjectsInGameTemp);

        if (!planetInfoList.ContainsKey(gridPosition)) { Debug.LogWarning("PlanetGenError"); return; }
        foreach (GameObject planet in planetsObjectsInGame)
        {
            if (planetInfoList[gridPosition].Contains(planet.GetComponent<Planet>().info)) // INEFFICIENT
            {
                Destroy(planet);
                planetsObjectsInGameArrayTemp.Remove(planet);
            }
        }

        planetsObjectsInGame = planetsObjectsInGameArrayTemp;

    }
    public void destroyPlanet(Planet planet)
    {
        if(planet.gameObject.GetComponent<HomePlanet>() == null)
        {
            planetInfoList[GetGridPosition(planet.position)].Remove(planet.info);
            planetsObjectsInGame.Remove(planet.gameObject);
        }
        map.addPlanetToMap(planet, false, -1);
        Destroy(planet.gameObject);
    }
}
 