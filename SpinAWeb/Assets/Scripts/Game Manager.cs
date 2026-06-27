using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// This script handles game, aka level loading and saving/wiping data.

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("State Data")]
    [SerializeField] GameState currentGameState = GameState.mainMenu;
    [HideInInspector]
    public GameState previousState; //Used for handling pausing

    [Header("Save/Load Settings")]
    public int highestLevelUnlocked = 1; //Used for saving/tracking what levels are completed for PlayerDataScript
    [SerializeField] private int numScenesBeforeLevelSelect; //Used to calculate highestLevel build index
    public enum GameState
    {
        mainMenu,
        levelSelect,
        game,
        pause
    }

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            currentGameState = GameState.mainMenu;
            switchState(GameState.mainMenu);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            currentGameState = GameState.levelSelect;
            switchState(GameState.levelSelect);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            currentGameState = GameState.game;
            switchState(GameState.game);
        }

            previousState = currentGameState;
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

    #region GameState //How to use: GameManager.instance.switchState(GameManager.GameState.{State You Want});
    public void switchState(GameState newGameState)
    {
        previousState = currentGameState;
        currentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.mainMenu:
                StartCoroutine(SoundManager.MusicTransition(SoundType.MenuMusic));
                Time.timeScale = 1f;
                break;
            case GameState.levelSelect:
                StartCoroutine(SoundManager.MusicTransition(SoundType.MenuMusic));
                Time.timeScale = 1f;
                break;
            case GameState.game:
                StartCoroutine(SoundManager.MusicTransition(SoundType.GameMusic));
                break;
            case GameState.pause:
                SoundManager.onPauseMusic();
                break;
        }
    }

    public GameState getCurrentGameState()
    { 
        return currentGameState;
    }

    public void Unpause(GameState previousState)
    {
        SoundManager.onUnpauseMusic();
        switchState(previousState);
    }
    #endregion

    #region Save/Load
    public void SaveGame()
    {
        SaveScript.Save(this);
    }

    public void LoadGame()
    {
        PlayerDataScript data = SaveScript.Load(this);
        if (data != null)
        {
            highestLevelUnlocked = data.level;
        }
    }

    public void WipeData()
    {
        highestLevelUnlocked = 1;
        SaveScript.Save(this);
    }
    #endregion
}
