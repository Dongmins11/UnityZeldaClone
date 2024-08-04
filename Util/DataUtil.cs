using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data;
using System.IO;

public static class DataUtil
{
    public static T GetJson<T>(string _TableName, bool _IsFirst) where T : class
    {
        string path = Application.persistentDataPath  + "/" + _TableName + ".json";

        string value = string.Empty;

        FileInfo info = new FileInfo(path);

        if (false == info.Exists)
        {
            return null;
        }

        value = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<T>(value);

        #region 유니티 상 Resources매니저 사용할 때
        //if (false == _IsFirst)
        //    path = string.Concat("JsonData/SaveJson/", _TableName);
        //else
        //    path = string.Concat("JsonData/", _TableName);

        //string value = string.Empty;

        //value = (GameManager.ResourceInstance.Load(path) as TextAsset).ToString();

        //if (string.Empty == value)
        //{
        //    //Debug.Log($"Failed to GetData{_TableName}");
        //    return null;
        //}

        //return JsonConvert.DeserializeObject<T>(value);
        #endregion
    }
    public static DataTable GetDataTable(string fileName, string tableName)
    {
        var obj = Resources.Load(fileName);
        string value = (obj as TextAsset).ToString();
        DataTable data = JsonConvert.DeserializeObject<DataTable>(value);
        data.TableName = tableName;

        return data;
    }
    public static DataTable GetDataTable(FileInfo info)
    {
        string fileName = Path.GetFileNameWithoutExtension(info.Name);
        string path = string.Concat("JsonData/", fileName);
        string value = string.Empty;

        value = (Resources.Load(path) as TextAsset).ToString();

        if (string.Empty == value)
            return null;

        DataTable data = JsonConvert.DeserializeObject<DataTable>(value);
        data.TableName = fileName;

        return data;
    }
    public static string GetDataValue(DataSet dataSet, string tableName, string primary, string value, string column)
    {
        DataRow[] rows = dataSet.Tables[tableName].Select(string.Concat(primary, " = '", value, "'"));

        return rows[0][column].ToString();
    }
    public static void SetObjectFile<T>(string key, T data, bool _otherSaveCheck = false)
    {
        string value = JsonConvert.SerializeObject(data);

        string path = string.Empty;

        if (false == _otherSaveCheck)
           path = string.Concat("/" + key + ".json");
        else
           path = string.Concat("/Resources/JsonData/SaveJson/" + key + ".json");

        File.WriteAllText(Application.persistentDataPath + path, value);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.ImportAsset(string.Concat("Assets" + path));
#endif
    }



}
