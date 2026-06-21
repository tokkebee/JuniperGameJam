using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles any and all things that need to be displayed
// on the GUI, often referencing the Game Manager.

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private GameObject pausePanel;

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
        SceneManager.LoadScene("Game");
    }

    public void QuitToTitle()
    {
        SceneManager.LoadScene("Main Screen");
    }

    public void QuitToDesktop()
    {
        Debug.Log("Successfully Quitted to Desktop!");
        Application.Quit();
    }

    public void TogglePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
        }
    }

    public void ClosePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
}