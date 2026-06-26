using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = GetComponent<PlayerController>().camera;
    }

    void Update()
    {
        if (GameManager.instance.getCurrentGameState() == GameManager.GameState.pause)
            return;
            
        Vector3 mousePos = camera.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                -camera.transform.position.z
            )
        );
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;
        
        // Debug.Log(angleDeg);

        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        
        // Debug.DrawLine(transform.position, mousePos, Color.red);
        // Debug.DrawRay(transform.position, transform.up * 2f, Color.green);

        // Debug.Log(Input.mousePosition);
    }
}
