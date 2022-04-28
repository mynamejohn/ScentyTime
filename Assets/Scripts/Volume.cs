using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour {

    public Vector3 touchPosition;
    public Vector3 newPosition;
    public Vector3 offset;
    public Transform circle;
	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (circle.transform.localPosition.x < -190)
            circle.transform.localPosition = new Vector3(-190, 0, 0);
        else if(circle.transform.localPosition.x > 190)
            circle.transform.localPosition = new Vector3(190, 0, 0);
    }
    private void OnMouseDown()
    {
        //touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //offset = touchPosition - circle.transform.position;
    }
    private void OnMouseDrag()
    {
        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //newPosition = touchPosition - offset;
        circle.transform.position = new Vector3(touchPosition.x, 0, 0) ;
        circle.localPosition = new Vector3(circle.localPosition.x, 0, 0);
    }
}
