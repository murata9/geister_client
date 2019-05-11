using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PiecePositionConverer  {
    const int PieceSize = 96;
    // PiecePos = サーバーとやり取りするときの0～7のマス目の位置
    // ObjPos = unityのゲームオブジェクトの座標
    static public PiecePos convertToPiecePosition(Vector2 objPos)
    {
        float ajust = PieceSize / 2;
        float x = (objPos.x + 339 + ajust  ) / PieceSize;
        float y = -(objPos.y - 339 - ajust) / PieceSize;
        return new PiecePos((int)x, (int)y);
    }

    static public Vector2 convertToObjPosition(PiecePos piecePos)
    {
        float x = -339 + (PieceSize * piecePos.x);
        float y = 339 - (PieceSize * piecePos.y);
        return new Vector2(x, y);
    }
}
