using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    [SerializeField]
    public GameObject baseObj { get; private set; }
    private int add;
    private Stack<GameObject> pool = new Stack<GameObject>();
    private Transform inactive;
    private Transform active;

    // ObjectPool 생성자, 생성시 입력받은대로 세팅 후 초기값만큼 풀을 채운 후 ObjectPool을 반환
    public ObjectPool(GameObject baseObj, int start, int add, Transform inactive, Transform active)
    {
        this.baseObj = baseObj;
        this.add = add;
        this.inactive = inactive;
        this.active = active;
        PoolAdd(start);
    }


    //ObjectPool에 seed값만큼 baseObj의 복제본 생성후 보관, 복제본 생성시 풀링 인터페이스를 가져와서 생성한 오브젝트 풀의 정보를 입력(해당 오브젝트가 생성된 풀에 Return하기 위해 필요)
    private void PoolAdd(int seed)
    {
        for (int i = 0; i < seed; i++)
        {
            GameObject createdObj = GameObject.Instantiate(baseObj, inactive);
            createdObj.TryGetComponent(out IPoolingable poolingable);
            poolingable.home = this;
            pool.Push(createdObj);
        }
    }

    //ObjectPool에 Return시킴. 생성시 입력한 ObjectPool에 대한 정보로 호출해서 반환할 수 있도록 public으로 선언
    public void Return(GameObject go)
    {
        go.transform.SetParent(inactive, false);
        go.transform.position = Vector3.zero;
        pool.Push(go);
    }

    //ObjectPool에서 오브젝트 호출 및 호출 관련 오버로딩
    public Transform Call(Vector3 position, Quaternion rotate, Transform parent, bool worldPositonStay, bool isMove)
    {
        if (pool.Count == 0)
        {
            PoolAdd(add);
        }
        pool.Pop().TryGetComponent(out Transform Objtransform);
        if (isMove)
        {
            Objtransform.position = position;
        }
        Objtransform.rotation = rotate;
        if (parent != null)
        {
            Objtransform.SetParent(parent, worldPositonStay);
        }
        else
        {
            Objtransform.SetParent(active, false);
        }
        return Objtransform;
    }
    public Transform Call(Quaternion rotate) => Call(Vector3.zero, rotate, null, false, false);
    public Transform Call(Transform parent, bool worldPositonStay) => Call(Vector3.zero, Quaternion.identity, parent, worldPositonStay, false);
    public Transform Call(Quaternion rotate, Transform parent, bool worldPositonStay) => Call(Vector3.zero, rotate, parent, worldPositonStay, false);
    public Transform Call(Vector3 position) => Call(position, Quaternion.identity, null, false, true);
    public Transform Call(Vector3 position, Quaternion rotate) => Call(position, rotate, null, false, true);
    public Transform Call(Vector3 position, Transform parent, bool worldPositonStay) => Call(position, Quaternion.identity, parent, worldPositonStay, true);
    public Transform Call(Vector3 position, Quaternion rotate, Transform parent, bool worldPositonStay) => Call(position, rotate, parent, worldPositonStay, true);

}
