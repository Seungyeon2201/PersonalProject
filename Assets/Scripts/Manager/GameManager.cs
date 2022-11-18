using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    //public UnityAction groundAction;
    //public Ground[] grounds;

    //public void Select()
    //{
    //    groundAction?.Invoke();
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StoreManager.Instance.ReRollMonster();
        }
    }
    //public Vector3 FindBlank()
    //{
    //    for (int i = 0; i < grounds.Length; i++)
    //    {
    //        int index = i;
    //        if (grounds[i].filledMonster == false)
    //        {
    //            Debug.Log(index);
    //            return grounds[i].transform.position;
    //        }
    //    }
    //    return grounds[0].transform.position;
    //}
}
