using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Protocol;

public class Board : MonoBehaviour {
    public Piece m_selectedPiece;

    public List<Piece> Pieces { get; private set; }

    public bool m_firstRecvPieceList = true;

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

    public void recvPieceLists(ResponseListPieces param)
    {
        if (m_firstRecvPieceList)
        {
            m_firstRecvPieceList = false;
            if (Pieces.Count != param.pieces.Count)
            {
                Debug.LogError("Pieceの数が一致しない!");
                return;
            }
            int i = 0;
            foreach(var p in param.pieces)
            {
                Pieces[i].SetPieceInfo(p);
                ++i;
            }
            if (DataPool.Instance.isFirstMover())
            {
                // 先手の場合は盤面を逆にして、手前に自分の駒が来るようにする
                Quaternion q = gameObject.transform.localRotation;
                q.z = 180;
                gameObject.transform.localRotation = q;
            }
        }
    }
}
