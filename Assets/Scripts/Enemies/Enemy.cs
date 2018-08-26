using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoPoolable
{
    private const string colorPropertyName = "_Color";
    private const float playerCollisionThreshold = 10.0f;
    private const float sweepTestOffset = 2.1f;

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

    #endregion

    private int hp;

    private new BoxCollider collider = null;
    private new Renderer renderer = null;
    private FlashEffect flashEffect = null;
    private Material baseMaterial = null;

    private Color color;

    private Vector3 initialPivotPosition;
    private Quaternion initialPivotRotation;

    private Vector3 initialBodyPosition;
    private Quaternion initialBodyRotation;

    void Awake()
    {
        collider = GetComponent<BoxCollider>();
        flashEffect = GetComponent<FlashEffect>();

        renderer = GetComponentInChildren<Renderer>();
        baseMaterial = renderer.material;

        color = baseMaterial.GetColor(colorPropertyName);
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
        hp = enemyParams.HP;

        collider.enabled = true;
        enabled = true;

        bodyPivot.localPosition = initialPivotPosition;
        bodyPivot.localRotation = initialPivotRotation;

        body.localPosition = initialBodyPosition;
        body.localRotation = initialBodyRotation;

        renderer.material.SetColor(colorPropertyName, color);

        transform.forward = (Player.Instance.transform.position - transform.position).normalized;
    }

    void OnEnable()
    {
        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator UpdateRoutine()
    {
        yield return null;

        while(true)
        {
            transform.forward = GetNextStepFacing();

            if(MathHelpers.CustomBoxCheck(transform.position, transform.forward, sweepTestOffset, transform.rotation, transform.localScale * 0.5f))
            {
                yield return collisionAvoidanceDelay;
                continue;
            }

            yield return StartCoroutine(GetSpecialBehavior());

            float elapsed = 0.0f;
            float time = enemyParams.StepTime;

            Quaternion desiredRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            Quaternion fromRotation = bodyPivot.localRotation;

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

            bodyPivot.localPosition = initialPivotPosition;
            body.position = bodyPosition;

            if(TryHitPlayer())
            {
                yield break;
            }

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

    protected bool TryHitPlayer()
    {
        float sqrMagToPlayer = (Player.Instance.transform.position - transform.position).sqrMagnitude;

        if (sqrMagToPlayer <= playerCollisionThreshold)
        {
            float normalizedHP = Mathf.Clamp01((float) hp / (float) enemyParams.HP);
            flashEffect.Flash(normalizedHP);

            Player.Instance.Hit(enemyParams.Damage);
            Dispose();

            return true;
        }

        return false;
    }

    public void Hit(int damage, out bool dead)
    {
        hp -= damage;

        float normalizedHP = Mathf.Clamp01((float)hp / (float) enemyParams.HP);
        flashEffect.Flash(normalizedHP);

        if (hp <= 0)
        {
            Dispose();
            dead = true;

            OnKill();
        }
        else
        {
            dead = false;
        }
    }

    protected virtual void OnKill() { }

    public void Dispose()
    {
        GameLoop.Instance.OnEnemyKilled();

        collider.enabled = false;
        enabled = false;

        StartCoroutine(DespawnRoutine());
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(0.075f);

        Despawn(0.0f);
    }
}
