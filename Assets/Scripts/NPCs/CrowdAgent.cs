using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CrowdAgent : MonoBehaviour
{
    private CrowdManager crowdManager;

    [SerializeField, Min(0.5f)] private float moveSpeed = 1f;
    [SerializeField, Min(0.01f)] private float arriveDistance = 0.1f;
    [SerializeField] private Vector2 repathIntervalRange = new Vector2(1.0f, 3.0f);

    private Rigidbody2D rb;
    private Vector2 target;
    private float repathTimer;
    private bool hasTarget;
    private bool paused;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        FindManager();

        if (crowdManager == null)
        {
            Debug.LogError($"CrowdAgent '{name}': CrowdManager not found in scene.");
            enabled = false;
            return;
        }

        crowdManager.AddAgent(this);
        ResetRepathTimer();
    }

    private void OnDisable()
    {
        if (crowdManager != null)
            crowdManager.RemoveAgent(this);
    }

    private void FindManager()
    {
        if (crowdManager != null)
            return;

        crowdManager = CrowdManager.Instance;

        if (crowdManager == null)
            crowdManager = Object.FindFirstObjectByType<CrowdManager>();
    }

    public void PauseAI()
    {
        paused = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void ResumeAI()
    {
        paused = false;
        ResetRepathTimer();
    }

    private void Update()
    {
        if (paused)
            return;

        repathTimer -= Time.deltaTime;

        if (!hasTarget || repathTimer <= 0f || IsAtTarget())
            AcquireNewTarget();
    }

    private void FixedUpdate()
    {
        if (paused || !hasTarget)
            return;

        var pos = rb.position;
        var toTarget = target - pos;
        var dist = toTarget.magnitude;

        if (dist <= arriveDistance)
            return;

        var step = moveSpeed * Time.fixedDeltaTime;
        var next = pos + toTarget / dist * Mathf.Min(step, dist);
        rb.MovePosition(next);
    }

    private void AcquireNewTarget()
    {
        ResetRepathTimer();

        if (crowdManager == null)
        {
            hasTarget = false;
            return;
        }

        if (crowdManager.TryGetWanderTarget(rb.position, out var newTarget))
        {
            target = newTarget;
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
    }

    private bool IsAtTarget()
    {
        var distSqr = (target - rb.position).sqrMagnitude;
        return distSqr <= arriveDistance * arriveDistance;
    }

    private void ResetRepathTimer()
    {
        repathTimer = Random.Range(
            Mathf.Min(repathIntervalRange.x, repathIntervalRange.y),
            Mathf.Max(repathIntervalRange.x, repathIntervalRange.y)
        );
    }
}
