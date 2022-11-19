using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunc : MonoBehaviour
{
    public MONSTER_TYPE monsterType;
    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Buy()
    {
        StoreManager.Instance.BuyMonster(monsterType);
        button.interactable = false;
    }

    public void Sell()
    {
        Grabber.Instance.ClickForSell();
        Debug.Log("Test");
    }
    public void OnInteractable()
    {
        button.interactable = true;
    }
}
