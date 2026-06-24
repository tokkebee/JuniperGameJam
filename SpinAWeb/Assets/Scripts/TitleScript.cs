using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    //Point of this script is to add juice to titles gameObjects

    [SerializeField] bool isLevelTitle;
    [SerializeField] bool Rotate;
    [Header("Rotate Settings\nParameters: GameObject, RotatePower, MaxAngle")] //For hovering over button UI
    [SerializeField] float rotatePower = 1f;
    [SerializeField] float maxAngle = 3.5f;

    private void Start()
    {
        if (isLevelTitle)
        {
            levelTitleAssigner();
        }
    }
    private void Update()
    {
        if (Rotate)
        {
            RotateObject();
        }
    }

    void levelTitleAssigner() //Auto renames level Number to respective output
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("LevelTitleAssigner: Couldn't find TextMeshProUGUI component.");
            return;
        }
        text.text = $"{SceneManager.GetActiveScene().name}";
    }

    public void RotateObject()
    {
        float z = Mathf.Sin(Time.time * rotatePower) * maxAngle;
        gameObject.transform.eulerAngles = new Vector3(0, 0, z);
    }
}
