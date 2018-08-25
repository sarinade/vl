﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoPoolable
{
    private int hitMask;

    public float speed = 10.0f;

    [Space]

    public float radius = 0.33f;

    private float spawnTimestamp = 0.0f;
    private RaycastHit[] hits = new RaycastHit[10];

    private WeaponParams weaponParams = null;

    void Start()
    {
        hitMask = LayerMask.GetMask("Enemy", "Enviro");
    }

    public void SetWeaponParams(WeaponParams parameters)
    {
        weaponParams = parameters;
    }

    public override void Reinitialize()
    {
        enabled = true;
        spawnTimestamp = Time.time;
    }

    private void Update()
    {
        if(Time.time - spawnTimestamp >= weaponParams.projectileLifespan)
        {
            Dispose(0.0f);
            return;
        }

        Vector3 desiredPosition = transform.position + speed * transform.forward * Time.deltaTime;

        int hitCount = Physics.SphereCastNonAlloc(transform.position, radius, transform.forward, hits, speed * Time.deltaTime, hitMask);

        if (hitCount > 0)
        {
            for(int i = 0; i < hitCount; i++)
            {
                Enemy enemy = hits[i].transform.GetComponent<Enemy>();

                if(enemy != null)
                {
                    bool killingBlow;

                    enemy.Hit(weaponParams.Damage, out killingBlow);

                    if(killingBlow)
                    {
                        Dispose(0.05f);
                    }
                    else
                    {
                        Dispose(0.1f);
                    }

                    return;
                }
            }

            Dispose(0.0f);
        }
        else
        {
            transform.position = desiredPosition;
        }

    }

    private void Dispose(float delay)
    {
        enabled = false;
        Despawn(delay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
