using FullSerializer;
using UnityEngine;

public class DatabaseBase : MonoBehaviour
{
    protected TextAsset textAsset;
    protected fsSerializer mSerialiser = null;
    protected fsData parsedData;
    protected object deserializedData = null;
    [SerializeField] protected string jsonFilePath = "Jsons/ItemDatabase - MapItems";

    private void Awake()
    {
        textAsset = (TextAsset)Resources.Load(jsonFilePath);
        mSerialiser = new fsSerializer();
        mSerialiser.AddConverter(new UnityObjectConverter());
        parsedData = fsJsonParser.Parse(textAsset.text);
        LoadFromJson();
    }

    protected virtual void LoadFromJson()
    {

    }
}
