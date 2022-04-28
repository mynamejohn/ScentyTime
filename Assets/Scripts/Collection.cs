using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
public class Collection : MonoBehaviour {

    int selected_number = 0;
    SelectManager sm;
    JsonData stage_data;
    ImageEffects ie;
    AudioSource fxsound;
    
    bool[] isCleared = new bool[6];
    bool ischanging = false;
    //선택창
    public Image[] selection_stage_texts = new Image[6];
    public Image[] selection_stage_tea_cup = new Image[6];
    public Sprite[] selection_stage_texts_sprite = new Sprite[6];
    public Sprite[] selection_stage_tea_cup_sprite = new Sprite[6];

    //상세창
    public GameObject story_button;
    public Image glass;
    public Image title;
    public Image clear_stamp;
    public Image detail_tea_cup;
    public Image detail_stage_texts;
    public Sprite none;

    public Sprite[] title_sprite = new Sprite[6];
    public Sprite[] detail_stage_texts_sprite = new Sprite[6];
    public Sprite[] detail_stage_texts_lock = new Sprite[6];
    public Sprite[] detail_tea_cup_sprite = new Sprite[6];
    public Sprite detail_tea_cup_lock;

    public GameObject teacup_selection;
    public GameObject details;
    public GameObject FX_stamp;
    public GameObject blackpanel;

    public AudioClip button_fx;

    private void Awake()
    {
        sm = FindObjectOfType<SelectManager>();
        ie = FindObjectOfType<ImageEffects>();
        stage_data = sm.getJsondata();
        fxsound = GameObject.Find("Fxsound").GetComponent<AudioSource>();

        InitialSetting();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select_Cup(int number)
    {
        selected_number = number;

        teacup_selection.SetActive(false);

        InitialDetailSetting();

        details.SetActive(true);

        fxsound.PlayOneShot(button_fx);
    }

    public void TriggerChange(bool isNext)
    {
        if (ischanging)
            return;
        fxsound.PlayOneShot(button_fx);
        if (isNext)
        {
            if (selected_number == 5)
                return;
            selected_number++;
        }

        else
        {
            if (selected_number == 0)
                return;
            selected_number--;
        }
        Fadeall();
        StartCoroutine(ChangeSetting());
    }

    public void BacktoSelection()
    {
        details.SetActive(false);
        teacup_selection.SetActive(true);
        fxsound.PlayOneShot(button_fx);
    }
    public void Close_panel()
    {
        fxsound.PlayOneShot(button_fx);
        Destroy(gameObject);
    }

    void InitialSetting()
    {
        //선책창 초기화
        for (int i = 0; i < 6; i++)
        {
            isCleared[i] = false;
        }
        for (int i = 0; i < 4; i++)
        {
            //클리어된 스테이지만 변경
            if (stage_data["index"][i]["isClear"].ToString() == "1")
            {
                selection_stage_texts[i].sprite = selection_stage_texts_sprite[i];
                selection_stage_tea_cup[i].sprite = selection_stage_tea_cup_sprite[i];
                isCleared[i] = true;
            }
        }
    }

    void InitialDetailSetting()
    {
        if (selected_number < 4)
        {
            if (stage_data["index"][selected_number]["isClear"].ToString() == "1")
            {
                clear_stamp.gameObject.SetActive(true);
                story_button.SetActive(true);
                glass.gameObject.SetActive(false);
                title.sprite = title_sprite[selected_number];
                detail_tea_cup.sprite = detail_tea_cup_sprite[selected_number];
                detail_stage_texts.sprite = detail_stage_texts_sprite[selected_number];
                Showing(true);
            }
            else
            {
                detail_tea_cup.sprite = detail_tea_cup_lock;
                detail_stage_texts.sprite = detail_stage_texts_lock[selected_number];
                title.gameObject.SetActive(false);
                clear_stamp.gameObject.SetActive(false);
                story_button.SetActive(false);
                glass.gameObject.SetActive(true);
                Showing(false);
            }
        }
        else
        {
            detail_tea_cup.sprite = detail_tea_cup_lock;
            detail_stage_texts.sprite = detail_stage_texts_lock[selected_number];
            title.gameObject.SetActive(false);
            clear_stamp.gameObject.SetActive(false);
            story_button.SetActive(false);
            glass.gameObject.SetActive(true);
            Showing(false);
        }
    }
    void Fadeall()
    {
        ie.Trigger_Fading(title.gameObject, 0.05f);
        ie.Trigger_Fading(clear_stamp.gameObject, 0.05f);
        ie.Trigger_Fading(glass.gameObject, 0.05f);
        ie.Trigger_Fading(detail_tea_cup.gameObject, 0.05f);
        ie.Trigger_Fading(detail_stage_texts.gameObject, 0.05f);
        ie.Trigger_Fading(story_button, 0.05f);
    }
    void Showing(bool isClear)
    {
        if(isClear)
        {
            ie.Trigger_Showing(detail_stage_texts.gameObject, 0.05f);
            ie.Trigger_Showing(detail_tea_cup.gameObject, 0.05f);
            ie.Trigger_Showing(title.gameObject, 0.05f);
            ie.Trigger_Showing(story_button, 0.05f);
            Showing_stamp();
        }
        else
        {
            ie.Trigger_Showing(detail_stage_texts.gameObject, 0.05f);
            ie.Trigger_Showing(detail_tea_cup.gameObject, 0.05f);
            ie.Trigger_Showing(glass.gameObject, 0.05f);
        }
    }

    void Showing_stamp()
    {
        ie.Trigger_Showing(clear_stamp.gameObject, 0.05f);
        Instantiate(FX_stamp, clear_stamp.gameObject.transform);
    }
    public void SelectStory()
    {
        string dirpath = Application.persistentDataPath + "/tempDatas";
        string data = File.ReadAllText(dirpath + "/stage_data.json");
        JsonData stagedata = JsonMapper.ToObject(data);

        stagedata["CurrentStage"] = selected_number;

        File.WriteAllText(dirpath + "/stage_data.json", stagedata.ToJson());

        fxsound.PlayOneShot(button_fx);
        StartCoroutine(StoryStep());
    }

    IEnumerator StoryStep()
    {
        Instantiate(blackpanel, transform.parent);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Story");
        yield break;
    }

    IEnumerator ChangeSetting()
    {
        ischanging = true;

        yield return new WaitForSeconds(0.5f);
        if (selected_number < 4)
        {
            if (stage_data["index"][selected_number]["isClear"].ToString() == "1")
            {
                title.sprite = title_sprite[selected_number];
                detail_tea_cup.sprite = detail_tea_cup_sprite[selected_number];
                detail_stage_texts.sprite = detail_stage_texts_sprite[selected_number];
                Showing(true);
            }
            else
            {
                detail_tea_cup.sprite = detail_tea_cup_lock;
                detail_stage_texts.sprite = detail_stage_texts_lock[selected_number];
                Showing(false);
            }

        }
        else
        {
            detail_tea_cup.sprite = detail_tea_cup_lock;
            detail_stage_texts.sprite = detail_stage_texts_lock[selected_number];
            Showing(false);
        }
        ischanging = false;

        yield break;
    }
}
