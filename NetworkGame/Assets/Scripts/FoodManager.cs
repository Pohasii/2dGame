using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using SocketIO;

public class FoodManager : MonoBehaviour
{
    Socket socket;
    Coord[] foodCoords;

    public GameObject foodPrefab;

    List<GameObject> foods = new List<GameObject>();

    GameObject food;

    void Awake()
    {
        socket = FindObjectOfType<Socket>();
    }

    void ReciveFood(SocketIOEvent e)
    {
        if (food)
        {
            Destroy(food);
        }
        var newFood = JsonUtility.FromJson<Coord>(e.data.ToString());
        var pos = new Vector2(newFood.x, newFood.y);
        var obj = Instantiate(foodPrefab, pos, Quaternion.identity, transform);
        food = obj;
        //var newFood = JsonConvert.DeserializeObject<Coord[]>(e.data.ToString());

        //foreach (var item in foods)
        //{
        //    Destroy(item);
        //}

        //foods.Clear();

        //foreach (var item in newFood)
        //{
        //    var pos = new Vector2(item.x, item.y);
        //    var obj = Instantiate(foodPrefab, pos, Quaternion.identity, transform);
        //    foods.Add(obj);
        //}

    }

    void OnEnable()
    {
        socket.On(NetworkEvents.Foods, ReciveFood);
    }

    void OnDisable()
    {
        socket.Off(NetworkEvents.Foods, ReciveFood);
    }
}