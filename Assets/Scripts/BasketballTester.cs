using UnityEngine;
using System.Collections;

public class BasketballTester : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public Transform handPosition;
    public Transform hoop;
    public BallScript ballScript; // Reference to the BallScript

    public float maxPower = 10f;
    public float minAngle = 30f;
    public float maxAngle = 60f;

    private void Start()
    {
        // Check if all references are set
        if (ballRigidbody == null || handPosition == null || hoop == null || ballScript == null)
        {
            Debug.LogError("One or more references are not set in the BasketballTester script.");
            return;
        }

        // Enable testing mode to prevent episode ending
        ballScript.testingMode = true;

        // Start the testing process
        StartCoroutine(TestShots());
    }

    private IEnumerator TestShots()
    {
        float bestPower = 0f;
        float bestAngle = 0f;
        float closestDistance = float.MaxValue;

        for (float power = 1f; power <= maxPower; power += 0.5f)
        {
            for (float angle = minAngle; angle <= maxAngle; angle += 1f)
            {
                // Test the shot and get the distance
                float distance = TestShot(power, angle);
                yield return new WaitForSeconds(2f); // Wait for the ball to settle

                // Check if this shot is the best so far
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPower = power;
                    bestAngle = angle;
                }

                // Reset the ball position for the next test
                ResetBall();
            }
        }

        Debug.Log("Best Power: " + bestPower + ", Best Angle: " + bestAngle);

        // Disable testing mode after testing
        ballScript.testingMode = false;
    }

    private float TestShot(float power, float angle)
    {
        // Ensure the ball is at the hand position
        ResetBall();

        // Calculate the direction and apply force
        Vector3 direction = Quaternion.AngleAxis(angle, Vector3.right) * transform.forward;
        ballRigidbody.AddForce(direction * power, ForceMode.Impulse);

        // Wait for the shot to complete
        StartCoroutine(WaitForBall(2f));

        // Calculate the distance to the hoop
        float distance = Vector3.Distance(ballRigidbody.position, hoop.position);
        Debug.Log("Shot Distance: " + distance);
        return distance;
    }

    private IEnumerator WaitForBall(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void ResetBall()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.position = handPosition.position;
        ballRigidbody.rotation = Quaternion.identity;
        ballRigidbody.useGravity = true;
    }
}
