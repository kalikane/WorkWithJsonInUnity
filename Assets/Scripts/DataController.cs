using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Net;

public class DataController : MonoBehaviour
{

    readonly string USERS_URL = "https://jsonplaceholder.typicode.com/users";
    readonly string TODOS_URL = "https://jsonplaceholder.typicode.com/todos";
    readonly string dito = "https://pokeapi.co/api/v2/pokemon/151";

    public List<User> users = new List<User>();

    IEnumerator FecthData()
    {
        //try
        //{
        //    string loadDataPlayers = File.ReadAllText(Application.dataPath + "/StreamingAssets/dataPlayers.json");
        //    string data = fixJson(loadDataPlayers);
        //    Debug.Log(loadDataPlayers);
        //    users = JsonHelper.FromJson<User>(data);
        //    Debug.Log($"Users Length:{users.Count}");
        //    string indent = new String(' ', 2);
        //    foreach (var user in users)
        //    {
        //        Debug.Log($"{indent}ID: {user.id}");
        //        Debug.Log($"{indent}Name: {user.name}");
        //        Debug.Log($"{indent}Tel: {user.phone}");
        //        Debug.Log($"{indent}Email: {user.email}");
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(e);
        //}
        //yield return null;

        
        // USERS
        var webRequest = UnityWebRequest.Get(USERS_URL);
        yield return webRequest.SendWebRequest();

        if (!string.IsNullOrEmpty(webRequest.error))
        {
            Debug.Log("An error occurred");
            yield break;
        }
        if (webRequest.isDone)
        {
            byte[] result = webRequest.downloadHandler.data;
            string dataJSON = System.Text.Encoding.Default.GetString(result);
            if (dataJSON == null)
            {
                Debug.Log("No Data");
            }
            else
            {
                //Debug.Log(webRequest.responseCode + " Reponse du serveur " + dataJSON);
                string jsonString = fixJson(dataJSON);
                //JsonUtility.FromJsonOverwrite(dataJSON, users);
                users = JsonHelper.FromJson<User>(jsonString);


                try
                {
                    File.WriteAllText(Application.dataPath + "/StreamingAssets/dataPlayers.json", jsonString);
                    string indent = new String(' ', 2);
                    foreach (var user in users)
                    {
                        Debug.Log($"{indent}ID: {user.id}");
                        Debug.Log($"{indent}Name: {user.name}");
                        Debug.Log($"{indent}Tel: {user.phone}");
                        Debug.Log($"{indent}Email: {user.email}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
    }

    IEnumerator FecthPokemonData()
    {


        UnityWebRequest requestDito = UnityWebRequest.Get(dito);
        yield return requestDito.SendWebRequest();

        while (!requestDito.isDone) ;
        if (String.IsNullOrEmpty(requestDito.error))
        {
            byte[] data = requestDito.downloadHandler.data;
            string dataString = Encoding.UTF8.GetString(data);
            Debug.Log(dataString);

        }
        else
        {
            Debug.Log($"Error: {requestDito.error}");
        }
       
    }

    public void GetData()
    {
        using (var webClient = new WebClient())
        {
            webClient.DownloadStringCompleted += (sender, e) =>
            {
                Debug.Log(e.Result);
            };
            webClient.DownloadStringAsync(new Uri(dito));

        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FecthData());
        //StartCoroutine(FecthPokemonData());
        GetData();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
