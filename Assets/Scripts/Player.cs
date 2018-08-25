using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxLookDistance = 45.0f;
    private float maxAimDistance = 100.0f;

    private int aimMask;

    #region Inspector

    [SerializeField]
    private InputParams inputParams = null;

    #endregion

    private Weapon weapon = null;

    void Start()
    {
        aimMask = LayerMask.GetMask("Enemy");
        weapon = GetComponent<Weapon>();

        GameCamera.Instance.Target = transform;
    }

    void Update()
    {
        Ray lookRay = GameCamera.Instance.GameCam.ScreenPointToRay(Input.mousePosition);
        Vector3 lookDirection;

        Plane floorPlane = new Plane(Vector3.up, Vector3.zero);
        float enterDistance;

        if (floorPlane.Raycast(lookRay, out enterDistance))
        {
            Vector3 enterPoint = lookRay.GetPoint(enterDistance);
            lookDirection = (enterPoint - transform.position).normalized;
        }
        else
        {
            lookDirection = (lookRay.GetPoint(maxLookDistance) - transform.position).normalized;
        }

        float clampedY = Mathf.Clamp(lookDirection.y, 0.0f, 1.0f);
        transform.forward = new Vector3(lookDirection.x, clampedY, lookDirection.z);

        if(Input.GetButton(inputParams.FireButton))
        {
            Vector3 projectileFacing = transform.forward;
            Vector3 projectileSpawnPoint = transform.position + transform.forward;

            RaycastHit fireAimHit;

            if(Physics.Raycast(lookRay, out fireAimHit, maxAimDistance, aimMask))
            {
                projectileFacing = (fireAimHit.point - projectileSpawnPoint).normalized;
            }

            bool buttonDown = Input.GetButtonDown(inputParams.FireButton);
            weapon.Fire(projectileSpawnPoint, projectileFacing, buttonDown);
        }
    }
}
