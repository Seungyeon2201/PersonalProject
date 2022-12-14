using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Monster : MonoBehaviour, IHasStatable, IPoolingable, IDamagable
{
    public IState currentState;
    public IState idleState;
    public IState traceState;
    public IState attackState;
    public IState dieState;
    public TEAM_TYPE teamType;
    public ObjectPool home { get ; set; }
    [Range(0, 10)]
    public float detectRadius;
    [Range(0, 10)]
    public float attackRange;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public bool canFight = false;
    [Range(0,1)]
    public float atkMoment;
    public float atkDamage;
    private float hp = 100;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            Debug.Log(gameObject.name + " : " + HP);
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
        StageManager.Instance.endBattleAction += StageEnd; // 나중에 Ally로 옮겨야 함
        navMeshAgent = GetComponent<NavMeshAgent>();
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

    

    public void StageEnd()
    {
        currentState = idleState;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void TakeHit(float damage)
    {
        HP -= damage;
    }
}
