using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallPost : MonoBehaviour
{
    Transform postTrans;
    PCFieldController pcFieldController => PCFieldController.instance;
    SpriteRenderer renderer; 
    public int level = 0;


    List<FireWallPost> fireWallPosts = new List<FireWallPost>();
    List<FireWall> fireWalls = new List<FireWall>();
    public bool isSleep;

    // Start is called before the first frame update
    void Start()
    {
        postTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();

        fireWallPosts = pcFieldController.fireWallPosts;
        fireWalls = pcFieldController.fireWalls;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < level + 1; i++)
        {
            int startNum = i;
            int endNum = i + 1;
            if (endNum >= 3) 
            {
                endNum = 0;
            }

            Vector3 fireVec = fireWallPosts[startNum].transform.position - fireWallPosts[endNum].transform.position;
            Vector3 firePos = fireWallPosts[endNum].transform.position + (fireVec / 2);
            float fireLength = fireVec.magnitude;

            fireWalls[i].transform.position = firePos;
            fireWalls[i].transform.localScale = new Vector3(fireLength, 0.2f,0) ;

            Vector3 VirticalVec = (Quaternion.Euler(0, 0, 90) * fireVec).normalized;
            fireWalls[i].transform.rotation = Quaternion.FromToRotation(Vector3.up,VirticalVec);
        }

        if (level >= 0)
        {
            if (!fireWallPosts[0].isSleep && !fireWallPosts[1].isSleep) 
            {
                fireWalls[0].Initialize();
            }
        }
        if (level >= 1) 
        {
            if (!fireWallPosts[1].isSleep && !fireWallPosts[2].isSleep)
            {
                fireWalls[1].Initialize();
            }
        }
        if (level >= 2) 
        {
            if (!fireWallPosts[2].isSleep && !fireWallPosts[0].isSleep)
            {
                fireWalls[2].Initialize();
            }
        }
    }
    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
    }

    public void Initialize(Vector3 pos,int lev)
    {
        level = lev;
        postTrans.position = pos;
        isSleep = false;
        renderer.enabled = true;
    }
}
