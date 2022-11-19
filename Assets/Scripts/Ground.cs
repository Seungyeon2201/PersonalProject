using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isSelected = false;
    public bool isPlayerGround;
    public bool filledMonster = false;
    public GameObject tile;
    public Transform monsterTran;
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
        monsterTran = other.gameObject.transform;
        filledMonster = true;
    }

    private void OnTriggerExit(Collider other)
    {
        filledMonster = false;
    }

    public void ChangePosition(Vector3 selectPosition)
    {
        monsterTran.position = selectPosition;
        //monsterGameObj.transform.position = selectTransform.position;
    }
}
