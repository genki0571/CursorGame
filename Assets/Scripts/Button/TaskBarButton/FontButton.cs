using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FontType 
{
    English,
    Japanese
}

public class FontButton : MonoBehaviour,IButton
{

    PCFieldController pcFieldController => PCFieldController.instance;
    PlayerInput playerInput => PlayerInput.instance;
    Transform playerTrans;

    FontType fontType;


    SpriteRenderer renderer;
    [SerializeField] Color32 originalColor;
    [SerializeField] Color32 selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        originalColor = renderer.color;

        fontType = FontType.Japanese;
        playerTrans = playerInput.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Color32 color = originalColor;

        Vector3 playerPos = playerTrans.position;

        Ray2D ray = new Ray2D(playerPos, transform.forward);
        RaycastHit2D hit = new RaycastHit2D();
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, 1 << LayerMask.NameToLayer("TaskWindowButton"));
        if (hit.collider)
        {
            if (hit.transform == this.transform)
            {
                color = selectedColor;
            }
        }
        if (renderer.color != color)
        {
            renderer.color = color;
        }

        fontType = pcFieldController.fontType;
    }


    public Command Pushed()
    {
        if (fontType == FontType.Japanese) 
        {
            fontType = FontType.English;
        }
        else if(fontType == FontType.English) 
        {
            fontType = FontType.Japanese;
        }
        pcFieldController.fontType = fontType;

        Command command = Command.Empty;
        return command;
    }
}
