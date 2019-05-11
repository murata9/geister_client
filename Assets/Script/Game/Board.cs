using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public Piece m_selectedPiece;

    public List<Piece> Pieces { get; private set; }

    void Start()
    {
        Pieces = new List<Piece>( GetComponentsInChildren<Piece>() );
    }

    void Update()
    {
        if (m_selectedPiece)
        {
            m_selectedPiece.gameObject.transform.position = Input.mousePosition;
        }
    }

    public void SetSelectedPiece(Piece piece)
    {
        ReleaseSelectedPiece();
        m_selectedPiece = piece;
        m_selectedPiece.SetCollisionEnable(false);
    }

    public void ReleaseSelectedPiece()
    {
        if (m_selectedPiece == null) return;
        m_selectedPiece.SetCollisionEnable(true);
        m_selectedPiece = null;
    }
}
