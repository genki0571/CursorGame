using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerAttack playerAttack => PlayerAttack.instance;
    PlayerState playerState => PlayerState.instance;

    [SerializeField] SpriteRenderer renderer;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite grabbingSprite;
    [SerializeField] Sprite deadSprite;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.isDead) 
        {
            renderer.sprite = deadSprite;
            renderer.color = new Color(1,1,1,0.5f);
        }
        else if (playerAttack.isGrabbing) 
        {
            renderer.sprite = grabbingSprite;
            renderer.color = new Color(1, 1, 1, 1f);
        }
        else 
        {
            renderer.sprite = normalSprite;
            renderer.color = new Color(1, 1, 1, 1f);
        }
    }
}
