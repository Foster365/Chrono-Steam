using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField]
    private float smoothSpeed;
    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        Invoke("setTarget",0.1f);
        DontDestroyOnLoad(this.gameObject);
    }
    private void FixedUpdate()
    {
        Vector3 disairedPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, disairedPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;

       // transform.LookAt(target);
    }
    void setTarget()
    { 
        target = GameManager.Instance.PlayerInstance.transform;
        GameManager.Instance.Camera = this.gameObject;
    }
}
