using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 0;
    }

    public void tutorialUnpause()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
