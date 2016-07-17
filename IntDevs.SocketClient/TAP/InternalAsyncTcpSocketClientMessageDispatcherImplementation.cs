﻿using System;
using System.Threading.Tasks;

namespace IntDevs.SocketClient
{
    internal class InternalAsyncTcpSocketClientMessageDispatcherImplementation : IAsyncTcpSocketClientMessageDispatcher
    {
        private Func<AsyncTcpSocketClient, byte[], int, int, Task> _onServerDataReceived;
        private Func<AsyncTcpSocketClient, Task> _onServerConnected;
        private Func<AsyncTcpSocketClient, Task> _onServerDisconnected;

        public InternalAsyncTcpSocketClientMessageDispatcherImplementation()
        {
        }

        public InternalAsyncTcpSocketClientMessageDispatcherImplementation(
            Func<AsyncTcpSocketClient, byte[], int, int, Task> onServerDataReceived,
            Func<AsyncTcpSocketClient, Task> onServerConnected,
            Func<AsyncTcpSocketClient, Task> onServerDisconnected)
            : this()
        {
            _onServerDataReceived = onServerDataReceived;
            _onServerConnected = onServerConnected;
            _onServerDisconnected = onServerDisconnected;
        }

        public async Task OnServerConnected(AsyncTcpSocketClient client)
        {
            if (_onServerConnected != null)
                await _onServerConnected(client);
        }

        public async Task OnServerDataReceived(AsyncTcpSocketClient client, byte[] data, int offset, int count, Action<string> uiAction)
        {
            if (_onServerDataReceived != null)
                await _onServerDataReceived(client, data, offset, count);
        }

        public async Task OnServerDisconnected(AsyncTcpSocketClient client)
        {
            if (_onServerDisconnected != null)
                await _onServerDisconnected(client);
        }
    }
}
