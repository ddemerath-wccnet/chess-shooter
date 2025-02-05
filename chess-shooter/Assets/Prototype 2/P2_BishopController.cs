using UnityEngine;

public class P2_BishopController : MonoBehaviour
{
    public GameObject pivot;
    public GameObject targetObject;
    public Vector3 targetPos;
    public Vector3 originPos;
    public GameObject player;
    public float targetMoveTimer;
    bool attack;

    Vector3 attackDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetMoveTimer -= Time.deltaTime;
        if (targetMoveTimer > 0)
        {
            Vector3 target = transform.position + attackDir;
            target = target - transform.position;
            target.z = Vector2.SignedAngle(Vector2.up, new Vector2(target.x, target.y).normalized);

            pivot.transform.eulerAngles = new Vector3(0, 0, target.z);
            pivot.transform.position = transform.position;

            pivot.GetComponentInChildren<SpriteRenderer>().color = new Color(1,0,0,0.5f);
            if (targetMoveTimer < 14)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                transform.position += attackDir * Time.deltaTime * 3;
            }

            if (transform.position.x > 8 || transform.position.x < 1) attackDir.x *= -1;
            if (transform.position.y > 8 || transform.position.y < 1) attackDir.y *= -1;
        }
        else
        {
            pivot.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0, 0);
            GetComponent<SpriteRenderer>().color = Color.white;

            Vector3 target = player.transform.position;
            target = target - transform.position;
            target.z = Vector2.SignedAngle(Vector2.up, new Vector2(target.x, target.y).normalized);
            pivot.transform.eulerAngles = new Vector3(0, 0, target.z);
            pivot.transform.position = transform.position;

            if (Vector3.Distance(transform.position, player.transform.position) > 3)
            {
                Vector3 movedir = (targetObject.transform.position - transform.position).normalized;
                transform.position += movedir * Time.deltaTime;
            }
            else
            {
                Vector3 movedir = (player.transform.position - transform.position).normalized;
                transform.position -= movedir * Time.deltaTime * 0.5f;
            }
        }

        if (targetMoveTimer < -10)
        {
            targetMoveTimer = 15;
            Vector3 target = player.transform.position;
            target = target - transform.position;
            target.z = Vector2.SignedAngle(Vector2.up, new Vector2(target.x, target.y).normalized);

            pivot.transform.eulerAngles = new Vector3(0, 0, target.z + UnityEngine.Random.Range(-45f, 45f));
            pivot.transform.position = transform.position;
            attackDir = targetObject.transform.position - transform.position;
            attackDir.Normalize();
        }
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 1, 8), Mathf.Clamp(transform.position.y, 1, 8));
    }
}
