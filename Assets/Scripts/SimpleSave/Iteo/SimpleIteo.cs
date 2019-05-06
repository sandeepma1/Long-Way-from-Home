using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleIteo : MonoBehaviour
{

}
[System.Serializable]
public class Iteo
{
    public short id;
    public short dura;
    public Iteo() { }
    public Iteo(short _id, short _dur)
    {
        id = _id;
        dura = _dur;
    }
}
[System.Serializable]
public class IteoGrp
{
    public Iteo iteo;
    public short count;
    public IteoGrp() { }
    public IteoGrp(Iteo _iteo, short _count)
    {
        iteo = _iteo;
        count = _count;
    }
}