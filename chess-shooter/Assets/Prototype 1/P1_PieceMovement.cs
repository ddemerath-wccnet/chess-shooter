using UnityEngine;

public abstract class P1_PieceMovement : MonoBehaviour
{
    public P1_MovementController movementController;
    public void UpdateBase()
    {
        if (movementController == null)
        {
            try
            {
                movementController = FindAnyObjectByType<P1_MovementController>();
                if (!movementController.pieces.Contains(this)) movementController.pieces.Add(this);
            }
            catch { }
        }
    }
    public abstract void ExecuteMove();

    public Vector3 SnapToGrid(Vector3 toSnap)
    {
        return new Vector3(Mathf.RoundToInt(toSnap.x), Mathf.RoundToInt(toSnap.y), Mathf.RoundToInt(toSnap.z));
    }

    public Vector3 ClampToGrid(Vector3 toSnap, float minX, float minY, float maxX, float maxY)
    {
        return new Vector3(Mathf.Clamp(toSnap.x, minX, maxX), Mathf.Clamp(toSnap.y, minY, maxY), toSnap.z);
    }
}
