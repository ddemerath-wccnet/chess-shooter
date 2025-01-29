using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float cycleTime = 1;
    bool cycle = true;
    public float TillNextCycle;
    public GameObject visual;
    public PlayerMovement player;

    public List<PieceMovement> pieces = new List<PieceMovement>();
    public List<Vector3> takenPositions = new List<Vector3>();
    public List<int2> warningPositions = new List<int2>();
    public Dictionary<int2, SpriteRenderer> warningSprites = new Dictionary<int2, SpriteRenderer>();
    public Transform warningSpritesParent;

    //public GameObject spritePrefab;
    //public bool makePos = false;

    //private void OnValidate()
    //{
    //    if (makePos)
    //    {
    //        makePos = false;
    //        for (int i = 1; i <= 8; i++)
    //        {
    //            for (int j = 1; j <= 8; j++)
    //            {
    //                GameObject sprite = GameObject.Instantiate(spritePrefab, new Vector3(i, j, 0), spritePrefab.transform.rotation, spritePrefab.transform.parent);
    //                sprite.name = "["+i+", "+j+"]";
    //                warningSprites.Add(new int2(i, j), sprite.GetComponent<SpriteRenderer>());
    //            }
    //        }
    //    }
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        warningSprites.Clear();
        player = FindAnyObjectByType<PlayerMovement>();
        foreach (Transform child in warningSpritesParent)
        {
            warningSprites.Add(new int2(Mathf.RoundToInt(child.localPosition.x), Mathf.RoundToInt(child.localPosition.y)), child.GetComponent<SpriteRenderer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        TillNextCycle = 1 - ((Time.unscaledTime / cycleTime) % 1f);
        visual.transform.localScale = new Vector3(1, TillNextCycle, 1);

        if (TillNextCycle > 0.5f)
        {
            cycle = true;
        }
        else
        {
            if (cycle && Input.GetMouseButton(0))
            {
                Time.timeScale = 1;
                cycle = false;
                takenPositions.Clear();
                warningPositions.Clear();
                foreach (PieceMovement piece in pieces)
                {
                    takenPositions.Add(new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z)));
                }
                foreach (PieceMovement piece in pieces) { 
                    if (piece.isActiveAndEnabled) piece.ExecuteMove();
                }
                ShowWarningPos();
            }
            else if (cycle)
            {
                Time.timeScale = 0;
            }
        }
    }

    public void ShowWarningPos()
    {
        foreach (KeyValuePair<int2, SpriteRenderer> keyValuePair in warningSprites)
        {
            if (warningPositions.Contains(keyValuePair.Key))
            {
                keyValuePair.Value.gameObject.SetActive(true);
            }
            else
            {
                keyValuePair.Value.gameObject.SetActive(false);
            }
        }
    }

    public bool AttemptTarget(Vector3 target)
    {
        foreach (Vector3 takenPosition in takenPositions) {
            if (new int3(Mathf.RoundToInt(takenPosition.x), Mathf.RoundToInt(takenPosition.y), Mathf.RoundToInt(takenPosition.z))
                .Equals(new int3(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.y), Mathf.RoundToInt(target.z)))) {
                return false;
            }
        }
        takenPositions.Add(target);
        return true;
    }
}
