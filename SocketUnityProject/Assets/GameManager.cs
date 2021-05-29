using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Game game;
    public MovementSystemMessageSync movementSync;
    public NetworkManager NetworkManager;
    public bool start = false;
    void Start()
    {
        game = new Game();
        game.SetupServer(4);

        NetworkManager = FindObjectOfType<NetworkManager>();
        movementSync = new MovementSystemMessageSync(game, NetworkManager);
    }

    void Update()
    {
        if(!start)
            return;

        while (NetworkManager.server.Messages.Count > 0)
        {
            var message = NetworkManager.server.Messages[0];
            var movementSystem = game._movementSystems[message[0]];
            movementSystem.Move(new Vector3(message[1] - 1, 0, message[2] - 1), 5);
            NetworkManager.server.Messages.RemoveAt(0);
        }
        
        game.TickServer(Time.deltaTime);

        movementSync.Sync();
        
    }

    private void OnDrawGizmosSelected()
    {
        if (game != null && game._movementSystems != null)
        {
            for (int i = 0; i < game._movementSystems.Length; i++)
            {
                Gizmos.DrawSphere(game._movementSystems[i].Position, 1f);
            }
        }
    }
}

public class MovementSystemMessageSync
{
    private Game _game;
    private NetworkManager _networkManager;
    public static byte messageType = 1;
    public int numOffValues = 3;
    public MovementSystemMessageSync(Game game, NetworkManager networkManager)
    {
        _game = game;
        _networkManager = networkManager;
    }

    public async void Sync()
    {
        for (int i = 0; i < _game._movementSystems.Length; i++)
        {
            var positionX = BitConverter.GetBytes((double)_game._movementSystems[i].Position.x);
            var positionZ = BitConverter.GetBytes((double)_game._movementSystems[i].Position.z);
            var data = new byte[] {messageType, (byte)i, (byte) numOffValues, (byte) positionX.Length, (byte) positionZ.Length};
            
            int size = data.Length;
            Array.Resize<byte>(ref data, size + positionX.Length);
            Array.Copy(positionX, 0, data, size, positionX.Length);
            
            size = data.Length;
            Array.Resize<byte>(ref data, size + positionZ.Length);
            Array.Copy(positionZ, 0, data, size, positionZ.Length);
            
            
            _networkManager.SendData(data);
        }
    }

    public static Vector3 GetPosition(byte[] array)
    {
        var positionX = new byte[array[3]];
        var positionZ = new byte[array[4]];
        
        Array.Copy(array, 5, positionX, 0, positionX.Length);
        Array.Copy(array, 5 + positionZ.Length, positionZ, 0, positionZ.Length);

        double x = BitConverter.ToDouble(positionX, 0);
        double z = BitConverter.ToDouble(positionZ, 0);
        
        return new Vector3(Convert.ToSingle(x),0,Convert.ToSingle(z));
    }
}