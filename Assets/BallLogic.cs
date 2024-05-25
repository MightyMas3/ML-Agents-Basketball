using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallLogic : MonoBehaviour
{
    public Transform BallPosition;
    public Transform Pointer;
    public Rigidbody Ball;

    public Slider PowerSlider;
    public Slider AngleSlider;

    public TextMeshProUGUI PowerText;
    public TextMeshProUGUI AngleText;

    private float power;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        PowerSlider.onValueChanged.AddListener(PowerUpdate);
        AngleSlider.onValueChanged.AddListener(AngleUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ball.useGravity = true;
        Ball.AddForce(Pointer.right*power);
        Invoke(nameof(ResetBall), 3f);
    }

    void PowerUpdate(float power)
    {
        this.power = power*100f;
        PowerText.text = power.ToString();
    }
    void AngleUpdate(float angle)
    {
        this.angle = angle;
        AngleText.text = angle.ToString();
        Pointer.rotation = Quaternion.Euler(0, 0, angle);
    }
    void ResetBall()
    {
        Ball.velocity = Vector3.zero;
        Ball.useGravity = false;
        Ball.transform.position = BallPosition.position;
    }
}
