using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles all player movement

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] public Camera camera;
    [SerializeField] private Vector2 screenBounds;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private Vector3 moveDirection;

    [Header("Dependants")]
    [SerializeField] private WebManager webManager;

    [Header("State")]
    [SerializeField] private SpiderState state;

    [SerializeField] private Transform supportCheck;
    [SerializeField] private float supportRadius = 0.5f;

    [SerializeField] private LayerMask branches;
     [SerializeField] private LayerMask silk;

    public enum SpiderState {
        Supported, //on a valid surface (branch or silk)
        Spinning, //making silk
        Falling,
        Dead,
    }

    void Start() {
        //warnings
        if (webManager == null) {
            Debug.Log("Web Manager missing from Player Controller");
        }

        camera = Camera.main;
        //screenBounds = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenBounds.y = camera.orthographicSize;
        screenBounds.x = screenBounds.y * camera.aspect;

        Debug.Log(screenBounds);
    }

    void Update() {
        if (GameManager.instance != null &&
            GameManager.instance.getCurrentGameState() == GameManager.GameState.pause)
        {
            return;
        }
        switch (state)
        {
            case SpiderState.Supported:
                HandleSupported();
                break;
            
            case SpiderState.Spinning:
                HandleSpinning();
                break;

            case SpiderState.Falling:
                HandleFalling();
                break;

            case SpiderState.Dead:
                break;
        }
    }

    // void UpdateState() {
    //     if (IsSupported())
    //     {
    //         if (state != SpiderState.Supported)
    //         {
    //             Debug.Log("Supported");
    //         }

    //         state = SpiderState.Supported;
    //     }
    //     else
    //     {
    //         if (state != SpiderState.Falling)
    //         {
    //             Debug.Log("Falling");
    //         }

    //         state = SpiderState.Falling;
    //     }
    // }

    void HandleSupported() {
        MoveWASD();
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (webManager.StartSilk()) {
                state = SpiderState.Spinning;
            }
        }

        if (!IsSupported()) {
                state = SpiderState.Falling;
                return;
            }
        // if (Input.GetKey(KeyCode.Space) && webManager.silkActive) {
        //     MoveSpace();
        //     SpinWeb();
        // }
        // if (Input.GetKeyUp(KeyCode.Space) && webManager.silkActive) {
        //     webManager.EndSilk();
        // }
    }

    void HandleSpinning() {
        Vector3 oldPos = transform.position;
        MoveSpace();
        Vector3 newPos = transform.position;

        webManager.UpdateSilk();
        //webManager.UpdateSilk();

        if (Input.GetKeyUp(KeyCode.Space) || webManager.GetSilkRemaining() <= 0f) {
            webManager.EndSilk();

            state = IsSupported() ? SpiderState.Supported : SpiderState.Falling;
        }
    }

    void HandleFalling() {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (IsSupported()) {
            state = SpiderState.Supported;
            return;
        }

        float depth = transform.position.z - camera.transform.position.z;

        Vector3 bottom = camera.ScreenToWorldPoint(new Vector3(0, 0, depth));

        if (transform.position.y < bottom.y) {
            state = SpiderState.Dead;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void MoveSpace() {
        // Vector3 newPos = transform.position + transform.up * speed * Time.deltaTime;

        // ClampToScreen(ref newPos);

        // transform.position = newPos;

        float moveDistance = speed * Time.deltaTime;
        //float maxMove = speed * Time.deltaTime;
        //float moveDistance = Mathf.Min(maxMove, webManager.GetSilkRemaining());

        Vector3 oldPos = transform.position;
        Vector3 newPos = oldPos + transform.up * moveDistance;

        ClampToScreen(ref newPos);
        transform.position = newPos;

        webManager.ConsumeSilk(Vector3.Distance(oldPos, newPos));

        // if (IsSupported()) {
        //     state = SpiderState.Supported;
        // }
        // else {
        //     state = SpiderState.Falling;
        // }
    }

    void MoveWASD() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, moveY, 0).normalized;

        Vector3 newPos = transform.position + moveDirection * speed * Time.deltaTime;

        ClampToScreen(ref newPos);

        transform.position = newPos;
    }

    void ClampToScreen(ref Vector3 position) { 
        //ref is a copy of the original that updates the og if changed
        float depth = transform.position.z - camera.transform.position.z;
        
        Vector3 min = camera.ScreenToWorldPoint(
            new Vector3(0, 0, depth)
        );

        Vector3 max = camera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, depth)
        );

        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);
    }

    //Deposits web
    void SpinWeb() {
        webManager.UpdateSilk();
    }

    //state stuff
    bool IsSupported() {
        if (supportCheck == null) {
            Debug.LogWarning("Support Check not assigned!");
            return false;
        }
        LayerMask supportLayers = branches | silk;

        Collider2D hit = Physics2D.OverlapCircle(
            supportCheck.position,
            supportRadius,
            supportLayers
        );

        return hit != null;
    }
}
