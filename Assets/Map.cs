using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class Map : MonoBehaviour
{
    public GameObject displayArea;
    public GameObject planetIcon;
    public GameObject trackingIcon;

    public float scaleFactor = 1;
    public float playerScaleFactor = 1;


    public GameObject player;
    public GameObject playerIcon;

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

    public void open()
    {
        if (texture == null)
        {
            Initialize();
        }
        displayArea.GetComponent<RectTransform>().anchoredPosition = -1 * player.transform.position / scaleFactor * playerScaleFactor;
        playerIcon.GetComponent<RectTransform>().anchoredPosition = player.transform.position / scaleFactor * playerScaleFactor;
        playerIcon.GetComponent<RectTransform>().rotation = player.transform.rotation;
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
                case -1: // deletion
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

    public void Initialize()
    {
        texture = new Texture2D(4000, 4000, TextureFormat.ARGB32, false);
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
