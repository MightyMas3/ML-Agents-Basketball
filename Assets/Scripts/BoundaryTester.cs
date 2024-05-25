using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryTester : MonoBehaviour
{
    public Transform leftBoundary;
    public Transform rightBoundary;
    public Transform frontBoundary;
    public Transform backBoundary;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ResetAgentPosition), 0, 5f);
    }

    private void ResetAgentPosition()
    {
        // Randomize the agent's position within the boundaries
        Debug.Log(leftBoundary.localPosition.x + " + " + rightBoundary.localPosition.x);
        float randomX = Random.Range(leftBoundary.localPosition.x, rightBoundary.localPosition.x);
        float randomZ = Random.Range(frontBoundary.localPosition.z, backBoundary.localPosition.z);
        transform.localPosition = new Vector3(randomX, transform.localPosition.y, randomZ);
    }
    // Update is called once per frame
    void Update()
    {

    }
}