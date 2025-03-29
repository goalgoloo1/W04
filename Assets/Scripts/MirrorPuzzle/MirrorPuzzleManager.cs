using System;
using UnityEngine;
using System.Collections.Generic;

public class MirrorPuzzleManager : MonoBehaviour
{
    public GameObject pieces;
    public GameObject goalColls;
    
    private void Start()
    {
        Init();
        
    }


    private void Init()
    {
        int i = 0;
        Collider2D[] colls = goalColls.GetComponentsInChildren<Collider2D>();
        foreach (var piece in pieces.GetComponentsInChildren<MirrorPiece>())
        {
            piece.goalColl = colls[i].gameObject;
            i++;
        }
    }
}
