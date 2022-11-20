using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StoreManager.Instance.ReRollMonster();
        }
    }


}
