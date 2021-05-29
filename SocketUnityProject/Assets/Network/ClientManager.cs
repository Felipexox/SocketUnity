using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    private Client client;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        client = new Client();
        client.ConnectLoop();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            client.SendData(new byte[]{(byte)index, (byte)(horizontal + 1), (byte)(vertical + 1)});
        }
        while (client.receivedPackages.Count > 0)
        {
            try
            {
                if (client.receivedPackages[0][0] == MovementSystemMessageSync.messageType && client.receivedPackages[0][1] == index)
                    SyncPosition(MovementSystemMessageSync.GetPosition(client.receivedPackages[0]));
                client.receivedPackages.RemoveAt(0);
            }
            catch (Exception e)
            {
                client.receivedPackages.RemoveAt(0);
            }
            
        }
    }

    public void SyncPosition(Vector3 getPosition)
    {
        transform.position = getPosition;
    }
}
