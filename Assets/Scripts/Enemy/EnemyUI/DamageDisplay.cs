using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    RectTransform displayTrans;
    Text text;

    public bool isSleep;

    const float INTERVAL = 1;
    float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();
        displayTrans = GetComponent<RectTransform>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            displayTrans.position += new Vector3(0,0.003f,0);
            text.color -= new Color32(0,0,0,1);

            if (timer <= INTERVAL)
            {
                timer += Time.deltaTime;
            }
            if (timer >= INTERVAL)
            {
                Reset();
            }
        }
    }

    public void Reset()
    {
        isSleep = true;
        displayTrans.position = new Vector3(0,80,0);
        text.enabled = false;
        text.color += new Color32(0,0,0,255);
        timer = 0;
    }

    public void Initialize(Vector3 pos, float damage)
    {
        displayTrans.position = pos;
        isSleep = false;
        text.enabled = true;
        text.text = "" + damage;
    }
}
