using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StageManager : Singleton<StageManager>
{
    public UnityAction startBattleAction;
    public UnityAction endBattleAction;
    public bool isFight = false;

    private void Awake()
    {
        startBattleAction += StartBattle;
        endBattleAction += EndBattle;
    }

    public void StartBattle()
    {
        isFight = true;
    }

    public void EndBattle()
    {
        isFight = false;
    }

}