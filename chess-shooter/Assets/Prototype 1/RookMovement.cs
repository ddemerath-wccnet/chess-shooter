using Unity.Mathematics;
using UnityEngine;

public class RookMovement : PieceMovement
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
    int attackDir;

    public override void ExecuteMove()
    {
        if (attack)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            movementController.takenPositions.Add(originPos);
            movementController.takenPositions.Add(targetPos);

            for (int i = 0; i < 8; i++)
            {
                movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + (i * attackDir)));
            }

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
            if (moveCooldown != 0) loop = 0;

            if(Vector3.Distance(movementController.player.targetPos, transform.position) >= 3)
            {
                if (loop == 4 && (Mathf.RoundToInt(movementController.player.targetPos.x) == Mathf.RoundToInt(transform.position.x))
                    || Mathf.RoundToInt(movementController.player.originPos.x) == Mathf.RoundToInt(transform.position.x))
                {
                    if (Mathf.RoundToInt(movementController.player.targetPos.y) > Mathf.RoundToInt(transform.position.y))
                    {
                        movementController.takenPositions.Add(originPos);
                        targetPos = new Vector3(targetPos.x, 8, targetPos.z);
                        attackTimer = movementController.cycleTime * 2.1f;
                        attackDir = 1;
                        attack = true;

                        for (int i = 0; i < 8; i++) {
                            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + (i * attackDir)));
                        }

                        break;
                    }

                    if (Mathf.RoundToInt(movementController.player.targetPos.y) < Mathf.RoundToInt(transform.position.y))
                    {
                        movementController.takenPositions.Add(originPos);
                        targetPos = new Vector3(targetPos.x, 1, targetPos.z);
                        attackTimer = movementController.cycleTime * 2.1f;
                        attackDir = -1;
                        attack = true;

                        for (int i = 0; i < 8; i++)
                        {
                            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + (i * attackDir)));
                        }

                        break;
                    }
                }

                if (loop == 3 && Mathf.RoundToInt(movementController.player.targetPos.x) > Mathf.RoundToInt(transform.position.x))
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.right))
                    {
                        targetPos += Vector3.right;
                        break;
                    }
                    else if (Mathf.RoundToInt(movementController.player.targetPos.y) > Mathf.RoundToInt(transform.position.y)
                        && movementController.AttemptTarget(targetPos + Vector3.up))
                    {
                        targetPos += Vector3.up;
                        break;
                    }
                    else if (Mathf.RoundToInt(movementController.player.targetPos.y) < Mathf.RoundToInt(transform.position.y)
                        && movementController.AttemptTarget(targetPos + Vector3.down))
                    {
                        targetPos += Vector3.down;
                        break;
                    }
                    else continue;
                }

                if (loop == 2 && Mathf.RoundToInt(movementController.player.targetPos.x) < Mathf.RoundToInt(transform.position.x))
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.left))
                    {
                        targetPos += Vector3.left;
                        break;
                    }
                    else if (Mathf.RoundToInt(movementController.player.targetPos.y) > Mathf.RoundToInt(transform.position.y)
                        && movementController.AttemptTarget(targetPos + Vector3.up))
                    {
                        targetPos += Vector3.up;
                        break;
                    }
                    else if (Mathf.RoundToInt(movementController.player.targetPos.y) < Mathf.RoundToInt(transform.position.y)
                        && movementController.AttemptTarget(targetPos + Vector3.down))
                    {
                        targetPos += Vector3.down;
                        break;
                    }
                    else continue;
                }
            }
            else
            {
                if (loop == 4 && Mathf.RoundToInt(movementController.player.targetPos.y) > Mathf.RoundToInt(transform.position.y))
                {
                    movementController.takenPositions.Add(originPos);
                    targetPos = new Vector3(targetPos.x, 8, targetPos.z);
                    attackTimer = movementController.cycleTime * 2.1f;
                    attackDir = 1;
                    attack = true;

                    for (int i = 0; i < 8; i++)
                    {
                        movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + (i * attackDir)));
                    }

                    break;

                    if (movementController.AttemptTarget(targetPos + Vector3.down))
                    {
                        targetPos += Vector3.down;
                        break;
                    }
                    else continue;
                }

                if (loop == 3 && Mathf.RoundToInt(movementController.player.targetPos.y) < Mathf.RoundToInt(transform.position.y))
                {
                    movementController.takenPositions.Add(originPos);
                    targetPos = new Vector3(targetPos.x, 1, targetPos.z);
                    attackTimer = movementController.cycleTime * 2.1f;
                    attackDir = -1;
                    attack = true;

                    for (int i = 0; i < 8; i++)
                    {
                        movementController.warningPositions.Add(new int2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + (i * attackDir)));
                    }

                    break;

                    if (movementController.AttemptTarget(targetPos + Vector3.up))
                    {
                        targetPos += Vector3.up;
                        break;
                    }
                    else continue;
                }

                if (loop == 2 && Mathf.RoundToInt(movementController.player.targetPos.x) > Mathf.RoundToInt(transform.position.x))
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.left))
                    {
                        targetPos += Vector3.left;
                        break;
                    }
                    else continue;
                }

                if (loop == 1 && Mathf.RoundToInt(movementController.player.targetPos.x) < Mathf.RoundToInt(transform.position.x))
                {
                    if (movementController.AttemptTarget(targetPos + Vector3.right))
                    {
                        targetPos += Vector3.right;
                        break;
                    }
                    else continue;
                }
            }

            if (loop == 0)
            {
                movementController.takenPositions.Add(originPos);
                break;
            }
        }

        targetPos = ClampToGrid(targetPos, 1, 1, 8, 8);
        targetPos = SnapToGrid(targetPos);
    }
}
