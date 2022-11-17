using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc : MonoBehaviour
{
    public Monsters monsters;

    public void Buy()
    {
        MonsterManager.Instance.SummonMonster(monsters, GroundManager.Instance.FindBlank() + new Vector3(0f, 1f, 0f));
    }
}
