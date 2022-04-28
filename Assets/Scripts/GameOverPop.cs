using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverPop : MonoBehaviour {

    public Image[] Buttons = new Image[2];
    public Text[] texts = new Text[2];
    AudioSource fxsound;
    public AudioClip clip;
    ImageEffects ie;
    // Use this for initialization
    private void Awake()
    {
        ie = GetComponent<ImageEffects>();
        fxsound = FindObjectOfType<AudioSource>();
    }
    void Start () {
        StartCoroutine(showing());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator showing()
    {
        yield return new WaitForSeconds(1.5f);

        for(int i=0;i<2;i++)
        {
            ie.Trigger_Showing(Buttons[i].gameObject,0.05f);
            ie.Trigger_Showing(texts[i],0.05f);
        }
        yield break;
    }

    public void ReLoadScene(bool isRegame)
    {
        fxsound.PlayOneShot(clip);
        if (isRegame)
        {
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SceneManager.LoadScene("SelectScene");
        }
    }
}
