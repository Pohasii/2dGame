using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;
using System.Collections;
using System;
using Newtonsoft.Json;

public class Socket : SocketIOComponent
{
    static Socket inst;

    public override void Awake()
    {
        base.Awake();
        inst = this;
    }

    public static void Send<T>(string ev, T data)
    {
        var msg = JsonUtility.ToJson(data);
        inst.Emit(ev, new JSONObject(msg));
    }

    public static void SendNewtotsoft<T>(string ev, T data)
    {
        var msg = JsonConvert.SerializeObject(data);
        inst.Emit(ev, new JSONObject(msg));
    }

    public static void ON(string ev, Action<SocketIOEvent> callback)
    {
        inst.On(ev, callback);
    }

    public static void OFF(string ev, Action<SocketIOEvent> callback)
    {
        inst.Off(ev, callback);
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Emit(NetworkEvents.SceneLoaded);
    }

    public void StartGame()
    {
        var msg = JsonUtility.ToJson(new NickName { name = "FONTOOMAS" });
        Debug.Log(msg);
        Emit(NetworkEvents.ConnectToGame, new JSONObject(msg));
    }

    void OnConnectToServer(SocketIOEvent e)
    {
        Debug.Log("Connet to Server");
    }

    void OnStartGame(SocketIOEvent e)
    {
        //startValues = JsonUtility.FromJson<StartValues>(e.data.ToString());
        StartCoroutine(LoadYourAsyncScene());
    }

    void OnEnable()
    {
        On(NetworkEvents.Open, OnConnectToServer);
        On(NetworkEvents.StartGame, OnStartGame);
    }

    void OnDisable()
    {
        Off(NetworkEvents.Open, OnConnectToServer);
        Off(NetworkEvents.StartGame, OnStartGame);
    }
}