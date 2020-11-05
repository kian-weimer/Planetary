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
    public int rarity; // the rarity of the planet // used as planet identifier in home system
    public float maxHealth;
    public float health; // planet's health
    public int type; // the type of planet (used to create image)
    public bool discovered; // true is the planet has been seen
    public bool inHomeSystem;

    public PlanetInfo(float xpos, float ypos , int rarity, float maxHealth, int type, bool inHomeSystem)
    {
        position = new float[2];
        position[0] = xpos;
        position[1] = ypos;
        this.rarity = rarity;
        this.maxHealth = maxHealth;
        health = maxHealth; // may need to be changed when loading in a planet with less than max health
        this.type = type;
        discovered = false;
        this.inHomeSystem = inHomeSystem;
    }
}
