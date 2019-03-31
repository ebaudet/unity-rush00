﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class player : MonoBehaviour {

    public Camera cam;
    public GameObject Player;
    public GameObject weapon_sprite;
    private GameObject weapon;
    public float speed;
    private string path;
    private string weapon_name;
    private string weapon_tmp;
    private float vertical;
    private float horizontal;
    private Vector3 cameraDif;
    private Vector3 direction;
    private GameObject weapon_collision;
    private Collider2D take = null;

	// Use this for initialization
	void Start () {
        //cameraDif = cam.transform.position.y - transform.position.y;
        //weapon_name = int.Parse(weapon_tmp);
        //weapon_sprite = GetComponent<SpriteRenderer>();
        //Debug.Log(we)

        //path = "Assets/Sprites/weapons/attach-to-body/" + numbersOnly + ".png";
        //Debug.Log(path);
        //weapon_sprite.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<SpriteRenderer>("Assets/Sprites/weapons/attach-to-body/1.png", typeof(SpriteRenderer));
        //weapon_sprite.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
        //AssetDatabase.ImportAsset("Assets/Sprites/weapons/attach-to-body/1.png", ImportAssetOptions.Default);
        //Application.
                   //weapon_sprite = 
	}

    private bool getKeyUp()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey("up"));
    }

    private bool getKeyDown()
    {
        return (Input.GetKey(KeyCode.S) || Input.GetKey("down"));
    }

    private bool getKeyRight()
    {
        return (Input.GetKey(KeyCode.D) || Input.GetKey("right"));
    }

    private bool getKeyLeft()
    {
        return (Input.GetKey(KeyCode.A) || Input.GetKey("left"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (weapon && Input.GetMouseButtonDown(1))
            drop_weapon();
        else if (weapon && Input.GetMouseButtonDown(0))
            weapon.gameObject.GetComponent<Weapon>().fire(transform.position, transform.rotation);
                
        vertical = 0;
        horizontal = 0;
        if (getKeyDown())
            vertical += -0.2f;
        if (getKeyUp())
            vertical += 0.2f;
        if (getKeyLeft())
            horizontal += -0.2f;
        if (getKeyRight())
            horizontal += 0.2f;
        // GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * horizontal, speed * vertical);
        // transform.Translate(horizontal, vertical, 0);
        if (weapon)
        {
            weapon.transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y - 0.3f, 0);
        }
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction.z = transform.position.z;
        if (take != null)
            take_weapon();
	}

	private void FixedUpdate()
	{
        // Change direction of player to the mouse pointer
        SpriteRenderer currentSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        float angle = Mathf.Atan2(direction.y - currentSprite.transform.position.y, direction.x - currentSprite.transform.position.x) * Mathf.Rad2Deg;
        angle += 90;
        currentSprite.transform.rotation = Quaternion.Euler(0, 0, currentSprite.transform.rotation.z + angle);

        // put camera on player position
        Camera.main.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5F, -10);
	}

    private void drop_weapon()
    {
        weapon.GetComponent<CircleCollider2D>().enabled = true;
        weapon.GetComponent<SpriteRenderer>().enabled = true;
        //weapon.GetComponent<Rigidbody2D>().AddForce(transform.up * 10f);
        weapon = null;

        weapon_sprite.gameObject.SetActive(false);
        weapon_sprite.GetComponent<SpriteRenderer>().sprite = null;//(Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));

    }

    void take_weapon()
    {
        if(Input.GetKeyDown(KeyCode.E) && take.gameObject.tag == "weapon")
        {
            Debug.Log("ok");
            weapon = take.gameObject;
            path = "Assets/Sprites/weapons/attach-to-body/" + weapon.GetComponent<Weapon>().weapon_number + ".png";
            //Debug.Log(path);

            weapon_sprite.gameObject.SetActive(true);
            weapon_sprite.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));

            weapon.GetComponent<CircleCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<BoxCollider2D>().enabled = false;
        take = collision;
	}



	//private void OnTriggerStay2D(Collider2D collision)
	//{
 //       Debug.Log("cocou");
 //       //Debug.Log(collision.gameObject.tag);
 //       if (Input.GetKeyDown(KeyCode.E) && collision.gameObject.tag == "weapon")
 //       {
 //           Debug.Log("ok");
 //           weapon = collision.gameObject;
 //           path = "Assets/Sprites/weapons/attach-to-body/" + weapon.GetComponent<Weapon>().weapon_number + ".png";
 //           //Debug.Log(path);

 //           weapon_sprite.gameObject.SetActive(true);
 //           weapon_sprite.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));

 //           weapon.GetComponent<CircleCollider2D>().enabled = false;
 //           weapon.GetComponent<SpriteRenderer>().enabled = false;
 //       }
	//}

	private void OnTriggerExit2D(Collider2D other)
	{
        if (!weapon)
            other.GetComponent<BoxCollider2D>().enabled = true;
        take = null;
	}

	//public void OnCollisionEnter2D(Collision2D collision)
	//{
	//    //Debug.Log("coucou");
	//    Debug.Log(collision.gameObject.tag);
	//    if (Input.GetKeyDown(KeyCode.E) && collision.gameObject.tag == "weapon")
	//    {
	//        Debug.Log("ok");
	//        weapon = collision.gameObject;
	//        path = "Assets/Sprites/weapons/attach-to-body/" + weapon.GetComponent<Weapon>().weapon_number + ".png";
	//        Debug.Log(path);
	//        //weapon.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
	//        //string numbersOnly = Regex.Replace(weapon_tmp, "[^0-9]", "");
	//        weapon.GetComponent<BoxCollider2D>().enabled = false;
	//        weapon.GetComponent<SpriteRenderer>().enabled = false;
	//        weapon_sprite.gameObject.SetActive(true);
	//        weapon_sprite.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
	//    }
	//}

}
