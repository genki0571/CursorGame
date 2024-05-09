using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureFile : MonoBehaviour,ISelectable,IGrabbable
{
    Transform fileTrans;
    SpriteRenderer renderer;
    bool isGrabed;
    public bool isSleep;

    Vector3 diffPlayerVec = Vector2.zero;
    PCFieldController pcFieldController => PCFieldController.instance;
    ExpController expController => ExpController.instance;


    // Start is called before the first frame update
    void Start()
    {
        fileTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        Putting();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Grabbing(Transform cursorTrans)
    {
        if (!isGrabed)
        {
            diffPlayerVec = fileTrans.position - cursorTrans.position;
            pcFieldController.MovePos(this.gameObject);
        }
        fileTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting()
    {
        isGrabed = false;

        fileTrans.position = pcFieldController.GetPos(this.gameObject, fileTrans.position);
    }

    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        transform.localPosition = Vector3.zero;
        pcFieldController.MovePos(this.gameObject);
    }

    public void Initialize(Vector3 pos)
    {
        fileTrans.position = pos;
        isSleep = false;
        renderer.enabled = true;
        Putting();
    }


    public void Selected()
    { 
    
    }

    public void Open()
    {
        Vector3 filePos = fileTrans.position;
        for (int i = 0; i < 4; i++)
        {
            expController.InitializeExpS(filePos + new Vector3(Random.Range(-2f,2f), Random.Range(-2f, 2f),0)) ;
        }
        for (int i = 0; i < 3; i++)
        {
            expController.InitializeExpM(filePos + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0));
        }

        Reset();
    }

    public void Delete() 
    {
        Reset();
    }
}
