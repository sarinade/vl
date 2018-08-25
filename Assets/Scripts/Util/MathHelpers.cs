using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelpers
{
    public static bool CustomBoxCheck(Vector3 position, Vector3 forward, float forwardOffset, Quaternion orientation, Vector3 halfExtents)
    {
        int enemyMaskLayerMask = LayerMask.GetMask("Enemy");

        if (Physics.CheckBox(position + forward * halfExtents.z * forwardOffset, halfExtents, orientation, enemyMaskLayerMask))
            return true;

        return false;
    }
}
