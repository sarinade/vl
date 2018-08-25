using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZigZagEnemySpecialParams", menuName = "VL/Gameplay/Enemies/ZigZagEnemyParams", order = 1)]
public class ZigZagEnemyParams : ScriptableObject
{
    public int StepsPerDir = 3;
    public EZigZagEnemyDir[] DirectionQueue = null;
}
