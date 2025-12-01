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
    [SerializeField]
    float moveDuration = 0.3f;

    bool moving = false;

    private void Awake()
    {
        controls = new PlayerActions();
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
        controls.Main.Movement.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());
        controls.Main.Movement.canceled += ctx => HandleMoveCanceled();
    }

    private void HandleMove(Vector2 direction)
    {
        if (CanMove(direction))
        {
            moving = true;
            StartCoroutine(Move(transform.position + (Vector3)direction));
        }
        else
        {
            moving = false;
        }
    }

    private void HandleMoveCanceled()
    {
        moving = false;
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
        Vector2 startPosition = transform.position;
        Vector2 endPosition = direction;

        float elapsedTime = 0;
        while(elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        transform.position = endPosition;
    }
}
