using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SharedEnemyParams", menuName = "VL/Gameplay/Enemies/SharedParams", order = 1)]
public class SharedEnemyParams : ScriptableObject
{
    public int HP = 5;
    public float StepTime = 1.0f;
    public float StepInterval = 0.33f;
}
