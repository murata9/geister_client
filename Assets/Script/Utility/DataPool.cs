using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool : SimpleSingleton<DataPool> {
    public void Reset()
    {
        userid = 0;
        sessionid = 0;
    }
    public int userid { get; set; }
    public int sessionid { get; set; }
}
