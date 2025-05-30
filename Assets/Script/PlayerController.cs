using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GattoStats gattoStats;

    public float moveSpeed;

    public event Action<GattoStats> OnBattle;
    private bool isMoving;
    public bool hasWin;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;
    }

    public Vector3 getInteractPos()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        return transform.position + facingDir;
    }

    public void InputPlayer(InputAction.CallbackContext _context)
    {
        input = _context.ReadValue<Vector2>();
    } 

    public void HandleUpdate()
    {
        if (!isMoving)
        {   

            if (input != Vector2.zero)
            {

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);


                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }

        }

        animator.SetBool("isMoving", isMoving);

        if (Physics2D.OverlapCircle(getInteractPos(), 0.2f, interactableLayer) != null)
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
            StartCoroutine(Interact());
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool isWalkable(Vector3 targetPos)
    {
        return !(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null);
    }


    public bool hasNearbyInteractable()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 300f, interactableLayer);
        return collider != null && collider.GetComponent<Interactable>() != null;
    }

    public void EnemyDefeated()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        collider.gameObject.SetActive(false);
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact();
            yield return new WaitForSeconds(0.2f);

            Debug.Log($"Encountered {collider.GetComponent<Interactable>()?.GetGattoStats().Name}");

            OnBattle?.Invoke(collider.GetComponent<Interactable>()?.GetGattoStats());
        }
        
        
        
    }    


    
}