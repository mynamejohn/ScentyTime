using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;
public class Select_info
{
    public int maxrank;
    public int maxScore;

    public Select_info(int a=0, int b=0)
    {
        maxrank = a;
        maxScore = b;
    }
};

public class SelectManager : MonoBehaviour
{
    enum stages {stage1, stage2, stage3, stage4};

    public ImageEffects imageef;
    public Animator ani;

    public AudioSource BGsound;
    public AudioSource FXsound;
    public AudioClip fxsound_potpour;
    public AudioClip fxsound_button;

    public Transform Tea_cup_controler;
    bool Tea_cup_isMoving = false;
    public float movespeed = 0.01f;
    Vector3[] Tea_cup_pos = new Vector3[4];

    public Image[] Teacup_Image = new Image[4];
    public Sprite[] Teacup_sprite_Clear = new Sprite[4];
    public Sprite Teacup_sprite_Unlock;
    public Sprite Teacup_sprite_Lock;

    public Image Stage_Symbol;
    public Sprite[] symbol_sprite = new Sprite[4];
    public Image Stamp_image;

    public Image Score_Deco;
    public Image Score_Rank;
    public Sprite[] rank_sprite = new Sprite[6];

    public Image Score_Reward;
    public Sprite[] reward_sprite = new Sprite[4];

    public Image Song_Title;
    public Sprite[] songtitle_sprite = new Sprite[4];
    
    Select_info[] si = new Select_info [4];

    public Image Stage_Scribe_Top;
    public Sprite scribe_top_lock;
    public Sprite[] scribe_top_unlock = new Sprite[4];

    public Image Stage_Scribe_Bottom;
    public Sprite scribe_bottom_lock;
    public Sprite[] scribe_bottom_unlock = new Sprite[4];

    public Text maxScore_text;
    public Text maxScore_number;

    public int currentSelectedStage = 0 ;

    string dirpath;
    JsonData stage_data;
    public TextAsset initialdata;
    public TextAsset initialoption;
    JsonData optiondata;

    string data;

    public Sprite None;

    public GameObject loading_panel;

    public GameObject option;
    public GameObject tutorial_obj;
    public GameObject stamp_effect;
    public GameObject Potpour_effect;
    public GameObject pinkshine;
    public Transform etc_fx;
    public GameObject replay_panel;
    public GameObject collection_panel;

    bool testversion = true;

    private void Awake()
    {
        //dirpath = Application.persistentDataPath + "/tempData";
        dirpath = Application.persistentDataPath + "/tempDatas";
        DirectoryInfo di = new DirectoryInfo(dirpath);
        if (!di.Exists)
        {
            di.Create();
            stage_data = JsonMapper.ToObject(initialdata.text);
            File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());
            optiondata = JsonMapper.ToObject(initialoption.text);
            File.WriteAllText(dirpath + "/Option_data.json", optiondata.ToJson());
        }

        data = File.ReadAllText(dirpath + "/stage_data.json");
        stage_data = JsonMapper.ToObject(data);

        data = File.ReadAllText(dirpath + "/Option_data.json");
        optiondata = JsonMapper.ToObject(data);

