using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MoneySetting")]
public class MoneySetting : ScriptableObject
{
    public int[] getMoneyPerStage;
    public int[] getMoneyPerWin;
    public int[] getMoneyPerMoney;
    public int reRollMoney;
    public int buyExpMoney;
}
