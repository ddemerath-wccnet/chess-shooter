using UnityEngine;

public class RookController : MonoBehaviour
{
    public GameObject pivot;
    public GameObject targetObject;
    public Vector3 targetPos;
    public Vector3 originPos;
    public GameObject player;
    public float targetMoveTimer;
    bool attack;

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
            pivot.GetComponentInChildren<SpriteRenderer>().color = new Color(1,0,0,0.5f);
            if (targetMoveTimer < 1)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                transform.position = Vector3.Lerp(originPos, targetPos, Mathf.Pow(1 - (targetMoveTimer), 2f));
            }
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
        }
    }
}
