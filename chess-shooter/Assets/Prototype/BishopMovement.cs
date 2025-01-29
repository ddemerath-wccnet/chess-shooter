using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BishopMovement : PieceMovement
{
    public Vector3 originPos;
    public Vector3 targetPos;
    float targetMoveTimer;
    int oddBishop = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPos = transform.position;
        targetPos = transform.position;

        if (Mathf.RoundToInt(transform.position.x + transform.position.y) % 2 == 1) oddBishop *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();

        if (targetMoveTimer > 0 && attack == false) { 
            targetMoveTimer -= Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow(1 - (targetMoveTimer / (movementController.cycleTime * 0.75f)), 2f));
        }

        attackCooldown -= Time.deltaTime;
        if (attack)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                //float attackTimerScaled = 1 - ((attackTimer - 0.1f) / (movementController.cycleTime * 2));

                //if (attackTimerScaled > 0.5f)
                //{
                //    transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow((attackTimerScaled * 2) - 1, 2f));
                //}
            }
            else
            {
                transform.position = targetPos;
                originPos = targetPos;
                attack = false;
                attackCooldown = UnityEngine.Random.Range(movementController.cycleTime * 6f, movementController.cycleTime * 8f);
            }
        }
    }

    int moveCooldown = 0;
    bool attack = true;
    public float attackTimer = 0;
    Vector2 moveDirNorm;
    float moveAngle;
    int2 attackDir;
    public float attackCooldown = 1;

    public override void ExecuteMove()
    {
        transform.position = targetPos;
        targetPos = transform.position;
        originPos = targetPos;
        targetMoveTimer = movementController.cycleTime * 0.75f;

        if (attackCooldown < 0 && attack == false)
        {
            attack = true;
            attackTimer = movementController.cycleTime * UnityEngine.Random.Range(10f, 15f);
            attackDir = new int2(oddBishop, -1);
        }
        
        if (attack)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            movementController.takenPositions.Add(originPos);
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(originPos.x), Mathf.RoundToInt(originPos.y)));

            if ((targetPos.x + attackDir.x) > 8.5 || (targetPos.x + attackDir.x) < 0.5) attackDir.x *= -1;
            if ((targetPos.y + attackDir.y) > 8.5 || (targetPos.y + attackDir.y) < 0.5) attackDir.y *= -1;

            targetPos = targetPos + new Vector3(attackDir.x, attackDir.y, 0);
            movementController.takenPositions.Add(targetPos);
            movementController.warningPositions.Add(new int2(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)));

            return;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

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

            if (moveAngle >= 0 && moveAngle < 90)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.up + Vector3.right))
                {
                    targetPos += Vector3.up + Vector3.right;
                    break;
                }
            }
            else if (moveAngle >= 90 && moveAngle < 180)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.down + Vector3.right))
                {
                    targetPos += Vector3.down + Vector3.right;
                    break;
                }
            }
            else if (moveAngle >= 180 && moveAngle < 270)
            {
                if (movementController.AttemptTarget(targetPos + Vector3.down + Vector3.left))
                {
                    targetPos += Vector3.down + Vector3.left;
                    break;
                }
            }
            else if (moveAngle >= 270 && moveAngle < 360)
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
