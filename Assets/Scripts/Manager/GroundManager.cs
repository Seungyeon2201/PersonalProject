using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum GROUND_TYPE
{
    WaitingSeat, FightSeat
}

public class GroundManager : Singleton<GroundManager>
{
    public UnityAction groundAction;
    public Ground[] grounds;
    private Vector3 offSetHight = new Vector3 (0f, 1f, 0f);

    private void Awake()
    {
        CategorizeGround();
    }

    public void Select()
    {
        groundAction?.Invoke();
    }

    public Vector3 FindBlank()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            //int index = i;
            if (grounds[i].filledMonster == false)
            {
                return grounds[i].transform.position + offSetHight;
            }
        }
        return grounds[0].transform.position + offSetHight;
    }

    //몬스터 합성
    public Vector3 ReturnMonster(MONSTER_TYPE monsterTYPE, int upgradeCount)
    {
        int index = -1;
        for (int i = 0; i < grounds.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(grounds[i].transform.position, grounds[i].transform.up, out hit, 10f, 1<<LayerMask.NameToLayer("Monster")))
            {
                Ally monster = hit.transform.GetComponent<Ally>();
                if (monster.monsterType == monsterTYPE)
                {
                    if(monster.upgradeCount == upgradeCount)
                    {
                        MonsterManager.Instance.ReturnPool(hit.transform.gameObject);
                        grounds[i].filledMonster = false;
                        if(index < 0)
                        index = i;
                        if (grounds[i].groundType == GROUND_TYPE.FightSeat)
                            GameManager.Instance.CurPopulation--;
                    }
                }
            }
        }
        return grounds[index].transform.position + offSetHight;
    }

    //대기석과 싸우는 필드를 열거형으로 나눔
    public void CategorizeGround()
    {
        for (int i = 0; i < 8; i++)
        {
            grounds[i].groundType = GROUND_TYPE.WaitingSeat;
        }
        for(int i = 8; i< grounds.Length; i++)
        {
            grounds[i].groundType = GROUND_TYPE.FightSeat;
        }
    }

    public void SetMonterToFight()
    {
        for(int i = 0; i < grounds.Length;i++)
        {
            if (grounds[i].groundType == GROUND_TYPE.WaitingSeat && grounds[i].filledMonster == true)
            {
                for(int j = 0; j < grounds.Length; j++)
                {
                    if (grounds[j].groundType == GROUND_TYPE.FightSeat && grounds[j].filledMonster == false)
                    {
                        grounds[j].filledMonster = true;
                        grounds[i].filledMonster = false;
                        grounds[i].monsterTran.GetComponent<Monster>().canFight = true;
                        grounds[i].monsterTran.position = grounds[j].transform.position + offSetHight;
                        return;
                    }
                }
            }
        }
    }
}
