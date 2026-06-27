using UnityEngine;
using System.Collections; 

public class Bug : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector2 moveDir; 
    // the bug scriptable object, refernce this to acces points values. 
    public BugSO bugSO;
    public BugSpawn bs; 
    LayerMask webbing;
    public WebManager wm;
    public float activationTime;
    public GameObject bugRig;

    public SpriteRenderer sr;
    public Color webcolor;

    public enum bugState
    {
        free, 
        caught
    }
    public bugState currentBugState; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        webcolor = Color.white;
        moveDir = Vector2.down;
        currentBugState = bugState.free;

        webbing = LayerMask.GetMask("Silk");
        wm = GameObject.Find("Web Manager").GetComponent<WebManager>();
        bs = GameObject.Find("BugSpawner").GetComponent<BugSpawn>();
    }

    void Start()
    {
        activationTime = Time.time; 
        StartCoroutine(Lifespan());
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
            sr.transform.localScale = new Vector2(0.1f, 0.1f); 
            sr.sprite = bugSO.caughtImage; 
            sr.color = webcolor;
            webcolor.a = 1.0f; 
            bugRig.SetActive(false);
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
