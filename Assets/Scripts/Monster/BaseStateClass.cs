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
        if (StageManager.Instance.isFight == false) return;
        Monster monster = hasStatable.GetObj() as Monster;
        monster.SetState(monster.traceState);
    }
}

public class TraceState : BaseState
{
    public TraceState(IHasStatable hasStatable) : base(hasStatable) { }

    public override void StateEnter()
    {
        
        Debug.Log("추적 시작");
    }

    public override void StateExit()
    {
        Debug.Log("대기 나감");
    }

    public override void StateUpdate()
    {
        //공격 범위 안으로 적이 포착되면 공격상태로 변경
        //스테이지 내에 적이 없으면 Idle상태로 전환
        Monster monster = hasStatable.GetObj() as Monster;
        Collider[] colliders = Physics.OverlapSphere(monster.transform.position, monster.detectRadius, 1 << LayerMask.NameToLayer("Monster"));
        if (colliders.Length <= 0) return;
        TEAM_TYPE teamType = monster.teamType;
        if (teamType == colliders[0].GetComponent<Monster>().teamType) return;
        monster.transform.LookAt(colliders[0].transform);
        if (teamType == TEAM_TYPE.Ally) return;
        monster.cha.Move((colliders[0].transform.position - monster.transform.position).normalized * Time.deltaTime);
        monster.animator.Play("Walk");
        if ((colliders[0].transform.position - monster.transform.position).sqrMagnitude < monster.attackRange)
            monster.SetState(monster.attackState);
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
        Monster monster = hasStatable.GetObj() as Monster;
        monster.animator.Play("Attack");
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
