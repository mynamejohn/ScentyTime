using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCleaner : MonoBehaviour {

    public PlayManager PM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Notes")
        {
            PM.AddScore(0);
            Destroy(collision.gameObject);
        }
    }
}
