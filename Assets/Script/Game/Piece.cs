using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Protocol;

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

    public int m_id;
    public bool m_isCaputured = false;

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

    void SetKind(e_kind kind)
    {
        m_kind = kind;
        SetSprite();
    }

    public void SetPos(PiecePos pos)
    {
        m_pos = pos;
        gameObject.transform.localPosition = PiecePositionConverer.convertToObjPosition(m_pos);
    }

    public void SetCollisionEnable(bool flag)
    {
        GetComponent<Image>().raycastTarget = flag;
    }

    public void SetIsOwner(bool isOwner)
    {
        if (m_isOwner == isOwner) return;
        m_isOwner = isOwner;
        // 反転する
        Quaternion q = gameObject.transform.localRotation;
        bool reverse = isOwner;
        if (DataPool.Instance.isFirstMover())
        {
            reverse = !reverse;
        }
        if (reverse)
        {
            q.z = 0;
        }
        else
        {
            q.z = 180;
        }
        gameObject.transform.localRotation = q;
    }

    public void SetPieceInfo(PieceInfo info)
    {
        if (m_id == 0)
        {
            m_id = info.piece_id;
        }
        else if (m_id != info.piece_id)
        {
            Debug.LogError("異なるIDのPieceInfoで情報を設定しようとした");
            return;
        }
        SetIsOwner(info.owner_user_id == DataPool.Instance.userid);
        m_isCaputured = info.captured; // TODO:取られた場合の処理
        SetPos(new PiecePos(info.point_x, info.point_y));
        var kind = (e_kind)System.Enum.Parse(typeof(e_kind), info.kind);
        SetKind(kind);
    }

    public void onClickPiece()
    {
        Debug.Log("onClickPiece");
        if (!m_isOwner) return;
        if (m_isCaputured) return;
        if (board.gameProcessor.m_state == GameProcessor.e_State.preparing)
        {
            if (board.m_selectedPiece == null)
            {
                board.SetSelectedPiece(this);
                return;
            }
            else
            {
                PiecePos tmpPos = m_pos;
                this.SetPos(board.m_selectedPiece.m_pos);
                board.m_selectedPiece.SetPos(tmpPos);

                board.ReleaseSelectedPiece();
            }
        }
        if (board.gameProcessor.m_state == GameProcessor.e_State.playing)
        {
            board.CreateMovablePosition(this);
        }
    }
}
