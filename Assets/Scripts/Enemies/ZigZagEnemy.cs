using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EZigZagEnemyDir
{
    Forward = 0,
    Left,
    Right
}

public class ZigZagEnemy : Enemy
{
    private EZigZagEnemyDir nextDir = EZigZagEnemyDir.Forward;

    private int nextDirectionIndex = 0;
    private int steps = 0;

    #region Inspector

    [SerializeField]
    private ZigZagEnemyParams specialParams = null;

    #endregion

    public override void Reinitialize()
    {
        base.Reinitialize();
        steps = 0;
        nextDirectionIndex = 0;
    }

    protected override IEnumerator GetSpecialBehavior()
    {
        if (steps < specialParams.StepsPerDir)
        {
            steps++;
            yield break;
        }

        nextDirectionIndex = (nextDirectionIndex + 1) % specialParams.DirectionQueue.Length;
        steps = 0;
    }

    protected override Vector3 GetNextStepFacing()
    {
        EZigZagEnemyDir nextDir = specialParams.DirectionQueue[nextDirectionIndex];

        Vector3 forward = base.GetNextStepFacing();

        if (nextDir == EZigZagEnemyDir.Forward)
        {
            return forward;
        }
        else if(nextDir == EZigZagEnemyDir.Left)
        {
            return Vector3.Cross(forward, Vector3.up);
        }
        else
        {
            return Vector3.Cross(forward, Vector3.down);
        }
    }
}
