using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxLookDistance = 25.0f;
    private float maxAimDistance = 100.0f;

    private int aimMask;

    #region Inspector

    [SerializeField]
    private LoadoutParams loadout = null;

    [Space]

    [SerializeField]
    private InputParams inputParams = null;

    #endregion

    private Weapon weapon = null;
    private int weaponIndex = 0;

    void Start()
    {
        aimMask = LayerMask.GetMask("Enemy");

        weapon = GetComponent<Weapon>();
        SetWeapon(loadout.StartingWeaponIndex);

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

        for(int i = (int) KeyCode.Alpha1; i < (int) KeyCode.Alpha9; i++)
        {
            if(Input.GetKeyDown((KeyCode) i))
            {
                SetWeapon(i - (int) KeyCode.Alpha1);
            }
        }

        if(Input.mouseScrollDelta.y != 0.0f)
        {
            int weaponCount = loadout.GetWeaponCount();
            weaponIndex = (weaponIndex - (int) Mathf.Sign(Input.mouseScrollDelta.y)) % weaponCount;

            if(weaponIndex < 0)
            {
                weaponIndex = weaponCount - 1;
            }

            SetWeapon(weaponIndex);
        }
    }

    private void SetWeapon(int index)
    {
        weaponIndex = index;
        WeaponParams newWeapon = loadout.GetWeapon(weaponIndex);

        weapon.SetWeaponParams(newWeapon);
        HUD.Instance.SetWeaponNameLabel(newWeapon.Name);
    }
}
