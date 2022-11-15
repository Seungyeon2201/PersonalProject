using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityAction groundAction;

    public void Select()
    {
        groundAction?.Invoke();
    }

}
