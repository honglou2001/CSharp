using MutipleClient;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IntDevs.SocketClient.TestAsyncTcpSocketClient
{
    public class SimpleMessageDispatcher : IAsyncTcpSocketClientMessageDispatcher
    {
        public async Task OnServerConnected(AsyncTcpSocketClient client)
        {
            Console.WriteLine(string.Format("TCP server {0} has connected.", client.RemoteEndPoint));
            //await Task.CompletedTask
            await Task.FromResult<SimpleMessageDispatcher>(this);
        }

        public async Task OnServerDataReceived(AsyncTcpSocketClient client, byte[] data, int offset, int count,Action<string> uiAction)
        {
            var text = Encoding.UTF8.GetString(data, offset, count);
            //Console.Write(string.Format("Server : {0} --> ", client.RemoteEndPoint));
            //Console.WriteLine(string.Format("{0}", text));

            string hexText = Tools.ByteToHexStr(data);
            uiAction(hexText);
            //await Task.CompletedTask
            await Task.FromResult<SimpleMessageDispatcher>(this);
            //await client.SendAsync(Encoding.UTF8.GetBytes(text));

        }

        public async Task OnServerDisconnected(AsyncTcpSocketClient client)
        {
            Console.WriteLine(string.Format("TCP server {0} has disconnected.", client.RemoteEndPoint));
            //await Task.CompletedTask
            await Task.FromResult<SimpleMessageDispatcher>(this);
        }
    }
}
