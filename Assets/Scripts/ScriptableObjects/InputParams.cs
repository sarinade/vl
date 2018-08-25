using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputParams", menuName = "VL/Gameplay/InputParams", order = 1)]
public class InputParams : ScriptableObject
{
    [Header("ButtonNames")]

    public string HorizontalAxis = "Horizontal";
    public string FireButton = "Fire1";
}
