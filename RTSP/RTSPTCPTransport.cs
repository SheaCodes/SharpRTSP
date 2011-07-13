﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Contracts;

namespace Rtsp
{
    /// <summary>
    /// TCP Connection for Rtsp
    /// </summary>
    public class RtspTcpTransport : IRtspTransport, IDisposable
    {
        private IPEndPoint _currentEndPoint;
        private TcpClient _RtspServerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtspTcpTransport"/> class.
        /// </summary>
        /// <param name="TCPConnection">The underlying TCP connection.</param>
        public RtspTcpTransport(TcpClient TCPConnection)
        {
            if (TCPConnection == null)
                throw new ArgumentNullException("TCPConnection");
            Contract.EndContractBlock();

            _currentEndPoint = (IPEndPoint)TCPConnection.Client.RemoteEndPoint;
            _RtspServerClient = TCPConnection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RtspTcpTransport"/> class.
        /// </summary>
        /// <param name="aHost">A host.</param>
        /// <param name="aPortNumber">A port number.</param>
        public RtspTcpTransport(string aHost, int aPortNumber)
            : this(new TcpClient(aHost, aPortNumber))
        {
        }


        #region IRtspTransport Membres

        /// <summary>
        /// Gets the stream of the transport.
        /// </summary>
        /// <returns>A stream</returns>
        public Stream GetStream()
        {
            return _RtspServerClient.GetStream();
        }

        /// <summary>
        /// Gets the remote address.
        /// </summary>
        /// <value>The remote address.</value>
        public string RemoteAddress
        {
            get
            {
                return string.Format("{0}:{1}", _currentEndPoint.Address, _currentEndPoint.Port);
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IRtspTransport"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get { return _RtspServerClient.Connected; }
        }

        /// <summary>
        /// Reconnect this instance.
        /// <remarks>Must do nothing if already connected.</remarks>
        /// </summary>
        public void ReConnect()
        {
            if (_RtspServerClient.Connected)
                return;
            _RtspServerClient = new TcpClient();
            _RtspServerClient.Connect(_currentEndPoint);
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _RtspServerClient.Close();
                /*   // free managed resources
                   if (managedResource != null)
                   {
                       managedResource.Dispose();
                       managedResource = null;
                   }*/
            }
        }
    }
}
