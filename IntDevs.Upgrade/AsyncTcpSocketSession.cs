using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public class AsyncTcpSocketSession
    {
        private readonly string _sessionKey;
        public string SessionKey { get { return _sessionKey; } }
        public DateTime StartTime { get; private set; }

        private TcpClient _tcpClient;
        private readonly AsyncTcpSocketServer _server;
        private readonly AsyncTcpSocketServerConfiguration _configuration;

        private int _state;
        private const int _none = 0;
        private const int _connecting = 1;
        private const int _connected = 2;
        private const int _disposed = 5;

        private byte[] _receiveBuffer;
        private int _receiveBufferOffset = 0;
        private readonly IBufferManager _bufferManager;

        private IPEndPoint _remoteEndPoint;
        private IPEndPoint _localEndPoint;
        private readonly IAsyncTcpSocketServerMessageDispatcher _dispatcher;

        public AsyncTcpSocketServer Server { get { return _server; } }


        private Stream _stream;

        public AsyncTcpSocketSession(
            TcpClient tcpClient,
            AsyncTcpSocketServerConfiguration configuration,
            IBufferManager bufferManager,
            IAsyncTcpSocketServerMessageDispatcher dispatcher,
            AsyncTcpSocketServer server)
        {
            _sessionKey = Guid.NewGuid().ToString();

            this.StartTime = DateTime.UtcNow;

            _tcpClient = tcpClient;
            _bufferManager = bufferManager;
            _configuration = configuration;
            _dispatcher = dispatcher;
            _server = server;

            _remoteEndPoint = (_tcpClient != null && _tcpClient.Client.Connected) ?
            (IPEndPoint)_tcpClient.Client.RemoteEndPoint : null;
            _localEndPoint = (_tcpClient != null && _tcpClient.Client.Connected) ?
                    (IPEndPoint)_tcpClient.Client.LocalEndPoint : null;

            _stream =_tcpClient.GetStream();

        }

        public override string ToString()
        {
            return string.Format("SessionKey[{0}], RemoteEndPoint[{1}], LocalEndPoint[{2}]",
                this.SessionKey, this.RemoteEndPoint, this.LocalEndPoint);
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return (_tcpClient != null && _tcpClient.Client.Connected) ?
                    (IPEndPoint)_tcpClient.Client.RemoteEndPoint : _remoteEndPoint;
            }
        }
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return (_tcpClient != null && _tcpClient.Client.Connected) ?
                    (IPEndPoint)_tcpClient.Client.LocalEndPoint : _localEndPoint;
            }
        }

        public Stream Stream { get { return _stream; } }

        public async Task SendAsync(byte[] data)
        {
            await SendAsync(data, 0, data.Length);
        }

        public TcpSocketConnectionState State
        {
            get
            {
                switch (_state)
                {
                    case _none:
                        return TcpSocketConnectionState.None;
                    case _connecting:
                        return TcpSocketConnectionState.Connecting;
                    case _connected:
                        return TcpSocketConnectionState.Connected;
                    case _disposed:
                        return TcpSocketConnectionState.Closed;
                    default:
                        return TcpSocketConnectionState.Closed;
                }
            }
        }
        public async Task SendAsync(byte[] data, int offset, int count)
        {
            if (State != TcpSocketConnectionState.Connected)
            {
                throw new InvalidOperationException("This session has not connected.");
            }

            try
            {
                //byte[] frameBuffer;
                //int frameBufferOffset;
                //int frameBufferLength;
                //_configuration.FrameBuilder.Encoder.EncodeFrame(data, offset, count, out frameBuffer, out frameBufferOffset, out frameBufferLength);

                //await _stream.WriteAsync(frameBuffer, frameBufferOffset, frameBufferLength);

                await _stream.WriteAsync(data, offset, count);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public void SendSync(byte[] data, int offset, int count)
        {
            if (State != TcpSocketConnectionState.Connected)
            {
                //throw new InvalidOperationException("This session has not connected.");

                LogHelper.Error("SendSync", new InvalidOperationException("This session has not connected."));
            }

            try
            {
                //byte[] frameBuffer;
                //int frameBufferOffset;
                //int frameBufferLength;
                //_configuration.FrameBuilder.Encoder.EncodeFrame(data, offset, count, out frameBuffer, out frameBufferOffset, out frameBufferLength);

                //await _stream.WriteAsync(frameBuffer, frameBufferOffset, frameBufferLength);

                _stream.Write(data, offset, count);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                //throw ex;
            }
        }

        internal async Task Start()
        {
            int origin = Interlocked.CompareExchange(ref _state, _connecting, _none);
            if (origin == _disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            else if (origin != _none)
            {
                throw new InvalidOperationException("This tcp socket session has already started.");
            }

            try
            {
                ConfigureClient();  

                _receiveBuffer = _bufferManager.BorrowBuffer();
                _receiveBufferOffset = 0;

                if (Interlocked.CompareExchange(ref _state, _connected, _connecting) != _connecting)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                bool isErrorOccurredInUserSide = false;
                try
                {
                    await _dispatcher.OnSessionStarted(this);
                }
                catch (Exception ex)
                {
                    isErrorOccurredInUserSide = true;
                    HandleUserSideError(ex);
                }

                LogHelper.InfoFormat("Session started for [{0}] on [{1}] in dispatcher [{2}] with session count [{3}].",
                    this.RemoteEndPoint,
                    this.StartTime.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"),
                    _dispatcher.GetType().Name,
                    this.Server.SessionCount);

                if (!isErrorOccurredInUserSide)
                {
                    await Process();
                }
                else
                {
                    await Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task Close()
        {
            if (Interlocked.Exchange(ref _state, _disposed) == _disposed)
            {
                return;
            }

            try
            {
                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    //_tcpClient.Dispose();
                    _tcpClient = null;
                }
            }
            catch (Exception) { }

            if (_receiveBuffer != null)
                _bufferManager.ReturnBuffer(_receiveBuffer);

            _receiveBufferOffset = 0;

            LogHelper.InfoFormat("Session closed for [{0}] on [{1}] in dispatcher [{2}] with session count [{3}].",
                this.RemoteEndPoint,
                DateTime.UtcNow.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"),
                _dispatcher.GetType().Name,
                this.Server.SessionCount - 1);
            try
            {
                await _dispatcher.OnSessionClosed(this);
            }
            catch (Exception ex)
            {
                HandleUserSideError(ex);
            }
        }
        private void HandleUserSideError(Exception ex)
        {
            LogHelper.Error(string.Format("Session [{0}] error occurred in user side [{1}].", this, ex.Message), ex);
        }

    private async Task Process()
        {
            try
            {
                //int frameLength;
                //byte[] payload;
                //int payloadOffset;
                //int payloadCount;
                //int consumedLength = 0;

                while (State == TcpSocketConnectionState.Connected)
                {
                    int receiveCount = await _stream.ReadAsync(_receiveBuffer, _receiveBufferOffset, _receiveBuffer.Length - _receiveBufferOffset);
                    if (receiveCount == 0)
                        break;

                    await _dispatcher.OnSessionDataReceived(this, _receiveBuffer, 0, receiveCount);

                    //consumedLength = 0;

                    //while (true)
                    //{
                        //frameLength = 0;
                        //payload = null;
                        //payloadOffset = 0;
                        //payloadCount = 0;

                        //if (_configuration.FrameBuilder.Decoder.TryDecodeFrame(_receiveBuffer, consumedLength, _receiveBufferOffset - consumedLength,
                        //    out frameLength, out payload, out payloadOffset, out payloadCount))
                        //{
                        //    try
                        //    {
                        //        await _dispatcher.OnSessionDataReceived(this, payload, payloadOffset, payloadCount);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        //HandleUserSideError(ex);
                        //    }
                        //    finally
                        //    {
                        //        consumedLength += frameLength;
                        //    }
                        //}
                        //else
                        //{
                        //    break;
                        //}
                    //}

            
                }
            }
            catch (Exception ex) 
            {
                LogHelper.Error(ex.Message, ex);

                throw ex;
            }
            finally
            {
               
            }
        }

        private void ConfigureClient()
        {
            _tcpClient.ReceiveBufferSize = _configuration.ReceiveBufferSize;
            _tcpClient.SendBufferSize = _configuration.SendBufferSize;
            _tcpClient.ReceiveTimeout = (int)_configuration.ReceiveTimeout.TotalMilliseconds;
            _tcpClient.SendTimeout = (int)_configuration.SendTimeout.TotalMilliseconds;
            _tcpClient.NoDelay = _configuration.NoDelay;
            _tcpClient.LingerState = _configuration.LingerState;
        }
    }
}
