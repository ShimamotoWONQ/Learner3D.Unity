using UnityEngine;

public class CarGroundAlign : CustomAnimationScript
{
    public float speed = 8f;
    public float rayLength = 2f;
    public float duration = 2f;
    public float fadeDuration = 0.5f;

    bool _isRunning;
    float _elapsed;
    bool _isFading;
    float _fadeElapsed;
    Renderer[] _renderers;

    public override void OnPlay()
    {
        _isRunning = true;
        _isFading = false;
        _elapsed = 0f;
        _fadeElapsed = 0f;
        _renderers = GetComponentsInChildren<Renderer>();
        SetAlpha(1f);
    }

    public override void OnStop()
    {
        _isRunning = false;
        _isFading = false;
    }

    public override void OnPause() { _isRunning = false; }
    public override void OnResume() { _isRunning = true; }

    void Update()
    {
        if (!_isRunning) return;

        if (_isFading)
        {
            _fadeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_fadeElapsed / fadeDuration);
            SetAlpha(1f - t);

            if (t >= 1f)
            {
                _isRunning = false;
                gameObject.SetActive(false);
                Complete();
            }
            return;
        }

        _elapsed += Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, rayLength))
        {
            Vector3 normal = hit.normal;
            Quaternion rot = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (_elapsed >= duration)
        {
            _isFading = true;
            _fadeElapsed = 0f;
        }
    }

    void SetAlpha(float alpha)
    {
        if (_renderers == null) return;
        foreach (var r in _renderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
        }
    }
}
