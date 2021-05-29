using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public Server server;
    // Start is called before the first frame update
    void Start()
    {
        server = new Server();
        server.SetupSocket();
    }

    public void Update()
    {
        
    }

    public void SendData(byte[] data)
    {
        server.SendData(data);
    }
}
