using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float FollowSpeed = 2f;
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
