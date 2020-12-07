using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [Range(1, 10)]
    public int numberOfHomePlanets = 5; // poorly named, just number of home planets
    public int planetRingSeperation = 10;
    public int sunOffset = 20;
    public List<Planet> homePlanets;


    [SerializeField]
    public List<PlanetInfo> planetInfo;

    [SerializeField]
    public List<HomePlanetInfo> homePlanetInfo;



    public HomePlanet homePlanet;
    public planetGenerator PG;

    public Planet rockPlanet;

    public HomeInfo info; // TO BE IMPLEMENTED LATER

    public Camera main;
    public Camera planetView;
    public int currentViewingPlanet;

    public GameObject planetHUD;
    public PlanetComboList comboList;

    public ResourceInventory resourceInventory;

    public BroadcastMessage BM;
    public Map map;

    public GameObject planetShield;

    bool loaded;

    public void Save()
    {
        List<rsrce> resources = new List<rsrce>();
        List<string> planetResourceInfos = new List<string>();

        //index,nameOfResource,count/index2,nameOfResource2,count2
        foreach (Planet planet in homePlanets)
        {
            if (planet != null)
            {
                int i = 0;
                if (planet.GetComponent<HomePlanet>().items != null)
                {
                    string tmp = "";
                    foreach (PlanetResource planetResource in planet.GetComponent<HomePlanet>().items)
                    {
                        if (planetResource.resource != null)
                        {
                            tmp += i + "," + planetResource.resource.name.Replace("(Clone)", "") + "," + planetResource.quantity + "/";
                        }
                    }
                    planetResourceInfos.Add(tmp);
                }
            }
            else
            {
                planetResourceInfos.Add("");
            }
        }

        FileStream fs = new FileStream("savedHomePlanetData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, planetInfo);
        fs.Close();

        FileStream fs2 = new FileStream("savedHomePlanetInfoData.dat", FileMode.Create);
        BinaryFormatter bf2 = new BinaryFormatter();
        bf2.Serialize(fs2, homePlanetInfo);
        fs2.Close();

        FileStream fs3 = new FileStream("savedHomePlanetResourceInfoData.dat", FileMode.Create);
        BinaryFormatter bf3 = new BinaryFormatter();
        bf3.Serialize(fs3, planetResourceInfos);
        fs3.Close();
    }

    public void Load()
    {
        if (loaded) { return; }
        numberOfHomePlanets = 0;
        loaded = true;
        using (Stream stream = File.Open("savedHomePlanetData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            planetInfo = (List<PlanetInfo>)bformatter.Deserialize(stream);
        }

        using (Stream stream = File.Open("savedHomePlanetInfoData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            homePlanetInfo = (List<HomePlanetInfo>)bformatter.Deserialize(stream);
        }

        List<string> resourceInfos = new List<string>();
        using (Stream stream = File.Open("savedHomePlanetResourceInfoData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            resourceInfos = (List<string>)bformatter.Deserialize(stream);
        }

        int i = 0;
        foreach (PlanetInfo info in planetInfo)
        {
            Planet planet = loadExistingPlanet(info);

            string resources = resourceInfos[i];
            
            string[] resourceInfo = resources.Split('/');

            foreach (string resourceSlot in resourceInfo)
            {
                if (resourceSlot != "")
                {
                    //slot[0] = index slot[1] = name slot[2] = count
                    string[] slot = resourceSlot.Split(',');

                    int index = int.Parse(slot[0]);
                    string nameOfResource = slot[1];
                    int count = int.Parse(slot[2]);

                    GameObject resourceToSpawn = null;
                    foreach (GameObject resource in FindObjectOfType<Inventory>().resources)
                    {
                        if (nameOfResource == resource.name)
                        {
                            resourceToSpawn = Instantiate(resource);
                        }
                    }
                    resourceToSpawn.GetComponent<rsrce>().isInAnInventory = true;
                    bool chk = planet.GetComponent<HomePlanet>().addItem(resourceToSpawn, count);
                    FindObjectOfType<ResourceInventory>().checkForItemAndRemove(resourceToSpawn.GetComponent<rsrce>().nameOfResource, count);
                }
            }
            i++;
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.loadingFromSave)
        {
            Load();
            return;
        }
        // Randomly generate home planets
        for (int i = 0; i < numberOfHomePlanets; i++)
        {
            int numberOfHomeRings = (int)Math.Floor((float)(PG.homeOffset - sunOffset) / planetRingSeperation);
            Vector2 pos;
            float variance = 1f;
            do
            {
                pos = (((float)i % numberOfHomeRings) * planetRingSeperation * variance + sunOffset) * PG.PositionGenerator(i);
                Collider2D[] collidersHit = Physics2D.OverlapCircleAll(pos, 5 * rockPlanet.transform.localScale.x);

                if (collidersHit.Length > 1)
                {
                    foreach (Collider2D collider in collidersHit)
                    {
                        if (collider != null)
                        {
                            if (collider.gameObject.tag == "Planet" || collider.gameObject.tag == "Player")
                            {
                                pos = Vector2.zero;
                            }
                        }
                    }

                }
                variance += 0.001f;
            } while (pos == Vector2.zero && variance < 1.1);

            if (variance > 20)
            {
                pos = (((float)i % numberOfHomeRings) * planetRingSeperation + sunOffset) * PG.PositionGenerator(i);
            }

            // Vector2 gridPosition = PG.GetGridPosition(pos); // calculate the grid position that this planet falls in
            PlanetInfo info = new PlanetInfo(pos.x, pos.y, i, rockPlanet.maxHealth, 0, true);
            planetInfo.Add(info);
            HomePlanetInfo hpi = (new HomePlanetInfo("PLANET" + i));
            homePlanetInfo.Add(hpi);

            Planet planet = Instantiate(rockPlanet);

            planet.inHomeSystem = true;
            planet.Initialize(info);

            if (planet != null)
            {
                planet.gameObject.SetActive(false);
                planet.gameObject.AddComponent<HomePlanet>();
                planet.gameObject.GetComponent<HomePlanet>().homePlanetInfo = hpi;
                planet.gameObject.GetComponent<HomePlanet>().name = "PLANET" + i;
                planet.gameObject.GetComponent<HomePlanet>().shield = planetShield;
                planet.gameObject.GetComponent<HomePlanet>().items = new List<PlanetResource>();
                for (int item = 0; item < 3; item++)
                {
                    planet.gameObject.GetComponent<HomePlanet>().items.Add(new PlanetResource());
                }
                planet.gameObject.SetActive(true);

                //planet.gameObject.name = "PLANET" + i;
                planet.transform.parent = gameObject.transform;

                homePlanets.Add(planet);
                map.addPlanetToMap(planet, true);
            }
        }
        GameManager.loadingFromSave = false;
    }

    public void addRockPlanet()
    {
        numberOfHomePlanets++;
        int numberOfHomeRings = (int)Math.Floor((float)(PG.homeOffset - sunOffset) / planetRingSeperation);

        int planetNumber = homePlanets.Count;

        Vector2 pos;
        float variance = 1f;
        do
        {
            pos = (((float)planetNumber % numberOfHomeRings) * planetRingSeperation * variance + sunOffset) * PG.PositionGenerator(planetNumber);
            Collider2D[] collidersHit = Physics2D.OverlapCircleAll(pos, 5 * rockPlanet.transform.localScale.x);

            if (collidersHit.Length > 1)
            {
                foreach (Collider2D collider in collidersHit)
                {
                    if (collider != null)
                    {
                        if (collider.gameObject.tag == "Planet" || collider.gameObject.tag == "Player")
                        {
                            Debug.Log("Overlap Detected for planet: PLANET" + planetNumber);
                            pos = Vector2.zero;
                        }
                    }
                }

            }
            variance += 0.001f;
        } while (pos == Vector2.zero && variance < 1.1f);

        if (variance >= 1.1f)
        {
            Debug.LogWarning("OverflowWarning! Planet creation overlapped due to maximum retry threshold.");
            pos = (((float)planetNumber % numberOfHomeRings) * planetRingSeperation + sunOffset) * PG.PositionGenerator(planetNumber);
        }


        PlanetInfo info = new PlanetInfo(pos.x, pos.y, planetNumber, rockPlanet.maxHealth, 0, true);
        planetInfo.Add(info);
        HomePlanetInfo hpi = (new HomePlanetInfo("PLANET" + planetNumber));
        homePlanetInfo.Add(hpi);

        Planet planet = Instantiate(rockPlanet);
        planet.inHomeSystem = true;
        planet.Initialize(info);

        planet.gameObject.SetActive(false);
        planet.gameObject.AddComponent<HomePlanet>();
        planet.gameObject.GetComponent<HomePlanet>().homePlanetInfo = hpi;
        planet.gameObject.GetComponent<HomePlanet>().name = "PLANET" + planetNumber;
        planet.gameObject.GetComponent<HomePlanet>().shield = planetShield;
        planet.gameObject.SetActive(true);

        //planet.gameObject.name = "PLANET" + planetNumber;
        planet.transform.parent = gameObject.transform;
        homePlanets.Add(planet);
        map.addPlanetToMap(planet, true);

    }

    public Planet loadExistingPlanet(PlanetInfo planetInfo)
    {
        if (planetInfo == null || planetInfo.position == null)
        {
            homePlanets.Add(null);
            numberOfHomePlanets++;
            return null;
        }
        numberOfHomePlanets++;
        Planet planet = Instantiate(rockPlanet);
        planet.inHomeSystem = true;
        planet.Initialize(planetInfo);
        planet.transform.parent = gameObject.transform;

        planet.gameObject.SetActive(false);
        planet.gameObject.AddComponent<HomePlanet>();
        planet.gameObject.GetComponent<HomePlanet>().homePlanetInfo = homePlanetInfo[planetInfo.rarity];
        planet.gameObject.GetComponent<HomePlanet>().name = homePlanetInfo[planetInfo.rarity].name;
        planet.gameObject.GetComponent<HomePlanet>().shield = planetShield;
        planet.gameObject.GetComponent<HomePlanet>().items = new List<PlanetResource>();
        for (int item = 0; item < 3; item++)
        {
            planet.gameObject.GetComponent<HomePlanet>().items.Add(new PlanetResource());
        }
        planet.gameObject.SetActive(true);


        if (homePlanetInfo[planetInfo.rarity].hasShield)
        {
            planet.gameObject.GetComponent<HomePlanet>().addShield();
            planet.gameObject.GetComponent<HomePlanet>().damageShield(planet.gameObject.GetComponent<HomePlanet>().maxShield - homePlanetInfo[planetInfo.rarity].shieldHealth);
        }

        homePlanets.Add(planet);
        if (homePlanetInfo[planetInfo.rarity].comboIndex != -1)
        {
            ComboFromIndex(planet.gameObject, homePlanetInfo[planetInfo.rarity].comboIndex);
        }
        return planet;
    }

    public void IncreasePlanetView(int changeValue)
    {
        Planet planet = homePlanets[currentViewingPlanet];
        bool flipped = false;
        if (changeValue < 0 && currentViewingPlanet == 0)
        {
            changeValue = numberOfHomePlanets - 1;
            flipped = true;
        }

        currentViewingPlanet = (planet.GetComponent<Planet>().rarity + changeValue) % numberOfHomePlanets;
        

        while (homePlanets[currentViewingPlanet] == null)
        {
            if (changeValue > 0 && !flipped)
            {
                changeValue++;
            }
            else
            {
                changeValue--;
                if (changeValue < 0 && currentViewingPlanet == 0)
                {
                    changeValue = numberOfHomePlanets - 1;
                    flipped = true;
                }
            }
            currentViewingPlanet = (planet.GetComponent<Planet>().rarity + changeValue) % numberOfHomePlanets;
        }

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
        try
        {
            HUD.Find("PlanetName").GetComponent<InputField>().text = homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name;
            homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name = HUD.Find("PlanetName").GetComponent<InputField>().text;
            homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().homePlanetInfo.name = homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name;

            //Debug.Log(HUD.Find("PlanetName").GetComponent<InputField>().text.GetType());
            //Debug.Log(homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().name.GetType());
        }
        //for tutorial
        catch (Exception e){ Debug.Log("Fuck you"); }

        // change health bar
        HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale =
            new Vector3(homePlanets[currentViewingPlanet].health / (float)homePlanets[currentViewingPlanet].maxHealth,
            HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale.y,
            HUD.Find("Health Bar").Find("BarBG").Find("HealthBar").GetComponent<RectTransform>().localScale.z);

        // change health number
        HUD.Find("Health Bar").Find("NumeratedHealth").GetComponent<Text>().text = homePlanets[currentViewingPlanet].health + "/" + homePlanets[currentViewingPlanet].maxHealth;

        homePlanets[currentViewingPlanet].GetComponent<HomePlanet>().UpdateUI();
    }

    public void Combo()
    {
        GameObject comboPlanet = homePlanets[currentViewingPlanet].gameObject;
        HomePlanet comboHomeInfo = comboPlanet.GetComponent<HomePlanet>();
        PlanetCombo combo;
        if (comboList.Combo(comboHomeInfo.items[0], comboHomeInfo.items[1], comboHomeInfo.items[2]) != null)
        {
            combo = comboList.Combo(comboHomeInfo.items[0], comboHomeInfo.items[1], comboHomeInfo.items[2]);
            comboPlanet.gameObject.name = combo.planet.name;
            comboPlanet.GetComponent<SpriteRenderer>().sprite = combo.planet.GetComponent<SpriteRenderer>().sprite;
            comboPlanet.GetComponent<HomePlanet>().productionItems = combo.productionItems;
            comboPlanet.GetComponent<HomePlanet>().homePlanetInfo.comboIndex = combo.index;


            resourceInventory.checkForItemAndRemove(getCurrentViewingPlanet().GetComponent<HomePlanet>().items[0].resource.GetComponent<rsrce>().nameOfResource, 1);
            resourceInventory.checkForItemAndRemove(getCurrentViewingPlanet().GetComponent<HomePlanet>().items[1].resource.GetComponent<rsrce>().nameOfResource, 1);
            resourceInventory.checkForItemAndRemove(getCurrentViewingPlanet().GetComponent<HomePlanet>().items[2].resource.GetComponent<rsrce>().nameOfResource, 1);

            getCurrentViewingPlanet().GetComponent<HomePlanet>().removeItem(0);
            getCurrentViewingPlanet().GetComponent<HomePlanet>().removeItem(1);
            getCurrentViewingPlanet().GetComponent<HomePlanet>().removeItem(2);
            getCurrentViewingPlanet().GetComponent<HomePlanet>().UpdateUI();
        }
    }

    // to only be used when loading a save
    public void ComboFromIndex(GameObject comboPlanet, int comboIndex)
    {
        PlanetCombo combo;
        combo = comboList.ComboFromIndex(comboIndex);
        comboPlanet.gameObject.name = combo.planet.name;
        comboPlanet.GetComponent<SpriteRenderer>().sprite = combo.planet.GetComponent<SpriteRenderer>().sprite;
        comboPlanet.GetComponent<HomePlanet>().productionItems = combo.productionItems;
    }
}