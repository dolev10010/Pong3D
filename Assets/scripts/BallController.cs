using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;       // Starting speed of the ball
    public float maxSpeed = 15f;
    public float minSpeed = 10f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartBall();
    }

    private void Update()
    {
        ClampBallSpeed();

        // Ensure the ball's z position stays fixed at 0
        Vector3 position = transform.position;
        position.z = 0;  // Keep the ball on the Z plane
        transform.position = position;
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
            rb.velocity = rb.velocity.normalized * (minSpeed + 1f);  // Add a buffer to ensure speed doesn't drop
        }
        else if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Ensure the ball always has sufficient vertical velocity
        if (Mathf.Abs(rb.velocity.y) < 0.7f)  // Adjust this threshold for better vertical movement
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * 0.7f, rb.velocity.z);
        }

        // Ensure the ball always has sufficient horizontal velocity
        if (Mathf.Abs(rb.velocity.x) < 0.7f)  // Ensure X-axis has enough velocity
        {
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * 0.7f, rb.velocity.y, rb.velocity.z);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Handle Paddle and Obstacle Collisions
        if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Obstacle"))
        {
            HandleBallVelocity();  // Keep velocity consistent

            // Move ball slightly away from paddle or obstacle to prevent overlap
            Vector3 objectCenter = collision.gameObject.transform.position;

            // Calculate push-back direction
            Vector3 pushDirection = (transform.position - objectCenter).normalized;
            transform.position += pushDirection * 0.2f;  // Increase push distance to avoid stickiness
        }

        // Handle Wall Collision
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 ballVelocity = rb.velocity;

            // Re-correct the vertical movement if too small
            if (Mathf.Abs(ballVelocity.y) < 0.5f)
            {
                rb.velocity = new Vector3(ballVelocity.x, Mathf.Sign(ballVelocity.y) * 0.5f, ballVelocity.z);
            }
        }
    }

    // Keep velocity consistent
    private void HandleBallVelocity()
    {
        Vector3 ballVelocity = rb.velocity;

        // Ensure minimum speed is maintained
        if (ballVelocity.magnitude < minSpeed)
        {
            rb.velocity = ballVelocity.normalized * minSpeed;
        }

        // Clamp ball position to prevent it from going out of bounds
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, -3.8f, 3.8f);  // Adjust bounds as per your wall position
        position.z = 0;  // Force the Z position to remain fixed
        transform.position = position;

        // Ensure vertical velocity is maintained
        if (Mathf.Abs(ballVelocity.y) < 0.5f)  // Adjust vertical threshold
        {
            rb.velocity = new Vector3(ballVelocity.x, Mathf.Sign(ballVelocity.y) * 0.5f, ballVelocity.z);
        }

        // Ensure horizontal velocity is maintained
        if (Mathf.Abs(ballVelocity.x) < 0.5f)
        {
            rb.velocity = new Vector3(Mathf.Sign(ballVelocity.x) * 0.5f, rb.velocity.y, ballVelocity.z);
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
