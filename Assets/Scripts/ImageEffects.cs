using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageEffects : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger_Fading(GameObject targetobj,float amount = 0.1f)
    {
        StartCoroutine(Fading(targetobj,amount));
    }
    public void Trigger_Fading(Text targetobj, float amount = 0.1f)
    {
        StartCoroutine(Fading(targetobj, amount));
    }
    public void Trigger_Showing(GameObject targetobj, float amount = 0.1f)
    {
        StartCoroutine(Showing(targetobj, amount));
    }
    public void Trigger_Showing(Text targetobj, float amount = 0.1f)
    {
        StartCoroutine(Showing(targetobj, amount));
    }

    IEnumerator Showing(GameObject targetobj, float amount)
    {
        targetobj.SetActive(true);
        Color tempcolor = targetobj.GetComponent<Image>().color;
        float f = 0;

        while (true)
        {
            if (f >= 1)
                break;
            targetobj.GetComponent<Image>().color = Color.Lerp(tempcolor, new Color(255, 255, 255, 1), f);
            f += amount;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
    IEnumerator Showing(Text targetobj, float amount)
    {
        targetobj.gameObject.SetActive(true);

        Color tempcolor = targetobj.color;
        float f = 0;
        while (true)
        {
            if (f >= 1)
                break;
            targetobj.color = Color.Lerp(tempcolor, new Color(tempcolor.r, tempcolor.g, tempcolor.b, 1), f);
            f += amount;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    IEnumerator Fading(GameObject targetobj, float amount)
    {
        Color tempcolor = targetobj.GetComponent<Image>().color;
        float f = 0;
        while (true)
        {
            if (f >= 1)
                break;
            targetobj.GetComponent<Image>().color = Color.Lerp(tempcolor, new Color(255, 255, 255, 0), f);
            f += amount;
            yield return new WaitForSeconds(0.01f);
        }
        targetobj.SetActive(false);
        yield break;
    }

    IEnumerator Fading(Text targetobj, float amount)
    {
        Color tempcolor = targetobj.color;
        float f = 0;
        while (true)
        {
            if (f >= 1)
                break;
            targetobj.color = Color.Lerp(tempcolor, new Color(tempcolor.r, tempcolor.g, tempcolor.b, 0), f);
            f += amount;
            yield return new WaitForSeconds(0.01f);
        }
        targetobj.gameObject.SetActive(false);
        yield break;
    }

}