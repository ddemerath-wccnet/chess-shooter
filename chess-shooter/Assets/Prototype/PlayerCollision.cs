using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        PieceMovement piece;
        if (collision.gameObject.TryGetComponent<PieceMovement>(out piece))
        {
            if (piece.GetComponent<SpriteRenderer>().color == Color.red)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                FindAnyObjectByType<MovementController>().pieces.Remove(piece);
                GameObject.Destroy(piece.gameObject);
            }
        }
    }
}
