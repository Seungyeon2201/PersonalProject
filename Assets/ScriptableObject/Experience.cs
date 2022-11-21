using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Experience")]
public class Experience : ScriptableObject
{
    public int[] needExperience;
    public int getExperiencePerStage;
    public int pushLevelUp;
}
