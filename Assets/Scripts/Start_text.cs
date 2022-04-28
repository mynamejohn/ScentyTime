using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Start_text : MonoBehaviour {

    PlayManager pm;
    public Image[] controls = new Image[3];

    // Use this for initialization
    private void Awake()
    {
        pm = GameObject.FindObjectOfType<PlayManager>();
    }
    void Start () {
        StartCoroutine(text_steps());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator text_steps()
    {
        float alpha = 0f;

        while(alpha<1)
        {
            for (int i = 0; i < 3;i++)
            {   
                controls[i].color = Color.Lerp(new Color(255, 255, 255,0), new Color(255, 255, 255, 1),alpha); 
            }
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 3; i++)
        {
            controls[i].color = new Color(255, 255, 255, 1);
        }

        yield return new WaitForSeconds(1.5f);
        while (alpha > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                controls[i].color = Color.Lerp(new Color(255, 255, 255,0), new Color(255, 255, 255, 1), alpha);
            }
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 3; i++)
        {
            controls[i].color = new Color(255, 255, 255, 0);    
        }

        pm.StartGame();
        Destroy(gameObject);
    }
}
