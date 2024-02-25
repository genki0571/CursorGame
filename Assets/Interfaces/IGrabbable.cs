using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void Grabbing(Transform cursorTrans) { }

    void Putting() { }
}
