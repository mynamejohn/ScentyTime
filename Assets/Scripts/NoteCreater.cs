using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;

public class NoteCreater : MonoBehaviour {


    JsonData JD;

    public MusicPlayer MP;

    public float musicTime;

    public float deltaCreatNote;

    public int ObjectCnt;
    public int actualNoteCnt;

    public float hitTime;

    public GameObject Note;

    public GameObject ParantNote;
    public PlayManager PM;

    public AudioClip[] stages_song = new AudioClip[4];
    public TextAsset[] stages_note = new TextAsset[4];

    float notespeed = 1;
    int total_notes = 0;

    private void Awake()
    {
        ParantNote = gameObject;
    }
    void Start ()
    {
        
    }
	
	void Update ()
    {
        musicTime = MP.AS.time;
	}

    public void LoadNotes(float nsp)
    {
        JD = JsonMapper.ToObject(stages_note[PM.stagenum].text);

        ObjectCnt = JD["Notes"].Count;
        notespeed = nsp;
        StartCoroutine(CreatNote(1.0f));

    }
    public void Stop()
    {
        MP.AS.Stop();
        Debug.Log("끗");
        PM.EndSong(total_notes);
    }
    public void Resume(float newspeed)
    {
        notespeed = newspeed;
    }
    IEnumerator CreatNote(float StreamingSec)
    {
        MP.PlayMusic(stages_song[PM.stagenum]);

        for (int i = 0; i < ObjectCnt; i++)
        {
            actualNoteCnt = JD["Notes"][i]["Index"].Count;
            hitTime = float.Parse(JD["Notes"][i]["HitTime"].ToString());

            yield return new WaitUntil(() => musicTime >= hitTime-1f);

            for (int j = 0; j < actualNoteCnt; j++)
            {
                yield return new WaitUntil(() => PM.isPlaying);
                GameObject newNote = Instantiate(Note, new Vector3(0,-30,0), Quaternion.identity);
                newNote.transform.SetParent(ParantNote.transform);
                newNote.transform.localScale = new Vector3(1,1,1);
                newNote.transform.localPosition = new Vector3(0, -30, 0);

                newNote.GetComponent<Notes>().SetNote(int.Parse(JD["Notes"][i]["Index"][j]["Path"].ToString()), notespeed);
                total_notes++;
            }
        }

        yield return new WaitUntil(()=> !MP.AS.isPlaying);
        Debug.Log("끗");
        PM.EndSong(total_notes);
    }
}
