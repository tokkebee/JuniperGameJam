using UnityEngine;
using UnityEngine.SceneManagement;

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
        SoundManager.PlaySound(SoundType.UIPositive);
        SceneManager.LoadScene("Game");
    }

    public void QuitToTitle()
    {
        SoundManager.PlaySound(SoundType.UINegative);
        SceneManager.LoadScene("Main Screen");
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
}