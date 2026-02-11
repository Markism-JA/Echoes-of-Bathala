
using Project.Scripts.Server;
using Unity.Netcode;
using UnityEngine;

namespace Project.Scripts.Client
{
    public struct ZoneType
    {
        
    }

    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] CharacterController characterController;
        private NetworkVariable<ZoneType> currentZone = new NetworkVariable<ZoneType>();
        private ServerPlayerController serverPlayerController;
        


        public ZoneType CurrentZone
        {
            get => currentZone.Value;
            set => currentZone.Value = value;
        }


        public void HandleInteract()
        {
            // Put Server Interact RPC
        }

        public void PlayerDash()
        {
            serverPlayerController.DashServerRPC();
        }

        private void PlayerLook()
        {
            serverPlayerController.LookServerRPC();
        }

        private void PlayerMove()
        {
            if (IsOwner)
            {
                serverPlayerController.MoveServerRPC();
            }
        }

        void Start()
        {
            PlayerLook();
            PlayerDash();
        }

        void Update()
        {
            PlayerMove();

        }
    }
    
}

