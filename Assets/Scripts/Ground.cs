using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool isPlayerGround;
    private bool isSelected = false;
    public GameObject tile;

    private void Awake()
    {
        if (isPlayerGround)
            GameManager.Instance.groundAction += Select;
    }

    public void Select()
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

}
