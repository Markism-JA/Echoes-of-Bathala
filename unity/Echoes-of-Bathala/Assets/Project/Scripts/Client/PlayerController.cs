
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerController : MonoBehaviour 
{
    [SerializeField] InputReader inputReader;
    public CharacterController controller;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 rotationTarget;
    private Vector3 movement;
    private bool isDashing;
    
    void Start()
    {
        inputReader.moveEvent += HandleMove;
        inputReader.interactEvent += HandleInteract;
        inputReader.lookEvent += HandleLook;
        inputReader.dashEvent += HandleDash;
    }
    
    public void HandleInteract()
    {

    }
    public void HandleDash(){
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *=  dashSpeed;
            trailRenderer.material.color = Color.red;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }

    }
    public void HandleLook(Vector2 mouseLook)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);

        if (Physics.Raycast(ray, out hit))
        {
            rotationTarget = hit.point;
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
    public void HandleMove(Vector2 moveInput)
    {
        movement = new Vector3(moveInput.x, 0f, moveInput.y);
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

    void Update()
    {
        controller.Move(movement * (moveSpeed * Time.deltaTime));
    }
}

