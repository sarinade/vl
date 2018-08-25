﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoPoolable
{
    private string colorPropertyName = "_Color";
    private int enemyMaskLayerMask;
    private const float playerCollisionThreshold = 10.0f;

    private YieldInstruction collisionAvoidanceDelay = new WaitForSeconds(1.0f);
    private YieldInstruction stepInterval;

    #region Inspector

    [SerializeField]
    protected SharedEnemyParams enemyParams = null;

    [Space]

    [SerializeField]
    private Transform bodyPivot = null;

    [SerializeField]
    private Transform body = null;

    [Space]

    [SerializeField]
    private Material hitMaterial = null;

    #endregion

    private int baseHp;
    private int hp;

    private new BoxCollider collider = null;
    private new Renderer renderer = null;
    private Material baseMaterial = null;

    private Color color;

    private Vector3 initialPivotPosition;
    private Quaternion initialPivotRotation;

    private Vector3 initialBodyPosition;
    private Quaternion initialBodyRotation;

    void Awake()
    {
        enemyMaskLayerMask = LayerMask.GetMask("Enemy");

        collider = GetComponent<BoxCollider>();

        renderer = GetComponentInChildren<Renderer>();
        baseMaterial = renderer.material;

        color = baseMaterial.GetColor(colorPropertyName);
        baseHp = enemyParams.HP;
        hp = enemyParams.HP;

        stepInterval = new WaitForSeconds(enemyParams.StepInterval);

        initialPivotPosition = bodyPivot.localPosition;
        initialPivotRotation = bodyPivot.localRotation;

        initialBodyPosition = body.localPosition;
        initialBodyRotation = body.localRotation;

        OnAwake();
    }

    protected virtual void OnAwake() { }

    public override void Reinitialize()
    {
        hp = baseHp;

        collider.enabled = true;
        enabled = true;

        bodyPivot.localPosition = initialPivotPosition;
        bodyPivot.localRotation = initialPivotRotation;

        body.localPosition = initialBodyPosition;
        body.localRotation = initialBodyRotation;

        renderer.material.SetColor(colorPropertyName, color);
    }

    void OnEnable()
    {
        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator UpdateRoutine()
    {
        while(true)
        {
            if(Physics.CheckBox(transform.position + transform.forward * collider.bounds.size.z * 1.1f, collider.size * 0.5f, transform.rotation, enemyMaskLayerMask))
            {
                yield return collisionAvoidanceDelay;
                continue;
            }

            Vector3 test = GetNextStepFacing();
            transform.forward = test;

            yield return GetSpecialBehavior();

            float elapsed = 0.0f;
            float time = enemyParams.StepTime;

            Quaternion desiredRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            Quaternion fromRotation = bodyPivot.localRotation;

            Vector3 pivotOffset = (body.position - bodyPivot.position) * 2.0f;

            while(elapsed < time)
            {
                Quaternion rotation = Quaternion.Slerp(fromRotation, desiredRotation, elapsed / time);
                bodyPivot.localRotation = rotation;

                Vector3 newPosition = new Vector3(body.position.x, transform.position.y, body.position.z);

                Vector3 movementThisFrame = newPosition - transform.position;
                transform.position = newPosition;
                bodyPivot.position -= movementThisFrame;

                yield return null;
                elapsed += Time.deltaTime;
            }

            bodyPivot.localRotation = desiredRotation;

            Vector3 bodyPosition = body.position;
            Quaternion bodyRotation = body.rotation;

            bodyPivot.localRotation = Quaternion.identity;
            body.rotation = bodyRotation;

            bodyPivot.position -= new Vector3(pivotOffset.x, 0.0f, pivotOffset.z);
            body.position = bodyPosition;

            TryHitPlayer();

            yield return stepInterval;
        }
    }

    protected virtual IEnumerator GetSpecialBehavior()
    {
        yield break;
    }

    protected virtual Vector3 GetNextStepFacing()
    {
        Vector3 dir = (Player.Instance.transform.position - transform.position).normalized;
        return new Vector3(dir.x, 0.0f, dir.z);
    }

    protected void TryHitPlayer()
    {
        float sqrMagToPlayer = (Player.Instance.transform.position - transform.position).sqrMagnitude;

        if (sqrMagToPlayer <= playerCollisionThreshold)
        {
            StartCoroutine(FlashRoutine());
            Player.Instance.Hit();
            Dispose();
        }
    }

    public void Hit(int damage, out bool dead)
    {
        hp -= damage;
        StartCoroutine(FlashRoutine());

        if (hp <= 0)
        {
            Dispose();
            dead = true;
        }
        else
        {
            dead = false;
        }
    }

    public void Dispose()
    {
        collider.enabled = false;
        enabled = false;

        StartCoroutine(DespawnRoutine());
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(0.075f);

        Despawn(0.0f);
    }

    private IEnumerator FlashRoutine()
    {
        renderer.material = hitMaterial;

        yield return new WaitForSeconds(0.033f);

        renderer.material = baseMaterial;

        float colorFadeRate = 1.0f - Mathf.Clamp01((float) hp / (float) baseHp);
        Color newColor = Color.Lerp(color, Color.white, colorFadeRate);
        renderer.material.SetColor(colorPropertyName, newColor);
    }
}