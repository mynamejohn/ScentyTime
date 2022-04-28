using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerFade: MonoBehaviour {

    Image handle;

    public bool isShowing = false;
    public bool isTrigger = false;

    private void Awake()
    {
        handle = gameObject.GetComponent<Image>();
    }
    // Use this for initialization
    void Start ()
    {
        if (!isTrigger)
            StartCoroutine(Fading(isShowing));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFade()
    {
        if(isTrigger)
            StartCoroutine(Fading(isShowing));
    }

    IEnumerator Fading(bool isShowing)
    {
        if (isShowing)
        {
            float currnetalpha = 0f;
            while(currnetalpha<1)
            {
                handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, currnetalpha);
                currnetalpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 1);
        }
        else
        {
            float currnetalpha = 1f;
            while (currnetalpha > 0)
            {
                handle.color = new Color(0, 0, 0, currnetalpha);
                currnetalpha -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            handle.color = new Color(0, 0, 0, 0);
            Destroy(gameObject);
        }
        yield break;    
    }
}
