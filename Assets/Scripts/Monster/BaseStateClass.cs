using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE_TYPE
{
    ILDE, TRACE, ATTACK, DIE //스킬은 전략패턴으로 구현
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
    public IDleState(IHasStatable hasStatable) : base(hasStatable) { }
    
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
        //Debug.Log("대기 중");
        //스테이지 시작되면 TraceState로 상태 변환
    }
}

public class TraceState : BaseState
{
    public TraceState(IHasStatable hasStatable) : base(hasStatable) { }

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
        //공격 범위 안으로 적이 포착되면 공격상태로 변경
        //스테이지 내에 적이 없으면 Idle상태로 전환
    }
}

public class AttackState : BaseState
{
    public AttackState(IHasStatable hasStatable) : base(hasStatable) { }

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
        //공격하다가 공격범위를 나가거나 상대가 죽으면 Trace 상태로 변경
        //마나가 다 차면 AttackState를 스킬 어택 스테이트로 전략패턴 넣기
        //스킬 어택 스테이트는 단일 / 범위 / 힐 / CC
    }
}

public class DieState : BaseState
{
    public DieState(IHasStatable hasStatable) : base(hasStatable) { }

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
