using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles all player movement

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] public Camera mainCamera;
    [SerializeField] private Vector2 screenBounds;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;

    [Header("Dependants")]
    [SerializeField] private WebManager webManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;

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
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (webManager == null) {
            Debug.Log("Web Manager missing from Player Controller");
        }
        if (levelManager == null) {
            Debug.Log("Level Manager missing from Player Controller");
        }

        mainCamera = Camera.main;
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
                HandleDead();
                break;
        }
    }

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
    }

    void HandleSpinning() {
        MoveSpace();

        webManager.UpdateSilk();

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

        float depth = transform.position.z - mainCamera.transform.position.z;

        Vector3 bottom = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, depth));

        if (transform.position.y < bottom.y) {
            state = SpiderState.Dead;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void HandleDead() {
        levelManager.GameLose();
        //GameManager.instance.switchState(GameManager.GameState.pause);
    }

    void MoveSpace() {
        float moveDistance = speed * Time.deltaTime;

        Vector3 newPos = transform.position + transform.up * moveDistance;

        ClampToScreen(ref newPos);
        transform.position = newPos;
    }

    void MoveWASD() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

        Vector3 newPos = transform.position + moveDirection * speed * Time.deltaTime;

        ClampToScreen(ref newPos);

        transform.position = newPos;
    }

    void ClampToScreen(ref Vector3 position) { 
        //ref is a copy of the original that updates the og if changed
        float depth = transform.position.z - mainCamera.transform.position.z;
        
        Vector3 min = mainCamera.ScreenToWorldPoint(
            new Vector3(0, 0, depth)
        );

        Vector3 max = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, depth)
        );

        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);
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
