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
    }
    public void onExitGame()
    {
        player_entry_id = 0;
        roomid = 0;
        gameid = 0;
    }
    public int userid { get; set; }
    public int sessionid { get; set; }
    public int player_entry_id { get; set; }
    public int roomid { get; set; }
    public int gameid { get; set; }
}
