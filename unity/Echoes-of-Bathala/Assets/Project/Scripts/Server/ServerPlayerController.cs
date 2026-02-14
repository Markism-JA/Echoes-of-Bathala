using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Project.Scripts.Client;

namespace Project.Scripts.Server
{
    public class ServerPlayerController : NetworkBehaviour
    {
        [SerializeField] InputReader inputReader;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float dashSpeed = 4f;
        [SerializeField] private TrailRenderer trailRenderer;
        private bool isDashing;
        private Vector2 moveInput;
        private Vector3 rotationTarget;

        private bool isMouseDown;
        [SerializeField] private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            inputReader.moveEvent += CacheMoveInput;
            inputReader.dashEvent += OnDashRequested;
            inputReader.lookEvent += OnCharacterLookRequested;
        }

        public override void OnNetworkDespawn()
        {
            inputReader.moveEvent -= CacheMoveInput;
            inputReader.dashEvent -= OnDashRequested;

        }

        private void CacheMoveInput(Vector2 clientMoveInput)
        {
            moveInput = clientMoveInput;
        }

        // private void CacheMouseMoveInput(Vector2 clientMouseMoveInput)
        // {
        //     mouseMoveInput = clientMouseMoveInput;
        // }

        private void OnDashRequested()
        {
            if (!IsOwner) return;
            DashServerRPC();
        }

        private void OnCharacterLookRequested(Vector2 mousePoint)
        {
            if (!IsOwner) return;
            LookServerRPC(mousePoint);
        }

        void Update()
        {
            if (!IsOwner) return;
            MoveServerRPC(moveInput);

        }


        [ServerRpc]
        private void MoveServerRPC(Vector2 input)
        {
            Vector3 movement = new Vector3(input.x, 0f, input.y);
            controller.Move(movement * (moveSpeed * Time.deltaTime));
        }

        [ServerRpc]
        private void DashServerRPC()
        {

            if (!isDashing)
            {
                isDashing = true;
                moveSpeed *= dashSpeed;
                trailRenderer.material.color = Color.red;
                trailRenderer.emitting = true;
                StartCoroutine(EndDashRoutine());
            }
        }

        [ServerRpc]
        private void LookServerRPC(Vector2 input)

        {
            RaycastHit hit;
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(input);
            
                if (Physics.Raycast(ray, out hit))
                {
                    rotationTarget = hit.point;
                }
            }
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }
        }

        [ServerRpc]
        private void InteractServerRPC()
        {

        }

        private IEnumerator EndDashRoutine()
        {
            float dashTime = .2f;
            float dashCD = .25f;
            yield return new WaitForSeconds(dashTime);
            moveSpeed /= dashSpeed;
            trailRenderer.emitting = false;
            yield return new WaitForSeconds(dashCD);
            isDashing = false;
        }

    // private Vector3 calculatePlayerDirection(Vector2 mouseMoveInput)
    // {
    //     RaycastHit hit;
    //     if (Camera.main != null)
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(mouseMoveInput);
    //
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             return hit.point;
    //         }
    //     }
    //     return rotationTarget;
    // }
}

}


