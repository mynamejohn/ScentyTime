using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class Result_Setting : MonoBehaviour
{
    int stage;
    int rank;
    int currentscore;
    int maxcombo;
    int perfectcnt;
    int goodcnt;
    int misscnt;

    public Sprite[] Teacup_sprite = new Sprite[4];
    public Image Teacup;

    public Sprite[] Title_sprite = new Sprite[4];
    public Image Title;

    public Sprite[] Rank_sprite = new Sprite[6];
    public GameObject Rank;
    public Transform transform_Score_bg;
    public Transform tranform_Story_bg;
    public Transform transform_score_boundary;

    public Text[] texts = new Text[5];

    JsonData clear_info;
    string clear_info_textfile;

    JsonData stage_info;
    string stage_info_textfile;

    bool isSkip = false;
    string dirpath;

    public GameObject black_panel;
    public GameObject Fx_clear;
    private void Awake()
    {
        //json객체생성
        dirpath = Application.persistentDataPath + "/tempDatas";
        clear_info_textfile = File.ReadAllText(dirpath + "/current_clear.json");
        clear_info = JsonMapper.ToObject(clear_info_textfile);

        stage_info_textfile = File.ReadAllText(dirpath + "/stage_data.json");
        stage_info = JsonMapper.ToObject(stage_info_textfile);

        stage = int.Parse(clear_info["Stage"].ToString());
        rank = int.Parse(clear_info["Rank"].ToString());
        currentscore = int.Parse(clear_info["CurrentScore"].ToString());
        maxcombo = int.Parse(clear_info["MaxCombo"].ToString());
        perfectcnt = int.Parse(clear_info["Perfect"].ToString());
        goodcnt = int.Parse(clear_info["Good"].ToString());
        misscnt = int.Parse(clear_info["Miss"].ToString());
        
        Teacup.sprite = Teacup_sprite[stage];
        Title.sprite = Title_sprite[stage];
    }
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(resultevent());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PageDown()
    {
        StartCoroutine(Pagemoving(true));
    }
    public void PageUp()
    {
        StartCoroutine(Pagemoving(false));
    }

    public void Skip()
    {
        if (!isSkip)
            isSkip = true;
        else
        {
            StartCoroutine(Return_to_Select());
        }
    }

    IEnumerator resultevent()
    {
        StartCoroutine(FX_clear());
        GameObject panel = Instantiate(black_panel, Vector3.zero, Quaternion.identity, transform_Score_bg);
        panel.GetComponent<TriggerFade>().isShowing = false;
        yield return new WaitForSeconds(2.0f);
        Debug.Log(currentscore);
        StartCoroutine(Scoregoingup(texts[0], currentscore, true));
        StartCoroutine(Scoregoingup(texts[1], maxcombo));
        StartCoroutine(Scoregoingup(texts[2], perfectcnt));
        StartCoroutine(Scoregoingup(texts[3], goodcnt));
        StartCoroutine(Scoregoingup(texts[4], misscnt));

        yield return new WaitUntil(() => isSkip);

        GameObject rankimg = Instantiate(Rank, Rank.transform.position, Quaternion.identity, transform_score_boundary);
        rankimg.GetComponent<Image>().sprite = Rank_sprite[rank];

        yield break;
    }
    IEnumerator Scoregoingup(Text textobj, int max, bool trigger=false)
    {
        int currentnum = 0;
        while (currentnum <= max)
        {
            if(isSkip)
            {
                textobj.text = max.ToString();
                break;
            }
            textobj.text = currentnum.ToString();
            if (max > 1000)
                currentnum += 5;
            else
                currentnum +=1;
            yield return new WaitForSeconds(0.0001f);
        }
        if (trigger)
            isSkip = true;
        yield break;
    }

    IEnumerator Return_to_Select()
    {
        GameObject panel = Instantiate(black_panel, Vector3.zero, Quaternion.identity, transform_Score_bg);
        panel.GetComponent<TriggerFade>().isShowing = true;

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Story");
    }
    IEnumerator Pagemoving(bool isUp)
    {
        float percentage = 0;
        if(isUp)
        {
            Vector3 start_story = tranform_Story_bg.localPosition;
            Vector3 destination_story = Vector3.zero;
            Vector3 start_score = Vector3.zero;
            Vector3 destination_score = new Vector3(transform_Score_bg.localPosition.x, transform_Score_bg.localPosition.y + 800, 0);

            while(percentage<1)
            {
                tranform_Story_bg.localPosition = Vector3.Lerp(start_story, destination_story, percentage);
                transform_Score_bg.localPosition = Vector3.Lerp(start_score, destination_score, percentage);
                percentage += 0.05f;
                yield return new WaitForSeconds(0.01f);
                Debug.Log(percentage);
            }
            tranform_Story_bg.localPosition = Vector3.Lerp(start_story, destination_story, 1);
            transform_Score_bg.localPosition = Vector3.Lerp(start_score, destination_score, 1);
        }
        else
        {
            Vector3 start_score = transform_Score_bg.localPosition;
            Vector3 destination_score = Vector3.zero;
            Vector3 start_story = Vector3.zero;
            Vector3 destination_story = new Vector3(tranform_Story_bg.localPosition.x, tranform_Story_bg.localPosition.y - 800, 0);

            while (percentage < 1)
            {
                tranform_Story_bg.localPosition = Vector3.Lerp(start_story, destination_story, percentage);
                transform_Score_bg.localPosition = Vector3.Lerp(start_score, destination_score, percentage);
                percentage += 0.05f;
                yield return new WaitForSeconds(0.01f);
                Debug.Log(percentage);
            }
            tranform_Story_bg.localPosition = Vector3.Lerp(start_story, destination_story, 1);
            transform_Score_bg.localPosition = Vector3.Lerp(start_score, destination_score, 1);

        }
        yield break;
    }

    IEnumerator FX_clear()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject fx = Instantiate(Fx_clear, gameObject.transform);
        fx.transform.localPosition = new Vector3(0, 650, 0);
        yield break;
    }
}
