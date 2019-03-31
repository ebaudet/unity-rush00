using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public List<AudioClip>	musics = new List<AudioClip>();

	private AudioSource _source;

	// Use this for initialization
	void Start () {
		_source = gameObject.GetComponent<AudioSource>();
		int id = Random.Range (0, musics.Count);
		_source.clip = musics[id];
		_source.Play();
	}
}
