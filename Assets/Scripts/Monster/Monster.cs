using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour, IHasStatable, IPoolingable
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
    public CharacterController characterController;
    public bool canFight = false;
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
        StageManager.Instance.startBattleAction += StageStart;
        StageManager.Instance.endBattleAction += StageEnd;
        characterController = GetComponent<CharacterController>();
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

    public void StageStart()
    {
        
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

        //Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        //Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        //Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);

        //Debug.DrawRay(transform.position, lookDir * viewRadius, Color.green);
        //Debug.DrawRay(transform.position, rightDir * viewRadius, Color.blue);
        //Debug.DrawRay(transform.position, leftDir * viewRadius, Color.blue);
    }
}
