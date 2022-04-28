using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class Score_info
{
    public int Stage;
    public int Rank;
    public int CurrentScore;
    public int MaxCombo;
    public int Perfect;
    public int Good;
    public int Miss;
    public Score_info(int a,int b,int c,int d,int e,int f,int g)
    {
        Stage = a;
        Rank = b;
        CurrentScore = c;
        MaxCombo = d;
        Perfect = e;
        Good = f;
        Miss = g;
    }
};
public class Option_info
{
    public float volume;
    public int vibration;
    public int notespeed;

    public Option_info(float vol, int vib, int notesp)
    {
        volume = vol;
        vibration = vib;
        notespeed = notesp;
    }
}
public class PlayManager : MonoBehaviour
{
    public MusicPlayer MP;
    public NoteCreater NC;

    public Text ScoreText;
    public int CurrentScore = 0;
    int maxcombo, currentcombo, perfectcnt, goodcnt, misscnt = 0;

    public Text combo_text;

    public Image[] Healthbar = new Image[2];
    public float HealthScore = 100.0f;

    public Image FeverGage;
    public float FeverScore = 0;
    public int FeverBlock = 0;

    public bool isDecreasing = false;
    public GameObject FeverGageLight;
    Vector2 FeverGageLightStart;
    Vector2 FeverGageLightEnd;
    public GameObject FeverEffect;
    public GameObject FeverEffect_particle;
    public GameObject[] FeverBlockObject = new GameObject[4];
    public Image feverImage;
    public Sprite[] feverImage_sprite = new Sprite[4];


    bool judge_isScaling = false;
    Coroutine judge_handle;
    public GameObject judge_obj;
    public Transform Judge_Parent;
    public Sprite[] judge_sprite = new Sprite[3];

    public Image[] FeverGrid = new Image[4];

    public GameObject ClearEffect;
    public GameObject ClearImg;
    public GameObject Clearpanel;
    public GameObject BlackPanel;

    public GameObject Layout;
    string dirpath;

    int FeverLevel = 1;
    public bool isFeverTime = false;

    public AudioSource audiohandle;
    public AudioSource fxaudio;
    public AudioClip clearsound;

    public bool isPlaying = false;

    public GameObject Fever_star;
    public GameObject Fever_Cloud;
    public int stagenum = 0;

    Score_info sci = new Score_info(0, 0, 0, 0, 0, 0, 0);
    JsonData score_info;
    public JsonData stage_data;
    JsonData option_data;
    string data;

    public Option_info opin;
    public int isVibration = 0;
    public float notespeeds = 1;

    public GameObject pause_object;
    public GameObject GameOver_prefeb;

    public int isfirst;

    public AudioClip fxsound_fever;
    public AudioClip fxsound_pause;
    public AudioClip fxsound_gaveover;

