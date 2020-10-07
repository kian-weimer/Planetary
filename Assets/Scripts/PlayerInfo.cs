//
//IMPORTANT:
// only basic data types are allowed in this class
// this class will be saved in a later version and cannot have 
// non basic data types
//
[System.Serializable]
public class PlayerInfo
{
    public float[] position; // two element array of x and y coords
    public int maxHealth; // player's max health
    public int health; // player's health

    public float maxGas;
    public float gas;

    public PlayerInfo(float xpos, float ypos, int maxHealth, int health, float maxGas, float gas)
    {
        position = new float[2];
        position[0] = xpos;
        position[1] = ypos;
        this.maxHealth = maxHealth;
        this.health = health;
        this.maxGas = maxGas;
        this.gas = gas;
    }
}
