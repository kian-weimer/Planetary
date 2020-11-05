using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public GameObject player;
    public List<Sprite> playerSprites;
    public ShopManager shopManager;
    public GameObject boostBar;
    private int whichShipLevel = 0;
    public Money money;
    public GameObject doubleShot;
    public GameObject bigShot;

    public void levelUp(string levelUpName)
    {
        switch (levelUpName)
        {
            case "Warp":
                player.GetComponent<PlayerController>().hasWarpSpeed = true;
                boostBar.SetActive(true);
                break;

            case "Money":
                money.addMoney(1000);
                break;

            case "FastCooldown":
                player.GetComponent<Player>().fasterCooldown = true;
                player.GetComponent<PlayerController>().fasterCooldown = true;
                //add sheildBar faster
                break;

            case "PlanetDamage":
                foreach(GameObject weapon in player.GetComponent<Player>().weapons)
                {
                    weapon.GetComponent<Weapon>().bullet.GetComponent<Bullet>().extraDamageToPlanets = true;
                }
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
                GameObject doubleToadd = Instantiate(doubleShot);
                doubleToadd.GetComponent<Weapon>().bullet.GetComponent<Bullet>().extraDamageToPlanets = true;
                player.GetComponent<Player>().AddWeapon(doubleToadd);
                break;

            case "BigGun":
                GameObject bigShotToAdd = Instantiate(bigShot);
                bigShotToAdd.GetComponent<Weapon>().bullet.GetComponent<Bullet>().extraDamageToPlanets = true;
                player.GetComponent<Player>().AddWeapon(bigShotToAdd);
                break;
            case "DoubleDrop":
                Player.doubleResource = true;
                break;
            case "CheaperShop":
                ShopManager.isCheaper = true;
                break;
        }
    }
}
