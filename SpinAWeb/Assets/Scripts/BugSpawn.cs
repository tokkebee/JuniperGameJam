using UnityEngine;
using System.Collections;

public class BugSpawn : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Vector3 camSpace;

    public GameObject smallBug, medBug, largeBug; 
    public bool active = false;
    public int bugCount = 0;
    [SerializeField] private int bugLimit = 20;
    [SerializeField] private WebManager webManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;

    void Start()
    {
        active = true;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>(); 
        StartCoroutine(BugDrop());
    }

    void SpawnBug()
    {
        float spawnX = Random.Range(0.10f, 0.90f);
        float spawnY = Random.Range(1.0f, 0.9f);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, cam.nearClipPlane);
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
        // while (active)
        // {
        //     if (bugCount < bugLimit)
        //     {
        //         SpawnBug();
        //         bugCount++; 
        //         yield return new WaitForSeconds(3);
        //     }
        //     else
        //     {
        //         playerController.HandleDead();
        //         yield return null; 
        //     }
        // }

        while (bugCount < bugLimit) {
            SpawnBug();
            bugCount++;
            yield return new WaitForSeconds(3);
        }

        while (GameObject.FindGameObjectsWithTag("Bug").Length > 0) {
            yield return null;
        }


        if (webManager.caughtBugs < levelManager.bugsQuota) {
            playerController.HandleDead();
        }
    }
}
