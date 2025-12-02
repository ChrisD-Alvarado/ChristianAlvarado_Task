using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;

    private PlayerActions controls;
    private WorldDirection currentFacingDirection = WorldDirection.Down;

    [SerializeField]
    float moveDuration = 0.3f;
    [SerializeField]
    float interactionDelay = 0.3f;

    bool moveOrdered = false;
    bool isCurrentlyMoving = false;
    bool playerIsInteracting = false;
    bool playerCanInteract = true;

    PlayerInteractableDetector interactableDetector;

    Animator animator;
    const string ANIMATOR_MOVING_PARAMETER = "Moving";
    const string ANIMATOR_FACING_PARAMETER_PREFIX = "Facing";
    const string ANIMATOR_USE_PARAMETER_PREFIX = "Use";


    private void Awake()
    {
        controls = new PlayerActions();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls.Main.Movement.started += ctx => HandleMove(ctx.ReadValue<Vector2>());
        controls.Main.Movement.canceled += ctx => HandleMoveCanceled();
        //controls.Main.UseItem.performed
        //controls.Main.Interact.performed
    }

    private void Update()
    {
        if (moveOrdered && !isCurrentlyMoving)
        {
            if (CanMove(GetFacingDirectionAsVector(currentFacingDirection)))
            {
                StartCoroutine(Move(transform.position + (Vector3)GetFacingDirectionAsVector(currentFacingDirection)));
            }
        }
    }

    private void HandleMove(Vector2 direction)
    {
        //ignore empty movements
        if (direction == Vector2.zero) return;

        if (!playerCanInteract) return;

        Debug.Log($"HandleMove. Direction: {(direction)}");

        SetFacingDirection(GetFacingDirection(direction));


        if (CanMove(direction))
        {
            moveOrdered = true;
        }
        else
        {
            moveOrdered = false;
        }
    }

    bool facingDirectionChanged = false;
    void SetFacingDirection(WorldDirection direction)
    {
        if(direction != WorldDirection.None && currentFacingDirection != direction)
        {
            facingDirectionChanged = true;
        }

        if (direction != WorldDirection.None)
        {
             animator.SetBool(ANIMATOR_FACING_PARAMETER_PREFIX + currentFacingDirection.ToString(), false);

            currentFacingDirection = direction;

            if (!isCurrentlyMoving)
            {
                animator.SetBool(ANIMATOR_FACING_PARAMETER_PREFIX + currentFacingDirection.ToString(), true);
                facingDirectionChanged = false;
            }
        }
    }

    private void HandleMoveCanceled()
    {
        if(controls.Main.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            Debug.Log("Movement is still ocurring");
            return;
        }

        Debug.Log("Handle Move Canceled");
        moveOrdered = false;
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPos) || collisionTilemap.HasTile(gridPos))
        {
            return false;
        }

        return true;
    }

    IEnumerator Move(Vector2 direction)
    {
        isCurrentlyMoving = true;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = direction;
        
        animator.SetBool(ANIMATOR_FACING_PARAMETER_PREFIX + currentFacingDirection.ToString(), true);
    

        animator.SetBool(ANIMATOR_MOVING_PARAMETER, true);

        float elapsedTime = 0;
        while(elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        transform.position = endPosition;
        isCurrentlyMoving = false;

        if (GetFacingDirectionAsVector(currentFacingDirection) != controls.Main.Movement.ReadValue<Vector2>())
        {
            Debug.Log($"Direction changed. New direction:{controls.Main.Movement.ReadValue<Vector2>()}");
            SetFacingDirection(GetFacingDirection(controls.Main.Movement.ReadValue<Vector2>()));
        }

        if (!moveOrdered)
        {
            StartCoroutine(WaitForMovementEndDelay());
        }
    }

    IEnumerator WaitForInteractionDelay()
    {
        while (isCurrentlyMoving)
        {
            yield return null;
        }

        playerIsInteracting = true;

        float elapsedTime = 0;
        while (elapsedTime < interactionDelay)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / interactionDelay;
            yield return null;
        }

        playerIsInteracting = false;
    }

    IEnumerator WaitForMovementEndDelay()
    {
        while (isCurrentlyMoving)
        {
            yield return null;
        }

        if (controls.Main.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            yield break;
        }


        animator.SetBool(ANIMATOR_MOVING_PARAMETER, false);

        playerCanInteract = false;

        float elapsedTime = 0;
        while (elapsedTime < interactionDelay)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / interactionDelay;
            yield return null;
        }

        playerCanInteract = true;


        if (controls.Main.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            moveOrdered = true;
            SetFacingDirection(GetFacingDirection(controls.Main.Movement.ReadValue<Vector2>()));
        }
    }



    WorldDirection GetFacingDirection(Vector2 direction)
    {
        return direction switch
        {
            _ when direction == new Vector2(1, 0) => WorldDirection.Right,
            _ when direction == new Vector2(0, 1) => WorldDirection.Up,
            _ when direction == new Vector2(-1, 0) => WorldDirection.Left,
            _ when direction == new Vector2(0, -1) => WorldDirection.Down,

            //In case something goes wrong:
            _ when direction == new Vector2(0, 0) => WorldDirection.None,
        };
    }

    Vector2 GetFacingDirectionAsVector(WorldDirection direction)
    {
        switch (direction)
        {
            case WorldDirection.Up:
                return new Vector2(0, 1);

            case WorldDirection.Down:
                return new Vector2(0, -1);

            case WorldDirection.Right:
                return new Vector2(1, 0);

            case WorldDirection.Left:
                return new Vector2(-1, 0);

            case WorldDirection.None:
            default:
                return new Vector2(0, 0);
        }
    }
}

public enum WorldDirection
{
    Up,
    Down,
    Right,
    Left,
    None
}
