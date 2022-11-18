using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IHasStatable, IGrabable, IPoolingable
{
    protected IState currentState;
    protected IState idleState;
    protected IState traceState;
    protected IState attackState;
    protected IState dieState;
    public MONSTER_TYPE monsterType;
    public ObjectPool home { get ; set; }
    public int upgradeCount = 1;
    private float hp;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                SetState(dieState);
            }
        }
    }


    public object GetObj()
    {
        return this; //박싱 - 언박싱을 위해
    }

    private void Awake()
    {
        idleState = new IDleState(this);
        attackState = new AttackState(this);
        dieState = new DieState(this);
        traceState = new TraceState(this);
        SetState(idleState);
    }

    private void Update()
    {
        currentState.StateUpdate();
    }

    public void SetState(IState inputState)
    {
        if (currentState != null)
            currentState.StateExit();
        currentState = inputState;
        currentState.StateEnter();
    }

}
