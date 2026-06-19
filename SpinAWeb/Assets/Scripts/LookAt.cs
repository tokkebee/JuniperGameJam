using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                -camera.transform.position.z
            )
        );
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;

        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        Debug.DrawLine(transform.position, mousePos, Color.red);

        // Debug.Log(Input.mousePosition);
    }
}
