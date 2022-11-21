using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/StoreProbability")]
public class Store : ScriptableObject
{
    [System.Serializable]
    public class ReRollProb
    {
        [Header("Re-Roll Probability")]
        [SerializeField, Range(0, 100)] private float oneCost;
        public float OneCost { get { return oneCost; } }
        [SerializeField, Range(0, 100)] private float twoCost;
        public float TwoCost { get { return twoCost; } }
    }
    [SerializeField]
    private ReRollProb[] reRollProbs;
    public ReRollProb[] ReRollProbs { get { return reRollProbs; } }
}
