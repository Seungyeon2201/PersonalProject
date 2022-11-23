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
        if(GameManager.Instance.CurPopulation < GameManager.Instance.totalPopulation)
        {
            for (int i = GameManager.Instance.CurPopulation; i < GameManager.Instance.totalPopulation; i++)
            {
                Debug.Log(GameManager.Instance.CurPopulation + " / " + GameManager.Instance.totalPopulation);
                GroundManager.Instance.SetMonterToFight();
            }
        }
        
        isFight = true;
    }

    public void EndBattle()
    {
        isFight = false;
    }

}