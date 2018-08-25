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
    private int directionIndex = 0;
    private int steps = 0;

    #region Inspector

    [SerializeField]
    private ZigZagEnemyParams specialParams = null;

    #endregion

    public override void Reinitialize()
    {
        steps = 0;
        directionIndex = 0;

        base.Reinitialize();
    }

    protected override IEnumerator GetSpecialBehavior()
    {
        int maxSteps = (int) Mathf.Clamp(specialParams.StepsPerDir - 1, 1, Mathf.Infinity);

        if (steps < maxSteps)
        {
            steps++;
            yield break;
        }

        directionIndex = (directionIndex + 1) % specialParams.DirectionQueue.Length;
        steps = 0;
    }

    protected override Vector3 GetNextStepFacing()
    {
        EZigZagEnemyDir direction = specialParams.DirectionQueue[directionIndex];
        return EnumToFacing(direction);
    }

    private Vector3 EnumToFacing(EZigZagEnemyDir dir)
    {
        Vector3 forward = base.GetNextStepFacing();
        Vector3 result;

        if (dir == EZigZagEnemyDir.Forward)
        {
            result = forward;
        }
        else if (dir == EZigZagEnemyDir.Left)
        {
            result = Vector3.Cross(forward, Vector3.up);
        }
        else
        {
            result = Vector3.Cross(forward, Vector3.down);
        }

        return result;
    }
}
