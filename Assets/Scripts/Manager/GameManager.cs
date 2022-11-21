using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public MoneySetting moneySetting;
    public Experience experience;
    public UnityAction expAction;
    public UnityAction goldAction;
    public UnityAction storeLevelUpAction;
    public UnityAction pupulationAction;
    [SerializeField]
    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldAction();
        }
    }
    //private int hp = 50;
    //public int Hp { get { return hp; } }
    private int curPopulation = 0;
    public int CurPopulation
    {
        get { return curPopulation; }
        set
        {
            curPopulation = value;
            pupulationAction();
        }
    }
    public int totalPopulation = 1;
    private int curExp = 0;
    public int CurExp
    {
        get { return curExp; }
        set
        {
            curExp = value;
            expAction();
            Debug.Log("경험치 획득");
            if(CurExp >= needExp)
            {
                CurExp -= needExp;
                totalPopulation++;
                storeLevelUpAction();
                Debug.Log("레벨업!");
            }
        }
    }
    public int needExp;

    //매 라운드 시작시 획득하는 기본골드 2라 2골 3 2골 4 3골드 2-1 4골드 나머지 5골드

    private void Awake()
    {
        needExp = experience.needExperience[StoreManager.Instance.storeLevel];
        storeLevelUpAction += LevelUp;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            StoreManager.Instance.ReRollMonster();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            //최고 레벨 예외처리 하기
            BuyExp();
        }
    }

    public void BuyExp() //LevelUp 버튼 만들면 추가
    {
        if (Gold < moneySetting.buyExpMoney) return;
        CurExp += experience.pushLevelUp;
        Gold -= moneySetting.buyExpMoney;
    }

    public void LevelUp()
    {
        needExp = experience.needExperience[StoreManager.Instance.storeLevel];
        expAction();
    }
}
