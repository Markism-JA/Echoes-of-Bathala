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
        private Vector3 rotationTarget;
        private bool isDashing;
        private Vector2 moveInput;
        private Vector2 mouseMoveInput;
        private bool isMouseDown;
        [SerializeField] private CharacterController controller;
        
        
        void Start()
        {
            inputReader.moveEvent += CacheMoveInput;
            inputReader.interactEvent += InteractServerRPC;
            inputReader.lookEvent += CacheMouseMoveInput;
            inputReader.dashEvent += DashServerRPC;
        }

        private void CacheMoveInput(Vector2 clientMoveInput)
        {
            moveInput = clientMoveInput;
        }
        
        private void CacheMouseMoveInput(Vector2 clientMouseMoveInput)
        {
            mouseMoveInput = clientMouseMoveInput;
        }

        
        
        [ServerRpc]
        public void MoveServerRPC()
        {
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
            controller.Move(movement * (moveSpeed * Time.deltaTime));
        }
        
        [ServerRpc]
        public void DashServerRPC()
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
        public void LookServerRPC()
        {
            
            RaycastHit hit;
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(mouseMoveInput);

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
        public void InteractServerRPC()
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
    }
}