        for (int i=0;i<4;i++)
        {
            si[i] = new Select_info(0, 0);
            Tea_cup_pos[i] = new Vector3(-(1150 * i), 0, 0);

            if (stage_data["index"][i]["isClear"].ToString() == "1")
            {
                Teacup_Image[i].sprite = Teacup_sprite_Clear[i];
            }
            else if (stage_data["index"][i]["isUnlock"].ToString() == "1")
            {
                Teacup_Image[i].sprite = Teacup_sprite_Unlock;
            }
            if (stage_data["index"][i]["CurrentScore"] !=null)
            {
                si[i].maxrank = int.Parse(stage_data["index"][i]["Rank"].ToString());
                si[i].maxScore = int.Parse(stage_data["index"][i]["CurrentScore"].ToString());
            }

        }
        ///////////////시연버전
        if (testversion)
        {
            Teacup_Image[2].sprite = Teacup_sprite_Lock;
            Teacup_Image[3].sprite = Teacup_sprite_Lock;

        }
        BGsound.volume = float.Parse(optiondata["Option"]["Volume"].ToString());

    }
    // Use this for initialization
    void Start ()
    {
        Song_Title.sprite = songtitle_sprite[currentSelectedStage];
        if (stage_data["index"][currentSelectedStage]["isUnlock"].ToString() == "0")        //잠긴상태
        {
            Stage_Symbol.sprite = None;
            Score_Rank.sprite = None;
            Score_Reward.sprite = None;

            //나의 최대기록 , 숫자
            maxScore_number.text = "";

            Stage_Scribe_Top.sprite = scribe_top_lock;
            Stage_Scribe_Bottom.sprite = scribe_bottom_lock;
        }
        else
        {
            if (stage_data["index"][currentSelectedStage]["isClear"].ToString() == "0")        //잠금 풀림 , 노클리어
            {
                Stage_Symbol.sprite = None;
                Score_Rank.sprite = None;
                Score_Reward.sprite = None;

                imageef.Trigger_Fading(maxScore_text);
                imageef.Trigger_Fading(maxScore_number);

                Stage_Scribe_Top.sprite = scribe_top_unlock[currentSelectedStage];
                Stage_Scribe_Bottom.sprite = scribe_bottom_unlock[currentSelectedStage];
            }
            else                                                                                //잠금 풀림, 클리어
            {
                Stage_Symbol.sprite = symbol_sprite[currentSelectedStage];
                Score_Reward.sprite = reward_sprite[currentSelectedStage];
                Score_Rank.sprite = rank_sprite[int.Parse(stage_data["index"][currentSelectedStage]["Rank"].ToString())];
                //나의 최대기록 , 숫자

                Stage_Scribe_Top.sprite = None;
                Stage_Scribe_Bottom.sprite = None;
                maxScore_number.text = stage_data["index"][currentSelectedStage]["CurrentScore"].ToString();
                imageef.Trigger_Showing(maxScore_text);
                imageef.Trigger_Showing(maxScore_number);
                imageef.Trigger_Showing(Stage_Symbol.gameObject);
                imageef.Trigger_Showing(Stamp_image.gameObject);
                imageef.Trigger_Showing(Score_Deco.gameObject);
            }
        }
        if(stage_data["Select_isFirst"].ToString() =="1")
        {
            Instantiate(tutorial_obj, Vector3.zero, Quaternion.identity, transform);
            stage_data["Select_isFirst"] = 0;
            File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());
        }
        StartCoroutine(FirstEffect());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SelectStage()
    {
        if (stage_data["index"][currentSelectedStage]["isUnlock"].ToString() == "0")
            return;

        //////////////시연버전
        if (testversion && currentSelectedStage >= 2)
            return;

        //imageef.Trigger_Fading(Stage_Symbol);
        StartCoroutine(LoadStage());
    }

    public void Teacup_moving(bool isLeft)
    {
        if (Tea_cup_isMoving)
            return;
        StartCoroutine(TeacupMoving(isLeft));
    }
    public void Trigger_change()
    {
        StartCoroutine(Changinginfo());
    }
    public void Config()
    {
        GameObject newConfig = Instantiate(option, option.transform.localPosition, Quaternion.identity, gameObject.transform);
    }

    public void FXcontrol(bool isActive)
    {
        for (int i = 0; i < 2; i++)
        {
            Tea_cup_controler.GetChild(currentSelectedStage).GetChild(i).gameObject.SetActive(isActive);
        }
            etc_fx.GetChild(0).gameObject.SetActive(isActive);

        //for(int i=0;i<4;i++)
        //{
        //    etc_fx.GetChild(i).gameObject.SetActive(isActive);
        //}
    }

    public void Replay_panel()
    {
        if (stage_data["index"][currentSelectedStage]["isClear"].ToString() == "0")
        {
            Debug.Log("거절");
            return;
        }
        FXsound.PlayOneShot(fxsound_button);
        Instantiate(replay_panel, transform);
    }
    public void Collection_panel()
    {
        FXsound.PlayOneShot(fxsound_button);
        Instantiate(collection_panel, transform);
    }
    public JsonData getJsondata()
    {
        return stage_data;
    }
    IEnumerator Changinginfo()
    {
        imageef.Trigger_Fading(Stamp_image.gameObject);
        imageef.Trigger_Fading(Stage_Symbol.gameObject);
        imageef.Trigger_Fading(Score_Rank.gameObject);
        imageef.Trigger_Fading(Song_Title.gameObject);
        imageef.Trigger_Fading(Score_Reward.gameObject);
        imageef.Trigger_Fading(Stage_Scribe_Top.gameObject);
        imageef.Trigger_Fading(Stage_Scribe_Bottom.gameObject);
        imageef.Trigger_Fading(maxScore_text);
        imageef.Trigger_Fading(maxScore_number);
        imageef.Trigger_Fading(Score_Deco.gameObject);

        yield return new WaitForSeconds(0.4f);

        Song_Title.sprite = songtitle_sprite[currentSelectedStage];
        if (stage_data["index"][currentSelectedStage]["isUnlock"].ToString() == "0")        //잠긴상태
        {
            Stage_Symbol.sprite = None; 
            Score_Rank.sprite = None;
            Score_Reward.sprite = None;
            //나의 최대기록 , 숫자
            maxScore_number.text = "";

            Stage_Scribe_Top.sprite = scribe_top_lock;
            Stage_Scribe_Bottom.sprite = scribe_bottom_lock;
        }
        else 
        {
            if (stage_data["index"][currentSelectedStage]["isClear"].ToString() == "0")        //잠금 풀림 , 노클리어
            {
                Stage_Symbol.sprite = None;
                Score_Rank.sprite = None;
                Score_Reward.sprite = None;

                Stage_Scribe_Top.sprite = scribe_top_unlock[currentSelectedStage];
                Stage_Scribe_Bottom.sprite = scribe_bottom_unlock[currentSelectedStage];
            }
            else                                                                                //잠금 풀림, 클리어
            {
                Stage_Symbol.sprite = symbol_sprite[currentSelectedStage];
                Score_Reward.sprite = reward_sprite[currentSelectedStage];
                Score_Rank.sprite = rank_sprite[int.Parse(stage_data["index"][currentSelectedStage]["Rank"].ToString())];
                //나의 최대기록 , 숫자

                Stage_Scribe_Top.sprite = None;
                Stage_Scribe_Bottom.sprite = None;
                maxScore_number.text = stage_data["index"][currentSelectedStage]["CurrentScore"].ToString();
                imageef.Trigger_Showing(maxScore_text);
                imageef.Trigger_Showing(maxScore_number);
                imageef.Trigger_Showing(Stamp_image.gameObject);
                imageef.Trigger_Showing(Score_Deco.gameObject);
                GameObject stamp = Instantiate(stamp_effect, Stamp_image.transform);
                stamp.transform.localPosition = Vector3.zero;
            }
        }

       /////////////////////////////////// //시연버전
        if (testversion && currentSelectedStage >= 2)
        {
            Stage_Symbol.sprite = None;
            Score_Rank.sprite = None;
            Score_Reward.sprite = None;
            //나의 최대기록 , 숫자
            maxScore_number.text = "";

            Stage_Scribe_Top.sprite = scribe_top_lock;
            Stage_Scribe_Bottom.sprite = scribe_bottom_lock;
        }

        imageef.Trigger_Showing(Stage_Symbol.gameObject);
        imageef.Trigger_Showing(Score_Rank.gameObject);
        imageef.Trigger_Showing(Song_Title.gameObject);
        imageef.Trigger_Showing(Score_Reward.gameObject);
        imageef.Trigger_Showing(Stage_Scribe_Top.gameObject);
        imageef.Trigger_Showing(Stage_Scribe_Bottom.gameObject);

        yield return new WaitForSeconds(0.4f);

        yield break;
    }
    //public void Draged_Stage(float amount)
    //{
    //    if (Tea_cup_controler.localPosition.x <=0 && Tea_cup_controler.localPosition.x>=-2910)
    //        Tea_cup_controler.localPosition.x 
    //}
    IEnumerator TeacupMoving(bool isLeft)
    {
        float percentage = 0;
        if (isLeft)
        {
            if (currentSelectedStage!=0)
            {
                Tea_cup_isMoving = true;
                currentSelectedStage--;
                Vector3 startpoint = Tea_cup_controler.localPosition;
                Trigger_change();
                do
                {
                    Tea_cup_controler.localPosition = Vector3.Lerp(startpoint, Tea_cup_pos[currentSelectedStage], percentage);
                    percentage += movespeed;
                    yield return new WaitForSeconds(0.0001f);
                } while (percentage < 1);
                Tea_cup_controler.localPosition = Vector3.Lerp(startpoint, Tea_cup_pos[currentSelectedStage], 1);
                /*while(Tea_cup_controler.localPosition.x < -(970*currentSelectedStage))
                {
                    Tea_cup_controler.localPosition = new Vector3(Tea_cup_controler.localPosition.x + movespeed, Tea_cup_controler.localPosition.y, Tea_cup_controler.localPosition.z);
                    yield return new WaitForSeconds(0.01f);
                }*/
                Tea_cup_isMoving = false;
            }
            else
                yield break ;

        }
        else
        {
            if (currentSelectedStage!=3)
            {
                Tea_cup_isMoving = true;
                currentSelectedStage++;
                Vector3 startpoint = Tea_cup_controler.localPosition;
                Trigger_change();
                do
                {
                    Tea_cup_controler.localPosition = Vector3.Lerp(startpoint, Tea_cup_pos[currentSelectedStage], percentage);
                    percentage += movespeed;
                    yield return new WaitForSeconds(0.0001f);
                } while (percentage < 1);
                Tea_cup_controler.localPosition = Vector3.Lerp(startpoint, Tea_cup_pos[currentSelectedStage], 1);
                Tea_cup_isMoving = false;
            }
            else
                yield break;
        }
        yield break;
    }
    IEnumerator LoadStage()
    {
        stage_data["CurrentStage"] = currentSelectedStage;
        File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());

        imageef.Trigger_Fading(Stamp_image.gameObject);
        imageef.Trigger_Fading(Stage_Symbol.gameObject);
        imageef.Trigger_Fading(Stage_Scribe_Top.gameObject);
        
        imageef.Trigger_Showing(pinkshine,0.01f);
        StartCoroutine(OffMusic());
        ani.SetTrigger("Tri");


        for (int i=0;i < 2;i++)
        {
            Tea_cup_controler.GetChild(currentSelectedStage).GetChild(i).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1.2f);
        //for (int i = 0; i < 4; i++)                                                                                                   //이펙트끄기 (보류)
        //{
        //    etc_fx.GetChild(i).gameObject.SetActive(false);
        //}
        GameObject pour = Instantiate(Potpour_effect, transform);
        pour.transform.localPosition = new Vector3(66, 177, 0);
        FXsound.PlayOneShot(fxsound_potpour);
        yield return new WaitForSeconds(2.0f);

        GameObject loading = Instantiate(loading_panel, loading_panel.transform.localPosition, Quaternion.identity, gameObject.transform);
        yield break;
    }

    IEnumerator OffMusic()
    {
        while(BGsound.volume>0)
        {
            BGsound.volume -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator FirstEffect()
    {
        yield return new WaitForSeconds(1.5f);
        if(stage_data["index"][currentSelectedStage]["isClear"].ToString() == "1")
        {
            GameObject stamp = Instantiate(stamp_effect, Stamp_image.transform);
            stamp.transform.localPosition = Vector3.zero;
        }
        yield break;
    }

    public int GetCurrentStage()
    {
        return currentSelectedStage;
    }
}
