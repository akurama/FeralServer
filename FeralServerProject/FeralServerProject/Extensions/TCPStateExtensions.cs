using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject.Extensions
{
    public class TCPStateExtensions
    {
        public static TcpState GetState(TcpClient thisClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections()
                .SingleOrDefault(x => x.LocalEndPoint.Equals(thisClient.Client.LocalEndPoint));
            return foo != null ? foo.State : TcpState.Unknown;
        }
    }
}
