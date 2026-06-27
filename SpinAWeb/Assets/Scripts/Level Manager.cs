using UnityEngine;
using UnityEngine.UI;
using TMPro;

// this script handles the game's operations per level.
public class LevelManager : MonoBehaviour {
    [SerializeField] public LevelState currentLevelState;

    [Header("Win Conditions")]
    [SerializeField] public int bugsQuota = 5;
    [SerializeField] public int bugsCaught;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameLoseScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TextMeshProUGUI bugQuotaUI;

    [Header("Dependants")]
    [SerializeField] private WebManager webManager;

    public enum LevelState {
        Play,
        Win,
        Lose
    }
    void Start() {
        UpdateBugQuota();
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
            Time.timeScale = 1f;
            gameLoseScreen.SetActive(false);
            gameWinScreen.SetActive(false);
        }
    }

    public void GameLose() {
        if (currentLevelState == LevelState.Lose) {
            Time.timeScale = 0f;
            gameLoseScreen.SetActive(true);
        }
    }

    private void GameWin() {
        if (currentLevelState == LevelState.Win) {
            Time.timeScale = 0f;
            gameWinScreen.SetActive(true);
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
