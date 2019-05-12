using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;

public class Board : MonoBehaviour {
    [SerializeField]
    GameObject movablePositionPrefab;

    public GameProcessor gameProcessor;

    public Piece m_selectedPiece;

    public List<Piece> Pieces { get; private set; }

    public bool m_firstRecvPieceList = true;

    void Start()
    {
        Pieces = new List<Piece>( GetComponentsInChildren<Piece>() );
        if (gameProcessor == null) Debug.LogError("gameProcessor is null");
        ApiClient.Instance.ResponseUpdatePiece = (p) => { gameProcessor.onPieceMoved(); };
    }

    void Update()
    {
        if (gameProcessor.m_state == GameProcessor.e_State.preparing)
        {
            if (m_selectedPiece)
            {
                m_selectedPiece.gameObject.transform.position = Input.mousePosition;
            }
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
            return;
        }
        // 二回目以降は更新
        foreach (var p in param.pieces)
        {
            var piece = Pieces.SingleOrDefault((x) => { return x.m_id == p.piece_id; });
            if (piece == null)
            {
                Debug.LogError("PieceID:" + p.piece_id + "が見つかりません");
                return;
            }
            piece.SetPieceInfo(p);
        }
    }

    void RequestMovePiece(Piece piece, PiecePos pos)
    {
        var param = new RequestUpdatePiece();
        param.piece_id = piece.m_id;
        param.point_x = pos.x;
        param.point_y = pos.y;
        ApiClient.Instance.RequestUpdatePiece(param);
    }

    public void CreateMovablePositionInternal(Piece piece, PiecePos pos)
    {
        // 移動可能かチェック
        int goalY = DataPool.Instance.isFirstMover() ? 6 : 0;
        bool isGoal = (pos.y == goalY && piece.m_kind == Piece.e_kind.good && (pos.x == 0 || pos.x == 7));
        if (!isGoal)
        {
            if (pos.x < 1 || pos.x > 6) return;
            if (pos.y < 1 || pos.y > 6) return;
        }
        if (Pieces.Any((p) => { return (p.m_isOwner) && (p.m_pos.x == pos.x) && (p.m_pos.y == pos.y); }))
        {
            // 自駒がある
            return;
        }

        var obj = GameObject.Instantiate(movablePositionPrefab); // as RectTransform;
        var r = obj.transform;
        r.SetParent(this.gameObject.transform);
        r.localScale = Vector3.one;
        r.transform.localPosition = PiecePositionConverer.convertToObjPosition(pos);
        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            // note:リクエストと同時に駒を動かしたことにするなら、ここでSetPosを実行する
            // piece.SetPos(pos);
            DeleteAllMovablePosition();
            RequestMovePiece(piece, pos);
        });
    }

    public void CreateMovablePosition(Piece piece)
    {
        DeleteAllMovablePosition();
        // 4方向に対して移動可能かチェック
        int x = piece.m_pos.x;
        int y = piece.m_pos.y;
        CreateMovablePositionInternal(piece, new PiecePos(x, y - 1));
        CreateMovablePositionInternal(piece, new PiecePos(x, y + 1));
        CreateMovablePositionInternal(piece, new PiecePos(x - 1, y));
        CreateMovablePositionInternal(piece, new PiecePos(x + 1, y));
        m_selectedPiece = piece;
    }

    public void DeleteAllMovablePosition()
    {
        var objs = GameObject.FindGameObjectsWithTag("MovablePosition");
        foreach (var obj in objs)
        {
            Destroy(obj.gameObject);
        }
    }
}
