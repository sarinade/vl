using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    private YieldInstruction windup;

    #region Inspector

    [SerializeField]
    private JumpingEnemyParams specialParams = null;

    #endregion

    private float jumpTimestamp = 0.0f;
    private float nextJumpInteval;

    protected override void OnAwake()
    {
        nextJumpInteval = Random.Range(specialParams.JumpIntervalMin, specialParams.JumpIntervalMax);
        windup = new WaitForSeconds(specialParams.JumpWindupTime);
    }

    public override void Reinitialize()
    {
        base.Reinitialize();

        jumpTimestamp = Time.time;
        nextJumpInteval = Random.Range(specialParams.JumpIntervalMin, specialParams.JumpIntervalMax);
    }

    protected override IEnumerator GetSpecialBehavior()
    {
        if (Time.time - jumpTimestamp < nextJumpInteval)
            yield break;

        jumpTimestamp = Time.time;
        nextJumpInteval = Random.Range(specialParams.JumpIntervalMin, specialParams.JumpIntervalMax);

        Vector3 from = transform.position;
        Vector3 peak = transform.position + transform.forward * specialParams.JumpDistance * 0.5f + Vector3.up * specialParams.JumpHeight;
        Vector3 destination = transform.position + transform.forward * specialParams.JumpDistance;

        float elapsed = 0.0f;

        while(elapsed < specialParams.JumpTime)
        {
            float normalizedTime = elapsed / specialParams.JumpTime;
            transform.position = MathHelpers.GetQuadraticBezierPoint(from, peak, destination, specialParams.JumpCurve.Evaluate(normalizedTime));

            yield return null;
            elapsed += Time.deltaTime;
        }

        transform.position = destination;

        if (TryHitPlayer())
        {
            yield break;
        }

        yield return windup;
    }
}
