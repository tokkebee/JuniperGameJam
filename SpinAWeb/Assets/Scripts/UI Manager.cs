using System.Collections;
using UnityEditorInternal;
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
    //public static UIManager instance { get; private set; }

    [Header("PauseUI")]
    [SerializeField] private GameObject pausePanel;

    [Header("TransitionUI")]
    public Animator transiton; //TRANS?!?!!
    private float transitionTime = .333f; //How long it takes for transition to cover screen

    // private void Awake()
    // {
    //     if (instance != null && instance != this)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }

    //     instance = this;

    //     DontDestroyOnLoad(gameObject);

    //     if (pausePanel != null) {
    //         pausePanel.SetActive(false);
    //     }
    // }

    private void Awake() {
        if (pausePanel != null) {
            pausePanel.SetActive(false);
        }
    }

    // void OnGUI()
    // {
    //     Event pressedKey = Event.current;
    //     if (pressedKey.isKey)
    //     {
    //         if (Input.GetKeyDown(KeyCode.Escape)) {
    //             TogglePanel();
    //         }
    //         //Debug.Log("Detected key code: " + pressedKey.keyCode);
    //     }
    // }

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

        SetPause(false); //Not used closePanel() because of audio cue conflict

        StartCoroutine(LoadLevel("Level Select"));
        GameManager.instance.switchState(GameManager.GameState.levelSelect);
    }

    public void QuitToTitle()
    {
        SoundManager.PlaySound(SoundType.UINegative);

        if (pausePanel != null) pausePanel.SetActive(false); //Not used closePanel() because of audio cue conflict

        StartCoroutine(LoadLevel("Main Screen"));
        GameManager.instance.switchState(GameManager.GameState.mainMenu);
        //SceneManager.LoadScene("Main Screen");
    }

    public void QuitToDesktop()
    {
        Debug.Log("Successfully Quitted to Desktop!");
        SoundManager.PlaySound(SoundType.UINegative);
        Application.Quit();
    }

    // public void TogglePanel()
    // {
    //     bool isPaused = pausePanel.activeSelf;

    //     if (!isPaused) {
    //         if (pausePanel.activeInHierarchy == false) {
    //             pausePanel.SetActive(true);
    //             GameManager.instance.switchState(GameManager.GameState.pause);
    //             Time.timeScale = 0f;
    //             SoundManager.PlaySound(SoundType.UIPositive);
    //         }
    //         else {
    //             ClosePanel();
    //         }
    //     }
    // }

    // public void ClosePanel()
    // {
    //     if (pausePanel != null)
    //     {
    //         pausePanel.SetActive(false);
    //         GameManager.instance.Unpause(GameManager.instance.previousState);
    //         Time.timeScale = 1f;
    //         SoundManager.PlaySound(SoundType.UINegative);
    //     }
    // }

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
        transiton.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(levelName);
        transiton.SetTrigger("Exit");

        yield return new WaitForSecondsRealtime(transitionTime);
    }
}