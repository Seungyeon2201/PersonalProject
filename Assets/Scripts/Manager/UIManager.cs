using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    private int index = 0;
    public List<TextMeshProUGUI> storeMonsterName = new List<TextMeshProUGUI>();
    public List<ButtonFunc> buttonMonsters = new List<ButtonFunc>();


    //상점 정보 보여주기
    public void StoreShowInfo(Monsters monster)
    {
        storeMonsterName[index].text = monster.MonsterName;
        buttonMonsters[index].monsterType = monster.monsterType;
        index++;
        if(index > 4)
        {
            index = 0;
        }
    }
}
