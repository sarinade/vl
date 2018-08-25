using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneParams", menuName = "VL/SceneParams", order = 1)]
public class SceneParams : ScriptableObject
{
    public int MenuSceneIndex = 0;
    public int GameSceneIndex = 1;
}
