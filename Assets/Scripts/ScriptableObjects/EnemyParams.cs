using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParams", menuName = "VL/Gameplay/EnemyParams", order = 1)]
public class EnemyParams : ScriptableObject
{
    [Header("Shared")]

    public int HP = 5;
    public float StepTime = 1.0f;
    public float StepInterval = 0.33f;

    [Header("Jumping Enemy")]

    public float JumpIntervalMin = 3.0f;
    public float JumpIntervalMax = 5.0f;

    [Space]

    public float JumpHeight = 5.0f;
    public float JumpDistance = 3.0f;
    public float JumpTime = 1.0f;
    public float JumpWindupTime = 0.5f;

    public AnimationCurve JumpCurve = null;
}
