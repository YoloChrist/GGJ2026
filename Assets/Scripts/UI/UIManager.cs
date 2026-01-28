using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiInteractPopup;

    private void Awake()
    {
        if (uiInteractPopup != null)
            uiInteractPopup.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerCollision.OnNpcContactStarted += HandleNpcContactStarted;
        PlayerCollision.OnNpcContactEnded += HandleNpcContactEnded;
    }

    private void OnDisable()
    {
        PlayerCollision.OnNpcContactStarted -= HandleNpcContactStarted;
        PlayerCollision.OnNpcContactEnded -= HandleNpcContactEnded;
    }

    private void HandleNpcContactStarted(CrowdAgent npc)
    {
        if (uiInteractPopup != null)
            uiInteractPopup.SetActive(true);
    }

    private void HandleNpcContactEnded()
    {
        if (uiInteractPopup != null)
            uiInteractPopup.SetActive(false);
    }
}
