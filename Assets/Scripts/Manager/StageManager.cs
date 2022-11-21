using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StageManager : Singleton<StageManager>
{
    public UnityAction startBattleAction;
    public UnityAction endBattleAction;
    public bool isFight = false;


    public void StartBattle()
    {
        isFight = true;
        startBattleAction();
    }

    public void EndBattle()
    {
        isFight = false;
        endBattleAction();
    }

}