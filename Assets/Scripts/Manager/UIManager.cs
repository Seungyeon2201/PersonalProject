using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    private int index = 0;
    public List<TextMeshProUGUI> storeMonsterName = new List<TextMeshProUGUI>();
    public List<ButtonFunc> buttonMonsters = new List<ButtonFunc>();

    public void StoreShowInfo(Monsters monster)
    {
        storeMonsterName[index].text = monster.MonsterName;
        buttonMonsters[index].monsters = monster;
        index++;
        if(index > 4)
        {
            index = 0;
        }
    }
}
