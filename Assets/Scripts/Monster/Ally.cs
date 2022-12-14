using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Monster, IGrabable
{
    public MONSTER_TYPE monsterType;
    public int upgradeCount = 1;

    private new void Awake()
    {
        base.Awake();
        teamType = TEAM_TYPE.Ally;
        animator = transform.GetChild(upgradeCount - 1).GetComponent<Animator>();
        StageManager.Instance.startBattleAction += StageStart;
    }

    public void StageStart()
    {
        
    }
}
