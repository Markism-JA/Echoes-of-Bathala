using Unity.Netcode;
using UnityEngine;

public class ZoneManager : NetworkBehaviour
{
    public PlayerController playerController;
    [SerializeField] private string tagFilter =  "Player";
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 playerPosition;


    void FixedUpdate()
    {
        checkPosition();
    }

    private Vector3 checkPosition()
    {
        return target.position = playerPosition;
    }
    
    
    private bool InSafeZone()
    {
        Vector3 safezonePoint = new Vector3(0f, 0f, 0f);
        if (Vector3.Distance(safezonePoint, playerPosition) !>= 50f )
        {
            return true;
        }

        return false;
    }

    private bool InContestedZone()
    {
        Vector3 contestedZonePoint = new Vector3(50f, 0f, 50f);
        if (Vector3.Distance(contestedZonePoint, playerPosition) !>= 100f)
        {
            return true;
        }
        return false;
    }

    private bool InVoidZone()
    {
        Vector3 voidZonePoint = new Vector3(100f, 0f, 100f);
        if (Vector3.Distance(voidZonePoint, playerPosition) <= 1f)
        {
            return true;
        }
        return false;
    }
    
    [ServerRpc]
    public void OnZoneChangeServerRpc(ZoneType zone)
    {
        playerController.CurrentZone = zone;
    }

}
