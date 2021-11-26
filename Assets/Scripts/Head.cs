using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public Rocket rocket;
    public GameObject rocketDummy;
    public GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Dummy")) {
            GameObject rocketDummyClone = Instantiate(rocketDummy, rocket.transform.position, rocket.transform.rotation);
            rocketDummyClone.GetComponent<Rigidbody2D>().AddForce(collision.contacts[0].normal * 1000.0f);
            rocketDummyClone.GetComponent<Rigidbody2D>().angularVelocity = collision.contacts[0].normalImpulse * 1000.0f;
            GameObject explosionClone = Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(rocketDummyClone, 2.0f);
            Destroy(explosionClone, 2.0f);
            GetComponent<AudioSource>().Play();
            rocket.Reset();
        }
    }
}
