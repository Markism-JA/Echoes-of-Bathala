
using Unity.Netcode;
using UnityEngine;
using Project.Scripts.Client;

namespace Project.Scripts.Server
{
    public class ZoneManager : NetworkBehaviour
    {   
        void FixedUpdate()
        {
            
            if (!IsServer) return;
            UpdatePlayersZone();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdatePlayersZone()
        { 
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var player = client.PlayerObject.GetComponent<Player>();
                Vector3 currentPosition = client.PlayerObject.transform.position;
                float playerDistance = Vector3.Distance(currentPosition, Vector3.zero);
                
                ZoneType detectedZone = DetermineZone(playerDistance);

                if (player.CurrentZone.Value != detectedZone)
                {
                    player.CurrentZone.Value = detectedZone;
                }
            }

        }

        private ZoneType DetermineZone(float distance)
        {
            if (distance <= 50f)
                return ZoneType.SafeZone;

            if (distance <= 150f && distance > 50f)
                return ZoneType.ContestedZone;

            if (distance > 150f)
                return ZoneType.VoidZone;

            return ZoneType.None;
        }
    }
}

