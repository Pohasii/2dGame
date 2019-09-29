using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PlayerManager : MonoBehaviour
{
    Dictionary<string, Transform> players = new Dictionary<string, Transform>();
    Dictionary<string, NetworkPosition> playerPositions = new Dictionary<string, NetworkPosition>();

    public GameObject playerPrefab;

    void OnNewPlayerConnect(SocketIOEvent e)
    {
        var playerInfo = JsonUtility.FromJson<PlayerInfo>(e.data.ToString());

        var pos = new Vector2(playerInfo.coord.x, playerInfo.coord.y);

        var playerObj = Instantiate(playerPrefab, pos, Quaternion.identity).transform;

        var networkPosition = new NetworkPosition { NewPosition = playerObj.position };
        playerObj.GetComponent<Movement>().networkPosition = networkPosition;
        playerObj.localScale = new Vector3(playerInfo.size, playerInfo.size);

        players.Add(playerInfo.id, playerObj);
        playerPositions.Add(playerInfo.id, networkPosition);
    }

    void ReciveOtherPlayerPosition(SocketIOEvent e)
    {
        var playerInfo = OtherCoord.FromJson(e.data.ToString());

        var pos = new Vector2(playerInfo.coord.x, playerInfo.coord.y);

        playerPositions[playerInfo.id].NewPosition = pos;
    }

    void OnEnable()
    {
        Socket.ON(NetworkEvents.NewPlayer, OnNewPlayerConnect);
        Socket.ON(NetworkEvents.OtherCoord, ReciveOtherPlayerPosition);
    }

    void OnDisable()
    {
        Socket.OFF(NetworkEvents.NewPlayer, OnNewPlayerConnect);
        Socket.OFF(NetworkEvents.OtherCoord, ReciveOtherPlayerPosition);
    }
}