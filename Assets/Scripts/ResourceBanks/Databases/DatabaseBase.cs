using FullSerializer;
using UnityEngine;

public abstract class DatabaseBase<T> : Singleton<T> where T : DatabaseBase<T>
{
    protected TextAsset textAsset;
    protected fsSerializer mSerialiser = null;
    protected fsData parsedData;
    protected object deserializedData = null;
    [SerializeField] protected string jsonFileName = "";

    protected override void Awake()
    {
        ReadJsonFiles();
    }

    private void ReadJsonFiles()
    {
        if (jsonFileName == "")
        {
            GEM.PrintDebugError("jsonFileName not mentioned");
            return;
        }
        else
        {
            textAsset = (TextAsset)Resources.Load("Jsons/ItemDatabase - " + jsonFileName);
        }
        mSerialiser = new fsSerializer();
        mSerialiser.AddConverter(new UnityObjectConverter());
        parsedData = fsJsonParser.Parse(textAsset.text);
        LoadFromJson();
    }

    protected virtual void LoadFromJson()
    {

    }
}