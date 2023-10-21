using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipEntity : MonoBehaviour
{
    Text tipTxt;
    float lifeTime;
    float time;

    void Awake()
    {
        tipTxt = GetComponent<Text>();
        lifeTime = 1f;
    }

    public void Ctor(string tip)
    {
        transform.localPosition = Vector3.zero;
        tipTxt.text = tip;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < lifeTime)
        {
            this.transform.position += new Vector3(0, 0.5f, 0);
            var a = (1-time/lifeTime);
            tipTxt.color = new Color(tipTxt.color.r, tipTxt.color.g, tipTxt.color.b, a);
            time += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
