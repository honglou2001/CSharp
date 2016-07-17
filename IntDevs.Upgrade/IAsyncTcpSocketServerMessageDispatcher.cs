using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public interface IAsyncTcpSocketServerMessageDispatcher
    {
        Task OnSessionStarted(AsyncTcpSocketSession session);
        Task OnSessionDataReceived(AsyncTcpSocketSession session, byte[] data, int offset, int count,Action<string> uiAction);
        Task OnSessionClosed(AsyncTcpSocketSession session);
    }

    public class SimpleMessageDispatcher : IAsyncTcpSocketServerMessageDispatcher
    {
        public async Task OnSessionStarted(AsyncTcpSocketSession session)
        {
            LogHelper.InfoFormat("TCP session {0} has connected {1}.", session.RemoteEndPoint, session);
            //await Task.CompletedTask
            await Task.FromResult<SimpleMessageDispatcher>(this);
        }

        public async Task OnSessionDataReceived(AsyncTcpSocketSession session, byte[] data, int offset, int count, Action<string> uiAction)
        {
            var text = Encoding.UTF8.GetString(data, offset, count);
            //Console.Write(string.Format("Client : {0} --> ", session.RemoteEndPoint));
            //Console.WriteLine(text);
            string hexText = Tools.ByteToHexStr(data);
            uiAction(hexText);
            await Task.FromResult<SimpleMessageDispatcher>(this);
            //await session.SendAsync(Encoding.UTF8.GetBytes(text));
        }

        public async Task OnSessionClosed(AsyncTcpSocketSession session)
        {
            LogHelper.InfoFormat("TCP session {0} has disconnected.", session);
            await Task.FromResult<SimpleMessageDispatcher>(this);
            //await Task.CompletedTask;
        }
    }
}
