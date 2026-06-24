using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] private int numScenesBeforeLevelSelect;
    [SerializeField] private GameObject LevelsGameObject;
    [SerializeField] private TMP_FontAsset levelFont;

    [SerializeField] private Color unlockedLevelColor = Color.white;
    [SerializeField] private Color lockedLevelColor = Color.grey;

    private void Start()
    {
        IdentifyButtons();
    }

    public void OpenScene(int levelNumber)
    {
        if (levelNumber <= GameManager.instance.highestLevelUnlocked)
        {
            SoundManager.PlaySound(SoundType.UIPositive);
            StartCoroutine(UIManager.instance.LoadLevel($"Level {levelNumber}"));
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
            TextMeshProUGUI buttonText = currentLevel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            buttonText.text = (i+1).ToString();
            buttonText.font = levelFont;

            if (i < GameManager.instance.highestLevelUnlocked)
            {
                image.color = unlockedLevelColor;
            }
            else
            { 
                image.color = lockedLevelColor;
            }
        }
    }

}
