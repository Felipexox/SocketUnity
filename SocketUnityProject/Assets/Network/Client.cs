using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client
{
    private static byte[] _buffer = new byte[1024];
    public Socket _socket;
    public List<byte[]> receivedPackages = new List<byte[]>();
    public Client()
    {
        _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    }

    public async void ConnectLoop()
    {
        while (!_socket.Connected)
        {
            await _socket.ConnectAsync("127.0.0.1", 3300);
        }
        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, _socket);        
    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;
        int received = socket.EndReceive(ar);

        byte[] data = new byte[received];
        Array.Copy(_buffer, data, received);
        receivedPackages.Add(data);
    

        socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, socket);
    }
    public void SendData(byte[] data)
    {
        _socket.BeginSend(data, 0,  data.Length, SocketFlags.None, Callback, null);
    }

    private void Callback(IAsyncResult ar)
    {
        _socket.EndSend(ar);
    }
}
