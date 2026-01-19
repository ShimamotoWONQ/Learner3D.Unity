using System;
using UnityEngine;

public class CanvasRotator : MonoBehaviour
{

    Camera targetCamera;

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
    }

    void Update()
    {
        Vector3 CamToComment = transform.position - targetCamera.transform.position;

        Vector3 locked = lockAxis switch
        {
            LockAxis.None => CamToComment,
            LockAxis.X => new Vector3(0.0f, CamToComment.y, CamToComment.z),
            LockAxis.Y => new Vector3(CamToComment.x, 0.0f, CamToComment.z),
            _ => throw new ArgumentOutOfRangeException()
        };

        transform.rotation = Quaternion.LookRotation(locked);
    }
}
