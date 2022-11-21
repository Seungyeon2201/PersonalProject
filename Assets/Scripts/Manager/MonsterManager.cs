using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

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

    //1성 몬스터 소환
    public Transform SummonMonster(Monsters monsters)
    {
        if (!monsterDic.ContainsKey(monsters.monsterType)) return this.transform;
        monsterPool = monsterDic[monsters.monsterType];
        Vector3 position = GroundManager.Instance.FindBlank();
        monsterPool.Call(position, Quaternion.Euler(0, 180, 0)).TryGetComponent(out Ally monster);
        monsterCountDic[monsters.monsterType]++;
        if(monsterCountDic[monsters.monsterType] == 3)
        {
            UpgradeMonster(monsters.monsterType, GroundManager.Instance.ReturnMonster(monsters.monsterType, 1));
            monsterCountDic[monsters.monsterType] = 0;
        }
        return monster.transform;
    }
    //2성 몬스터 소환
    public void UpgradeMonster(MONSTER_TYPE monsterType, Vector3 position)
    {
        Transform transform = monsterPool.Call(position, Quaternion.Euler(0, 180, 0));
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetComponent<Ally>().upgradeCount++;

        monsterStarDic[monsterType]++;
        if (monsterStarDic[monsterType] == 3)
        {
            GroundManager.Instance.ReturnMonster(monsterType, 2);
            UpgradeFinalMonster(monsterType, position);
            monsterStarDic[monsterType] = 0;
        }

    }
    //3성 몬스터 소환
    public void UpgradeFinalMonster(MONSTER_TYPE monsterType, Vector3 position)
    {
        Transform transform = monsterPool.Call(position, Quaternion.Euler(0, 180, 0));
        transform.GetComponent<Ally>().upgradeCount = 3;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    //오브젝트풀에 몬스터 리턴
    public void ReturnPool(GameObject gameObject)
    {
        for(int i = 0; i < 3; i++)
        {
            if(i == 0) gameObject.transform.GetChild(i).gameObject.SetActive(true);
            else gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.GetComponent<Ally>().upgradeCount = 1;
        gameObject.GetComponent<Ally>().home.Return(gameObject);
    }
}
