using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum COST_TYPE
{
    One = 1, Two = 2, Three = 3
}

public enum MONSTER_TYPE
{
    Bat, Ghost, Scorpion
}

public enum SKILL_TYPE
{
    
}

public enum TEAM_TYPE
{
    Ally, Enemy
}

[CreateAssetMenu(menuName = "ScriptableObject/Monsters")]
public class Monsters : ScriptableObject
{
    //[Header("Level Monster")] // 구분자
    public COST_TYPE cost;
    public MONSTER_TYPE monsterType;
    public GameObject monsterprefab;
    public Sprite monsterSprite;
    [SerializeField] private string monsterName;
    public string MonsterName { get { return monsterName; } }
    [SerializeField] private float atkDamage;
    public float AtkDamage { get { return atkDamage; } }
    [SerializeField] private float hp;
    public float HP { get { return hp; } }
}
