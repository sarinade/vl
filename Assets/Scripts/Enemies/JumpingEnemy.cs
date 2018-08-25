using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    private YieldInstruction windup;

    private float jumpTimestamp = 0.0f;
    private float nextJumpInteval;

    #region Inspector

    [SerializeField]
    private JumpingEnemyParams specialParams = null;

    #endregion

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
            transform.position = GetQuadraticBezierPoint(from, peak, destination, specialParams.JumpCurve.Evaluate(normalizedTime));

            if(TryHitPlayer())
            {
                yield break;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }

        transform.position = destination;
        yield return windup;
    }

    //https://catlikecoding.com/unity/tutorials/curves-and-splines/
    public static Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }
}
