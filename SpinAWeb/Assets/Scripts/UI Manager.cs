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
    public static UIManager instance { get; private set; }

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
    // }

    void OnGUI()
    {
        Event pressedKey = Event.current;
        if (pressedKey.isKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePanel();
            }
            //Debug.Log("Detected key code: " + pressedKey.keyCode);
        }
    }

    public void StartGame()
    {
        SoundManager.PlaySound(SoundType.UIPositive);
        pausePanel.SetActive(false); //Not used closePanel() because of audio cue conflict
        StartCoroutine(LoadLevel("Level Select"));
        GameManager.instance.switchState(GameManager.GameState.levelSelect);
        //SceneManager.LoadScene("Game");
    }

    public void QuitToTitle()
    {
        SoundManager.PlaySound(SoundType.UINegative);
        pausePanel.SetActive(false); //Not used closePanel() because of audio cue conflict
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

    public void TogglePanel()
    {
        if (pausePanel != null)
        {
            if (pausePanel.activeInHierarchy == false)
            {
                SoundManager.PlaySound(SoundType.UIPositive);
                pausePanel.SetActive(true); //minor adjustments for audio
                GameManager.instance.switchState(GameManager.GameState.pause);
            }
            else ClosePanel();
        }
    }

    public void ClosePanel()
    {
        if (pausePanel != null)
        {
            SoundManager.PlaySound(SoundType.UINegative);
            pausePanel.SetActive(false);
            GameManager.instance.Unpause(GameManager.instance.previousState);
        }
    }

    public IEnumerator LoadLevel(string levelName) //Transition (Dont switch game states here as it is used for all load scenes.
    {
        transiton.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
        transiton.SetTrigger("Exit");

        yield return new WaitForSeconds(transitionTime);
    }
}