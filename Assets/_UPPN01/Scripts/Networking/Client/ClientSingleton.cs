using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance;
    private ClientGameManager gameManager;
    public static ClientSingleton Instance { 
        get {
            if (instance!= null)
            {
                return instance;
            }
            instance = FindObjectOfType<ClientSingleton>();
            if (instance == null)
            {
                Debug.LogError("No client singleton in scene");
                return null;
            }
            return instance;
            

        } 
    }

    public ClientGameManager GameManager { get => gameManager;}

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public async Task<bool> CreateClient()
    {
        gameManager = new ClientGameManager();
        return await gameManager.InitAsync();
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
