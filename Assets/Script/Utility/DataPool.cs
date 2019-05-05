using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool : SimpleSingleton<DataPool> {
    public int userid { get; set; }
    public int sessionid { get; set; }
}
