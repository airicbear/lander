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
    private float force = 1.0f;
    private float forceRate = 5.0f;
    private float maxForce = 30.0f;
    private float rotationRate = 10.0f;
    public Text velocityLabel;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        tf = this.GetComponent<Transform>();
        initPos = this.transform.position;
        initRot = this.transform.eulerAngles;
    }

    private void Update()
    {
        velocityLabel.text = rb.velocity.y.ToString("N1") + " m/s";
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector2.up * force);

            if (force < maxForce)
            {
                force += Time.deltaTime * forceRate;
            }
        } else {
            force = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tf.Rotate(Vector3.forward * rotationRate * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            tf.Rotate(Vector3.back * rotationRate * Time.deltaTime);
        }

        if (IsUpsideDown() && rb.velocity == Vector2.zero)
        {
            Reset();
        }
    }

    public void Reset()
    {
        tf.position = initPos;
        tf.eulerAngles = initRot;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    private bool IsUpsideDown()
    {
        return 85 < tf.eulerAngles.z && tf.eulerAngles.z < 275;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (System.Math.Abs(rb.velocity.x) > 6 || System.Math.Abs(rb.velocity.y) > 6)
        {
            Reset();
        }  
    }
}
