using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlaz : MonoBehaviour
{
    private void Start()
    {

    }
}

[System.Serializable]
public class Plaz
{
    public byte maqId;
    public byte locX;
    public byte locY;
    public byte currentToom;
    public IteoGrp[] invm;
    public byte stats1;
    public byte stats2;
    public byte stats3;//...
}