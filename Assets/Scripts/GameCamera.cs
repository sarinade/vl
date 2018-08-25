using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : Singleton<GameCamera>
{
    #region Inspector

    [SerializeField]
    private CameraParams cameraParams = null;

    [SerializeField]
    private InputParams inputParams = null;

    #endregion

    private Camera gameCam = null;

    private float orbitRad = 0.0f;
    private Transform target = null;

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    public Camera GameCam
    {
        get
        {
            return gameCam;
        }
    }

    protected override void OnAwake()
    {
        gameCam = GetComponentInChildren<Camera>();
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis(inputParams.HorizontalAxis);
        orbitRad += horizontal * Time.deltaTime;

        Vector3 circlePoint = new Vector3(Mathf.Sin(orbitRad), 0.0f, Mathf.Cos(orbitRad));
        Vector3 desiredPosition = target.position + circlePoint * cameraParams.Radius + cameraParams.Height * Vector3.up;

        transform.position = desiredPosition;

        Vector3 toTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(toTarget);

        transform.rotation = lookRotation;
    }
}
