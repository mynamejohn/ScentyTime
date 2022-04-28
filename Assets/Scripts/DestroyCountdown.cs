using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCountdown : MonoBehaviour {

    public float timelimit = 1000;
    // Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timelimit = timelimit - Time.deltaTime * 1000;

        if (timelimit <= 0)
            Destroy(gameObject);
	}
}
