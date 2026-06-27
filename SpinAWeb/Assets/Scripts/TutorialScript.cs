using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f;
    }

    public void tutorialUnpause()
    { 
        Time.timeScale = 0;
        gameObject.SetActive(false);
    }
}
