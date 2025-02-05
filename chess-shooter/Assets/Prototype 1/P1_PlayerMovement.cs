using UnityEngine;

public class P1_PlayerMovement : P1_PieceMovement
{
    Vector2 buffer;
    bool spaceBuffer;
    float spaceTimer;
    public Vector3 originPos;
    public Vector3 targetPos;
    float targetMoveTimer;
    public GameObject rook;
    public GameObject rookVisual;

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

        if (Input.GetKeyDown(KeyCode.Space)) spaceBuffer = true;
        if (Input.GetKeyDown(KeyCode.W)) buffer += Vector2.up;
        if (Input.GetKeyDown(KeyCode.A)) buffer += Vector2.left;
        if (Input.GetKeyDown(KeyCode.S)) buffer += Vector2.down;
        if (Input.GetKeyDown(KeyCode.D)) buffer += Vector2.right;

        buffer = new Vector2(Mathf.Clamp(buffer.x, -1, 1), Mathf.Clamp(buffer.y, -1, 1));

        if (targetMoveTimer > 0) { 
            targetMoveTimer -= Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow(1 - (targetMoveTimer / (movementController.cycleTime * 0.75f)), 2f));
        }

        spaceTimer -= Time.deltaTime;
        if (spaceTimer > 0) {
            rook.SetActive(true);
            rook.transform.Rotate(new Vector3(0,0,0.2f));
            spaceTimerScaled = 1 - (spaceTimer / movementController.cycleTime);
            if (spaceTimerScaled < 0.5f) {
                rook.transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow((spaceTimerScaled) * 2, 0.25f));
            }
            if (spaceTimerScaled > 0.75f)
            {
                rook.SetActive(false);
                transform.position = targetPos;
            }
        }
        else
        {
            rook.transform.localPosition = new Vector3();
            rook.SetActive(false);
        }

        rookVisual.transform.localScale = new Vector3(1, Mathf.Clamp(spaceTimer/(movementController.cycleTime * -5), 0, 1), 1);
        if (Mathf.Clamp(spaceTimer / (movementController.cycleTime * -5), 0, 1) >= 1)
        {
            rookVisual.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            rookVisual.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public float spaceTimerScaled;
    public override void ExecuteMove()
    {
        if ((Input.GetKey(KeyCode.Space) || spaceBuffer) && spaceTimer <= movementController.cycleTime * -5)
        {
            if (Input.GetKey(KeyCode.W) || buffer.y >= 1) targetPos += Vector3.up * 4;
            if (Input.GetKey(KeyCode.A) || buffer.x <= -1) targetPos += Vector3.left * 4;
            if (Input.GetKey(KeyCode.S) || buffer.y <= -1) targetPos += Vector3.down * 4;
            if (Input.GetKey(KeyCode.D) || buffer.x >= 1) targetPos += Vector3.right * 4;

            originPos = transform.position;

            spaceTimer = movementController.cycleTime * 0.95f;
            buffer = Vector2.zero;
            spaceBuffer = false;
            return;
        }

        if (spaceTimer > 0) return;

        transform.position = targetPos;
        targetPos = transform.position;
        originPos = targetPos;
        targetMoveTimer = movementController.cycleTime * 0.75f;
        if (Input.GetKey(KeyCode.W) || buffer.y >= 1) targetPos += Vector3.up;
        if (Input.GetKey(KeyCode.A) || buffer.x <= -1) targetPos += Vector3.left;
        if (Input.GetKey(KeyCode.S) || buffer.y <= -1) targetPos += Vector3.down;
        if (Input.GetKey(KeyCode.D) || buffer.x >= 1) targetPos += Vector3.right;
        buffer = Vector2.zero;
        targetPos = ClampToGrid(targetPos, 1, 1, 8, 8);
        targetPos = SnapToGrid(targetPos);
        spaceBuffer = false;
    }
}
