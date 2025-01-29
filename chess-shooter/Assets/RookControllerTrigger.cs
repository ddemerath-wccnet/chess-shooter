using UnityEngine;

public class RookControllerTrigger : MonoBehaviour
{
    public RookController rookController;
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
        PlayerController PlayerController;
        //Debug.Log(collision.gameObject.name);
        if (collision.TryGetComponent<PlayerController>(out PlayerController))
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
