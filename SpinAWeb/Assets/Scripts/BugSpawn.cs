using UnityEngine;
using System.Collections;

public class BugSpawn : MonoBehaviour
{
    public Camera cam;
    public Vector3 camSpace;

    private float spawnY = 1.0f;

    public GameObject smallBug, medBug, largeBug; 
    public bool active = false;

    void Start()
    {
        active = true;
        StartCoroutine(BugDrop()); 
    }

    void SpawnBug()
    {
        float spawnX = Random.Range(0.10f, 0.90f);

        Vector3 spawnPos = new Vector3(spawnX, spawnY + 1.0f, cam.nearClipPlane);
        spawnPos = cam.ViewportToWorldPoint(spawnPos);

        int diceroll = Random.Range(0, 3);

        if (diceroll == 1)
        {
            Instantiate(smallBug, spawnPos, transform.rotation);
        }
        else if (diceroll == 2) 
        {
            Instantiate(medBug, spawnPos, transform.rotation);
        }
        else
        {
            Instantiate(largeBug, spawnPos, transform.rotation);
        }
    }

    IEnumerator BugDrop()
    {
        while (active)
        {
            SpawnBug();
            yield return new WaitForSeconds(3);
        }
    }
}
