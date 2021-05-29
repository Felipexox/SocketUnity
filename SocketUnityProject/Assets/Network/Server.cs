using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server
{
    public static byte[] _buffer = new byte[1024];
    public Socket _socket;
    public List<Socket> clients = new List<Socket>();
    public List<byte[]> Messages = new List<byte[]>();
    public Server()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void SetupSocket()
    {
        _socket.Bind(new IPEndPoint(IPAddress.Any, 3300));
        _socket.Listen(1);
        _socket.BeginAccept(Accept, null);
        
    }



    void Accept(IAsyncResult async)
    {
        Socket socket = _socket.EndAccept(async);
        clients.Add(socket);
        socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, socket);
        _socket.BeginAccept(Accept, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;
        int received = socket.EndReceive(ar);
        
        byte[] data = new byte[received];
        Array.Copy(_buffer, data, received);
        
        Messages.Add(data);
        
        socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, socket);
    }
    
 
    public void SendData(byte[] data)
    {
        for (int i = clients.Count - 1; i >= 0; i--)
        {
            if(!clients[i].Connected)
                clients.RemoveAt(i);
        }
        for (int i = 0; i < clients.Count; i++)
        {
            Socket socket = clients[i];
            clients[i].BeginSend(data, 0,  data.Length, SocketFlags.None, Callback, socket);
        }
      
    }

    private void Callback(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;
        socket.EndSend(ar);
        
    }
}
