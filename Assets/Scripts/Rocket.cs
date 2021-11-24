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
    private float initialForce = 15.0f;
    private float initialForceRate = 10.0f;
    private float force = 0.0f;
    private float forceRate = 0.0f;
    private float forceRateRate = 10.0f;
    private float maxForceRate = 50.0f;
    private float maxForce = 30.0f;
    private float rotationRate = 10.0f;
    private int points = 0;
    private float stabilizingTime = 0.0f;
    private bool waitingForStability = false;
    private Vector3 direction = Vector3.zero;
    public GameObject body;
    public Text scoreLabel;
    public Text forceLabel;
    public Text directionLabel;
    public TargetPlatform targetPlatform;

    private void Start()
    {
        Input.backButtonLeavesApp = true;
        rb = this.GetComponent<Rigidbody2D>();
        tf = this.GetComponent<Transform>();
        initPos = this.transform.position;
        initRot = this.transform.eulerAngles;
    }

    private void Thrust() {
        rb.AddRelativeForce(Vector2.up * (force + initialForce));

        if (force < maxForce)
        {
            if (forceRate < maxForceRate)
            {
                forceRate += Time.deltaTime * forceRateRate;
            }
            else
            {
                forceRate = maxForceRate;
            }

            force += Time.deltaTime * forceRate;
        }
        else
        {
            force = maxForce;
        }
    }

    public void StartThrust() {
        isThrusting = true;
    }

    public void StopThrust() {
        isThrusting = false;
    }

    public void TurnLeft() {
        directionLabel.text = "Turning Left";
        direction = Vector3.forward;
    }

    public void TurnRight() {
        directionLabel.text = "Turning Right";
        direction = Vector3.back;
    }

    public void StopTurning() {
        body.GetComponent<AudioSource>().Stop();
        direction = Vector3.zero;
    }

    private void FixedUpdate() {
        if (isThrusting) {
            Thrust();
            if (!GetComponent<AudioSource>().isPlaying) {
                GetComponent<AudioSource>().Play();
            }
        } else {
            GetComponent<AudioSource>().Stop();
            force = 0;
            forceRate = initialForceRate;
        }

        tf.Rotate(direction * rotationRate * Time.deltaTime);

        if (direction == Vector3.zero)
        {
            body.GetComponent<AudioSource>().Stop();
            directionLabel.text = "Forward";
        }
        else if (!body.GetComponent<AudioSource>().isPlaying)
        {
            body.GetComponent<AudioSource>().Play();
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

    private void Update()
    {
        if (isThrusting) {
            forceLabel.text = $"Force: {force.ToString("N1")} N";
        } else {
            forceLabel.text = "Standby";
        }
    }

    public void Reset()
    {
        direction = Vector3.zero;
        force = 0;
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
        targetPlatform.PlaySound();
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
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag.Equals("TargetPlatform")) {
            waitingForStability = false;
            stabilizingTime = 0;
        }
    }
}
