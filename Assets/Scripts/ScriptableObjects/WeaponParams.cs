using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon01", menuName = "VL/Gameplay/New Weapon", order = 1)]
public class WeaponParams : ScriptableObject
{
    public string Name = "Gun";

    [Space]

    public bool AllowContinuousFire = false;

    [Space]

    public int Damage = 1;
    public float projectileLifespan = 2.0f;
    public float Interval = 0.1f;

    [Space]

    public float ProjectileCount = 1;
    public float Spread = 0.1f;

    [Space]

    public Projectile ProjectilePrefab = null;
}
