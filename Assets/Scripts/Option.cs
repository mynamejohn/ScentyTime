using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
public class Option : MonoBehaviour
{
    string dirpath;
    JsonData optiondata;
    string optiondata_str;

    float volume =1.0f;
    bool vibration = false;
    int notespeed=1;

    public Vector3 volume_start = new Vector3(-190, 0, 0);
    public Vector3 volume_end = new Vector3(190,0,0);
    public Transform volume_circle;

    public Transform[] vibration_check = new Transform[2];
    public Transform vibration_circle;

    public Image notespeed_number;
    public Sprite[] notespeed_sprite = new Sprite[4];

    public AudioSource audioHandle;
    AudioSource fxsound;
    public AudioClip fxclip;
    public GameObject Tutorial_obj;

    public GameObject cupeffects_parent;
    private void Awake()
    {
        dirpath = Application.persistentDataPath + "/tempDatas";

        optiondata_str = File.ReadAllText(dirpath + "/Option_data.json");
        optiondata = JsonMapper.ToObject(optiondata_str);
        audioHandle = GameObject.Find("Layout").GetComponent<AudioSource>();
        fxsound = GameObject.Find("Fxsound").GetComponent<AudioSource>();
        cupeffects_parent = GameObject.Find("Tea-cup-control");
        for (int i=0;i<4;i++)
        {
            cupeffects_parent.transform.GetChild(i).gameObject.SetActive(false);
        }
        
    }
    // Use this for initialization
    void Start ()
    {
        Setting_Initial();
    }
	
	// Update is called once per frame
	void Update ()
    {
        volume = (volume_circle.localPosition.x - volume_start.x) / (volume_end.x - volume_start.x);
        audioHandle.volume = volume;
    }

    public void Setting_Initial()
    {
        volume = float.Parse(optiondata["Option"]["Volume"].ToString());
        volume_circle.localPosition = Vector3.Lerp(volume_start, volume_end, volume);

        if (optiondata["Option"]["Vibration"].ToString() == "1")
        {
            vibration_circle.SetParent(vibration_check[0]);
            vibration_circle.localPosition = Vector3.zero;
        }
        else
        {
            vibration_circle.SetParent(vibration_check[1]);
            vibration_circle.localPosition = Vector3.zero;
        }

        notespeed = int.Parse(optiondata["Option"]["NoteSpeed"].ToString());
        notespeed_number.sprite = notespeed_sprite[notespeed - 1];
    }

    public void NotespeedSet(bool isUp)
    {
        fxsound.PlayOneShot(fxclip);
        if (isUp)
        {
            if (notespeed == 4)
                return;
            notespeed++;
        }
        else
        {
            if (notespeed == 1)
                return;
            notespeed--;
        }
        notespeed_number.sprite = notespeed_sprite[notespeed - 1];
        optiondata["Option"]["NoteSpeed"] = notespeed;
    }

    public void VibrationSet(bool isOn)
    {
        fxsound.PlayOneShot(fxclip);
        if (isOn)
        {
            vibration = true;
            vibration_circle.SetParent(vibration_check[0]);
            vibration_circle.localPosition = Vector3.zero;
            optiondata["Option"]["Vibration"] = 1;
            Handheld.Vibrate();
        }
        else
        {
            vibration = false;
            vibration_circle.SetParent(vibration_check[1]);
            vibration_circle.localPosition = Vector3.zero;
            optiondata["Option"]["Vibration"] = 0;
        }
    }

    public void SaveandExit()
    {
        fxsound.PlayOneShot(fxclip);
        optiondata["Option"]["Volume"] = volume;
        File.WriteAllText(dirpath + "/Option_data.json", optiondata.ToJson());
        for (int i = 0; i < 4; i++)
        {
            cupeffects_parent.transform.GetChild(i).gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }

    public void PopTutorial()
    {
        Instantiate(Tutorial_obj, Vector3.zero, Quaternion.identity, transform.parent);
        SaveandExit();
    }
}
