using UnityEngine;

public class RollerCompact : CustomAnimationScript
{
    public float speed = 5f;
    public float rayLength = 2f;
    public float passDistance = 10f;
    public float soilFadeDuration = 1f;
    [SerializeField] GameObject soilObject;

    enum Phase { Forward1, Reverse, Forward2, Done }

    bool _isRunning;
    Phase _phase;
    float _distanceTravelled;
    bool _isSoilFading;
    float _soilFadeElapsed;
    Renderer[] _soilRenderers;

    public override void OnPlay()
    {
        _isRunning = true;
        _phase = Phase.Forward1;
        _distanceTravelled = 0f;
    }

    public override void OnStop()
    {
        _isRunning = false;
    }

    public override void OnPause() { _isRunning = false; }
    public override void OnResume() { _isRunning = true; }

    void Update()
    {
        if (!_isRunning || _phase == Phase.Done) return;

        if (_isSoilFading)
        {
            _soilFadeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_soilFadeElapsed / soilFadeDuration);
            SetSoilAlpha(1f - t);
            if (t >= 1f)
            {
                _isSoilFading = false;
                soilObject.SetActive(false);
            }
        }

        AlignToGround();

        Vector3 dir = (_phase == Phase.Reverse) ? Vector3.left : Vector3.right;
        float step = speed * Time.deltaTime;
        transform.Translate(dir * step);
        _distanceTravelled += step;

        if (_distanceTravelled >= passDistance)
            AdvancePhase();
    }

    void AdvancePhase()
    {
        _distanceTravelled = 0f;

        switch (_phase)
        {
            case Phase.Forward1:
                if (soilObject != null)
                {
                    _soilRenderers = soilObject.GetComponentsInChildren<Renderer>();
                    _isSoilFading = true;
                    _soilFadeElapsed = 0f;
                }
                _phase = Phase.Reverse;
                break;
            case Phase.Reverse:
                _phase = Phase.Forward2;
                break;
            case Phase.Forward2:
                _phase = Phase.Done;
                _isRunning = false;
                Complete();
                break;
        }
    }

    void SetSoilAlpha(float alpha)
    {
        if (_soilRenderers == null) return;
        foreach (var r in _soilRenderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
        }
    }

    void AlignToGround()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, rayLength))
        {
            Vector3 normal = hit.normal;
            Quaternion rot = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }
}
