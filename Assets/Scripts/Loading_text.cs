using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Loading_text : MonoBehaviour {
    Image control;
    public float amount = 0.01f;
    // Use this for initialization
    private void Awake()
    {
        control = GetComponent<Image>();
    }
    void Start () {
        StartCoroutine(Particle(amount));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Particle(float amount)
    {
        float alpha = 0f;
        Color start = new Color(255, 255, 255, 0);
        Color end = new Color(255, 255, 255, 1);
        while(true)
        {
            while(alpha<1)
            {
                control.color = Color.Lerp(start, end, alpha);
                alpha += amount;
                yield return new WaitForSeconds(0.01f);
            }
            alpha = 1;
            control.color = Color.Lerp(start, end, alpha);
            while(alpha>0)
            {
                control.color = Color.Lerp(start, end, alpha);
                alpha -= amount;
                yield return new WaitForSeconds(0.01f);
            }
            alpha = 0;
            control.color = Color.Lerp(start, end, alpha);
        }
    }
}
