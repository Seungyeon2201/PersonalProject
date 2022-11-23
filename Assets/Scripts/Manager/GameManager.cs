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
    public int needExp;
    public int totalPopulation = 1;
    private bool tempBool = false;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(tempBool == false)
            {
                StageManager.Instance.startBattleAction();
                tempBool = true;
            }
            else
            {
                StageManager.Instance.endBattleAction();
                tempBool = false;
            }
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
