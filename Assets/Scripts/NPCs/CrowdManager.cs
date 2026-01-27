using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public static CrowdManager Instance { get; private set; }

    [SerializeField] private Collider2D roomCollider;
    [SerializeField] private float wanderRadius;
    [SerializeField] private int sampleAttempts = 9;

    private readonly List<CrowdAgent> agents = new List<CrowdAgent>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Duplicate CrowdManager on '{name}'. Destroying.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        if (roomCollider == null)
            roomCollider = GetComponent<Collider2D>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void AddAgent(CrowdAgent agent)
    {
        if (agent == null || agents.Contains(agent))
            return;

        agents.Add(agent);
    }

    public void RemoveAgent(CrowdAgent agent)
    {
        agents.Remove(agent);
    }

    public bool TryGetWanderTarget(Vector2 fromPosition, out Vector2 target)
    {
        target = fromPosition;

        if (roomCollider == null)
            return false;

        for (int i = 0; i < sampleAttempts; i++)
        {
            var candidate = fromPosition + Random.insideUnitCircle * wanderRadius;

            if (!roomCollider.OverlapPoint(candidate))
                continue;

            target = candidate;
            return true;
        }

        // fallback - nearest point in bounds to last candidate
        var last = fromPosition + Random.insideUnitCircle * wanderRadius;
        target = roomCollider.ClosestPoint(last);
        return true;
    }
}
