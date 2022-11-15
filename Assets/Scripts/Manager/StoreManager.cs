using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [Header("Re-Roll Probability")]
    [Range(0,100)]
    public float oneCost;
    [Range(0, 100)]
    public float twoCost;
    [Range(0, 100)]
    public float threeCost;

    private List<float> percentage = new List<float>();

   
}
