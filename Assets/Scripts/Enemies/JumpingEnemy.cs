using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    private YieldInstruction windup;

    private float jumpTimestamp = 0.0f;
    private float nextJumpInteval;

    protected override void OnAwake()
    {
        nextJumpInteval = Random.Range(enemyParams.JumpIntervalMin, enemyParams.JumpIntervalMax);
        windup = new WaitForSeconds(enemyParams.JumpWindupTime);
    }

    public override void Reinitialize()
    {
        base.Reinitialize();

        jumpTimestamp = Time.time;
        nextJumpInteval = Random.Range(enemyParams.JumpIntervalMin, enemyParams.JumpIntervalMax);
    }

    protected override IEnumerator GetSpecialBehavior()
    {
        if (Time.time - jumpTimestamp < nextJumpInteval)
            yield break;

        jumpTimestamp = Time.time;
        nextJumpInteval = Random.Range(enemyParams.JumpIntervalMin, enemyParams.JumpIntervalMax);

        Vector3 from = transform.position;
        Vector3 peak = transform.position + transform.forward * enemyParams.JumpDistance * 0.5f + Vector3.up * enemyParams.JumpHeight;
        Vector3 destination = transform.position + transform.forward * enemyParams.JumpDistance;

        float elapsed = 0.0f;

        while(elapsed < enemyParams.JumpTime)
        {
            float normalizedTime = elapsed / enemyParams.JumpTime;
            transform.position = GetQuadraticBezierPoint(from, peak, destination, enemyParams.JumpCurve.Evaluate(normalizedTime));

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
