using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreManager : Singleton<StoreManager>
{
    [Header("Re-Roll Probability")]
    [Range(0,100)]
    public float oneCost;
    [Range(0, 100)]
    public float twoCost;
    [Range(0, 100)]
    public float threeCost;

    private List<float> percentage = new List<float>();
    private List<ScriptableObject> pickMonsterList = new List<ScriptableObject>();

    private void Awake()
    {
        ReRollMonster();
        Debug.Log("test");
    }

    //MonsterManager에 있는 ScriptableObject의 개수에서 랜덤으로 5개(상점에 나타나는 기물 개수)
    public void ReRollMonster()
    {
        for (int i = 0; i < 5; i++)
        {
            int ran = Random.Range(0, MonsterManager.Instance.scriptableObjects.Count);
            UIManager.Instance.StoreShowInfo(MonsterManager.Instance.scriptableObjects[ran]);
        }
    }

    //팔때 1성으로 되돌려서 풀에 집어넣기
}
