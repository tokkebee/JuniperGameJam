using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Camera camera;
    [SerializeField] private float speed;

    void Start()
    {
        camera = Camera.main;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            Move();
            SpinWeb();
        }
    }

    void Move()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                -camera.transform.position.z
            )
        );
        mousePos.z = transform.position.z;

        Vector3 dir = (mousePos - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;
    }

    //Deposits web
    void SpinWeb() {
        Debug.Log("Depositing web");
    }
}
