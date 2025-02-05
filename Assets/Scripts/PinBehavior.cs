using UnityEngine;

public class PinBehavior : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector2 newPosition;
    public Vector3 mousePosG;
    Camera cam;
    public Rigidbody2D body; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body = GetComponent<Rigidbody2D>();
        Vector2 currentPosition = body.position; 
        mousePosG = cam.ScreenToWorldPoint(Input.mousePosition);
        newPosition = Vector2.MoveTowards(body.position, mousePosG, speed * Time.fixedDeltaTime);
        body.MovePosition(newPosition); 

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collided = collision.gameObject.tag;
        Debug.Log("Collided with " + collided);
        if(collided == "Ball" || collided == "Wall")
        {
            Debug.Log("Game Over");
        }
    }
}
