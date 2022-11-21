using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IHasStatable, IPoolingable
{
    public IState currentState;
    public IState idleState;
    public IState traceState;
    public IState attackState;
    public IState dieState;
    public TEAM_TYPE teamType;
    public ObjectPool home { get ; set; }
    public float detectRadius;
    public float attackRange;
    public Animator animator;
    public CharacterController cha;
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

    protected void Awake()
    {
        idleState = new IDleState(this);
        attackState = new AttackState(this);
        dieState = new DieState(this);
        traceState = new TraceState(this);
        
        SetState(idleState);

    }


    public object GetObj()
    {
        return this; //박싱 - 언박싱을 위해
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
