using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasStatable
{
    public void SetState(IState inputState);
    public object GetObj(); 
}

public interface IState
{
    public void StateEnter();
    public void StateExit();
    public void StateUpdate();
}

public interface IGrabable
{
}

public interface IPoolingable
{
    public ObjectPool home { get; set; }
}

public interface IDamagable
{
    public void TakeHit(float damage);
}