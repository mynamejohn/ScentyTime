using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Playscene : MonoBehaviour {

    Image control;
    Transform layout;
    // Use this for initialization
    public GameObject start_text;
    public GameObject tutorial_obj;
    public Image grass_1;
    public Image grass_2;

    PlayManager pm;
    private void Awake()
    {
        control = GetComponent<Image>();
        layout = GameObject.Find("Layout").transform;
        pm = FindObjectOfType<PlayManager>();

    }
    void Start()
    {
        StartCoroutine(Panel_steps());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator Panel_steps()
    {
        yield return new WaitForSeconds(1.0f);

        float alpha = 0;

        while(alpha<1)
        {
            control.color = Color.Lerp(new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), alpha);
            grass_1.color = Color.Lerp(new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), alpha);
            grass_2.color = Color.Lerp(new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), alpha);
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        control.color = new Color(255, 255, 255, 0);
        if (pm.isfirst == 1)
        {
            Instantiate(tutorial_obj, Vector3.zero, Quaternion.identity, transform.parent);
            pm.Savedata();
        }
        else
        {
            GameObject start = Instantiate(start_text, start_text.transform.localPosition, Quaternion.identity, layout);
        }
        Destroy(gameObject);
}   }
