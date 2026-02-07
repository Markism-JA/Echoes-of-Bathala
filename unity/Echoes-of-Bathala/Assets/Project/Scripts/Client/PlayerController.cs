
using UnityEngine;


public class PlayerController : MonoBehaviour 
{
    [SerializeField] InputReader inputReader;
    [SerializeField] private float speed = 20f;
    private Vector3 rotationTarget;

    void Start()
    {
        inputReader.moveEvent += HandleMove;
        inputReader.interactEvent += HandleInteract;
        inputReader.lookEvent += HandleLook;
    }
    public void HandleInteract()
    {

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
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

    }

}

