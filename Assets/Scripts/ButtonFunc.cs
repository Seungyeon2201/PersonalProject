using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc : MonoBehaviour
{
    public MONSTER_TYPE monsterType;

    public void Buy()
    {
        StoreManager.Instance.BuyMonster(monsterType);
    }
}
