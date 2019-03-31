using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class enemi : MonoBehaviour {
    private GameObject head;
    private GameObject weapon;
    public player player;
    public Vector2 isAlerted = Vector2.zero;

    void OnEnable()
    {
        player.OnGunShooted += ListenBullet;
    }

    void OnDisable()
    {
        player.OnGunShooted -= ListenBullet;
    }

    void ListenBullet(Vector2 pos, float dist)
    {
        if (Vector2.Distance(pos, transform.position) <= dist)
            isAlerted = pos;
    }

	// Use this for initialization
    void Start ()
    {
        weapon = transform.Find("weapon").gameObject;
        head = transform.Find("head").gameObject;
        string path = "Assets/Sprites/characters/body/" + Random.Range(1, 4).ToString() + ".png";
        Debug.Log("body = " + path);
        gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
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
		if (isAlerted != Vector2.zero)
        {
            Debug.Log("Enemi is alerted");
            float angle = Mathf.Atan2(isAlerted.y - transform.position.y, isAlerted.x - transform.position.x) * Mathf.Rad2Deg;
            // bullet_rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);
            angle += 90;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);
            isAlerted = Vector2.zero;
        }
	}
}
