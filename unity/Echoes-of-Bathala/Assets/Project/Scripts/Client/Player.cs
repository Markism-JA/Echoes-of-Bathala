

    using System.Numerics;
    using Unity.Netcode;
    using UnityEngine;

    namespace Project.Scripts.Client
    {
        public enum ZoneType
        {
            SafeZone,
            ContestedZone,
            VoidZone,
            None
        }

        public class Player : NetworkBehaviour
        {

            public NetworkVariable<ZoneType> CurrentZone = new NetworkVariable<ZoneType>(
                ZoneType.None,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Server
            );
            
            public override void OnNetworkSpawn()
            {
                // Subscribe to changes so the client can react immediately
                CurrentZone.OnValueChanged += OnZoneChanged;
            }

            public override void OnNetworkDespawn()
            {
                CurrentZone.OnValueChanged -= OnZoneChanged;
            }
            
            private void OnZoneChanged(ZoneType previousValue, ZoneType newValue)
            {
                Debug.Log($"Client updated: Player is now in {newValue}");
            }
        }
    }

