using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear_script : MonoBehaviour {

    Image img;
    public float max;
    public float speed = 0.01f;
    public float amount = 0.05f;
    private void Awake()
    {
        img = GetComponent<Image>();
        StartCoroutine(Fadein(max));
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Fadein(float max)
    {
        float al = 0.0f;

        while(al<max)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, al);
            al += amount;
            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(10.0f);
    }
}
