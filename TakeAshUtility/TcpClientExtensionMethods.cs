using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace TakeAshUtility {

    public static class TcpClientExtensionMethods {

        /// <summary>
        /// Get TCP connection state
        /// </summary>
        /// <param name="tcpClient">TCP Client</param>
        /// <returns>TCP connection state</returns>
        /// <remarks>
        /// [c# - How to check if TcpClient Connection is closed? - Stack Overflow](http://stackoverflow.com/questions/1387459)
        /// </remarks>
        public static TcpState GetState(this TcpClient tcpClient) {
            var socket = tcpClient.Client;
            if (!socket.Connected) {
                return TcpState.Unknown;
            }
            var connection = IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpConnections()
                .FirstOrDefault(info =>
                    info.RemoteEndPoint.Equals(socket.RemoteEndPoint) &&
                    info.LocalEndPoint.Equals(socket.LocalEndPoint));
            return connection != null ?
                connection.State :
                TcpState.Unknown;
        }
    }
}
