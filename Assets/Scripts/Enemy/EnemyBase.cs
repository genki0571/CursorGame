using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float hp;
    public float maxHp;

    public enum State
    {
        None,
        StateDecide,
        Sleep,
        Grabed,
        GoServer,
        Attack,
        Stop,
        Death
    }
    public State state;

    public Element haveElement;

    public void Initialize()
    {
        state = State.StateDecide;
    }

    public void Reset()
    {
        state = State.Sleep;
    }
}
