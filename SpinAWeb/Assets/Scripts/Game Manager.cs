using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// This script handles game and level code! Aka, keeping score, handling win/lose conditions, game states, etc.

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] GameState currentGameState = GameState.mainMenu;

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
        switchState(GameState.mainMenu);
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
        currentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.mainMenu:
                StartCoroutine(SoundManager.MusicTransition(SoundType.MenuMusic));
                break;
            case GameState.levelSelect:
                StartCoroutine(SoundManager.MusicTransition(SoundType.MenuMusic));
                break;
            case GameState.game:
                StartCoroutine(SoundManager.MusicTransition(SoundType.GameMusic));
                break;
            case GameState.pause:
                break;
        }
    }

    public GameState getCurrentGameState()
    { 
        return currentGameState;
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
        highestLevelUnlocked = data.level;
    }

    public void WipeData()
    {
        highestLevelUnlocked = 1;
        SaveScript.Save(this);
    }
    #endregion
}
