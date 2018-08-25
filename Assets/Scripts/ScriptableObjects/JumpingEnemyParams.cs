using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpingEnemyParams", menuName = "VL/Gameplay/Enemies/JumpingEnemyParams", order = 1)]
public class JumpingEnemyParams : ScriptableObject
{
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
