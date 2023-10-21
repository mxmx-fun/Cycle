using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phantomEntity : MonoBehaviour
{
    SpriteRenderer sr;
    float lifeTime;
    float time;
    public phantomEntity parent;
    bool isInit;
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Ctor(float lifeTime)
    {
        this.lifeTime = lifeTime;
        time = 0;
        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(parent != null || !isInit) return;
        if(time < lifeTime)
        {
            var a = (1 - time / lifeTime);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            time += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
