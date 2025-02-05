using UnityEngine;
using UnityEngine.SceneManagement;

public class P1_PlayerCollision : MonoBehaviour
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
        P1_PieceMovement piece;
        if (collision.gameObject.TryGetComponent<P1_PieceMovement>(out piece))
        {
            if (piece.GetComponent<SpriteRenderer>().color == Color.red)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                P1_QueenMovement pieceQ;
                if (collision.gameObject.TryGetComponent<P1_QueenMovement>(out pieceQ))
                {
                    FindAnyObjectByType<P1_MovementController>().pieces.Remove(pieceQ);
                    FindAnyObjectByType<P1_MovementController>().pieces.Remove(pieceQ.rookMovement);
                    FindAnyObjectByType<P1_MovementController>().pieces.Remove(pieceQ.bishopMovement);
                    FindAnyObjectByType<P1_MovementController>().pieces.Remove(pieceQ.kingMovement);
                }
                else FindAnyObjectByType<P1_MovementController>().pieces.Remove(piece);
                GameObject.Destroy(piece.gameObject);
            }
        }
    }
}
