using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LobbyPosition
{
    public int x;
    public int y;

    public LobbyPosition(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
};

//ロビーの高さ:+5 ~ -5 ロビーの横幅: +8.88 ~ -8.88
public class LobbyDivision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// カーソルの場所から対応する区域を判別する.
    /// </summary>
    /// <param name="cursorPos">カーソルの座標</param>
    /// <returns>対応する区域</returns>
    public LobbyPosition CursorPartitioning(Vector3 cursorPos)
    {
        LobbyPosition position = new LobbyPosition(0, 0);
        if      (cursorPos.x > 6.6f) position.x = 0;
        else if (cursorPos.x > 4.4f) position.x = 1;
        else if (cursorPos.x > 2.2f) position.x = 2;
        else if (cursorPos.x > 0)    position.x = 3;
        else if (cursorPos.x > -2.2) position.x = 4;
        else if (cursorPos.x > -4.4) position.x = 5;
        else if (cursorPos.x > -6.6) position.x = 6;
        else                         position.x = 7;

        if      (cursorPos.y > 3 ) position.y = 0;
        else if (cursorPos.y > 1 ) position.y = 1;
        else if (cursorPos.y > -1) position.y = 2;
        else if (cursorPos.y > -3) position.y = 3;
        else                       position.y = 4;

        return position;
    }
}
