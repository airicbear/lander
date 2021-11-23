using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    private Vector2 initPos;
    private Vector3 initRot;
    private Rigidbody2D rb;
    private Transform tf;
    private bool isThrusting = false;
    private float force = 1.0f;
    private float forceRate = 5.0f;
    private float maxForce = 30.0f;
    private float rotationRate = 10.0f;
    private int points = 0;
    private float stabilizingTime = 0.0f;
    private bool waitingForStability = false;
    public Text scoreLabel;
    public TargetPlatform targetPlatform;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        tf = this.GetComponent<Transform>();
        initPos = this.transform.position;
        initRot = this.transform.eulerAngles;
    }

    private void Thrust() {
        rb.AddRelativeForce(Vector2.up * force);

        if (force < maxForce)
        {
            force += Time.deltaTime * forceRate;
        }
    }

    public void StartThrust() {
        isThrusting = true;
    }

    public void StopThrust() {
        isThrusting = false;
    }

    public void TurnLeft() {
        tf.Rotate(Vector3.forward * rotationRate * Time.deltaTime);
    }

    public void TurnRight() {
        tf.Rotate(Vector3.back * rotationRate * Time.deltaTime);
    }

    private void Update()
    {
        if (isThrusting) {
            Thrust();
        } else {
            force = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TurnLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            TurnRight();
        }

        if (IsUpsideDown() && rb.velocity == Vector2.zero)
        {
            Reset();
        }

        if (waitingForStability) {
            if (rb.velocity.y < 0.01) {
                stabilizingTime += Time.deltaTime;
            }

            if (stabilizingTime > 3) {
                Reset();
                WinPoint();
                targetPlatform.ChangeLocation();
            }
        }
    }

    public void Reset()
    {
        stabilizingTime = 0;
        tf.position = initPos;
        tf.eulerAngles = initRot;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public void Restart() {
        Reset();
        points = 0;
        scoreLabel.text = "0";
        targetPlatform.ChangeLocation();
    }

    private bool IsUpsideDown()
    {
        return 85 < tf.eulerAngles.z && tf.eulerAngles.z < 275;
    }

    private void WinPoint() {
        points++;
        scoreLabel.text = points.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (System.Math.Abs(rb.velocity.x) > 6 || System.Math.Abs(rb.velocity.y) > 6)
        {
            Reset();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.tag.Equals("TargetPlatform")) {
            waitingForStability = true;
        } else {
            waitingForStability = false;
            stabilizingTime = 0;
        }
    }
}
