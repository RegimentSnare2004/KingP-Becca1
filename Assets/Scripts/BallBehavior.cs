using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minSpeed;
    public float maxSpeed;
    Vector2 targetPosition;

    public GameObject target;
    public float minLaunchSpeed;
    public float maxLaunchSpeed;
    public float minTimeToLaunch;
    public float maxTimeToLaunch;
    public float cooldown;
    public bool launching;
    public float launchDuration;
    public float timeLaunchStart;
    public float timeLastLaunch;

    public int secondsToMaxSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D body;
    public bool rerouting;
    Vector2 getRandomPosition()
    {
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);
        Debug.Log("rx: " + randX + "ry: " + randY);
        Vector2 v = new Vector2(randX, randY);
        return v;
    }

    public float getDifficultyPercentage()
    {
        float difficulty = Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxSpeed);
        return difficulty; 
    }
    public void initialPosition()
    {
        body = GetComponent<Rigidbody2D>();
        body.position = getRandomPosition();
        targetPosition = getRandomPosition();
        launching = false;
        rerouting = true; 
    }
    public void launch()

    {
        Rigidbody2D targetBody = target.GetComponent<Rigidbody2D>();
        targetPosition = targetBody.position;
        if(launching == false)
        { 
            launching = true;
          
        }
        
    }
    public void Reroute(Collision2D collision)
    {
        GameObject otherBall = collision.gameObject;
        if(rerouting == true)
        {
            otherBall.GetComponent<BallBehavior>().rerouting = false;
            Rigidbody2D ballBody = otherBall.GetComponent<Rigidbody2D>();
            Vector2 contact = collision.GetContact(0).normal;
            targetPosition = Vector2.Reflect(targetPosition, contact).normalized;
            launching = false;
            float separationDistance = 0.1f;
            ballBody.position += contact * separationDistance; 

        }
        else
        {
            rerouting = true; 
        }
    }

    public bool onCooldown()
    {
        bool result = true;
        float timeSinceLastLaunch = Time.time - timeLastLaunch;
        if(timeSinceLastLaunch > cooldown)
        {
            return result = false;
        }
        return result;
    }

    public void startCooldown()
    {
        launching = false;
        timeLastLaunch = Time.time;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(this + "Collided with: " + collision.gameObject.name);
        if(collision.gameObject.tag == "Wall")
        {
            targetPosition = getRandomPosition(); 
        }
        if(collision.gameObject.tag == "Ball")
        {
            Reroute(collision);
        }
    }

    void Start()
    {
         secondsToMaxSpeed = 30;
        minSpeed = 0.25f;
        maxSpeed = 7f;
        targetPosition = getRandomPosition();
        minX = -10.9f;
        maxX = 11.1f;
        minY = -4.3f;
        maxY = 4.2f;




    }

    // Update is called once per frame
    void  FixedUpdate() { 
        if (onCooldown() == false){
            if (launching == true)
            {
                float currentLaunchTime = Time.time - timeLaunchStart;
                if (currentLaunchTime > launchDuration)
                {
                    startCooldown();
                }
            }
            else
            {
                //Debug.Log("becca");
                launch();
            }
        }
        body = GetComponent<Rigidbody2D>();
        Vector2 currentPosition = body.position;
        float distance = Vector2.Distance(currentPosition, targetPosition);
        if(distance > 0.1f)
        {
            float difficulty = getDifficultyPercentage();
            float currentSpeed;
            if(launching == true)
            {
                float launchingForHowLong = Time.time - timeLaunchStart;
                if(launchingForHowLong > launchDuration)
                {
                    startCooldown();
                }
                currentSpeed = Mathf.Lerp(minLaunchSpeed, maxLaunchSpeed, difficulty);
            }
            else{
                currentSpeed = currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, difficulty);
            }
            currentSpeed = currentSpeed * Time.deltaTime;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, currentSpeed);
            body.MovePosition(newPosition);
            //transform.position = newPosition;
        }
        else //You are at target
        {
            if(launching == true){
                startCooldown();
            }
            targetPosition = getRandomPosition();
        }
    }
}
