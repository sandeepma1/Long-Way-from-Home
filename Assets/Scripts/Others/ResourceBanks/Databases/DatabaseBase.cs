using FullSerializer;
using UnityEngine;

public class DatabaseBase : MonoBehaviour
{
    protected TextAsset textAsset;
    protected fsSerializer mSerialiser = null;
    protected fsData parsedData;
    protected object deserializedData = null;
    [SerializeField] protected string jsonFileName = "";

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (jsonFileName == "")
        {
            GEM.PrintDebugError("jsonFileName not mentioned");
            return;
            //Debug.LogWarning("Json file name not given, trying to get it automagically!!!");
            //string name = GetComponent<MonoBehaviour>().GetType().Name;
            //name = name.Replace("Database", "");
            //name = "Jsons/ItemDatabase - " + name;
            //textAsset = (TextAsset)Resources.Load(name);
            //if (textAsset == null)
            //{
            //    Debug.LogError("Database not loaded for " + name);
            //    return;
            //}
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
