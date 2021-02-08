using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] 
    private Transform target = null;
    [SerializeField]
    [Range(0.1f, 5)]
    private float lerpSpeed = 2;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, 0, target.position.z) + offset, Time.deltaTime * lerpSpeed);
    }
}
