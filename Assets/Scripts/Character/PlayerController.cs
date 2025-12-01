using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;

    private PlayerActions controls;
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
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            transform.position += (Vector3)direction;
        }
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
}
