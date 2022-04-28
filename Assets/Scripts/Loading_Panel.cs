using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Panel : MonoBehaviour {

    Image control;
    public Image loading_bar;
    public GameObject loading_text;
    public Image complete_image;
    // Use this for initialization

    private void Awake()
    {
        control = GetComponent<Image>();
    }
    void Start ()
    {
        StartCoroutine(Loading_Steps());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Loading_Steps()
    {
        float amount = 0;

        while(amount<1)
        {
            control.fillAmount = amount;
            amount += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
        control.fillAmount = 1;

        loading_text.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("PlayScene");
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(0.01f);
            loading_bar.fillAmount = operation.progress;

            if (operation.progress >= 0.9f)
            {
                loading_bar.fillAmount = 1f;

                yield return new WaitForSeconds(1.0f);

                operation.allowSceneActivation = true;
            }
        }
        Destroy(gameObject);
    }
}
