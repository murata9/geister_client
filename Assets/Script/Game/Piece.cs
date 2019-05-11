using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {
    [SerializeField]
    Sprite goodSprite;
    [SerializeField]
    Sprite evilSprite;
    [SerializeField]
    Sprite unknownSprite;
    [SerializeField]
    Board board;

    public enum e_kind
    {
        good,
        evil,
        unknown,
    }

    public bool m_isOwner;
    public e_kind m_kind;
    public PiecePos m_pos;

    void Start()
    {
        SetSprite();
        if (goodSprite == null || evilSprite == null || unknownSprite == null) Debug.LogError("Sprite is null");
        if (board == null) Debug.LogError("Board is null");

        m_pos = PiecePositionConverer.convertToPiecePosition(gameObject.transform.localPosition);
        gameObject.transform.localPosition = PiecePositionConverer.convertToObjPosition(m_pos);
    }

    void SetSprite()
    {
        var image = GetComponent<Image>();
        switch (m_kind)
        {
            case e_kind.good:
                image.sprite = goodSprite;
                break;
            case e_kind.evil:
                image.sprite = evilSprite;
                break;
            case e_kind.unknown:
                image.sprite = unknownSprite;
                break;
            default:
                Debug.LogError("unknown kind");
                break;
        }
    }

    void SetPos(PiecePos pos)
    {
        m_pos = pos;
        gameObject.transform.localPosition = PiecePositionConverer.convertToObjPosition(m_pos);
    }

    public void SetCollisionEnable(bool flag)
    {
        GetComponent<Image>().raycastTarget = flag;
    }

    public void onClickPiece()
    {
        Debug.Log("onClickPiece");
        if (!m_isOwner) return;
        if (board.m_selectedPiece == null)
        {
            board.SetSelectedPiece(this);
            return;
        }
        else
        {
            PiecePos tmpPos = m_pos;
            this.SetPos( board.m_selectedPiece.m_pos );
            board.m_selectedPiece.SetPos(tmpPos);

            board.ReleaseSelectedPiece();
        }
    }
}
