using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System;

public class NetworkAPI : MonoBehaviour
{
    private const string BASE_URL = "https://dummy.restapiexample.com/api/v1";
    public void CreateUser(string name, int run, int hp)
    {
        StartCoroutine(CreateUserCoroutine(name, run, hp));
    }
    public void DeleteUser()
    {
        StartCoroutine(DeleteUserCoroutine());
    }

    private IEnumerator CreateUserCoroutine(string name, int run, int hp)
    {
        string createUrl = $"{BASE_URL}/create";

        string json = "{\"name\":\"" + name + "\", \"Run\":\"" + run + "\", \"Hp\":\"" + hp + "\"}";

        UnityWebRequest createRequest = new UnityWebRequest(createUrl, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        createRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);
        createRequest.SetRequestHeader("Content-Type", "application/json");

        yield return createRequest.SendWebRequest();

        if (createRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error creating user: " + createRequest.error);
        }
        else
        {
            Debug.Log("User created successfully.");
        }
    }
    private IEnumerator DeleteUserCoroutine()
    {
        //Attention========================================================================
        // In the Dummy Rest API example, I couldn't find support for deleting data by name,
        // so I temporarily used 'https://dummy.restapiexample.com/api/v1/delete/719'
        // where there is support in the Dummy Rest API example

        string deleteUrl = $"{BASE_URL}/delete/719";

        using (UnityWebRequest deleteRequest = UnityWebRequest.Delete(deleteUrl))
        {
            yield return deleteRequest.SendWebRequest();

            if (deleteRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error deleting user: " + deleteRequest.error);
            }
            else
            {
                Debug.Log("User deleted successfully.");
            }
        }
    }
}