    public bool overwhelming = false;
    // Use th   is for initialization  
    private void Awake()
    {
        //File.WriteAllText(Application.dataPath + "/test.json", score_info.ToString());

        dirpath = Application.persistentDataPath + "/tempDatas";
        DirectoryInfo di = new DirectoryInfo(dirpath);
        if (!di.Exists)
        {
            di.Create();
        }

        data = File.ReadAllText(dirpath + "/stage_data.json");
        stage_data = JsonMapper.ToObject(data);

        data = File.ReadAllText(dirpath + "/Option_data.json");
        option_data = JsonMapper.ToObject(data);

        stagenum = int.Parse(stage_data["CurrentStage"].ToString());

        FeverGageLightStart = FeverGageLight.transform.localPosition;

        FeverGageLightEnd = new Vector2(-FeverGageLightStart.x, FeverGageLightStart.y);

        opin = new Option_info(float.Parse(option_data["Option"]["Volume"].ToString()), int.Parse(option_data["Option"]["Vibration"].ToString()), int.Parse(option_data["Option"]["NoteSpeed"].ToString()));

        audiohandle.volume = opin.volume;
        isVibration = opin.vibration;

        switch (opin.notespeed)
        {
            case 1:
                notespeeds = 1;
                break;
            case 2:
                notespeeds = 1.2f;
                break;
            case 3:
                notespeeds = 1.3f;
                break;
            case 4:
                notespeeds = 1.4f;
                break;
            default:
                notespeeds = 1f;
                break;
        }
        isfirst = int.Parse(stage_data["Play_isFirst"].ToString());
    }
    void Start ()
    {

        //StartGame();

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            FeverGage.fillAmount = (float)FeverScore / 100f;
            if (FeverScore >= 100)
            {
                if (FeverBlock < 4)
                {
                    if (!isDecreasing)
                    {
                        StartCoroutine(Fever_Scaler(FeverBlockObject[FeverBlock]));
                        StartCoroutine(FeverScore_Dncrease(100));
                        FeverBlock++;
                    }
                }
                else
                {
                    for(int i=0;i<4;i++)
                    {
                        StartCoroutine(Fever_deScaler(FeverBlockObject[i]));
                    }
                    FeverBlock = 0;
                    isFeverTime = true;
                    FeverTime();
                }
            }
            ScoreText.text = CurrentScore.ToString();
            if (HealthScore > 0)
            {
                HealthScore -= 0.05f;
                Healthbar[0].fillAmount = (float)HealthScore / 100f;
                Healthbar[1].fillAmount = (float)HealthScore / 100f;
            }
            FeverGageLight.transform.localPosition = Vector2.Lerp(FeverGageLightStart, FeverGageLightEnd, FeverGage.fillAmount);
            combo_text.text = currentcombo.ToString();
            if (HealthScore <= 0)
                GameOver();
        }
        if (overwhelming)
            HealthScore = 100;

    }

    public void FeverTime()
    {
        switch(FeverLevel)
        {
            case 1:
                Fever_star.SetActive(true);
                Fever_Cloud.SetActive(true);

                FeverLevel = 2;
                feverImage.sprite = feverImage_sprite[1];
                break;
            case 2:
                FeverLevel = 3;
                feverImage.sprite = feverImage_sprite[2];
                break;
            case 4:
                FeverLevel = 4;
                feverImage.sprite = feverImage_sprite[3];
                break;
            default:
                FeverLevel = 4;
                break;
        }
        fxaudio.PlayOneShot(fxsound_fever);
        StartCoroutine(FeverScore_Dncrease(100));
        Instantiate(FeverEffect_particle, Vector3.zero, Quaternion.identity, transform);
    }

    public void AddScore(int score)
    {
        if (score == 100)
        {
            perfectcnt++;
            ComboIncrease();
            if(judge_isScaling)
            {
                StopCoroutine(judge_handle);
            }
            judge_obj.GetComponent<Image>().sprite = judge_sprite[0];
            judge_handle = StartCoroutine(Judge_Scaler(judge_obj.transform));

        }
        else if (score == 50)
        {
            goodcnt++;
            ComboIncrease();
            if (judge_isScaling)
            {
                StopCoroutine(judge_handle);
            }
            judge_obj.GetComponent<Image>().sprite = judge_sprite[1];
            judge_handle = StartCoroutine(Judge_Scaler(judge_obj.transform));

        }
        else if (score == 0)
        {
            misscnt++;
            ComboReset();
            isFeverTime = false;
            FeverLevel = 1;
            HealthScore -= 10;
            feverImage.sprite = feverImage_sprite[0];
            Fever_star.SetActive(false);
            Fever_Cloud.SetActive(false);
            if (judge_isScaling)
            {
                StopCoroutine(judge_handle);
            }
            judge_obj.GetComponent<Image>().sprite = judge_sprite[2];
            judge_handle = StartCoroutine(Judge_Scaler(judge_obj.transform));

            if (isVibration == 0)
                return;
            Handheld.Vibrate();
        }

        CurrentScore += score * FeverLevel;

        StartCoroutine(FeverScore_Increase((score/10)));

        HealthScore += score / 10f;
        if (HealthScore > 100)
            HealthScore = 100;

        return;

    }
    public void ComboIncrease()
    {
        currentcombo++;
        if (currentcombo >= maxcombo)
            maxcombo = currentcombo;
        return;
    }
    public void ComboReset()
    {
        currentcombo = 0;
    }

    public void StartGame()
    {
       NC.LoadNotes(notespeeds);
       isPlaying = true;
    }
    
    public void GameOver()
    {
        MP.AS.Stop();
        isPlaying = false;
        GameObject gameover = Instantiate(GameOver_prefeb, GameOver_prefeb.transform.localPosition, Quaternion.identity,Layout.transform);
        fxaudio.PlayOneShot(fxsound_gaveover);
    }

    public void skipping()
    {
        EndSong(1);
    }
    public void EndSong(int actualNoteCnt)
    {
        StartCoroutine(Clear(actualNoteCnt));
    }
    
    public void Pause()
    {
        audiohandle.Pause();
        fxaudio.PlayOneShot(fxsound_pause);
        isPlaying = false;
        GameObject newobj = Instantiate(pause_object, Vector3.zero, Quaternion.identity, Layout.transform);
    }
    public void Savedata()
    {
        stage_data["Play_isFirst"] = 0;

        File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());
    }
    public void Resume(float newvol, int speedlvl, bool vibrat)
    {
        audiohandle.Play();
        isPlaying = true;

        audiohandle.volume = newvol;
        if (vibrat)
            isVibration = 1;
        else
            isVibration = 0;
        switch (speedlvl)
        {
            case 1:
               notespeeds = 1;
                break;
            case 2:
                notespeeds = 1.2f;
                break;
            case 3:
                notespeeds = 1.3f;
                break;
            case 4:
                notespeeds = 1.4f;
                break;
            default:
                notespeeds = 1f;
                break;
        }

        NC.Resume(notespeeds);
    }
    IEnumerator Scaler(GameObject Judge)
    {
        GameObject ScoreImage = Instantiate(Judge, new Vector3(0, 0, 0), Quaternion.identity);
        ScoreImage.transform.SetParent(Judge_Parent);
        ScoreImage.transform.localScale = new Vector3(0, 0, 0);
        ScoreImage.transform.localPosition = new Vector3(0, 180, 0);

        /*float f = 0;
        while (f < 1)
        {
            ScoreImage.transform.localScale = new Vector3(f, f, f);
            f = f + 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1.5f);

        Destroy(ScoreImage);*/
        yield break;
    }
    IEnumerator Fever_Scaler(GameObject obj)
    {
        float f = 0;
        int cnt = obj.transform.childCount;
        
        for(int i=0;i<cnt;i++)
        {
            Destroy(obj.transform.GetChild(i));
        }
        GameObject effect = Instantiate(FeverEffect, new Vector3(0, 0, 0), Quaternion.identity, obj.transform);
        effect.transform.localScale = new Vector3(1, 1, 1);
        while (f < 1)
        {
            obj.transform.localScale = new Vector3(f, f, f);
            f = f + 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
    IEnumerator Fever_deScaler(GameObject obj)
    {
        float f = 1;
        while (f > 0)
        {
            obj.transform.localScale = new Vector3(f, f, f);
            f = f - 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    IEnumerator FeverScore_Increase(int score)
    {
        for(int i=0;i<score;i++)
        {
            yield return new WaitWhile(() => isDecreasing);
            FeverScore++;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
    IEnumerator FeverScore_Dncrease(int score)
    {
        isDecreasing = true;
        for (int i = 0; i < score; i++)
        {
            FeverScore--;
            yield return new WaitForSeconds(0.01f);
        }
        isDecreasing = false;
    }
    IEnumerator Clear(int actualNoteCnt)
    {
        isPlaying = false;

        int EntirenotesCnt = actualNoteCnt;

        int score_rank = EntirenotesCnt-(int)(goodcnt*0.1)- misscnt;

        float percent = (score_rank / EntirenotesCnt) * 100;
        
        if (CurrentScore == 0)
            CurrentScore = 100;

        float Percentage = (score_rank / EntirenotesCnt) * 100;

        if (Percentage > 95)
        {
            sci.Rank = 0;
        }
        else if (Percentage>90)
        {
            sci.Rank = 1;
        }
        else if(Percentage>80)
        {
            sci.Rank = 2;
        }
        else if(Percentage>70)
        {
            sci.Rank = 3;
        }
        else if(Percentage > 60)
        {
            sci.Rank = 4;
        }
        else
        {
            sci.Rank = 5;
        }

        sci.Stage = stagenum;
        sci.CurrentScore = CurrentScore;
        sci.MaxCombo = maxcombo;
        sci.Perfect = perfectcnt;
        sci.Good = goodcnt;
        sci.Miss = misscnt;

        score_info = JsonMapper.ToJson(sci);
        File.WriteAllText(dirpath + "/current_clear.json", score_info.ToString());

        if(stage_data["index"][stagenum]["isClear"].ToString() == "0")                                                                      //최초클리어
        {
            stage_data["index"][stagenum]["isClear"] = 1;
            stage_data["index"][stagenum]["Rank"] = sci.Rank;
            stage_data["index"][stagenum]["CurrentScore"] = sci.CurrentScore;

            if (stagenum != 3)
            {
                stage_data["index"][stagenum + 1]["isUnlock"] = 1;
            }
            File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());
            Debug.Log("최초저장");
        }
        else if (sci.CurrentScore > int.Parse(stage_data["index"][stagenum]["CurrentScore"].ToString()))                                     //최대기록갱신
        {
            stage_data["index"][stagenum]["Rank"] = sci.Rank;
            stage_data["index"][stagenum]["CurrentScore"] = sci.CurrentScore;
            if (stagenum != 3)
            {
                stage_data["index"][stagenum + 1]["isUnlock"] = 1;
            }
            File.WriteAllText(dirpath + "/stage_data.json", stage_data.ToJson());
            Debug.Log("기록갱신");
        }

        audiohandle.PlayOneShot(clearsound); 

        GameObject Clear_panel = Instantiate(Clearpanel, Clearpanel.transform.position, Quaternion.identity, Layout.transform);
        Clear_panel.transform.localScale = new Vector3(1, 1, 1);
        Clear_panel.transform.localPosition = Vector3.zero;

        GameObject end_effect = Instantiate(ClearEffect, ClearEffect.transform.position, Quaternion.identity);
        end_effect.transform.SetParent(gameObject.transform);
        end_effect.transform.localPosition = new Vector3(0, 10, 0);

        yield return new WaitForSeconds(4.5f);


        GameObject Clear_image = Instantiate(ClearImg, ClearImg.transform.position, Quaternion.identity, Layout.transform);
        Clear_image.transform.localScale = new Vector3(1, 1, 1);
        Clear_image.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(2.5f);

        GameObject Black_Panel = Instantiate(BlackPanel, BlackPanel.transform.position, Quaternion.identity, Layout.transform);
        Clear_image.transform.localScale = new Vector3(1, 1, 1);
        Clear_image.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Result");
        yield break;
    }

    IEnumerator Judge_Scaler(Transform handle)
    {
        judge_isScaling = true;

        float currentscale = 0.0f;
        while (currentscale < 1.5)
        {
            handle.localScale = new Vector3(currentscale, currentscale, currentscale);
            currentscale += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.3f);

        while (currentscale > 0)
        {
            handle.localScale = new Vector3(currentscale, currentscale, currentscale);
            currentscale -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        judge_isScaling = false;
        yield break;
    }


}
