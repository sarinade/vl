﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponParams weaponParams = null;

    private float lastFireTimestamp = Mathf.NegativeInfinity;

    public void SetWeaponParams(WeaponParams parameters)
    {
        weaponParams = parameters;
    }

    public void Fire(Vector3 point, Vector3 direction, bool buttonDown)
    {
        if(!buttonDown)
        {
            if (!weaponParams.AllowContinuousFire)
                return;
        }

        if (Time.time - lastFireTimestamp < weaponParams.Interval)
            return;

        lastFireTimestamp = Time.time;

        for(int i = 0; i < weaponParams.ProjectileCount; i++)
        {
            float xSpread = Random.Range(-weaponParams.Spread, weaponParams.Spread);
            float ySpread = Random.Range(-weaponParams.Spread, weaponParams.Spread);
            Quaternion facing = Quaternion.LookRotation(direction + new Vector3(xSpread, ySpread, 0.0f));

            Projectile instance = PoolService.Instance.Spawn(weaponParams.ProjectilePrefab, point, facing);
            instance.SetWeaponParams(weaponParams);
        }
    }
}
