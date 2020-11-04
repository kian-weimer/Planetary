using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public GameObject player;
    public List<Sprite> playerSprites;
    public ShopManager shopManager;
    private int whichShipLevel = 0;

    public void levelUp(string levelUpName)
    {
        switch (levelUpName)
        {
            case "Warp":
                break;
            case "Money":
                break;
            case "FastCooldown":
                break;
            case "PlanetDamage":
                break;
            case "Sheild":
                break;
            case "UpgradeShip":
                player.GetComponent<Player>().maxHealth += 100;
                player.GetComponent<Player>().maxGas += 100;
                player.GetComponent<PlayerController>().maxSpeed += 2;
                player.GetComponent<PlayerController>().thrust += .3f;
                player.GetComponent<Player>().weapon.GetComponent<Weapon>().firerate += 2;
                player.GetComponent<Player>().weapon.GetComponent<Weapon>().bulletSpeed += 5;
                player.GetComponent<Player>().weapon.GetComponent<Weapon>().bullet.GetComponent<Bullet>().bulletDamage += 3;
                player.GetComponent<SpriteRenderer>().sprite = playerSprites[shopManager.whichShipColorBase + whichShipLevel + 1];

                shopManager.whichShipLevel = shopManager.whichShipLevel + 1;
                whichShipLevel++;
                if (whichShipLevel == 1)
                {
                    player.GetComponent<BoxCollider2D>().size = new Vector2(.7f, 1f);
                }

                if (whichShipLevel == 2)
                {
                    player.GetComponent<BoxCollider2D>().size = new Vector2(.7f, 1.25f);
                }
                
                break;
            case "DoubleShot":
                break;
            case "BigGun":
                break;
            case "DoubleDrop":
                break;
            case "CheaperShop":
                break;
        }
    }
}
