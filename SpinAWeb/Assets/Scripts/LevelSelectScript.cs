using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectScript : MonoBehaviour {
    [SerializeField] private List<string> levelScenes;
    [SerializeField] private GameObject LevelsGameObject;
    [SerializeField] private TMP_FontAsset levelFont;

    [SerializeField] private float lockedLevelAlpha = .2f;
    private int highestUnlocked;

    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        //highestUnlocked = PlayerPrefs.GetInt("HighestLevelUnlocked", 1); //For some reason GetInt() wasnt getting the correct number so it defaulted to 1 (seen below)
        highestUnlocked = GameManager.instance.highestLevelUnlocked;
        IdentifyButtons();
    }

    public void OpenScene(int levelNumber)
    {
        int index = levelNumber - 1;

        if (index < 0 || index >= levelScenes.Count) return;

        if (levelNumber <= highestUnlocked)
        {
            SoundManager.PlaySound(SoundType.UIPositive);
            Debug.Log($"uiManager = {uiManager}");
            Debug.Log($"Level index = {index}, scene = {levelScenes[index]}");
            StartCoroutine(uiManager.LoadLevel($"{levelScenes[index]}"));

            GameManager.instance.switchState(GameManager.GameState.game); //Enter game Gamestate
            //SceneManager.LoadScene(levelNumber + numScenesBeforeLevelSelect);
        }
        else
        {
            SoundManager.PlaySound(SoundType.UINegative);
        }
    }

    void IdentifyButtons() //Automatically asigns text number & font to respective level # as well as turns color dark for locked levels
    {
        for (int i = 0; i < LevelsGameObject.transform.childCount; i++)
        {
            GameObject currentLevel = LevelsGameObject.transform.GetChild(i).gameObject;
            
            Image image = currentLevel.GetComponent<Image>();
            TextMeshProUGUI buttonText = currentLevel.transform.GetChild(0).gameObject.
            GetComponent<TextMeshProUGUI>();
            
            buttonText.text = (i+1).ToString();
            buttonText.font = levelFont;

            Color tempColor = image.color;

            tempColor.a = (i < GameManager.instance.highestLevelUnlocked) ? 1f : lockedLevelAlpha;
            image.color = tempColor;
        }
    }

}
