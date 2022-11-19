using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Net.NetworkInformation;

public class StoreManager : Singleton<StoreManager>
{
    public Store[] storeProbability;
    private Dictionary<MONSTER_TYPE, int> typeToCount = new Dictionary<MONSTER_TYPE, int>();
    private Dictionary<MONSTER_TYPE, Monsters> typeToMonster = new Dictionary<MONSTER_TYPE, Monsters>();
    public List<Monsters> monsterScriptable = new List<Monsters>();
    private float oneCost;
    private float twoCost;
    private float threeCost;
    private int storeLevel = 0;
    private COST_TYPE pickCostType;
    public UnityAction StoreLevelUpAction;
    List<int> indexList = new List<int>();
    private Dictionary<int, MONSTER_TYPE> indexToMonster = new Dictionary<int, MONSTER_TYPE>();
    private void Awake()
    {
        for(int i = 0; i< MonsterManager.Instance.scriptableObjects.Count; i++)
        {
            monsterScriptable.Add(MonsterManager.Instance.scriptableObjects[i]);
            typeToMonster.Add(MonsterManager.Instance.scriptableObjects[i].monsterType, monsterScriptable[i]);
        }
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
        
        StoreLevelUpAction += StoreLevelUp;
        StoreLevelUpAction();
    }

    private void Start()
    {
        ReRollMonster();
    }

    //MonsterManager에 있는 ScriptableObject의 개수에서 랜덤으로 5개(상점에 나타나는 기물 개수)
    public void ReRollMonster()
    {
        UIManager.Instance.ButtonInteractableInit();
        for (int i = 0; i < 5; i++)
        {
            int ran = Random.Range(0, 100);
            if (ran < oneCost) pickCostType = COST_TYPE.One;
            else if (ran < twoCost) pickCostType = COST_TYPE.Two;
            else pickCostType = COST_TYPE.Three;
            PickMonster(pickCostType);
        }
    }

    public void StoreLevelUp()
    {
        SetStoreProbability();
    }

    //상점 코스트 별 확률 세팅
    public void SetStoreProbability()
    {
        storeLevel++;
        oneCost = storeProbability[storeLevel - 1].OneCost;
        twoCost = storeProbability[storeLevel - 1].TwoCost;
        threeCost = 100 - oneCost - twoCost;
    }

    public void PickMonster(COST_TYPE costType)
    {
        int totalMonsterCount = 0;
        int index = 0;
        indexList.Clear();
        indexToMonster.Clear();
        for (int i = 0; i < monsterScriptable.Count; i++)
        {
            if(monsterScriptable[i].cost == costType)
            {
                totalMonsterCount += typeToCount[monsterScriptable[i].monsterType];
                indexList.Add(totalMonsterCount);
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

    public void BuyMonster(MONSTER_TYPE monsterType)
    {
        MonsterManager.Instance.SummonMonster(typeToMonster[monsterType]);
        typeToCount[monsterType]--;
    }
    //팔때 1성으로 되돌려서 풀에 집어넣기
}
