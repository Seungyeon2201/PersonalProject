using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COST_TYPE
{
    One, Two ,Three
}

public enum MONSTER_TYPE
{
    Bat, Ghost, Scorpion
}

[CreateAssetMenu(menuName = "ScriptableObject/Monsters")]
public class Monsters : ScriptableObject
{
    //[Header("Level Monster")] // 구분자
    public COST_TYPE cost;
    public MONSTER_TYPE monsterType;
    public GameObject monsterprefab;
    public int index;
    [SerializeField] private string monsterName;
    public string MonsterName { get { return monsterName; } }
}
