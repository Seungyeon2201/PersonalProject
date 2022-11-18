using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField]
    private List<ObjectPool> pools = new List<ObjectPool>();
    private Transform active;
    private Transform inactive;

    private void InactiveSet()
    {
        if (active == null)
        {
            active = new GameObject("Active").transform;
            inactive = new GameObject("InActive").transform;
            inactive.gameObject.SetActive(false);
        }
    }

    public ObjectPool PoolRequest(GameObject baseObj, int start, int add)
    {
        InactiveSet();
        if (!baseObj.TryGetComponent(out IPoolingable temp)) //대상이 오브젝트풀 인터페이스를 가지고 있는지 체크후 없다면 null반환
        {
            return null;
        }
        foreach (var pool in pools) //현재 생성된 풀중에 해당 오브젝트에 해당하는 오브젝트풀이 있는지 체크후 있다면 기존의 풀을 반환
        {
            if (pool.baseObj == baseObj)
            {
                return pool;
            }
        }
        //현재 생성된 풀중에 없다면 새롭게 오브젝트 풀을 생성후 반환
        ObjectPool resultPool = new ObjectPool(baseObj, start, add, inactive, active);
        pools.Add(resultPool);
        return resultPool;
    }
}

