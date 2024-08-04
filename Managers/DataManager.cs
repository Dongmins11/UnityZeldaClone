using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

using System.Collections.Generic;
using UnityEngine;


public class DataManager
{
    private DataSet _database;

    public void Init()
    {
        _database = new DataSet("Database");

        
#if UNITY_EDITOR 
        MakeSheetDatset(_database);
#endif
        
    }

    //OtherSaveCheck란 저장위치를 SaveJson으로 할 것인지, 아니면 기본적 JsonData로할것인지를 정하는것.
    //SaveDataJson파일은 현재 씬의 활동중인 값을 저장하는것
    //JsonData는 맨처음 구글 스프레드 시트에서 가져온것을 설정하는 것.
    public void SaveData<T>(T _DataClass, string _strSavingFileName, bool _otherSaveCheck) where T : class
    {
        string strjsonvalue = JsonConvert.SerializeObject(_DataClass);

        DataTable DataTableObject = SpreadSheetToDataTable(strjsonvalue);

        DataTableObject.TableName = _strSavingFileName;

        SaveDataToFile(DataTableObject, _otherSaveCheck);
    }

    public T LoadData<T>(string _TableName, bool _IsFirst = true) where T : class
    {
        return DataUtil.GetJson<T>(_TableName, _IsFirst);
    }


    #region 구글 스프레드시트 Json화 기능
    private void MakeSheetDatset(DataSet dataset)
    {
        ClientSecrets pass = new ClientSecrets();
        pass.ClientId = "465291236832-4ed08dpv2864287s7frvc92jm4r77fep.apps.googleusercontent.com";
        pass.ClientSecret = "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";

        string[] scopes = new string[] { SheetsService.Scope.SpreadsheetsReadonly };
        UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(pass, scopes, "BotwData", CancellationToken.None).Result;

        SheetsService service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "EzenZeldaBotw"
        });

        Spreadsheet request = service.Spreadsheets.Get("보안토큰").Execute();

        foreach (Sheet sheet in request.Sheets)
        {
            DataTable table = SendRequest(service, sheet.Properties.Title);
            dataset.Tables.Add(table);
        }

    }
    private DataTable SendRequest(SheetsService service, string sheetName)
    {
        DataTable result = null;
        bool success = true;

        try
        {
            //!A1:M은 스프레드시트 A열부터 M열까지 데이터를 받아오겠다는 소리
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get("보안토큰", sheetName + "!A1:M");
            //API 호출로 받아온 IList 데이터
            var jsonObject = request.Execute().Values;
            //IList 데이터를 jsonConvert 하기위해 직렬화
            string jsonString = ParseSheetData(jsonObject);

            //DataTable로 변환
            result = SpreadSheetToDataTable(jsonString);
        }
        catch (Exception e)
        {
            success = false;
            Debug.LogError(e);
            string path = string.Format("JsonData/{0}", sheetName);
            //예외 발생시 로컬 경로에 있는 json 파일을 통해 데이터 가져옴
            result = DataUtil.GetDataTable(path, sheetName);
            //Debug.Log("시트 로드 실패로 로컬 " + sheetName + " json 데이터 불러옴");
        }

        //Debug.Log(sheetName + " 스프레드시트 로드 " + (success ? "성공" : "실패"));

        result.TableName = sheetName;

        if (result != null)
        {
            //변환한 테이블을 json 파일로 저장
            SaveDataToFile(result);
        }

        return result;
    }
    private DataTable SpreadSheetToDataTable(string json)
    {
        DataTable data = JsonConvert.DeserializeObject<DataTable>(json);
        return data;
    }
    private string ParseSheetData(IList<IList<object>> value)
    {
        StringBuilder jsonBuilder = new StringBuilder();

        IList<object> columns = value[0];

        jsonBuilder.Append("[");
        for (int row = 1; row < value.Count; row++)
        {
            var data = value[row];
            jsonBuilder.Append("{");
            for (int col = 0; col < data.Count; col++)
            {
                jsonBuilder.Append("\"" + columns[col] + "\"" + ":");
                jsonBuilder.Append("\"" + data[col] + "\"");
                jsonBuilder.Append(",");
            }
            jsonBuilder.Append("}");
            if (row != value.Count - 1)
                jsonBuilder.Append(",");
        }
        jsonBuilder.Append("]");
        return jsonBuilder.ToString();
    }
    private void SaveDataToFile(DataTable newTable, bool _otherSaveCheck = false)
    {
        string JsonPath = string.Empty;
        JsonPath = string.Concat(Application.persistentDataPath + "/" + newTable.TableName + ".json");

        FileInfo info = new FileInfo(JsonPath);
        //동일 파일 유무 체크
        if (info.Exists)
        {
            DataTable checkTable = DataUtil.GetDataTable(info);
            //파일 내용 체크
            if (BinaryCheck<DataTable>(newTable, checkTable))
            {
                return;
            }
        }
        //json파일 저장
        DataUtil.SetObjectFile(newTable.TableName, newTable, _otherSaveCheck);
    }
    private bool BinaryCheck<T>(T src, T target)
    {
        //두 대상을 바이너리로 변환해서 비교, 다르면 false 반환
        BinaryFormatter formatter1 = new BinaryFormatter();
        MemoryStream stream1 = new MemoryStream();
        formatter1.Serialize(stream1, src);

        BinaryFormatter formatter2 = new BinaryFormatter();
        MemoryStream stream2 = new MemoryStream();
        formatter2.Serialize(stream2, target);

        byte[] srcByte = stream1.ToArray();
        byte[] tarByte = stream2.ToArray();

        if (srcByte.Length != tarByte.Length)
        {
            return false;
        }
        for (int i = 0; i < srcByte.Length; i++)
        {
            if (srcByte[i] != tarByte[i])
                return false;
        }
        return true;
    }
    public string SelectTableData(string tableName, string primary, string column)
    {
        DataRow[] rows = _database.Tables[tableName].Select(string.Concat(primary, " = '", column, "'"));

        if (string.Empty == rows[0][column].ToString() || 0 == rows.Length)
            return string.Empty;

        return rows[0][column].ToString();
    }

    #endregion
}
