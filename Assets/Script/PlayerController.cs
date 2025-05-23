using System;
using System.Collections;
using UnityEngine;

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

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            #if UNITY_EDITOR
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");
            #else
                input = TouchInput.SwipeDirection;
            #endif

            Debug.Log("This is input.x" + input.x);
            Debug.Log("This is input.y" + input.y);

            if (input.x != 0) input.y = 0;

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


    IEnumerator Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            if (hasWin)
            {
                Debug.Log($"I've win the battle, i'm going to deactivate the collider {collider.gameObject.name}");
                collider.gameObject.SetActive(false);
                hasWin = false;
            }
            else
            {
                yield return collider.GetComponent<Interactable>()?.Interact();
                yield return new WaitForSeconds(0.2f);
                
                Debug.Log($"Encountered {collider.GetComponent<Interactable>()?.GetGattoStats().Name}");

                OnBattle?.Invoke(collider.GetComponent<Interactable>()?.GetGattoStats());
            }
        }
    }    


    
}