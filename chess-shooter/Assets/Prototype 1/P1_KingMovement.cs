using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class P1_KingMovement : P1_PieceMovement
{
    public Vector3 originPos;
    public Vector3 targetPos;
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

        if (movementController.pieces.Count > 2)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }

        //if (attack)
        //{
        //    if (attackTimer > 0)
        //    {
        //        attackTimer -= Time.deltaTime;
        //        float attackTimerScaled = 1 - ((attackTimer - 0.1f)/ (movementController.cycleTime * 2));

        //        if (attackTimerScaled > 0.5f)
        //        {
        //            transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow((attackTimerScaled * 2) -1, 2f));
        //        }
        //    }
        //    else
        //    {
        //        transform.position = targetPos;
        //        originPos = targetPos;
        //        attack = false;
        //    }
        //}
    }

    int moveCooldown = 0;
    bool attack;
    float attackTimer;
    public Vector2 moveDirNorm;
    public float moveAngle;

    public override void ExecuteMove()
    {
        if (attack)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            movementController.takenPositions.Add(originPos);
            movementController.takenPositions.Add(targetPos);

            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));

            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) + 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) - 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) + 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) - 1));

            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y)));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y)));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 1));

        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        transform.position = targetPos;
        originPos = targetPos;
        targetMoveTimer = movementController.cycleTime * 0.75f;

        // Decision AI
        if (moveCooldown == 0) moveCooldown = 1;
        else moveCooldown -= 1;

        float loop = 5;

        while (loop >= 0)
        {
            loop--;

            Vector2 moveDir = movementController.player.transform.position - transform.position;
            moveDirNorm = moveDir.normalized;
            moveAngle = Vector2.SignedAngle(moveDirNorm, Vector2.up);
            if (loop == 3) moveAngle += 45;
            if (loop == 2) moveAngle += -45;
            if (moveAngle < 0) moveAngle += 360;


            //if (Mathf.RoundToInt(Mathf.Abs(moveDir.x) * 100) == 100 && Mathf.RoundToInt(Mathf.Abs(moveDir.y) * 100) == 100)
            //{
            //    targetPos = movementController.player.transform.position;
            //    movementController.takenPositions.Add(originPos);
            //    movementController.takenPositions.Add(targetPos);
            //    attackTimer = movementController.cycleTime * 2.1f;
            //    attackDir = moveDir;
            //    attack = true;

            //    movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) + 1));
            //    movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) - 1));
            //    movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) + 1));
            //    movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) - 1));

            //    break;
            //}

            if (moveCooldown != 0) loop = 0;

            if (loop == 0)
            {
                movementController.takenPositions.Add(originPos);
                break;
            }

            if (moveAngle >= 337.5f || moveAngle < 22.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.up))
                {
                    targetPos += Vector3.up;
                    break;
                }
            }
            else if (moveAngle >= 22.5f && moveAngle < 67.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.up + Vector3.right))
                {
                    targetPos += Vector3.up + Vector3.right;
                    break;
                }
            }
            else if (moveAngle >= 67.5f && moveAngle < 112.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.right))
                {
                    targetPos += Vector3.right;
                    break;
                }
            }
            else if (moveAngle >= 112.5f && moveAngle < 157.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.down + Vector3.right))
                {
                    targetPos += Vector3.down + Vector3.right;
                    break;
                }
            }
            else if (moveAngle >= 157.5f && moveAngle < 202.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.down))
                {
                    targetPos += Vector3.down;
                    break;
                }
            }
            else if (moveAngle >= 202.5f && moveAngle < 247.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.down + Vector3.left))
                {
                    targetPos += Vector3.down + Vector3.left;
                    break;
                }
            }
            else if (moveAngle >= 247.5f && moveAngle < 292.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.left))
                {
                    targetPos += Vector3.left;
                    break;
                }
            }
            else if (moveAngle >= 292.5f && moveAngle < 337.5f)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.up + Vector3.left))
                {
                    targetPos += Vector3.up + Vector3.left;
                    break;
                }
            }
        }

        targetPos = ClampToGrid(targetPos, 1, 1, 8, 8);
        targetPos = SnapToGrid(targetPos);
    }
}
