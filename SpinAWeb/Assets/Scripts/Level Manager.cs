using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// this script handles the game's operations per level.
public class LevelManager : MonoBehaviour {
    [SerializeField] public LevelState currentLevelState;

    [Header("Win Conditions")]
    [SerializeField] public int bugsQuota = 5;
    [SerializeField] public int bugsCaught = 0;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameLoseScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TextMeshProUGUI bugQuotaUI;

    [Header("Dependants")]
    [SerializeField] private WebManager webManager;

    private bool WinConHasEnded = false;
    private bool LoseConHasEnded = false;

    public enum LevelState {
        Play,
        Win,
        Lose
    }
    void Start() {
        UpdateBugQuota();
        Time.timeScale = 1f; //bug fix, when going into next round, time would be paused
    }

    void Update() {
        if (bugsCaught >= bugsQuota) {
            currentLevelState = LevelState.Win;
        }
        switch (currentLevelState) {
            case LevelState.Play:
                GamePlay();
                break;
            
            case LevelState.Win:
                GameWin();
                break;

            case LevelState.Lose:
                GameLose();
                break;
        }
    }

    //state stuff
    public LevelState getLevelState() {
        return currentLevelState;
    }

    private void GamePlay() {
        if (currentLevelState == LevelState.Play) {
            //Time.timeScale = 1f; //Commented out. Made it so that pausing wouldnt work while in play state
            gameLoseScreen.SetActive(false);
            gameWinScreen.SetActive(false);
        }
    }

    public void GameLose() {
        /*
        if (currentLevelState == LevelState.Lose) {
            gameLoseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        */
        //currentLevelState = LevelState.Lose; //PlayerController is calling gamelose but never switches state so the if statement never runs
        if (LoseConHasEnded == false)
        {
            gameLoseScreen.SetActive(true);
            currentLevelState = LevelState.Lose;
            //Time.timeScale = 0f;
            LoseConHasEnded = true;
        }
    }

    private void GameWin() {
        if (currentLevelState == LevelState.Win) {
            Time.timeScale = 0f;
            gameWinScreen.SetActive(true);
        }
        if (WinConHasEnded == false)
        {
            if (GameManager.instance.highestLevelUnlocked == SceneManager.GetActiveScene().buildIndex - 2) //Hardcoded -2 because time constraint
            {
                GameManager.instance.highestLevelUnlocked++;
            }
            GameManager.instance.SaveGame();
            WinConHasEnded = true;
        }
    }

    //score stuff
    public void AddScore(int points) {
        bugsCaught += points;
        UpdateBugQuota();
    }

    void UpdateBugQuota() {
        bugQuotaUI.text = $"{bugsCaught} / {bugsQuota}";
    }
}
