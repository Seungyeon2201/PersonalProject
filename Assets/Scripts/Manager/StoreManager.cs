using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Net.NetworkInformation;

public class StoreManager : Singleton<StoreManager>
{
    public Store storeProbability;
    private Dictionary<MONSTER_TYPE, int> typeToCount = new Dictionary<MONSTER_TYPE, int>();
    public Dictionary<MONSTER_TYPE, Monsters> typeToMonster = new Dictionary<MONSTER_TYPE, Monsters>();
    public List<Monsters> monsterScriptable = new List<Monsters>();
    private Monsters sellMonsterInfo;
    private Monster sellMonster;
    private float oneCost;
    private float twoCost;
    private float threeCost;
    public int storeLevel = 0;
    private COST_TYPE pickCostType;
    
    List<int> indexList = new List<int>();
    private Dictionary<int, MONSTER_TYPE> indexToMonster = new Dictionary<int, MONSTER_TYPE>();
    private void Awake()
    {
        for(int i = 0; i< MonsterManager.Instance.scriptableObjects.Count; i++)
        {
            monsterScriptable.Add(MonsterManager.Instance.scriptableObjects[i]);
            typeToMonster.Add(MonsterManager.Instance.scriptableObjects[i].monsterType, monsterScriptable[i]);
        }
        //몬스터 Cost Type별 기물 개수 세팅
        for(int i =0; i< monsterScriptable.Count; i++ )
        {
            if(monsterScriptable[i].cost == COST_TYPE.One)
            {
                typeToCount.Add(monsterScriptable[i].monsterType, 20);
            }
            else if(monsterScriptable[i].cost == COST_TYPE.Two)
            {
                typeToCount.Add(monsterScriptable[i].monsterType, 15);
            }
            else
            {
                typeToCount.Add(monsterScriptable[i].monsterType, 9);
            }
        }
        GameManager.Instance.storeLevelUpAction += StoreLevelUp;
        SetStoreProbability();
    }

    private void Start()
    {
        ReRollMonster();
    }

    //MonsterManager에 있는 ScriptableObject의 개수에서 랜덤으로 5개(상점에 나타나는 기물 개수)
    public void ReRollMonster()
    {
        if (GameManager.Instance.Gold < GameManager.Instance.moneySetting.reRollMoney) return;
        UIManager.Instance.ButtonInteractableInit();
        GameManager.Instance.Gold -= GameManager.Instance.moneySetting.reRollMoney;
        for (int i = 0; i < 5; i++)
        {
            int ran = Random.Range(0, 100);
            if (ran < oneCost) pickCostType = COST_TYPE.One;
            else if (ran < oneCost + twoCost) pickCostType = COST_TYPE.Two;
            else pickCostType = COST_TYPE.Three;
            PickMonster(pickCostType);
        }
    }

    public void StoreLevelUp()
    {
        storeLevel++;
        SetStoreProbability();
        GameManager.Instance.LevelUp();
    }

    //상점 레벨에 따른 Cost Type별 확률 세팅
    public void SetStoreProbability()
    {
        oneCost = storeProbability.ReRollProbs[storeLevel].OneCost;
        twoCost = storeProbability.ReRollProbs[storeLevel].OneCost;
        threeCost = 100 - oneCost - twoCost;
        Debug.Log(oneCost + "/" + twoCost + "/" + threeCost);
    }

    //Cost Type내에 있는 몬스터의 종류를 찾고 기물의 개수를 비교하여 랜덤으로 뽑기
    public void PickMonster(COST_TYPE costType)
    {
        int totalMonsterCount = 0;
        int index = 0;
        indexList.Clear();
        indexToMonster.Clear();
        //Cost Type에 맞는 Monster Type을 찾고 Monster Type에 맞는 기물의 개수 찾기
        for (int i = 0; i < monsterScriptable.Count; i++)
        {
            if(monsterScriptable[i].cost == costType)
            {
                totalMonsterCount += typeToCount[monsterScriptable[i].monsterType];
                //몬스터 타입별 기물의 개수를 파악하기 위한 index를 넣어 놓는 List
                indexList.Add(totalMonsterCount); 
                //index가 가리키고 있는 몬스터 타입을 담아놓기 위한 Dictionary
                indexToMonster.Add(index, monsterScriptable[i].monsterType); 
                index++;
            }
        }
        int ran = Random.Range(0, totalMonsterCount);
        for (int i = 0; i < indexList.Count; i++)
        {
            if (ran < indexList[i])
            {
                UIManager.Instance.StoreShowInfo(typeToMonster[indexToMonster[i]]);
                break;
            }
        }
    }
    //몬스터 구매시 해당 몬스터 타입을 소환하고 기물의 개수 줄이기
    public void BuyMonster(MONSTER_TYPE monsterType)
    {
        if (GameManager.Instance.Gold < (int)typeToMonster[monsterType].cost) return;
        MonsterManager.Instance.SummonMonster(typeToMonster[monsterType]);
        typeToCount[monsterType]--;
        GameManager.Instance.Gold -= (int)typeToMonster[monsterType].cost;
    }
    
    //몬스터를 팔 때 upgradeCount를 통해 몬스터 기물을 파악하여 판매(1성 1마리, 2성 3마리, 3성 9마리)
    public void SellMonster(GameObject monsterObject)
    {
        sellMonster = monsterObject.GetComponent<Monster>();
        float floatUpgradeCount = sellMonster.upgradeCount;
        int monsterCount = (int)Mathf.Pow(3f, (floatUpgradeCount - 1));
        typeToCount[sellMonster.monsterType] += monsterCount;
        GameManager.Instance.Gold += (int)typeToMonster[sellMonster.monsterType].cost * monsterCount;
        //MonsterManager가 가지고 있는 기물 정보에 관한 Dictionary 갱신
        if (sellMonster.upgradeCount == 1)
        {
            MonsterManager.Instance.monsterCountDic[sellMonster.monsterType]--;
        }
        else if (sellMonster.upgradeCount == 2)
        {
            MonsterManager.Instance.monsterStarDic[sellMonster.monsterType]--;
        }
        MonsterManager.Instance.ReturnPool(monsterObject);
    }
}
