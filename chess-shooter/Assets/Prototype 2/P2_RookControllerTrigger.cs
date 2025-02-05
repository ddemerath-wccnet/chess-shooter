using UnityEngine;

public class P2_RookControllerTrigger : MonoBehaviour
{
    public P2_RookController rookController;
    public float anticipation = 1;
    public float cooldown = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        P2_PlayerController PlayerController;
        //Debug.Log(collision.gameObject.name);
        if (collision.TryGetComponent<P2_PlayerController>(out PlayerController))
        {
            if (rookController.targetMoveTimer < -cooldown)
            {
                rookController.targetMoveTimer = 1+ anticipation;
                rookController.targetPos = rookController.targetObject.transform.position;
                rookController.originPos = rookController.transform.position;
            }
        }
    }
}
