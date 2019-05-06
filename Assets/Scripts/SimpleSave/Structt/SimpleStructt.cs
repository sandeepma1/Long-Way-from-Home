using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree.Types;
using UnityEngine;
public class SimpleStructt : MonoBehaviour
{
    private void Start()
    {

    }
}

public class AllStructt
{
    public string name;
    public List<Structt> structts;
}

public class Structt
{
    public StructtType structtType;
    public IteoGrp[] inputItemIds;
    public IteoGrp[] outputItemIds;
    public short mediumItemId;
    public Structt() { }
    public Structt(StructtType _structtType, IteoGrp[] _inputItemIds, IteoGrp[] _outputItemIds, short _mediumItemId)
    {
        structtType = _structtType;
        inputItemIds = _inputItemIds;
        outputItemIds = _outputItemIds;
        mediumItemId = _mediumItemId;
    }
}

public enum StructtType
{
    Storf,
    Procest,
    Crafu
}