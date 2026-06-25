using UnityEngine;

// This script handles all player movement

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] public Camera camera;
    [SerializeField] private Vector2 screenBounds;

    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Dependants")]
    [SerializeField] private WebManager webManager;

    public enum PlayerState {
        Supported, //on a valid surface (branch or silk)
        Unsupported, //invalid surface aka most likely falling
        Dead,
    }

    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            webManager.StartSilk();
        }
        if (Input.GetKey(KeyCode.Space) && webManager.silkActive) {
            Move();
            SpinWeb();
        }
        if (Input.GetKeyUp(KeyCode.Space) && webManager.silkActive) {
            webManager.EndSilk();
        }
        //Debug.Log(webManager.silkActive);
    }

    void Move()
    {
        // Vector3 mousePos = camera.ScreenToWorldPoint(
        //     new Vector3(
        //         Input.mousePosition.x,
        //         Input.mousePosition.y,
        //         -camera.transform.position.z
        //     )
        // );
        // mousePos.z = transform.position.z;

        // Vector3 dir = (mousePos - transform.position).normalized;

        // transform.position += dir * speed * Time.deltaTime;

        Vector3 newPos = transform.position + transform.up * speed * Time.deltaTime;

        float depth = transform.position.z - camera.transform.position.z;

        Vector3 min = camera.ScreenToWorldPoint(
            new Vector3(0, 0, depth)
        );

        Vector3 max = camera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, depth)
        );

        newPos.x = Mathf.Clamp(newPos.x, min.x, max.x);
        newPos.y = Mathf.Clamp(newPos.y, min.y, max.y);

        transform.position = newPos;
    }

    //Deposits web
    void SpinWeb() {
        webManager.UpdateSilk();
    }
}
