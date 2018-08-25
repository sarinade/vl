using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        GameCamera.Instance.Target = transform;
    }

    void Update()
    {
        Ray lookRay = GameCamera.Instance.GameCam.ScreenPointToRay(Input.mousePosition);
        Vector3 lookDirection = lookRay.direction;

        Plane floorPlane = new Plane(Vector3.up, Vector3.zero);
        float enterDistance;

        if (floorPlane.Raycast(lookRay, out enterDistance))
        {
            Vector3 enterPoint = lookRay.GetPoint(enterDistance);
            lookDirection = (enterPoint - transform.position).normalized;
        }

        float clampedY = Mathf.Clamp(lookDirection.y, 0.0f, 1.0f);
        transform.forward = new Vector3(lookDirection.x, clampedY, lookDirection.z);
    }
}
