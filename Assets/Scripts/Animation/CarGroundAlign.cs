using UnityEngine;

public class CarGroundAlign : CustomAnimationScript
{
    public float speed = 5f;
    public float rayLength = 2f;

    bool _isRunning;

    public override void OnPlay() { _isRunning = true; }
    public override void OnStop() { _isRunning = false; }
    public override void OnPause() { _isRunning = false; }
    public override void OnResume() { _isRunning = true; }

    void Update()
    {
        if (!_isRunning) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, rayLength))
        {
            // 地形の法線
            Vector3 normal = hit.normal;

            // 車の向きを地形に合わせる
            Quaternion rot = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);

            // 前進
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
