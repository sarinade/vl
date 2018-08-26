using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelpers
{
    //https://catlikecoding.com/unity/tutorials/curves-and-splines/
    public static Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static bool CustomBoxCheck(Vector3 position, Vector3 forward, float forwardOffset, Quaternion orientation, Vector3 halfExtents)
    {
        int enemyMaskLayerMask = LayerMask.GetMask("Enemy");

        if (Physics.CheckBox(position + forward * halfExtents.z * forwardOffset, halfExtents, orientation, enemyMaskLayerMask))
            return true;

        return false;
    }
}
