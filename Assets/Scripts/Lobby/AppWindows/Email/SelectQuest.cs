using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuest : MonoBehaviour
{
    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数
    [SerializeField] QuestManager questManager;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector3.forward);
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                if (clickedGameObject.CompareTag("UpButton"))
                {
                    questManager.ClickUpButton();
                }
                else if (clickedGameObject.CompareTag("DownButton"))
                {
                    questManager.ClickDownButton();
                }
            }
        }
    }
}
