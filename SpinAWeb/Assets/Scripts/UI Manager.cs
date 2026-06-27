using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

// This script handles any and all things that need to be displayed
// on the GUI, often referencing the Game Manager.

public class UIManager : MonoBehaviour
{
    [Header("PauseUI")]
    [SerializeField] private GameObject pausePanel;

    [Header("TransitionUI")]
    public Animator transiton; //TRANS?!?!!
    private float transitionTime = .5f; //How long it takes for transition to cover screen

    private void Awake() {
        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //SetPause(!pausePanel.activeSelf);
            TogglePause();
        }
    }

    public void StartGame()
    {
        SoundManager.PlaySound(SoundType.UIPositive);

        if (pausePanel != null) pausePanel.SetActive(false); //Not used closePanel() because of audio cue conflict

        StartCoroutine(LoadLevel("Level Select"));
        GameManager.instance.switchState(GameManager.GameState.levelSelect);
        //SceneManager.LoadScene("Game");
    }

    public void LevelSelect() {
        SoundManager.PlaySound(SoundType.UIPositive);
        GameManager.instance.switchState(GameManager.GameState.levelSelect);
        StartCoroutine(LoadLevel("Level Select"));
        
    }

    public void QuitToTitle()
    {
        SoundManager.PlaySound(SoundType.UIPositive);
        GameManager.instance.switchState(GameManager.GameState.mainMenu);
        StartCoroutine(LoadLevel("Main Screen"));
    }

    public void QuitToDesktop()
    {
        Debug.Log("Successfully Quitted to Desktop!");
        SoundManager.PlaySound(SoundType.UINegative);
        Application.Quit();
    }

    public void Credits()
    {
        SoundManager.PlaySound(SoundType.UIPositive);

        StartCoroutine(LoadLevel("Credits"));
    }

    public void nextLevel()
    {
        SoundManager.PlaySound(SoundType.UIPositive);

        StartCoroutine(LoadLevel($"{SceneManager.GetActiveScene().buildIndex - 1}_Level"));
    }

    public void replayLevel()
    {
        SoundManager.PlaySound(SoundType.UIPositive);
        GameManager.instance.switchState(GameManager.GameState.game);

        StartCoroutine(LoadLevel($"{SceneManager.GetActiveScene().buildIndex - 2}_Level"));
    }

    public void SetPause(bool paused) {
        pausePanel.SetActive(paused);

        Time.timeScale = paused ? 0f : 1f;

        GameManager.instance.switchState(
            paused ? GameManager.GameState.pause: GameManager.instance.previousState
        );

        if (paused) {
            SoundManager.PlaySound(SoundType.UIPositive);
        }
        else {
            SoundManager.PlaySound(SoundType.UINegative);
        }
    }

    public void TogglePause() {
        SetPause(!pausePanel.activeSelf);
    }

    public void UnPause() {
        SetPause(false);
    }

    public IEnumerator LoadLevel(string levelName) //Transition (Dont switch game states here as it is used for all load scenes.
    {
        transiton.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(levelName);
    }
}