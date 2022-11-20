using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private int index = 0;
    public List<TextMeshProUGUI> storeMonsterName = new List<TextMeshProUGUI>();
    public List<Sprite> storePriceBoard = new List<Sprite>();
    public List<Image> buttonPriceBoard = new List<Image>();
    public List<ButtonFunc> buyButtons = new List<ButtonFunc>();
    private Dictionary<COST_TYPE, Sprite> costToSpriteDic = new Dictionary<COST_TYPE, Sprite>();


    private void Awake()
    {
        costToSpriteDic.Add(COST_TYPE.One, storePriceBoard[0]);
        costToSpriteDic.Add(COST_TYPE.Two, storePriceBoard[1]);
        costToSpriteDic.Add(COST_TYPE.Three, storePriceBoard[2]);
    }

    //상점 정보 보여주기
    public void StoreShowInfo(Monsters monster)
    {
        buttonPriceBoard[index].sprite = costToSpriteDic[monster.cost];
        storeMonsterName[index].text = monster.MonsterName;
        buyButtons[index].monsterType = monster.monsterType;
        index++;
        if (index > 4)
        {
            index = 0;
        }
    }

    public void ButtonInteractableInit()
    {
        for(int i = 0; i < buyButtons.Count; i++)
        {
            buyButtons[i].OnInteractable();
        }
    }
}
