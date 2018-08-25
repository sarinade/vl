using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanEnemy : Enemy
{
    protected override void OnKill()
    {
        HUD.Instance.ShowGameEndPanel(true);
    }
}
