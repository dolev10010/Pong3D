using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;  // Speed of paddle movement
    public bool isPlayer1;     // Check if this paddle belongs to Player 1

    private float minY, maxY;  // Boundaries for paddle movement
    private float paddleHeight;  // Height of paddle to adjust boundary calculations

    // Initialization method, private since it’s only used within this script
    private void Start()
    {
        // Calculate paddle boundaries
        paddleHeight = GetComponent<Collider>().bounds.size.y / 2;
        minY = -5.9f + paddleHeight;  // Adjust based on walls
        maxY = 5.9f - paddleHeight;   // Adjust based on walls
    }

    // Update method for paddle movement, kept private since it's automatic in Unity
    private void Update()
    {
        // Get input and move the paddle
        float moveInput = isPlayer1 ? Input.GetAxis("Vertical1") : Input.GetAxis("Vertical2");
        Vector3 move = new Vector3(0, moveInput * speed * Time.deltaTime, 0);
        transform.Translate(move);

        // Clamp paddle movement to within screen boundaries
        ClampPosition();
    }

    // Clamps paddle position within bounds, private since it's not called externally
    private void ClampPosition()
    {
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}
