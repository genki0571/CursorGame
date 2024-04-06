using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScroll : MonoBehaviour
{
    const float CHANGE_SCROLL_AMOUNT = 15;
    public int scrollLevel;
    public float scrollAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollAmount += Input.mouseScrollDelta.y;
        if (scrollAmount >= CHANGE_SCROLL_AMOUNT) 
        {
            scrollLevel += 1;
            scrollAmount = 0;
        }
        else if(scrollAmount <= -CHANGE_SCROLL_AMOUNT)
        {
            scrollLevel -= 1;
            scrollAmount = 0;
        }

        if (scrollLevel >= 4) 
        {
            scrollLevel = 0;
        }
        else if (scrollLevel <= -1) 
        {
            scrollLevel = 3;
        }
    }
}
