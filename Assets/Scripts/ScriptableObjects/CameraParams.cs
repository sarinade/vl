using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraParams", menuName = "VL/Gameplay/CameraParams", order = 1)]
public class CameraParams : ScriptableObject
{
    public float Height = 3.0f;
    public float Radius = 10.0f;
    public float RotationSpeed = 5.0f;
}
