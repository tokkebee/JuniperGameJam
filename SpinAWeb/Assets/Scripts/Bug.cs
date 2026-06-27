using UnityEngine;

public class Bug : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public BugSO bugSO;
    LayerMask webbing;
    public WebManager wm; 

    public enum bugState
    {
        free, 
        caught
    }
    public bugState currentBugState; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBugState = bugState.free;
        webbing = LayerMask.GetMask("Silk");
        wm = GameObject.Find("Web Manager").GetComponent<WebManager>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.gameObject.layer == 7 && other.gameObject != wm.currentSilk)
        {
            Invoke("WebCatch", 0.1f); 
        }
    }

    void WebCatch()
    {
        currentBugState = bugState.caught;
        moveSpeed = 0.0f;
        wm.caughtBugs++; 
    }
}
