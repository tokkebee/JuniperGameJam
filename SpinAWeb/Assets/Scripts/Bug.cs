using UnityEngine;
using System.Collections; 

public class Bug : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector2 moveDir; 
    public BugSO bugSO;
    public BugSpawn bs; 
    LayerMask webbing;
    public WebManager wm;
    public float activationTime; 

    public enum bugState
    {
        free, 
        caught
    }
    public bugState currentBugState; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveDir = Vector2.down;
        currentBugState = bugState.free;
        webbing = LayerMask.GetMask("Silk");
        wm = GameObject.Find("Web Manager").GetComponent<WebManager>();
    }

    void Awake()
    {
        StartCoroutine(Lifespan());
        bs = GameObject.Find("BugManager").GetComponent<BugSpawn>();
        activationTime = Time.time; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        if(currentBugState == bugState.free && Time.time - activationTime >= 5f)
        {
            Destroy(this.gameObject);
            bs.bugCount--; 
        }
    }

    void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.gameObject.layer == 7 && other.gameObject != wm.currentSilk)
        {
            Invoke("WebCatch", 0.1f); 
        }
        if (other.gameObject.tag == "Boundary")
        {
            moveDir = moveDir * -1;
        }
    }

    void WebCatch()
    {
        currentBugState = bugState.caught;
        moveSpeed = 0.0f;
        wm.caughtBugs++; 
    }

    void ChooseDirection()
    {
        int diceroll = Random.Range(0, 4);

        moveDir = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
    }

    IEnumerator Lifespan()
    {
        while (this.gameObject != null)
        {
            ChooseDirection();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
