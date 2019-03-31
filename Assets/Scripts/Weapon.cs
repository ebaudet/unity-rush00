using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class Weapon : MonoBehaviour {

    public int ammo;
    public bool inf_ammo;
    public float fire_rate;
    public float sound_propagation;
    public string weapon_number;
    public string weapon_name;
    public GameObject bullet;
    private string path;
    private float time;
    private GameObject bulletShooted;
    private AudioClip no_ammo;

	// Use this for initialization
	void Start ()
    {
        time = 0;
        no_ammo = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/Sounds/dry_fire.wav", typeof(AudioClip));
        weapon_name = GetComponent<SpriteRenderer>().sprite.name;
        weapon_number = Regex.Replace(GetComponent<SpriteRenderer>().sprite.name, "[^0-9]", "");
        path = "Assets/Sprites/weapons/shoot/" + weapon_number + ".png";
	}
	
	void Update () {
        time += Time.deltaTime;
	}

    public void fire(Vector3 pos, Quaternion player_rot)
    {
        Debug.Log("time = " + time);
        if (ammo == 0 && !inf_ammo)
            gameObject.GetComponent<AudioSource>().clip = no_ammo;
        if (time >= fire_rate)
            gameObject.GetComponent<AudioSource>().Play();
        if ((ammo > 0 || inf_ammo) && time >= fire_rate)
        {
            time = 0;
            if (!inf_ammo)
                ammo--;
            bullet.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
            //Debug.Log(player_rot.eulerAngles.z);
            bulletShooted = Instantiate(bullet, pos, player_rot);
            transform.rotation = player_rot;
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction.z = transform.position.z;
            float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;
            bulletShooted.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + angle);          
            bulletShooted.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.down * 10);
            if (inf_ammo)
                Destroy(bulletShooted, 0.1f);
            
        }
    }

}
