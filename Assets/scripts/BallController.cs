using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;       // Starting speed of the ball
    public float maxSpeed = 15f;
    public float minSpeed = 10f;

    private Rigidbody rb;
    private Vector3 initialVelocity;  // Cache to avoid creating new vectors every time

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartBall();
    }

    private void Update()
    {
        ClampBallSpeed();
    }

    public void StartBall()
    {
        float directionX = Random.Range(0, 2) == 0 ? 1 : -1;
        float directionY = Random.Range(0, 2) == 0 ? 1 : -1;

        // Start the ball with some vertical velocity to avoid purely horizontal movement
        Vector3 initialVelocity = new Vector3(speed * directionX, speed * directionY + Random.Range(0.5f, 1.5f), 0);
        rb.velocity = initialVelocity;
    }

    private void ClampBallSpeed()
    {
        float currentSpeed = rb.velocity.magnitude;

        // Ensure the ball's speed is not less than the minimum speed
        if (currentSpeed < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        else if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Ensure the ball always has some vertical velocity
        if (Mathf.Abs(rb.velocity.y) < minSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y > 0 ? minSpeed : -minSpeed, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Vector3 ballVelocity = rb.velocity;

            // If the speed becomes too low after hitting the paddle, ensure it's at least the minimum speed
            if (ballVelocity.magnitude < minSpeed)
            {
                rb.velocity = ballVelocity.normalized * minSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal1"))
        {
            GameManager.Instance.AddScore(2);
            ResetBall();
        }
        else if (other.CompareTag("Goal2"))
        {
            GameManager.Instance.AddScore(1);
            ResetBall();
        }
    }

    private void ResetBall()
    {
        transform.position = Vector3.zero;
        StartBall();
    }
}
