using UnityEngine;

// NGO/LiteNetLib implementation

// NGO/LiteNetLib implementation
// using other namespaces...

namespace Echoes.Game.Entrypoints
{
    public class AppBootstrapper : MonoBehaviour
    {
        // A simple static container or Service Locator
        // public static ServiceRegistry Container { get; private set; }

        private void Awake()
        {
            // Container = new ServiceRegistry();

#if UNITY_SERVER
            InitializeServer();
#else
            InitializeClient();
#endif
        }

        private void InitializeServer()
        {
            Debug.Log("Server Initialized.");
        }

        private void InitializeClient()
        {
            Debug.Log("Client Initialized.");
        }
    }
}
