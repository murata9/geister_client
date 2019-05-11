using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool : SimpleSingleton<DataPool> {
    public void Reset()
    {
        userid = 0;
        sessionid = 0;
        player_entry_id = 0;
        roomid = 0;
        gameid = 0;
        first_mover_user_id = 0;
    }
    public void onExitGame()
    {
        player_entry_id = 0;
        roomid = 0;
        gameid = 0;
        first_mover_user_id = 0;
    }
    public bool isFirstMover()
    {
        return userid == first_mover_user_id;
    }
    public int userid { get; set; }
    public int sessionid { get; set; }
    public int player_entry_id { get; set; }
    public int roomid { get; set; }
    public int gameid { get; set; }
    public int first_mover_user_id { get; set; }
}
