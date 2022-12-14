using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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
        Collider[] colliders = Physics.OverlapSphere(monster.transform.position, monster.detectRadius, 1 << LayerMask.NameToLayer("Monster"));

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
            if (monster.detectRadius > 7)
            {
                monster.detectRadius = detectRaduis;
                StageManager.Instance.isFight = false;
            }
            monster.SetState(monster.idleState);
            return;
        }
        monster.animator.Play("Walk");
        monster.navMeshAgent.SetDestination(colliders[temp].transform.position);
        monster.transform.LookAt(colliders[temp].transform.position);
        if (Vector3.Distance(colliders[temp].transform.position, monster.transform.position) < monster.attackRange)
        {
            monster.detectRadius = detectRaduis;
            monster.navMeshAgent.isStopped = true;
            monster.SetState(monster.attackState);
        }
    }
}

public class AttackState : BaseState
{
    int targetIndex;
    float animationLength;
    float atkMoment;
    float normalizedTime;
    AnimatorStateInfo animatorStateInfo;
    Monster target;
    public AttackState(IHasStatable hasStatable) : base(hasStatable) { }
    public override void StateEnter()
    {
        Debug.Log("공격 시작");
        monster = hasStatable.GetObj() as Monster;
        monster.animator.Play("Attack");
        animatorStateInfo = monster.animator.GetCurrentAnimatorStateInfo(0);
        animationLength = animatorStateInfo.length;
        atkMoment = monster.atkMoment;
        normalizedTime = 0;
    }

    public override void StateExit()
    {
        Debug.Log("공격 나감");
    }

    public override void StateUpdate()
    {
        //atkMoment = monster.atkMoment;
        animatorStateInfo = monster.animator.GetCurrentAnimatorStateInfo(0);
        Collider[] colliders = Physics.OverlapSphere(monster.transform.position, monster.attackRange, 1 << LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Monster>().canFight == false) continue;
            if (colliders[i].GetComponent<Monster>().teamType == monster.teamType) continue;
            targetIndex = i;
            target = colliders[targetIndex].GetComponent<Monster>();
            target.TakeHit(monster.atkDamage);
        }
        
        if (animatorStateInfo.IsName("Attack") && animatorStateInfo.normalizedTime - normalizedTime >= (animationLength * atkMoment))
        {
            normalizedTime = animatorStateInfo.normalizedTime;
            target.TakeHit(monster.atkDamage);
        }

        if (!(colliders.Length > 0))
        {
            monster.SetState(monster.idleState);
        }
        //공격하다가 공격범위를 나가거나 상대가 죽으면 Trace 상태로 변경
        //마나가 다 차면 AttackState를 스킬 어택 스테이트로 전략패턴 넣기
        //스킬 어택 스테이트는 단일 / 범위 / 힐 / CC
    }
}

public class DieState : BaseState
{
    AnimatorStateInfo animatorStateInfo;

    public DieState(IHasStatable hasStatable) : base(hasStatable) { }

    public override void StateEnter()
    {
        monster = hasStatable.GetObj() as Monster;
        monster.animator.Play("Die");
        animatorStateInfo = monster.animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log("죽음 시작");
    }

    public override void StateExit()
    {
        Debug.Log("죽기 끝");
    }

    public override void StateUpdate()
    {
        if(animatorStateInfo.normalizedTime > animatorStateInfo.length)
        {
            monster.gameObject.SetActive(false);
        }
    }
}
