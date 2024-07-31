using Biblioteca.Services;
using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    public class ConnectionStatus
    {
        private static bool _online = true;
        public static bool Online => _online;

        public static void CheckConnectivity()
        {
            _online = NetworkService.CheckConnection().Success;
        }
    }
}
