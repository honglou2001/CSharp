using System;
using System.Threading.Tasks;

namespace IntDevs.SocketClient
{
    public interface IAsyncTcpSocketClientMessageDispatcher
    {
        Task OnServerConnected(AsyncTcpSocketClient client);
        Task OnServerDataReceived(AsyncTcpSocketClient client, byte[] data, int offset, int count, Action<string> uiAction);
        Task OnServerDisconnected(AsyncTcpSocketClient client);
    }
}
