﻿#region USINGZ

using System;
using System.Collections;
using System.Linq;

#endregion

namespace Ros_CSharp
{
    public static class network
    {
        public static string host;
        public static int tcpros_server_port;

        public static bool splitURI(string uri, ref string host, ref int port)
        {
            if (uri == null)
                throw new Exception("NULL STUFF FAIL!");
            if (uri.Substring(0, 7) == "http://")
                host = uri.Substring(7);
            else if (uri.Substring(0, 9) == "rosrpc://")
                host = uri.Substring(9);
            string[] split = host.Split(':');
            if (split.Length < 2) return false;
            string port_str = split[1];
            port_str = port_str.Trim('/');
            port = int.Parse(port_str);
            host = split[0];
            return true;
        }

        public static bool isPrivateIp(string ip)
        {
            bool b = (string.Compare("192.168", ip) >= 7) || (string.Compare("10.", ip) > 3) ||
                     (string.Compare("169.253", ip) > 7);
            return b;
        }

        public static string determineHost()
        {
            return Environment.MachineName;
        }

        public static void init(IDictionary remappings)
        {
            Console.WriteLine("REMAPPINGS: ");
            foreach (object o in remappings.Keys)
            {
                Console.WriteLine("{0} = {1}", (string)o, (string)remappings[o]);
            }
            if (remappings.Contains("__hostname"))
                host = (string) remappings["__hostname"];
            else
            {
                if (remappings.Contains("__ip"))
                    host = (string) remappings["__ip"];
            }

            if (remappings.Contains("__tcpros_server_port"))
            {
                tcpros_server_port = int.Parse((string) remappings["__tcpros_server_port"]);
            }

            if (string.IsNullOrEmpty(host))
                host = determineHost();
        }
    }
}