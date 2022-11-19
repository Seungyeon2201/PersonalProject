using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHasStatable
{
    public object GetObj()
    {
        return this;
    }

    public void SetState(IState inputState)
    {
        
    }

}
