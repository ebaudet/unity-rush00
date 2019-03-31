using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class enemi : MonoBehaviour {
    private GameObject head;
    private GameObject weapon;
    public player player;

    private float timerRun;

    public enum stateEnemy {
        stay,
        sound_alert,
        view_player,
        routine
    }
    
    public stateEnemy state = stateEnemy.stay;
    public Vector3 soundPosition = Vector3.zero;

  

    void OnEnable()
    {
        player.OnGunShooted += ListenBullet;
    }

    void OnDisable()
    {
        player.OnGunShooted -= ListenBullet;
    }

    void ListenBullet(Vector3 pos, float dist)
    {
        if (Vector3.Distance(pos, transform.position) <= dist)
        {
            Debug.Log("Enemi "+ name+" heard the bullet shot.");
            state = stateEnemy.sound_alert;
            soundPosition = pos;
        }
    }

	// Use this for initialization
    void Start ()
    {
        GameObject body = transform.Find("body").gameObject;
        weapon = body.transform.Find("weapon").gameObject;
        head = body.transform.Find("head").gameObject;
        string path = "Assets/Sprites/characters/body/" + Random.Range(1, 4).ToString() + ".png";
        Debug.Log("body = " + path);
        body.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
        path = "Assets/Sprites/characters/head/" + Random.Range(1, 13).ToString() + ".png";
        Debug.Log("head = " + path);
        head.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
        //string path = "Assets/Sprites/weapons/attach-to-body/" + weapon.GetComponent<Weapon>().weapon_number + ".png";
        ////Debug.Log(path);

        //weapon_sprite.gameObject.SetActive(true);
        //weapon_sprite.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 heading = player.transform.position - transform.position;
        // RaycastHit2D hit = new RaycastHit2D();
        Debug.DrawRay(transform.position, heading, Color.red, 3f, true);
        LayerMask mask = LayerMask.GetMask("door", "Wall", "player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, heading, 10, mask);
        if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            Debug.Log("hit:  we hit that fucking player. Kill him !");
            state = stateEnemy.view_player;
            if (weapon.GetComponent<Weapon>())
                weapon.GetComponent<Weapon>().fire(player.transform.position, transform.localRotation);
            timerRun = 3;
        }
        else
        {
            // Debug.Log("pas de colision");
        }
        // return;
        switch (state)
        {
            case stateEnemy.sound_alert:
                timerRun -= Time.deltaTime;
                if (changeState(timerRun < 0, stateEnemy.stay)) break;             
                runToPos(soundPosition);
                changeState(soundPosition == transform.position, stateEnemy.stay);
                break;
            case stateEnemy.view_player:
                timerRun -= Time.deltaTime;
                if (changeState(timerRun < 0, stateEnemy.stay)) break;
                runToPos(player.transform.position);
                break;
        };
	}

    protected void runToPos(Vector3 pos)
    {
        GetComponent<Rigidbody2D>().velocity = pos - transform.position;
        float angle = Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg;
        angle += 90;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);
    }

    protected bool changeState(bool condition, stateEnemy newState)
    {
        if (condition)
        {
            state = stateEnemy.stay;
            return true;
        }
        return false;
    }
}
