using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public Transform handPosition;
    public Slider powerSlider;
    public Slider angleSlider;
    public TextMeshProUGUI powerText;  // Reference to display the power value
    public TextMeshProUGUI angleText;  // Reference to display the angle value
    public Transform pointer; // Reference to the pointer
    public GameManager gameManager;

    private bool canShoot = false;

    private void Update()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }

        // Update the pointer direction based on the angle slider
        float angle = angleSlider.value;
        pointer.localRotation = Quaternion.Euler(-angle, 0, 0); // Adjusting local rotation

        // Update the displayed slider values
        powerText.text = powerSlider.value.ToString("0.00");
        angleText.text = angleSlider.value.ToString("0.00");
    }

    public void EnableShooting()
    {
        canShoot = true;
        powerSlider.interactable = true;
        angleSlider.interactable = true;
    }

    public void DisableShooting()
    {
        canShoot = false;
        powerSlider.interactable = false;
        angleSlider.interactable = false;
    }

    private void ShootBall()
    {
        if (!canShoot) return;

        float power = powerSlider.value;
        float angle = angleSlider.value;

        Vector3 direction = Quaternion.Euler(-angle, 0, 0) * transform.forward;
        ballRigidbody.useGravity = true;
        ballRigidbody.AddForce(direction * power, ForceMode.Impulse);

        DisableShooting();

        // Notify the GameManager that the player has shot
        StartCoroutine(CheckIfScored());
    }

    private IEnumerator CheckIfScored()
    {
        yield return new WaitForSeconds(3); // Wait for the ball to land

        // Logic to check if scored (assume the ball has a script to detect scoring)
        bool scored = false; // Replace this with actual scoring logic

        gameManager.PlayerShot(scored);
        ResetBall();
    }

    private void ResetBall()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.position = handPosition.position;
    }
}
