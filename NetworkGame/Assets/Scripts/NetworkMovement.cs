using MEC;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMovement : MonoBehaviour
{
    private Transform myTransform;

    private Vector2 direction;
    public float tickRate = 0.1f;
    public NetworkPosition networkPosition;

    void Awake()
    {
        myTransform = transform;

        Timing.RunCoroutine(SendCoordUpdate());
        networkPosition = new NetworkPosition { NewPosition = myTransform.position };
        GetComponent<Movement>().networkPosition = networkPosition;
    }

    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        direction = new Vector2(h, v);
    }

    IEnumerator<float> SendCoordUpdate()
    {
        while (true)
        {
            var dir = (Vector3)direction + myTransform.position;
            Socket.Send(NetworkEvents.Coord, new Coord(dir));

            yield return Timing.WaitForSeconds(tickRate);
        }
    }

    void ReciveCoord(SocketIOEvent e)
    {
        Debug.Log("ReciveCoord");
        var coord = Coord.FromJson(e.data.ToString());
        networkPosition.NewPosition = coord;
    }

    void ReciveSize(SocketIOEvent e)
    {
        var scale = JsonUtility.FromJson<Size>(e.data.ToString()).size;
        transform.localScale = new Vector3(scale, scale);
    }

    void OnEnable()
    {
        Socket.ON(NetworkEvents.MyCoord, ReciveCoord);
        Socket.ON(NetworkEvents.MySize, ReciveSize);
    }

    void OnDisable()
    {
        Socket.OFF(NetworkEvents.MyCoord, ReciveCoord);
        Socket.OFF(NetworkEvents.MySize, ReciveSize);
    }
}