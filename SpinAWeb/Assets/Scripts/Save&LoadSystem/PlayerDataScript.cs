using UnityEngine;

[System.Serializable] //Makes script able to be saved in file
public class PlayerDataScript
{
    public int level = 1;

    public PlayerDataScript (GameManager gameManager) //Fetches levelsComplete from GameManager script and uses int for saveData
    {
        level = gameManager.highestLevelUnlocked;
    }
}
