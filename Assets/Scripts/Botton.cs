using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botton : MonoBehaviour {

    public MusicPlayer MP;
    public KeyCode k;
    public bool activeButton = false;
    public GameObject nextNote=null;
    public PlayManager PM;
    public GameObject[] notes = new GameObject[10];
    public int NotesCount = 0;

    public GameObject touch_effect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(k)&&nextNote!=null)
        {
            nextNote.GetComponent<Notes>().CheckScore();
            Destroy(nextNote);
        }
        //if (Input.GetKeyDown(k))
          //  Debug.Log(MP.AS.time);
	}

    public void Button_Touch()
    {
        GameObject effect = Instantiate(touch_effect,gameObject.transform.position, Quaternion.identity);
        effect.transform.SetParent(gameObject.transform);
        effect.transform.localScale = new Vector3(1, 1, 1);
        effect.transform.localPosition = gameObject.transform.position;

        if (nextNote!=null)
        {
            int score = nextNote.GetComponent<Notes>().CheckScore();
            PM.AddScore(score);
            Destroy(nextNote);
            nextNote = null;
            activeButton = false;
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(nextNote)
        {
            return;
        }
        activeButton = true;
        //if (collision.gameObject.tag == "Notes")
            nextNote = collision.gameObject;
    }*/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (nextNote)
            return;
        activeButton = true;
        nextNote = collision.gameObject;    

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        activeButton = false;
        nextNote = null;
    }
}
