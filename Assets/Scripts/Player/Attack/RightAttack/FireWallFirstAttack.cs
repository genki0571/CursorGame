using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallFirstAttack : MonoBehaviour,IRightAttacker
{
    int level = 2;
    const float FIRE_DAMAGE = 10;

    Command command = global::Command.FireWall;

    PCFieldController pcFieldController => PCFieldController.instance;

    List<FireWallPost> fireWallPosts = new List<FireWallPost>();
    int nextPutPostNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        fireWallPosts = pcFieldController.fireWallPosts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        fireWallPosts[nextPutPostNum].Initialize(pos, level);

        nextPutPostNum += 1;
        if (nextPutPostNum >= 3)
        {
            nextPutPostNum = 0;
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
