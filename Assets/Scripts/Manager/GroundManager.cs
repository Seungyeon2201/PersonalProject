using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GroundManager : Singleton<GroundManager>
{
    public UnityAction groundAction;
    public Ground[] grounds;
    
    

    public void Select()
    {
        groundAction?.Invoke();
    }

    public Vector3 FindBlank()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            int index = i;
            if (grounds[i].filledMonster == false)
            {
                return grounds[i].transform.position;
            }
        }
        return grounds[0].transform.position;
    }

    public Vector3 ReturnMonster(MONSTER_TYPE monsterTYPE, int upgradeCount)
    {
        int index = -1;
        for (int i = 0; i < grounds.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(grounds[i].transform.position, grounds[i].transform.up, out hit, 10f))
            {
                Monster monster = hit.transform.GetComponent<Monster>();
                if (hit.transform.GetComponent<Monster>().monsterType == monsterTYPE)
                {
                    if(hit.transform.GetComponent<Monster>().upgradeCount == upgradeCount)
                    {
                        MonsterManager.Instance.ReturnPool(hit.transform.gameObject);
                        grounds[i].filledMonster = false;
                        if(index < 0)
                        index = i;
                    }
                }
            }
        }
        return grounds[index].transform.position + new Vector3(0, 1f, 0);
    }
}
