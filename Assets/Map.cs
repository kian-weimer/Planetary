﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    public GameObject displayArea;
    public GameObject planetIcon;
    public GameObject trackingIcon;

    public float scaleFactor = 1;
    public float playerScaleFactor = 1;


    public GameObject player;
    public GameObject playerIcon;

    public GameObject boss1;
    public GameObject boss2;
    public GameObject boss3;
    public GameObject bossIcon1;
    public GameObject bossIcon2;
    public GameObject bossIcon3;

    public Texture2D texture = null;
    public RawImage mapImage;

    public static int vewableRarityLevel = 6;

    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            displayArea.transform.localScale *= 1.1f;
            //displayArea.transform.position *= 1.1f;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            displayArea.transform.localScale /= 1.1f;
            //displayArea.transform.position /= 1.1f;
        }
        if (Input.GetKey("h"))
        {
            displayArea.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        if (Input.GetKey("p"))
        {
            displayArea.GetComponent<RectTransform>().anchoredPosition = (-1 * player.transform.position / scaleFactor * playerScaleFactor) 
                * displayArea.transform.localScale.x;

        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            

        }

        //gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    public void increaseViewableRarityLevel()
    {
        vewableRarityLevel++;
    }

    public void reset()
    {
        displayArea.transform.localScale = Vector3.one;
    }

    public void addBoss(GameObject boss, int bossNumber)
    {
        if (bossNumber == 1)
        {
            bossIcon1.SetActive(true);
            boss1 = boss;
        }
        if (bossNumber == 2)
        {
            bossIcon2.SetActive(true);
            boss2 = boss;
        }
        if (bossNumber == 3)
        {
            bossIcon3.SetActive(true);
            boss3 = boss;
        }
    }
    public void removeBoss(GameObject boss, int bossNumber)
    {
        if (bossNumber == 1)
        {
            boss1 = null;
            Destroy(bossIcon1);
        }
        if (bossNumber == 2)
        {
            boss2 = null;
            Destroy(bossIcon2);
        }
        if (bossNumber == 3)
        {
            boss3 = null;
            Destroy(bossIcon3);
        }
    }

    public void open()
    {
        if (texture == null)
        {
            Initialize();
        }
        displayArea.GetComponent<RectTransform>().anchoredPosition = -1 * player.transform.position / scaleFactor * playerScaleFactor;
        playerIcon.GetComponent<RectTransform>().anchoredPosition = player.transform.position / scaleFactor * playerScaleFactor;
        playerIcon.GetComponent<RectTransform>().rotation = player.transform.rotation;

        // bosses display
        if (boss1 != null)
        {
            bossIcon1.GetComponent<RectTransform>().anchoredPosition = boss1.transform.position / scaleFactor * playerScaleFactor;
            bossIcon1.GetComponent<RectTransform>().rotation = boss1.transform.rotation;
        }
        if (boss2 != null)
        {
            bossIcon2.GetComponent<RectTransform>().anchoredPosition = boss2.transform.position / scaleFactor * playerScaleFactor;
            bossIcon2.GetComponent<RectTransform>().rotation = boss2.transform.rotation;
        }
        if (boss3 != null)
        {
            bossIcon3.GetComponent<RectTransform>().anchoredPosition = boss3.transform.position / scaleFactor * playerScaleFactor;
            bossIcon3.GetComponent<RectTransform>().rotation = boss3.transform.rotation;
        }

        texture.Apply();
    }
    // non-preferred method, takes a long time, low quality images. once made non-taxing on game
    // if home is true, will create grey planets
    public void addPlanetToMap(Planet planet, bool home = false, int rarity = -1) // if rarity is -1, will generate image based off of planet rarity
    {
        if (texture == null)
        {
            Initialize();
        }

        if (rarity == -1)
        {
            rarity = planet.rarity;
        }

        if (home)
        {
            texture.DrawCircle(Color.gray, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
        }
        else
        {
            switch (rarity)
            {
                case -2: // deletion
                    texture.DrawCircle(Color.black, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
                case 0:
                    texture.DrawCircle(Color.white, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
                case 1:
                    texture.DrawCircle(Color.cyan, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
                case 2:
                    texture.DrawCircle(Color.blue, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
                case 3:
                    texture.DrawCircle(Color.yellow, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
                case 4:
                    texture.DrawCircle(Color.magenta, (int)(2000 + planet.transform.position.x / scaleFactor),
                        (int)(2000 + planet.transform.position.y / scaleFactor), 3);
                    break;
            }
        }
        
    }

    public void Save()
    {
        SerializeTexture spriteTexture = new SerializeTexture();
        spriteTexture.x = mapImage.GetComponent<RawImage>().texture.width;
        spriteTexture.y = mapImage.GetComponent<RawImage>().texture.height;

        spriteTexture.bytes = texture.EncodeToPNG();
        FileStream fs = new FileStream("savedMapData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, spriteTexture);
        fs.Close();
    }

    public void Load()
    {
        using (Stream stream = File.Open("savedMapData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            SerializeTexture spriteTexture = (SerializeTexture)bformatter.Deserialize(stream);
            Texture2D tex = new Texture2D(spriteTexture.x, spriteTexture.y);
            ImageConversion.LoadImage(tex, spriteTexture.bytes);
            texture = tex;
            mapImage.GetComponent<RawImage>().texture = texture;
        }
    }
    public void Initialize()
    {
        texture = new Texture2D(4000, 4000, TextureFormat.RGB24, false);
        mapImage.GetComponent<RawImage>().texture = texture;
        Color fillColor = Color.black;
        Color[] fillColorArray = texture.GetPixels();

        for (var i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = fillColor;
        }

        texture.SetPixels(fillColorArray);
        texture.DrawCircle(Color.white, 2000, 2000, (int)(70 / scaleFactor));
        texture.DrawCircle(Color.black, 2000, 2000, (int)(68 / scaleFactor));
    }

    // preferred method, very fast creation, better images. Taxing on game, creates many game objects
    public void addPlanetToMapHighDef(Planet planet)
    {
        GameObject planetMapImage = Instantiate(planetIcon);
        switch (planet.rarity)
        {
            case 0:
                planetMapImage.GetComponent<Image>().color = Color.white;
                break;
            case 1:
                planetMapImage.GetComponent<Image>().color = Color.cyan;
                break;
            case 2:
                planetMapImage.GetComponent<Image>().color = Color.blue;
                break;
            case 3:
                planetMapImage.GetComponent<Image>().color = Color.yellow;
                break;
            case 4:
                planetMapImage.GetComponent<Image>().color = Color.green;
                break;
            case 5:
                planetMapImage.GetComponent<Image>().color = Color.magenta;
                break;
        }
        planetMapImage.transform.parent = displayArea.transform;
        planetMapImage.GetComponent<RectTransform>().anchoredPosition = planet.transform.position / scaleFactor;

    }

}
public static class Tex2DExtension
{
    public static Texture2D DrawCircle(this Texture2D tex, Color color, int x, int y, int radius)
    {
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    tex.SetPixel(u, v, color);

        return tex;
    }
    public static Texture2D Circle(this Texture2D tex, int x, int y, int r, Color color)
    {
        float rSquared = r * r;

        for (int u = 0; u < tex.width; u++)
        {
            for (int v = 0; v < tex.height; v++)
            {
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared) tex.SetPixel(u, v, color);
            }
        }

        return tex;
    }
}
