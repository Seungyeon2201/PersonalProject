using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    public List<Monsters> scriptableObjects = new List<Monsters>();
    public List<ObjectPool> monsterspool = new List<ObjectPool>();
    public Dictionary<MONSTER_TYPE, int> monsterCountDic = new Dictionary<MONSTER_TYPE, int>();
    public Dictionary<MONSTER_TYPE, int> monsterStarDic = new Dictionary<MONSTER_TYPE, int>();
    public Dictionary<MONSTER_TYPE, ObjectPool> monsterDic = new Dictionary<MONSTER_TYPE, ObjectPool>();
    ObjectPool monsterPool;

    private void Awake()
    {

        for (int i = 0; i < scriptableObjects.Count; i++)
        {
            monsterspool.Add(ObjectPoolManager.Instance.PoolRequest(scriptableObjects[i].monsterprefab, 10, 5));
            monsterDic.Add(scriptableObjects[i].monsterType, monsterspool[i]);
            monsterCountDic.Add(scriptableObjects[i].monsterType, 0);
            monsterStarDic.Add(scriptableObjects[i].monsterType, 0);
        }
    }

    //몬스터 소환
    public Transform SummonMonster(Monsters monsters, Vector3 position)
    {
        if (monsterDic.ContainsKey(monsters.monsterType))
        {
            monsterPool = monsterDic[monsters.monsterType];
            MonsterCountCheck();
            //monsterCountDic[monsters.monsterType]++;
            //if (monsterCountDic[monsters.monsterType] > 2)
            //{
            //    UpgradeMonster(monsters, GroundManager.Instance.FindSameMonster(monsters.monsterType));
            //    monsterCountDic[monsters.monsterType] = 0;
            //}
        }
        monsterPool.Call(position).TryGetComponent(out Monster monster);
        return monster.transform;
    }

    public void MonsterCountCheck()
    {

    }
    public void UpgradeMonster(Monsters monsters, Vector3 position)
    {
        MonsterManager.Instance.SummonMonster(monsters, GroundManager.Instance.FindBlank() + new Vector3(0f, 1f, 0f));
    }
}
