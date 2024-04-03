using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuest : MonoBehaviour
{
    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
            Ray ray = Camera.main.ScreenPointToRay(this.transform.position);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                Debug.Log(clickedGameObject.tag);//ゲームオブジェクトの名前を出力
            }
            else
            {
                Debug.Log("当たったと思った？気のせいだよwwww");
            }

        }
    }

    public void Select()
    {
        Debug.Log("ボタン押せた!");
    }
}
