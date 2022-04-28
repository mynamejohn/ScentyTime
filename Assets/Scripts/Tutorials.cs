using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour {
    
    Image[] tutorial_panels;
    int arraycnt;
    int current_cnt;

    bool canSwich = false;
    public bool isTrigger = false;
    public GameObject startgame = null;
    private void Awake()
    {
        arraycnt = transform.childCount-1;
        tutorial_panels = new Image[arraycnt];

        for(int i=0;i<arraycnt;i++)
        {
            tutorial_panels[i] = transform.GetChild(arraycnt-1-i).GetComponent<Image>();
        }
        current_cnt = 0;

    }
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Fading(tutorial_panels[0], true, true));
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Touch()
    {
        if(canSwich)
        {
            if(current_cnt == arraycnt-1)
            {
                StartCoroutine(Fading(tutorial_panels[current_cnt], false,false,true));

            }
            StartCoroutine(Fading(tutorial_panels[current_cnt], false));
            current_cnt++;
        }
    }
    IEnumerator Fading(Image target, bool isShowing, bool isFirst = false, bool isFinal=false)
    {
        canSwich = false;
        float amount = 0;
        Color start_color = target.color;
        Color end_color;
        if (isShowing)
            end_color = new Color(255, 255, 255, 1);
        else
            end_color = new Color(255, 255, 255, 0);

        while (amount < 1)
        {

           target.color = Color.Lerp(start_color, end_color, amount);
            amount += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        target.color = Color.Lerp(start_color, end_color, 1);
        if(isFirst)
        {
            for(int i=1;i<arraycnt;i++)
            {
                tutorial_panels[i].color = new Color(255, 255, 255, 1);
            }
        }
        else if(isFinal)
        {
            if (isTrigger)
                Instantiate(startgame, Vector3.zero, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }
        canSwich = true;
        yield break;
    }
}
