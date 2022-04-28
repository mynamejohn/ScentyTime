using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Manager_Select : MonoBehaviour {

    public GameObject touch_effect;
    public GameObject paraent;

    // Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.touchCount>0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero, Mathf.Infinity, 1 << 8);

                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                pos.z = 0;
                GameObject effect = Instantiate(touch_effect, pos, Quaternion.identity);
                effect.transform.SetParent(paraent.transform);
                effect.transform.localScale = new Vector3(1, 1, 1);
                effect.transform.position = pos;
            }
            //else if(Input.GetTouch(0).phase == TouchPhase.Moved)
            //{
            //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //    moving_touch = pos;

            //    float amount = start_touch.x - moving_touch.x;

            //}
        }
       /* for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero, Mathf.Infinity, 1 << 8);

                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                pos.z = 0;
                GameObject effect = Instantiate(touch_effect, pos, Quaternion.identity);
                effect.transform.SetParent(paraent.transform);
                effect.transform.localScale = new Vector3(1, 1, 1);
                effect.transform.position = pos;
            }

            else if(Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                
            }
        }*/
    }


}
