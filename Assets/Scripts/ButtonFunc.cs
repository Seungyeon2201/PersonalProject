using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunc : MonoBehaviour
{
    public MONSTER_TYPE monsterType;
    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    //몬스터 사면 몬스터 소환 및 버튼 비활성화
    public void Buy()
    {
        StoreManager.Instance.BuyMonster(monsterType);
        button.interactable = false;
    }

    //몬스터 팔기
    public void Sell()
    {
        Grabber.Instance.ClickForSell();
    }

    //상점 리롤시 버튼 활성화
    public void OnInteractable()
    {
        button.interactable = true;
    }
}
