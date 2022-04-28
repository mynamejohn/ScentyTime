using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour {
    public float size = 0;

    Transform trans;
    // Use this for initialization
    private void Awake()
    {
        trans = GetComponent<Transform>();
    }
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        trans.Rotate(0, 0, size);
	}
}
