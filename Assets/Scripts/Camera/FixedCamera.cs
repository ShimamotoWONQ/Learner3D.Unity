using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class FixedCamera : MonoBehaviour
{
    public new Camera camera;
    public bool doDisableInput;
    public float mouseSensitivity = 3.0f;
    public float moveSpeed = 10.0f;

    Rigidbody rigidBody;
    float rotationX;
    float rotationY;
    float movementHorizontal;
    float movementVertical;
    // readonly float maxAngleX = 30.0f;
    // readonly float maxAngleY = 40.0f;

    void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Init()
    {
        doDisableInput = false;
        rigidBody.useGravity = false;
    }

    void Update()
    {
        if (doDisableInput) return;

        // Rotate Input
        rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Move Input
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if (doDisableInput) return;

        RotateCamera();

        MoveCamera();
    }

    void RotateCamera()
    {
        camera.transform.Rotate(-rotationY, rotationX, 0.0f);

        Vector3 currentRotation = camera.transform.localEulerAngles;

        // if (Mathf.Abs(currentRotation.x) > maxAngleX)
        //     currentRotation.x = Mathf.Sign(currentRotation.x) * maxAngleX;

        // if (currentRotation.y < - maxAngleY - 30 || maxAngleY + 30 < currentRotation.y)
        //     currentRotation.y = Mathf.Sign(currentRotation.y) * maxAngleY;

        currentRotation.z = 0.0f;

        camera.transform.localEulerAngles = currentRotation;
    }

    void MoveCamera()
    {
        Vector3 cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * movementVertical + camera.transform.right * movementHorizontal;

        rigidBody.linearVelocity = moveForward * moveSpeed;
    }
}
