using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public AudioSource AS;
    // Use this for initialization
    private void Awake()
    {
    }

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void PlayMusic(AudioClip stage)
    {
        AS.clip = stage;
        AS.Play();
    }
}
