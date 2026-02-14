
using Unity.Netcode;
using UnityEngine;
using Project.Scripts.Client;

namespace Project.Scripts.Server
{
    public class ZoneManager : NetworkBehaviour
    {   
        Player player;
        void FixedUpdate()
        {
            if (!IsServer) return;
            UpdatePlayerZone();
        }

        private void UpdatePlayerZone()
        {
            float playerDistance = Vector3.Distance(player.transform.position, Vector3.zero);

            ZoneType detectedZone = DetermineZone(playerDistance);


            if (player.CurrentZone.Value != detectedZone)
            {
                player.CurrentZone.Value = detectedZone;
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

