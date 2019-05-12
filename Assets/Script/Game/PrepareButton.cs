using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using HTTP;
using Protocol;
public class PrepareButton : MonoBehaviour {
    [SerializeField]
    Board board;

    [SerializeField]
    GameProcessor gameProcessor;

    void Start()
    {
        if (board == null) Debug.LogError("board is null");
        if (gameProcessor == null) Debug.LogError("gameProcessor is null");
    }

    PiecePos reversePos(PiecePos original)
    {
        int x = 7 - original.x;
        int y = 7 - original.y;
        return new PiecePos(x, y);
    }

    PiecePreparationInfo toPieceInfo(Piece piece)
    {
        var pos = piece.m_pos;
        if (DataPool.Instance.isFirstMover())
        {
            // 先手なら盤面をひっくり返して送る必要がある
            pos = reversePos(pos);
        }
        return new PiecePreparationInfo { point_x = pos.x, point_y = pos.y, kind = piece.m_kind.ToString() };
    }

    public void onClickPrepareButton()
    {
        ApiClient.Instance.ResponsePrepareGame = (p) => {
            // TODO:本来はここでは遅い。リクエストした時点でボタンを無効にすべきだが、不正リクエストがテストできるようにしておく
            gameObject.SetActive(false);
            gameProcessor.onEndPreparing();
        };

        var myPieces = board.Pieces.Where(x => x.m_isOwner).Select(toPieceInfo).ToList();
        var param = new RequestPrepareGame();
        param.game_id = DataPool.Instance.gameid;
        param.piece_preparations = myPieces;
        ApiClient.Instance.RequestPrepareGame(param);
    }
}
