using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE_TYPE
{
    ILDE, ATTACK, DIE //스킬은 전략패턴으로 구현
}

public abstract class BaseState : IState
{
    public IHasStatable hasStatable;

    public BaseState(IHasStatable hasStatable)
    {
        this.hasStatable = hasStatable; 
    }
    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
}

public class IDleState : BaseState
{
    public IDleState(IHasStatable hasStatable) : base(hasStatable)
    {
    }

    public override void StateEnter()
    {
        Debug.Log("대기 시작");
    }

    public override void StateExit()
    {
        Debug.Log("대기 나감");
    }

    public override void StateUpdate()
    {
        Debug.Log("대기 중");
    }
}

public class AttackState : BaseState
{
    public AttackState(IHasStatable hasStatable) : base(hasStatable)
    {
    }

    public override void StateEnter()
    {
        Debug.Log("공격 시작");
    }

    public override void StateExit()
    {
        Debug.Log("공격 나감");
    }

    public override void StateUpdate()
    {
        Debug.Log("공격 중");
    }
}

public class DieState : BaseState
{
    public DieState(IHasStatable hasStatable) : base(hasStatable)
    {
    }

    public override void StateEnter()
    {
        Debug.Log("죽음 시작");
    }

    public override void StateExit()
    {
        Debug.Log("죽기 끝");
    }

    public override void StateUpdate()
    {
        Debug.Log("죽는 중");
    }
}

public class Monster : MonoBehaviour, IHasStatable
{
    protected IState currentState;
    protected IState idleState;
    protected IState attackState;
    protected IState dieState;

    public object GetObj()
    {
        return this; //박싱 - 언박싱을 위해
    }

    private void Awake()
    {
        idleState = new IDleState(this);
        attackState = new AttackState(this);
        dieState = new DieState(this);
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
