using System.Collections.Generic;
using UnityEngine;

public class SimpleStructt : MonoBehaviour
{
    private void Start()
    {

    }
}

public struct AllStructt
{
    public string name;
    public List<Structt> structts;
}

public struct Structt
{
    public StructtType structtType;
    public InuIteo[] inputItemIds;
    public InuIteo[] outputItemIds;
    public short mediumItemId;
}

public enum StructtType
{
    Storf,
    Procest,
    Crafu,
    Decos
}