using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isSelected = false;
    public bool filledMonster = false;
    public bool isPlayerGround;
    public GameObject tile;
    public Transform monsterTran;
    public GROUND_TYPE groundType;

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

    //내 몬스터랑 적 몬스터 구분하기
    private void OnTriggerEnter(Collider other)
    {
        monsterTran = other.gameObject.transform;
        filledMonster = true;
        if (groundType == GROUND_TYPE.FightSeat)
        {
            GameManager.Instance.CurPopulation++;
            other.GetComponent<Ally>().canFight = true;
            Debug.Log(other.GetComponent<Ally>().canFight);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        filledMonster = false;
        if (groundType == GROUND_TYPE.FightSeat)
        {
            GameManager.Instance.CurPopulation--;
            other.GetComponent<Ally>().canFight = false;
        }
            
    }

    public void ChangePosition(Vector3 selectPosition)
    {
        monsterTran.position = selectPosition;
    }

}
