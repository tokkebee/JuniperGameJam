using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button exitButton;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        ButtonRebinder();
    }
    void ButtonRebinder() //To fix issue when UIManager leaving scene, buttons lose their reference
    { 
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(uiManager.StartGame);
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(uiManager.QuitToDesktop);
    }
}
