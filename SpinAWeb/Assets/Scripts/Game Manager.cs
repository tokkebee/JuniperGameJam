using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles game and level code! Aka, keeping score, handling win/lose conditions, game states, etc.

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Save/Load Settings")]
    public int highestLevelUnlocked = 1; //Used for saving/tracking what levels are completed for PlayerDataScript
    [SerializeField] private int numScenesBeforeLevelSelect; //Used to calculate highestLevel build index

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        
        LoadGame();
    }

    public void levelComplete()
    {
        int currentLevelNum = SceneManager.GetActiveScene().buildIndex - numScenesBeforeLevelSelect;
        if (highestLevelUnlocked < currentLevelNum)
        { 
            highestLevelUnlocked = currentLevelNum;
        }
        SaveGame();
    }
    public void SaveGame()
    {
        SaveScript.Save(this);
    }

    public void LoadGame()
    {
        //PlayerDataScript data = SaveScript.Load(this);
        //highestLevelUnlocked = data.level;
    }

    public void WipeData()
    {
        highestLevelUnlocked = 1;
        SaveScript.Save(this);
    }
}
