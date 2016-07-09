using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace IntDevs.Upgrade
{
    public class AsyncTcpSocketServer
    {
        public IPEndPoint ListenedEndPoint { get; private set; }
        private readonly AsyncTcpSocketServerConfiguration _configuration;
        private readonly ConcurrentDictionary<string, AsyncTcpSocketSession> _sessions = new ConcurrentDictionary<string, AsyncTcpSocketSession>();

        private int _state;
        private const int _none = 0;
        private const int _listening = 1;
        private const int _disposed = 5;
        private TcpListener _listener;
        private IBufferManager _bufferManager;
        private readonly IAsyncTcpSocketServerMessageDispatcher _dispatcher;


        public bool IsListening { get { return _state == _listening; } }
        public int SessionCount { get { return _sessions.Count; } }
      
        public AsyncTcpSocketServer(string ip,int port)
        {

            this.ListenedEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            _configuration = new AsyncTcpSocketServerConfiguration();

            _dispatcher = new SimpleMessageDispatcher();

            Initialize();
        }
        public void Listen()
        {
            int origin = Interlocked.CompareExchange(ref _state, _listening, _none);
            if (origin == _disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            else if (origin != _none)
            {
                throw new InvalidOperationException("This tcp server has already started.");
            }

            try
            {
                _listener = new TcpListener(this.ListenedEndPoint);
                ConfigureListener();

                _listener.Start(_configuration.PendingConnectionBacklog);

                Task.Run(async () =>
                {
                    await Accept();
                }).Forget();
        
            }
            catch (Exception ex) {
                throw ex;           
            }
        }

       public void Shutdown()
        {
            if (Interlocked.Exchange(ref _state, _disposed) == _disposed)
            {
                return;
            }

            try
            {
                _listener.Stop();
                _listener = null;

                Task.Run(async () =>
                {
                    try
                    {
                        foreach (var session in _sessions.Values)
                        {
                            await session.Close();
                        }
                    }
                    catch (Exception ex) {
                        LogHelper.Error(ex.Message,ex);
                    }
                })
                .Wait();
            }
            catch (Exception ex)  {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public async Task BroadcastAsync(byte[] data)
        {
            await BroadcastAsync(data, 0, data.Length);
        }

        public async Task BroadcastAsync(byte[] data, int offset, int count)
        {
            foreach (var session in _sessions.Values)
            {
                await session.SendAsync(data, offset, count);
            }
        }


        public void BroadcastSync(byte[] data)
        {
            BroadcastSync(data, 0, data.Length);
        }

        public void BroadcastSync(byte[] data, int offset, int count)
        {
            foreach (var session in _sessions.Values)
            {
                session.SendSync(data, offset, count);
            }
        }


        public bool HasSession(string sessionKey)
        {
            return _sessions.ContainsKey(sessionKey);
        }

        public AsyncTcpSocketSession GetSession(string sessionKey)
        {
            AsyncTcpSocketSession session = null;
            _sessions.TryGetValue(sessionKey, out session);
            return session;
        }

        public async Task CloseSession(string sessionKey)
        {
            AsyncTcpSocketSession session = null;
            if (_sessions.TryGetValue(sessionKey, out session))
            {
                await session.Close();
            }
        }

        private void ConfigureListener()
        {
            _listener.AllowNatTraversal(_configuration.AllowNatTraversal);
        }
        private async Task Process(TcpClient acceptedTcpClient)
        {
            var session = new AsyncTcpSocketSession(acceptedTcpClient, _configuration, _bufferManager, _dispatcher,this);

            if (_sessions.TryAdd(session.SessionKey, session))
            {
                LogHelper.InfoFormat("New session [{0}].", session);

                try
                {
                    await session.Start();
                }
                catch (TimeoutException ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                finally
                {
                    AsyncTcpSocketSession throwAway;
                    if (_sessions.TryRemove(session.SessionKey, out throwAway))
                    {
                        LogHelper.InfoFormat("Close session [{0}].", throwAway);
                    }
                }
            }
        }

        private void Initialize()
        {
            _bufferManager = new GrowingByteBufferManager(_configuration.InitialPooledBufferCount, _configuration.ReceiveBufferSize);
        }

       private async Task Accept()
        {
            try
            {
                while (IsListening)
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    Task.Run(async () =>
                    {
                        await Process(tcpClient);
                    }).Forget();
                   
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
    }
}
