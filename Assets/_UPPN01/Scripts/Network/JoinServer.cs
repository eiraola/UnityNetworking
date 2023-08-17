using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class JoinServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Join()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void JoinHost()
    {
        NetworkManager.Singleton.StartHost();
    }

}
