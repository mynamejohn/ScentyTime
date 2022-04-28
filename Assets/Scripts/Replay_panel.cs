using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class Replay_panel : MonoBehaviour {

    SelectManager sm;
    int selectedstage;
    AudioSource fxsound;

    public Image reward_obj;
    public Sprite[] reward_sprite = new Sprite[4];

    public Text stage_text;
    string[] whatstage = new string[4];

    public GameObject blackpanel;
    public AudioClip fxsound_clip;
    private void Awake()
    {
        fxsound = GameObject.Find("Fxsound").GetComponent<AudioSource>();
        sm = FindObjectOfType<SelectManager>();

        selectedstage = sm.currentSelectedStage;

        whatstage[0] = "첫 번째 손님";
        whatstage[1] = "두 번째 손님";
        whatstage[2] = "세 번째 손님";
        whatstage[3] = "네 번째 손님";

        stage_text.text = whatstage[selectedstage];
        reward_obj.sprite = reward_sprite[sm.currentSelectedStage];
    }
    // Use this for initialization
    void Start ()
    {
        sm.FXcontrol(false);		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public void SelectExit()
    {
        sm.FXcontrol(true);
        fxsound.PlayOneShot(fxsound_clip);
        Destroy(gameObject);
    }

    public void SelectStory()
    {
        string dirpath = Application.persistentDataPath + "/tempDatas";
        string data = File.ReadAllText(dirpath + "/stage_data.json");
        JsonData stagedata = JsonMapper.ToObject(data);

        stagedata["CurrentStage"] = selectedstage;

        File.WriteAllText(dirpath + "/stage_data.json",stagedata.ToJson());

        fxsound.PlayOneShot(fxsound_clip);
        StartCoroutine(StoryStep());
    }

    IEnumerator StoryStep()
    {
        Instantiate(blackpanel, transform.parent);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Story");
        yield break;
    }
}
