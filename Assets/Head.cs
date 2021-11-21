using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public Rocket rocket;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rocket.Reset();
    }
}
