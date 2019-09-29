using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Newtonsoft.Json;

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

    void OtherPlayer(SocketIOEvent e)
    {
        Debug.Log("OTHERPLAYER");
    }

    void ReciveOtherPlayerPosition(SocketIOEvent e)
    {
        var playerInfo = OtherCoord.FromJson(e.data.ToString());

        var pos = new Vector2(playerInfo.coord.x, playerInfo.coord.y);

        playerPositions[playerInfo.id].NewPosition = pos;
    }

    void ReciveOtherSize(SocketIOEvent e)
    {
        var sizeInfo = OtherSize.FromJson(e.data.ToString());
        players[sizeInfo.id].localScale = new Vector3(sizeInfo.size, sizeInfo.size);
    }

    void OnDisconnect(SocketIOEvent e)
    {
        Debug.Log("OTHERPLAYERDISCONNECTED");
        var idInfo = JsonUtility.FromJson<Id>(e.data.ToString());
        Destroy(players[idInfo.id].gameObject);
        players.Remove(idInfo.id);
    }

    void OnEnable()
    {
        Socket.ON(NetworkEvents.OtherPlayers, OtherPlayer);
        Socket.ON(NetworkEvents.OtherPlayers, OnNewPlayerConnect);
        Socket.ON(NetworkEvents.NewPlayer, OnNewPlayerConnect);
        Socket.ON(NetworkEvents.OtherCoord, ReciveOtherPlayerPosition);
        Socket.ON(NetworkEvents.OtherSize, ReciveOtherSize);
        Socket.ON(NetworkEvents.Disconnect, OnDisconnect);
    }

    void OnDisable()
    {
        Socket.OFF(NetworkEvents.OtherPlayers, OnNewPlayerConnect);
        Socket.OFF(NetworkEvents.NewPlayer, OnNewPlayerConnect);
        Socket.OFF(NetworkEvents.OtherCoord, ReciveOtherPlayerPosition);
        Socket.OFF(NetworkEvents.OtherSize, ReciveOtherSize);
        Socket.OFF(NetworkEvents.Disconnect, OnDisconnect);
    }
}