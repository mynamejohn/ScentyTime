using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
public class Option_Pause : MonoBehaviour
{
    string dirpath;
    JsonData optiondata;
    string optiondata_str;

    float volume =1.0f;
    bool vibration = false;
    int speedlevel=1;

    public Vector3 volume_start = new Vector3(-190, 0, 0);
    public Vector3 volume_end = new Vector3(190,0,0);
    public Transform volume_circle;

    public Transform[] vibration_check = new Transform[2];
    public Transform vibration_circle;

    public Image notespeed_number;
    public Sprite[] notespeed_sprite = new Sprite[4];

    PlayManager pm;
    AudioSource fxsound;
    public AudioClip fxclip;
    private void Awake()
    {
        dirpath = Application.persistentDataPath + "/tempDatas";

        optiondata_str = File.ReadAllText(dirpath + "/Option_data.json");
        optiondata = JsonMapper.ToObject(optiondata_str);
        pm = FindObjectOfType<PlayManager>();
        fxsound = GameObject.Find("PlayManager").GetComponent<AudioSource>();
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

        speedlevel = int.Parse(optiondata["Option"]["NoteSpeed"].ToString());
        notespeed_number.sprite = notespeed_sprite[speedlevel - 1];
    }

    public void NotespeedSet(bool isUp)
    {
        fxsound.PlayOneShot(fxclip);
        if (isUp)
        {
            if (speedlevel == 4)
                return;
            speedlevel++;
        }
        else
        {
            if (speedlevel == 1)
                return;
            speedlevel--;
        }
        notespeed_number.sprite = notespeed_sprite[speedlevel - 1];
        optiondata["Option"]["NoteSpeed"] = speedlevel;
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
    public void ReLoadScene(bool isRegame)
    {
        if (isRegame)
        {
            SaveandExit(false);
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SaveandExit(false);
            SceneManager.LoadScene("SelectScene");
        }
    }
    public void SaveandExit(bool isExit=true)
    {
        fxsound.PlayOneShot(fxclip);
        optiondata["Option"]["Volume"] = volume;
        File.WriteAllText(dirpath + "/Option_data.json", optiondata.ToJson());
        pm.Resume(volume, speedlevel,vibration);
        if(isExit)
            Destroy(gameObject);
    }
}
