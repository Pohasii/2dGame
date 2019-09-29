using UnityEngine;

public class Movement : MonoBehaviour
{
    public NetworkPosition networkPosition;

    Transform myTransform;

    public float moveSpeed;

    void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        myTransform.position = Vector3.MoveTowards(myTransform.position, networkPosition.NewPosition, moveSpeed * Time.deltaTime);
    }
}

public class NetworkPosition
{
    public Vector2 NewPosition { get; set; }
}