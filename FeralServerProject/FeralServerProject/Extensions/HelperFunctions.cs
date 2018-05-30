using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject.Extensions
{
    public class HelperFunctions
    {
        public static void RemoveDisconnectedClients(List<Connection> disconnectedConnections,
            List<Connection> allConnections)
        {
            foreach (var connection in disconnectedConnections)
            {
                if (allConnections.Contains(connection))
                {
                    allConnections.Remove(connection);
                }
            }

            disconnectedConnections.Clear();
        }
    }
}
