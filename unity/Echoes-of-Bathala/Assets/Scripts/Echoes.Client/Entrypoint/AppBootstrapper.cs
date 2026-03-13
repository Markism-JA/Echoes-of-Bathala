using UnityEngine;

namespace Echoes.Client.Entrypoint
{
    public class AppBootstrapper : MonoBehaviour
    {
        // A simple static container or Service Locator
        // public static ServiceRegistry Container { get; private set; }

        private void Awake()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            Debug.Log("Client Initialized.");
        }
    }
}
