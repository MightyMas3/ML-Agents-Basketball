using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BasketballAgent : Agent
{
    public Transform hoop;
    public Rigidbody ballRigidbody;
    public Transform handPosition;
    public Transform basketTrigger;

    public Transform leftBoundary;
    public Transform rightBoundary;
    public Transform frontBoundary;
    public Transform backBoundary;

    public float baselinePower = 10f; // Adjusted with your determined optimal power
    public float minBaselineAngle = 300f; // Minimum angle in the optimal range
    public float maxBaselineAngle = 340f; // Maximum angle in the optimal range
    
    public float maxPowerAdjustment = 5f; // Reduced range to adjust power
    public float maxAngleAdjustment = 30f; // Range to adjust angle



    public override void Initialize()
    {
        ballRigidbody.GetComponent<BallScript>().onHit += OnBallHit;
    }

    private void OnBallHit(bool state)
    {
        if (state)
        {
            AddReward(10.0f); // Positive reward for scoring
        }
        else
        {
            AddReward(-1.0f); // Negative reward for missing
        }
        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        //ResetAgentPosition();
        ResetBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(ballRigidbody.position);
        sensor.AddObservation(hoop.localPosition);
        sensor.AddObservation(ballRigidbody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float powerAdjustment = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f) * maxPowerAdjustment;
        float angleAdjustment = Mathf.Clamp(actionBuffers.ContinuousActions[1], 0f, 1f);

        float shootPower = baselinePower + powerAdjustment;
        float shootAngle = Mathf.Lerp(minBaselineAngle, maxBaselineAngle, angleAdjustment);

        ShootBall(shootPower, shootAngle);
    }

    public void ShootBall(float power, float angle)
    {
        Debug.Log("Shooting with angle: " + angle + " and power: " + power);
        

        handPosition.rotation = Quaternion.Euler(angle, 0, 0);
        //Vector3 direction = angle * transform.forward;
        ballRigidbody.AddForce(handPosition.forward * power*2f);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform == basketTrigger)
    //    {
    //        Debug.Log("OnEpisodeEnd Hoop");
    //        AddReward(10.0f); // Positive reward for scoring
    //        EndEpisode();
    //    }
    //    else if (other.gameObject.tag == "Bounds")
    //    {
    //        Debug.Log("OnEpisodeEnd Bounds");
    //        AddReward(-1.0f); // Negative reward for hitting the bounds
    //        EndEpisode();
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bounds"))
    //    {
    //        Debug.Log("OnEpisodeEnd Bounds");
    //        AddReward(-1.0f); // Negative reward for hitting the ground or bounds
    //        EndEpisode();
    //    }
    //}

    private void ResetBall()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.transform.position = handPosition.position;
    }

    private void ResetAgentPosition()
    {
        float randomX = Random.Range(leftBoundary.localPosition.x, rightBoundary.localPosition.x);
        float randomZ = Random.Range(frontBoundary.localPosition.z, backBoundary.localPosition.z);
        transform.localPosition = new Vector3(randomX, transform.localPosition.y, randomZ);

        // Ensure the agent is always upright and facing the hoop
        transform.rotation = Quaternion.LookRotation(hoop.position - transform.position, Vector3.up);

        // Ensure the agent is upright
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        Debug.Log($"Agent Position: {transform.position}, Rotation: {transform.rotation}");
    }
}
