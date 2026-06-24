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
    public Animator transiton;
    private bool isTransitioning = false; //AMEN!!!!

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

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
        //SceneManager.LoadScene("Game");
    }

    public void QuitToTitle()
    {
        SoundManager.PlaySound(SoundType.UINegative);
        pausePanel.SetActive(false); //Not used closePanel() because of audio cue conflict
        StartCoroutine(LoadLevel("Main Screen"));
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
        }
    }

    public IEnumerator LoadLevel(string levelName) //Transition
    {
        isTransitioning = true;

        transiton.SetTrigger("Start");
        yield return new WaitForSeconds(.333f); //How long it takes for transition to cover screen

        SceneManager.LoadScene(levelName);
        isTransitioning = false;
        transiton.SetTrigger("Exit");

        yield return new WaitForSeconds(.333f);
        //transiton.Play("IdleState", 0, 0f);
        
    }
}