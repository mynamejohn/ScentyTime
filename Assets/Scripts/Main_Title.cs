using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class Main_Title : MonoBehaviour {

    public Image Panel;

    public AudioSource audiohandle;
    public AudioSource fxsound;
    public VideoPlayer videohandler;
    public float waittingtime = 0;
    bool isplayingvideo = false;
    bool ischanging = false;
    //public Text start_text;


    // Use this for initialization
    private void Awake()
    {
        videohandler.waitForFirstFrame = false;
        videohandler.Prepare();
    }
    void Start ()
    {
        StartCoroutine(ShowingBlackPanel(false));
        StartCoroutine(VolumeFade(true));
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*if (!isplayingvideo)
        {
            waittingtime += Time.deltaTime;

            if (waittingtime > 25)
            {
                Turn_to_Video();
            }
        }
        else
        {
            if(!videohandler.isPlaying)
            {
                videohandler.Stop();
                Turn_to_Title();
            }
        }*/

    }

    public void Turn_to_Video()
    {
        StartCoroutine(VolumeFade(false));
        waittingtime = 0;
        isplayingvideo = true;
        fxsound.Stop();
        videohandler.Play();
    }

    public void Turn_to_Title()
    {
        isplayingvideo = false;
        StartCoroutine(ShowingBlackPanel(false));
        StartCoroutine(VolumeFade(true));
        fxsound.Play();
    }

    public void SkipVideo()
    {
        if (videohandler.isPlaying)
        {
            videohandler.Stop();
            ischanging = true;
            StartCoroutine(ToSelectScene());
        }
        else
        {
            Turn_to_Video();
        }
    }

    IEnumerator VolumeFade(bool isOn)
    {
        if(isOn)
        {
            audiohandle.Play();
            float amount = 0;
            while (amount < 1)
            {
                audiohandle.volume = amount;
                amount += 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
            audiohandle.volume = 1f;
            yield break;
        }
        else
        {
            float amount = 1;
            while (amount >0)
            {
                audiohandle.volume = amount;
                amount -= 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
            audiohandle.volume = 0f;
            audiohandle.Stop();
            yield break;
        }
    }
    IEnumerator ToSelectScene()
    {
        StartCoroutine(ShowingBlackPanel(true));
        StartCoroutine(VolumeFade(false));
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("SelectScene");
        yield break;
    }
    IEnumerator ShowingBlackPanel(bool isShowing)
    {
        float alpha = 0;
        if (isShowing)
        {
            while (alpha < 1)
            {
                Panel.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), alpha);
                alpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            Panel.color = new Color(0, 0, 0, 1);
        }
        else
        {
            while (alpha < 1)
            {
                Panel.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), alpha);
                alpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            Panel.color = new Color(0, 0, 0, 0);
        }
        yield break;
    }
}
