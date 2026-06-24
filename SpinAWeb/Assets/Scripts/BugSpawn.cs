using UnityEngine;

public class BugSpawn
{
    public GameObject spawner; 

    private float spawnMin; 
    private float spawnMax; 

    public GameObject smallBug, medBug, largeBug; 

    void Start()
    {
        float spawnMax = spawner.GetComponent<SpriteRenderer>().bounds.size.x; 
    }
}
