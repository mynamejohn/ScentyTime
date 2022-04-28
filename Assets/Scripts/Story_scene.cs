using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using LitJson;

public class Story_scene : MonoBehaviour
{
    JsonData stage_info;
    string data;
    string dirpath;
    int stagenum;
    
    public Text story_title;
    string[] title = new string[4];

    public Image symbol_obj;
    public Sprite[] symbol_sprite = new Sprite[4];

    public Text story_text;
    public TextAsset[] story_file = new TextAsset[4];

    public GameObject blackpanel;
    public Transform boxsize;

    private void Awake()
    {
        dirpath = Application.persistentDataPath + "/tempDatas";
        data = File.ReadAllText(dirpath + "/stage_data.json");
        stage_info = JsonMapper.ToObject(data);

        stagenum = int.Parse(stage_info["CurrentStage"].ToString());

        title[0] = "첫번째 손님, 청년";
        title[1] = "두번째 손님, 꼬마";
        title[2] = "세번째 손님, 할머니";
        title[3] = "네번째 손님, ○○";
    }

    public void InitialSetting()
    {
        story_title.text = title[stagenum];
        symbol_obj.sprite = symbol_sprite[stagenum];
        story_text.text = story_file[stagenum].text;
    }
    // Use this for initialization
    void Start ()
    {
        InitialSetting();

        GameObject panel = Instantiate(blackpanel, transform);
        panel.GetComponent<TriggerFade>().isShowing = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void trigger_BacktoSelectScene()
    {
        StartCoroutine(BacktoSelectScene());
    }
    IEnumerator BacktoSelectScene()
    {
        GameObject panel = Instantiate(blackpanel, transform);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("SelectScene");
        yield break;
    }
}
