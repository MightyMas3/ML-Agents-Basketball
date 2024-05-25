using System;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Action<bool> onHit;
    public bool testingMode = false; // Flag to disable episode-ending behavior during testing

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Hoop")
        {
            Debug.Log("Scored!");
            onHit?.Invoke(true);
        }
        else if (other.gameObject.tag == "Bounds" && !testingMode)
        {
            Debug.Log("Hit Bounds");
            onHit?.Invoke(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bounds")) && !testingMode)
        {
            Debug.Log("Hit Ground or Bounds");
            onHit?.Invoke(false);
        }
    }
}
