using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class player : MonoBehaviour {
    public Camera       cam;
    public GameObject   Player;
    public GameObject   weapon_sprite;
    public Weapon       weapon = null;
    public float        speed;
    public delegate     void GunShoot(Vector3 pos, float dist);
    public event GunShoot OnGunShooted;

    private Vector3     direction;
    private Collider2D  take = null;
    private Quaternion  player_rot;
    private Quaternion  bullet_rotation;

    public static player instance {get; private set;}

	// Use this for initialization
	void Start () {
        instance = this;
	}

    // Key Getters
    private bool getKeyUp()
    { return (Input.GetKey(KeyCode.W) || Input.GetKey("up")); }

    private bool getKeyDown()
    { return (Input.GetKey(KeyCode.S) || Input.GetKey("down")); }

    private bool getKeyRight()
    { return (Input.GetKey(KeyCode.D) || Input.GetKey("right")); }
    
    private bool getKeyLeft()
    { return (Input.GetKey(KeyCode.A) || Input.GetKey("left")); }
	
	// Update is called once per frame
	void Update ()
    {
        if (weapon && Input.GetMouseButtonDown(1))
            drop_weapon();
        else if (weapon && Input.GetMouseButtonDown(0)) {
            weapon.fire(transform.position, transform.localRotation);
            Debug.Log("Raised event: OnGunShooted");
            OnGunShooted(transform.position, weapon.sound_propagation);
        }
               
        float vertical = 0;
        float horizontal = 0;
        if (getKeyDown())
            vertical += -0.2f;
        if (getKeyUp())
            vertical += 0.2f;
        if (getKeyLeft())
            horizontal += -0.2f;
        if (getKeyRight())
            horizontal += 0.2f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * horizontal, speed * vertical);
        if (weapon)
        {
            weapon.transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y - 0.3f, 0);
        }

        if (take != null)
            take_weapon();
	}

	private void FixedUpdate()
	{
        // Change direction of player to the mouse pointer
        //SpriteRenderer currentSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction.z = transform.position.z;
        float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;
        bullet_rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);
        angle += 90;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);
        Camera.main.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5F, -10);

        // put camera on player position
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
        if (weapon == null
            && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            && take.gameObject.tag == "weapon")
        {
            Debug.Log("ok");
            weapon = take.GetComponent<Weapon>();
            string path = "Assets/Sprites/weapons/attach-to-body/" + weapon.GetComponent<Weapon>().weapon_number + ".png";
            //Debug.Log(path);

            weapon_sprite.gameObject.SetActive(true);
            weapon_sprite.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));

            weapon.GetComponent<CircleCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "weapon" && collision.GetComponent<BoxCollider2D>())
        {
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }
        take = collision;
	}

    private void OnTriggerExit2D(Collider2D other)
	{
        if (!weapon && other.GetComponent<BoxCollider2D>())
            other.GetComponent<BoxCollider2D>().enabled = true;
        if (other.tag == "bullet")
            other.GetComponent<BoxCollider2D>().isTrigger = false;
        take = null;
	}
}
