using System;
using UnityEngine;

public class CanvasRotator : MonoBehaviour
{
    const float PositionChangeThreshold = 0.001f;

    Camera targetCamera;
    Vector3 lastCameraPosition;
    Quaternion lastCameraRotation;

    enum LockAxis
    {
        None,
        X,
        Y
    }

    [SerializeField] LockAxis lockAxis;

    public void Init(Camera targetCamera)
    {
        this.targetCamera = targetCamera;
        UpdateRotation();
    }

    void Update()
    {
        if (!HasCameraTransformChanged()) return;

        UpdateRotation();
    }

    bool HasCameraTransformChanged()
    {
        Transform camTransform = targetCamera.transform;
        bool hasChanged = (camTransform.position - lastCameraPosition).sqrMagnitude > PositionChangeThreshold
                       || camTransform.rotation != lastCameraRotation;

        if (hasChanged)
        {
            lastCameraPosition = camTransform.position;
            lastCameraRotation = camTransform.rotation;
        }

        return hasChanged;
    }

    void UpdateRotation()
    {
        Vector3 directionToCamera = transform.position - targetCamera.transform.position;

        Vector3 lockedDirection = lockAxis switch
        {
            LockAxis.None => directionToCamera,
            LockAxis.X => new Vector3(0f, directionToCamera.y, directionToCamera.z),
            LockAxis.Y => new Vector3(directionToCamera.x, 0f, directionToCamera.z),
            _ => throw new ArgumentOutOfRangeException()
        };

        transform.rotation = Quaternion.LookRotation(lockedDirection);
    }
}
