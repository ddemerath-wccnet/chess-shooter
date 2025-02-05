using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class P1_QueenMovement : P1_PieceMovement
{
    public P1_RookMovement rookMovement;
    public P1_BishopMovement bishopMovement;
    public P1_KingMovement kingMovement;

    float SwitchTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rookMovement.enabled = false;
        bishopMovement.enabled = false;
        kingMovement.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
        SwitchTimer -= Time.deltaTime;
        if (SwitchTimer < 0)
        {
            rookMovement.enabled = false;
            bishopMovement.enabled = false;
            kingMovement.enabled = false;

            rookMovement.targetPos = transform.position;
            rookMovement.originPos = transform.position;
            bishopMovement.targetPos = transform.position;
            bishopMovement.originPos = transform.position;
            kingMovement.targetPos = transform.position;
            kingMovement.originPos = transform.position;

            SwitchTimer = movementController.cycleTime * UnityEngine.Random.Range(6f, 10f);
            if (UnityEngine.Random.Range(1, 4) == 1) rookMovement.enabled = true;
            else if (UnityEngine.Random.Range(1, 3) == 1) bishopMovement.enabled = true;
            else kingMovement.enabled = true;
        }
    }

    public override void ExecuteMove()
    {

    }
}
