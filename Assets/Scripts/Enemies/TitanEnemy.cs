using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanEnemy : Enemy
{
    protected override void OnKill()
    {
        GameLoop.Instance.EndGame(1.0f, true);
    }
}
