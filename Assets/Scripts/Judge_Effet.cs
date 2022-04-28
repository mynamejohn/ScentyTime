using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge_Effet : MonoBehaviour {
    Transform handle;
    public float max = 1;
    private void Awake()
    {
        handle = gameObject.transform;
    }
    // Use this for initialization
    void Start () {

        StartCoroutine(Scaler());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Scaler()
    {   
        float currentscale = 0.0f;
        while (currentscale < max)
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
        Destroy(gameObject);
        yield break;
    }
}
