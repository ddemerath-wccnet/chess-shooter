using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class P1_KnightMovement : P1_PieceMovement
{
    Vector3 originPos;
    Vector3 targetPos;
    float targetMoveTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPos = transform.position;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();

        if (targetMoveTimer > 0 && attack == false) { 
            targetMoveTimer -= Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow(1 - (targetMoveTimer / (movementController.cycleTime * 0.75f)), 2f));
        }

        if (attack)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                //float attackTimerScaled = 1 - ((attackTimer - 0.1f)/ (movementController.cycleTime * 2));

                //if (attackTimerScaled > 0.5f)
                //{
                //    transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow((attackTimerScaled * 2) -1, 2f));
                //}
            }
            else
            {
                transform.position = targetPos;
                originPos = targetPos;
                attack = false;
            }
        }
    }

    int moveCooldown = 0;
    bool attack;
    float attackTimer;
    Vector2 attackDir;

    public override void ExecuteMove()
    {
        transform.position = targetPos;
        originPos = targetPos;
        targetMoveTimer = movementController.cycleTime * 0.75f;

        // Decision AI
        if (moveCooldown == 0) moveCooldown = 3;
        else moveCooldown -= 1;

        if (moveCooldown == 2) GetComponent<SpriteRenderer>().color = Color.white;

        float loop = 10;

        while (loop >= 0)
        {
            loop--;

            if (moveCooldown != 0) loop = 0;

            if (loop == 0)
            {
                movementController.takenPositions.Add(originPos);
                break;
            }

            Vector3 moveDir = new Vector2();

            if (UnityEngine.Random.Range(-1, 1) >= 0)
            {
                if (UnityEngine.Random.Range(-1, 1) >= 0)
                {
                    moveDir.x = 2;
                }
                else
                {
                    moveDir.x = -2;
                }
                if (UnityEngine.Random.Range(-1, 1) >= 0)
                {
                    moveDir.y = 1;
                }
                else
                {
                    moveDir.y = -1;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(-1, 1) >= 0)
                {
                    moveDir.y = 2;
                }
                else
                {
                    moveDir.y = -2;
                }
                if (UnityEngine.Random.Range(-1, 1) >= 0)
                {
                    moveDir.x = 1;
                }
                else
                {
                    moveDir.x = -1;
                }
            }

            if (Mathf.RoundToInt((targetPos + moveDir).x) <= 8 &&
                Mathf.RoundToInt((targetPos + moveDir).y) <= 8 &&
                Mathf.RoundToInt((targetPos + moveDir).x) >= 1 &&
                Mathf.RoundToInt((targetPos + moveDir).y) >= 1 &&
                movementController.AttemptTarget(targetPos + moveDir))
            {
                targetPos += moveDir;
                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)));
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            }
        }

        targetPos = ClampToGrid(targetPos, 1, 1, 8, 8);
        targetPos = SnapToGrid(targetPos);
    }
}
