using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] private int numScenesBeforeLevelSelect;
    [SerializeField] private GameObject LevelsGameObject;
    [SerializeField] private TMP_FontAsset levelFont;

    private void Start()
    {
        renameButtons();
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

    void renameButtons() //Automatically asigns text number & font to respective level #
    {
        for (int i = 0; i < LevelsGameObject.transform.childCount; i++)
        {
            GameObject currentLevel = LevelsGameObject.transform.GetChild(i).gameObject;
            TextMeshProUGUI buttonText = currentLevel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            buttonText.text = (i+1).ToString();
            buttonText.font = levelFont;
        }
    }

}
