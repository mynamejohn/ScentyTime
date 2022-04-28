using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Notes : MonoBehaviour {

    public enum Path
    {
        LeftUp,
        Left,
        LeftDown,
        RightUp,
        Right,
        RightDown
    };

    public float distantFromButton = 0;
    public float Initialdistant = 0;
    public float speed;

    public GameObject[] Buttons = new GameObject[6];

    public Vector3 destination = new Vector3(0, 0, 0);
    public Vector3 startposition = new Vector3(0, -30, 0);
    public float t = 0;

    int direction;

    public Rigidbody2D rb;

    public Vector3 targetButton = new Vector3(0, 0, 0);

    public float distantX;
    public float distantY;
    public float speedX;
    public float speedY;

    PlayManager pm;
    private void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();

        for (int i = 0; i < 6; i++)
        {
            Buttons[i] = GameObject.Find("Touch_Area").transform.GetChild(i).gameObject;
        }
        pm = FindObjectOfType<PlayManager>();
    }
    void Start() {

    }

    void Update()
    {
        if (pm.isPlaying)
        {
            CalculateDistant();

            //transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 0.2f, transform.localRotation.w);
            //zRot += Time.deltaTime * 10;
            //transform.rotation = Quaternion.Euler(0, 0,10);
            if(direction>2)
                transform.Rotate(0, 0, 180 * Time.deltaTime, Space.Self);
            else
                transform.Rotate(0, 0, 180 * -Time.deltaTime, Space.Self);

            t += Time.deltaTime / 2.2f * speed;

            transform.localPosition = Vector3.Lerp(startposition, destination, t);
            //transform.Translate(new Vector2(speedX, speedY));
            //transform.Translate(speedX, speedY, 0, Space.Self);
            //transform.Translate(new Vector3(speedX, speedY));
        }
    }
    
    public void SetNote(int path, float notespeed)
    {
        direction = path;
        
        targetButton = Buttons[direction].transform.localPosition;
        Initialdistant = Vector2.Distance(new Vector2(0, 0), targetButton);
        destination = new Vector3(targetButton.x*2.0f, targetButton.y *2.0f);

        distantX = targetButton.x - transform.localPosition.x;
        distantY = targetButton.y - transform.localPosition.y;

        speedX = Time.deltaTime / 10;
        speedY = Time.deltaTime / 10;

        speed = notespeed;
        /*if(targetButton.x>0)
        {
            distantX = targetButton.x - transform.localPosition.x;
            if (targetButton.y>0)
            {
                distantY = targetButton.y - transform.localPosition.y;
            }
            else
            {
                distantY = transform.localPosition.y - targetButton.y;
            }
        }
        else
        {
            distantX = transform.localPosition.x - targetButton.x;
            if (targetButton.y > 0)
            {
                distantY = targetButton.y - transform.localPosition.y;
            }
            else
            {
                distantY = transform.localPosition.y - targetButton.y;
            }
        }*/
    }

    void CalculateDistant()
    {
        distantFromButton = Vector2.Distance(targetButton, gameObject.transform.localPosition);

    }

    public int CheckScore()
    {
        if (distantFromButton < 25)
        {
            //PM.AddScore(100);
            return 100;
        }
        else if (distantFromButton < 80)
        {
            //PM.AddScore(50);
            //Handheld.Vibrate();
            return 50;
        }
        else
            //PM.AddScore(0);
            return 0;
    }
}
