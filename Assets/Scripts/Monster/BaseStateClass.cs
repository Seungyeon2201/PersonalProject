using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum STATE_TYPE
{
    ILDE, TRACE, ATTACK, DIE //스킬은 전략패턴으로 구현
}

public abstract class BaseState : IState
{
    public IHasStatable hasStatable;
    protected Monster monster;
    protected Collider target;

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
        monster = hasStatable.GetObj() as Monster;
    }

    public override void StateExit()
    {
        Debug.Log("대기 나감");
    }

    public override void StateUpdate()
    {
        monster.animator.Play("Idle");
        if (monster.canFight == true && StageManager.Instance.isFight == true)
        {
            monster.SetState(monster.traceState);
        }
        
    }
}

public class TraceState : BaseState
{
    public TraceState(IHasStatable hasStatable) : base(hasStatable) { }
    float detectRaduis = 0;
    bool isFindTarget;
    int temp;
    Collider[] colliders;
    public override void StateEnter()
    {
        isFindTarget = false;
        monster = hasStatable.GetObj() as Monster;
        detectRaduis = monster.detectRadius;
        Debug.Log("추적 시작");
    }

    public override void StateExit()
    {
        Debug.Log("추적 나감");
        isFindTarget = false;
    }

    public override void StateUpdate()
    {
        //공격 범위 안으로 적이 포착되면 공격상태로 변경
        //스테이지 내에 적이 없으면 Idle상태로 전환
        //if(monster.teamType == TEAM_TYPE.Ally)
        //{
        //    monster.SetState(monster.idleState);
        //}
            colliders = Physics.OverlapSphere(monster.transform.position, monster.detectRadius, 1 << LayerMask.NameToLayer("Monster"));

        if (colliders.Length <= 0)
        {
            monster.detectRadius++;
            return;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Monster>().canFight == false) continue;
            if (colliders[i].GetComponent<Monster>().teamType == monster.teamType) continue;
            isFindTarget = true;
            temp = i;
        }
        if (isFindTarget == false)
        {
            monster.detectRadius++;
            if(monster.detectRadius > 7)
            {
                monster.detectRadius = detectRaduis;
                StageManager.Instance.isFight = false;
            }
            monster.SetState(monster.idleState);
            return;
        }
        monster.transform.LookAt(colliders[temp].transform);
        monster.characterController.Move((colliders[temp].transform.position - monster.transform.position).normalized * Time.deltaTime);
        monster.animator.Play("Walk");
        if ((colliders[temp].transform.position - monster.transform.position).sqrMagnitude < monster.attackRange)
        {
            monster.detectRadius = detectRaduis;
            monster.SetState(monster.attackState);
        }   
    }
}

public class AttackState : BaseState
{
    public AttackState(IHasStatable hasStatable) : base(hasStatable) { }
    Collider[] colliders;
    public override void StateEnter()
    {
        Debug.Log("공격 시작");
        monster = hasStatable.GetObj() as Monster;
    }

    public override void StateExit()
    {
        Debug.Log("공격 나감");
    }

    public override void StateUpdate()
    {
        
        colliders = Physics.OverlapSphere(monster.transform.position, monster.attackRange, 1 << LayerMask.NameToLayer("Monster"));
        monster.animator.Play("Attack");
        if (colliders.Length <= 0) monster.SetState(monster.idleState);
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
