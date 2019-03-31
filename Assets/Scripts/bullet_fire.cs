using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fire : MonoBehaviour
{
	void Update () {
        Debug.Log("update = " + GetComponent<BoxCollider2D>().enabled);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "enemi")
        {
            Debug.Log("Destroy bullet");
            Destroy(gameObject, 0);
            if (collision.gameObject.tag == "enemi")
                Destroy(collision.gameObject, 0.5f);
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log("Destroy bullet");
            Destroy(gameObject, 0);
            if (collision.gameObject.tag == "enemi")
                Destroy(collision.gameObject, 0.5f);
        }
	}
}
