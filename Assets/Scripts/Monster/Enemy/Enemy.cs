using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Monster
{
    
    private new void Awake()
    {
        base.Awake();
        teamType = TEAM_TYPE.Enemy;
        animator = GetComponent<Animator>();
        cha = GetComponent<CharacterController>();
    }

}
