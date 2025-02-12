using System;
using UnityEngine;

public class PinBehavior : MonoBehaviour
{
    //Variables for dash
    public float dashSpeed = 5f;
    public float baseSpeed = 2f;
    public bool dashing;
    public float dashDuration = 0.3f; 
    public float timedashStart;

    //Variables for mouse follow
    public float speed = 2.0f;
    public Vector2 newPosition;
    public Vector3 mousePosG;
    Camera cam;
    public Rigidbody2D body;

    //Variables for cooldown
    public static float cooldownRate = 5;
    public static float cooldown;
    public float timelastDashEnded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); 
        cam = Camera.main;

    }

    // Update is called once per frame
    private void Update()
    {
        Dash(); 
        
    }
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }
    private void Dash()
    {
        if (dashing == true)
        {
            float howLong = Time.time - timedashStart;
            if (howLong > dashDuration)
            {
                dashing = false;
                speed = baseSpeed;
                timelastDashEnded = Time.time;
                cooldown = cooldownRate; 
            }
        }
        else
        {
            //time that has passed since last frame rate = deltatime
            cooldown = cooldown - Time.deltaTime; 
            if(cooldown < 0)
            {
                cooldown = 0; 
            }

            if (Input.GetMouseButtonDown(0) == true && cooldown <= 0)
            {
                dashing = true;
                speed = dashSpeed;
                timedashStart = Time.time;
            }
        }
    }
}
