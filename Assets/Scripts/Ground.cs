using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool isPlayerGround;
    private bool isSelected = false;
    public bool filledMonster = false;
    public GameObject tile;
    private MONSTER_TYPE monsterType;
    private void Awake()
    {
        if (isPlayerGround)
            GroundManager.Instance.groundAction += ShowTile;
    }

    //몬스터가 선택되었을 때 놓을 수 있는 위치 보여주기
    public void ShowTile()
    {
        if (!isSelected)
        {
            tile.SetActive(true);
            isSelected = true;
        }
        else
        {
            tile.SetActive(false);
            isSelected = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        filledMonster = true;
    }

    private void OnTriggerExit(Collider other)
    {
        filledMonster = false;
    }
}
