using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Project.Scripts.Client
{
    [CreateAssetMenu(fileName = "New Input", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        private PlayerControls controls;

        // Public events for other scripts to subscribe to
        public event Action<Vector2> moveEvent;
        public event Action<Vector2> lookEvent;
        public event Action interactEvent;
        public event Action dashEvent;

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new PlayerControls(); // Initialize the input actions
                controls.Player.SetCallbacks(this); // Set this class to handle the callbacks
            }
            controls.Player.Enable(); // Enable the action map
        }

        private void OnDisable()
        {
            controls?.Player.Disable(); // Disable when not needed
        }

        // Implement the interface methods (called when inputs occur)
        public void OnMove(InputAction.CallbackContext context)
        {
            moveEvent?.Invoke(context.ReadValue<Vector2>()); // Trigger move event with input value
        }


        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                interactEvent?.Invoke();
            }
        }

        public void OnMouseLook(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                lookEvent?.Invoke(context.ReadValue<Vector2>()); // Trigger look event with input value
            }
        }
        public void OnDash(InputAction.CallbackContext context)
        {
            dashEvent?.Invoke();
        }
    }
}
