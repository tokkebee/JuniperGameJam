using UnityEngine;

public class Bug : MonoBehaviour
{
    public float moveSpeed = 5.0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime, Space.World);

        Raycast detector = Physics.Raycast(transform.position, Vector3.forward)
    }
}
