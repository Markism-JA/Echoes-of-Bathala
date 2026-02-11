
// using Unity.Netcode;
// using UnityEngine;
//
// namespace Project.Scripts.Client
// {
//     public class ZoneManager : NetworkBehaviour
//     {
//         public PlayerController PlayerController;
//         [SerializeField] private Transform target;
//         [SerializeField] private Vector3 playerPosition;
//         
//
//
//         void FixedUpdate()
//         {
//             CheckPosition();
//         }
//
//         private Vector3 CheckPosition()
//         {
//             target.position = playerPosition;
//
//             if (InSafeZone())
//             {
//                  //Handle Safe Zone Restrictions and Features
//
//             }
//             else if (InContestedZone())
//             {
//                 //Handle Contested Zone Restrictions and Features
//
//             }
//             else if (InVoidZone())
//             {
//                 //Handle Void Zone Restrictions and Features
//             }
//             return target.position;
//             
//         }
//
//         // A method that checks the position of the player
//         // if it is within the bound of the safe zone which is
//         // withing 0-50 meters.//
//         private bool InSafeZone()
//         {
//             Vector3 safezonePoint = new Vector3(0f, 0f, 0f);
//             if (Vector3.Distance(safezonePoint, playerPosition) ! >= 50f)
//             {
//                 return true;
//             }
//
//             return false;
//         }
//
//         // A method that checks the position of the player
//         // if it is within the bound of the safe zone which is
//         // withing 50-100 meters.//
//         private bool InContestedZone()
//         {
//             Vector3 contestedZonePoint = new Vector3(50f, 0f, 50f);
//             if (Vector3.Distance(contestedZonePoint, playerPosition) ! >= 100f)
//             {
//                 return true;
//             }
//
//             return false;
//         }
//
//         // A method that checks the position of the player
//         // if it is within the bound of the safe zone which is
//         // withing 100 and beyond meters.//
//         private bool InVoidZone()
//         {
//             Vector3 voidZonePoint = new Vector3(100f, 0f, 100f);
//             if (Vector3.Distance(voidZonePoint, playerPosition) <= 1f)
//             {
//                 return true;
//             }
//
//             return false;
//         }
//
//
//         // Sends the type of zone the player is in to the server
//         [ServerRpc]
//         private void OnZoneChangeServerRpc(ZoneType zone)
//         {
//             PlayerController.CurrentZone = zone;
//         }
//
//     }
// }

