using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class PlayerCamera : MonoBehaviour
{

    public new Camera camera;
    public bool doDisableInput;
    public float mouseSensitivity = 3.0f;
    public float moveSpeed = 10.0f;
    public float jumpSpeed = 3.0f;

    Rigidbody rigidBody;
    bool isJumping;
    bool jumpInput;
    float rotationX;
    float rotationY;
    float movementHorizontal;
    float movementVertical;

    void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Init()
    {
        doDisableInput = false;
        isJumping = false;
    }

    public void ConfigureGravityUse(bool doEnable)
    {
        rigidBody.useGravity = doEnable;
    }

    void Update()
    {
        // Rotate Input
        rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Move Input
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis("Vertical");

        // jump Input
        if (Input.GetKeyDown(KeyCode.Space)) jumpInput = true;
    }

    void FixedUpdate()
    {
        if (doDisableInput) return;

        RotateCamera();

        // Don't allow jumpping or moving while jumping
        if (isJumping) return;

        if (jumpInput) Jump();

        MoveCamera();
    }

    void RotateCamera()
    {
        camera.transform.Rotate(-rotationY, rotationX, 0.0f);

        Vector3 currentRotation = camera.transform.localEulerAngles;
        currentRotation.z = 0.0f;
        camera.transform.localEulerAngles = currentRotation;
    }

    void MoveCamera()
    {
        Vector3 cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * movementVertical + camera.transform.right * movementHorizontal;

        rigidBody.linearVelocity = moveForward * moveSpeed + new Vector3(0, rigidBody.linearVelocity.y, 0);
    }

    void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        isJumping = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
        jumpInput = false;
    }
}
