using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class P1_PawnMovement : P1_PieceMovement
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
                float attackTimerScaled = 1 - ((attackTimer - 0.1f)/ (movementController.cycleTime * 2));

                if (attackTimerScaled > 0.5f)
                {
                    transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow((attackTimerScaled * 2) -1, 2f));
                }
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
        if (attack)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            movementController.takenPositions.Add(originPos);
            movementController.takenPositions.Add(targetPos);

            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) + 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) - 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) + 1));
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) - 1));

            return;
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
            Vector2 moveDirNorm = moveDir.normalized;

            if (Mathf.RoundToInt(Mathf.Abs(moveDir.x) * 100) == 100 && Mathf.RoundToInt(Mathf.Abs(moveDir.y) * 100) == 100)
            {
                targetPos = movementController.player.transform.position;
                movementController.takenPositions.Add(originPos);
                movementController.takenPositions.Add(targetPos);
                attackTimer = movementController.cycleTime * 2.1f;
                attackDir = moveDir;
                attack = true;

                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) + 1));
                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y) - 1));
                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) + 1));
                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y) - 1));

                break;
            }

            if (moveCooldown != 0) loop = 0;

            if (loop == 0)
            {
                movementController.takenPositions.Add(originPos);
                break;
            }

            if (Mathf.Abs(moveDirNorm.x) > Mathf.Abs(moveDirNorm.y))
            {
                if (moveDirNorm.x > 0)
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.right))
                    {
                        if (new int3(Mathf.RoundToInt((targetPos + Vector3.right).x), Mathf.RoundToInt((targetPos + Vector3.right).y), Mathf.RoundToInt((targetPos + Vector3.right).z))
                            .Equals(new int3(Mathf.RoundToInt(movementController.player.transform.position.x), Mathf.RoundToInt(movementController.player.transform.position.y), Mathf.RoundToInt(movementController.player.transform.position.z))))
                        {
                            if (UnityEngine.Random.Range(-1, 1) >= 0)
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.up))
                                {
                                    targetPos += Vector3.up;
                                    break;
                                }
                            }
                            else
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.down))
                                {
                                    targetPos += Vector3.down;
                                    break;
                                }
                            }
                        }
                        targetPos += Vector3.right;
                        break;
                    }
                }
                else
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.left))
                    {
                        if (new int3(Mathf.RoundToInt((targetPos + Vector3.left).x), Mathf.RoundToInt((targetPos + Vector3.left).y), Mathf.RoundToInt((targetPos + Vector3.left).z))
                            .Equals(new int3(Mathf.RoundToInt(movementController.player.transform.position.x), Mathf.RoundToInt(movementController.player.transform.position.y), Mathf.RoundToInt(movementController.player.transform.position.z))))
                        {
                            if (UnityEngine.Random.Range(-1, 1) >= 0)
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.up))
                                {
                                    targetPos += Vector3.up;
                                    break;
                                }
                            }
                            else
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.down))
                                {
                                    targetPos += Vector3.down;
                                    break;
                                }
                            }
                        }
                        targetPos += Vector3.left;
                        break;
                    }
                }
            }
            else
            {
                if (moveDirNorm.y > 0)
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.up))
                    {
                        if (new int3(Mathf.RoundToInt((targetPos + Vector3.up).x), Mathf.RoundToInt((targetPos + Vector3.up).y), Mathf.RoundToInt((targetPos + Vector3.up).z))
                            .Equals(new int3(Mathf.RoundToInt(movementController.player.transform.position.x), Mathf.RoundToInt(movementController.player.transform.position.y), Mathf.RoundToInt(movementController.player.transform.position.z))))
                        {
                            if (UnityEngine.Random.Range(-1, 1) >= 0)
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.right))
                                {
                                    targetPos += Vector3.right;
                                    break;
                                }
                            }
                            else
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.left))
                                {
                                    targetPos += Vector3.left;
                                    break;
                                }
                            }
                        }
                        targetPos += Vector3.up;
                        break;
                    }
                }
                else
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.down))
                    {
                        if (new int3(Mathf.RoundToInt((targetPos + Vector3.down).x), Mathf.RoundToInt((targetPos + Vector3.down).y), Mathf.RoundToInt((targetPos + Vector3.down).z))
                            .Equals(new int3(Mathf.RoundToInt(movementController.player.transform.position.x), Mathf.RoundToInt(movementController.player.transform.position.y), Mathf.RoundToInt(movementController.player.transform.position.z))))
                        {
                            if (UnityEngine.Random.Range(-1, 1) >= 0)
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.right))
                                {
                                    targetPos += Vector3.right;
                                    break;
                                }
                            }
                            else
                            {
                                if (movementController.AttemptTarget(targetPos + Vector3.left))
                                {
                                    targetPos += Vector3.left;
                                    break;
                                }
                            }
                        }
                        targetPos += Vector3.down;
                        break;
                    }
                }
            }
        }

        targetPos = ClampToGrid(targetPos, 1, 1, 8, 8);
        targetPos = SnapToGrid(targetPos);
    }
}
