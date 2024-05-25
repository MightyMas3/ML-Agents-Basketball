using UnityEngine;

public class ManualShooting : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public Transform handPosition;
    public Transform hoop;

    [Range(1f, 20f)]
    public float shootPower = 10f;

    [Range(0f, 360f)]
    public float shootAngle = 45f; // Full angle range

    public KeyCode shootKey = KeyCode.Space;
    public KeyCode resetKey = KeyCode.D;

    private void Start()
    {
        // Ensure the ball is in the correct starting position
        ResetBall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            ShootBall(shootPower, shootAngle);
        }

        if (Input.GetKeyDown(resetKey))
        {
            ResetBall();
        }
    }

    private void ShootBall(float power, float angle)
    {
        Vector3 direction = Quaternion.Euler(angle, 0, 0) * transform.forward;
        ballRigidbody.useGravity = true;
        ballRigidbody.AddForce(direction * power, ForceMode.Impulse);

        Debug.Log("Shooting with angle: " + angle + " and power: " + power);
    }

    private void ResetBall()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.position = handPosition.position;
        ballRigidbody.rotation = Quaternion.identity;
    }
}
