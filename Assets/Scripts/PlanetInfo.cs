//
//IMPORTANT:
// only basic data types are allowed in this class
// this class will be saved in a later version and cannot have 
// non basic data types
//
[System.Serializable]
public class PlanetInfo
{
    public float[] position; // two element array of x and y coords
    public int rarity; // the rarity of the planet
    public int health; // planet's health
    public int type; // the type of planet (used to create image)
    public bool discovered; // true is the planet has been seen

    public PlanetInfo(float xpos, float ypos , int rarity, int health, int type)
    {
        position = new float[2];
        position[0] = xpos;
        position[1] = ypos;
        this.rarity = rarity;
        this.health = health;
        this.type = type;
        discovered = false;
    }
}
