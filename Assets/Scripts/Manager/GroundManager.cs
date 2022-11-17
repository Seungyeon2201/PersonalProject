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

    public Vector3 FindSameMonster(MONSTER_TYPE mONSTER_TYPE)
    {
        int index = -1;
        for (int i = 0; i < grounds.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(grounds[i].transform.position, grounds[i].transform.up, out hit, 10f))
            {
                if (hit.transform.GetComponent<Monster>().monsterType == mONSTER_TYPE)
                {
                    hit.transform.GetComponent<Monster>().home.Return(hit.transform.gameObject);
                    grounds[i].filledMonster = false;
                    if (index < 0)
                        index = i;
                }
            }
        }
        return grounds[index].transform.position;
    }
}
