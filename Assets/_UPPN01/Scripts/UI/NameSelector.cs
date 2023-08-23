using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameField;
    [SerializeField]
    Button connectButton;
    [SerializeField]
    private int minNameLenght = 1;
    [SerializeField]
    private int maxNameLenght = 12;
    public const string PlayerNameKey = "PlayerName";
    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene("NetBoostrap");
            return;
        }
        nameField.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
        HandleNameChanged();
    }
    public void HandleNameChanged(){
        connectButton.interactable = nameField.text.Length >= minNameLenght && nameField.text.Length <= maxNameLenght;
    }
    public void Connect()
    {
        PlayerPrefs.SetString(PlayerNameKey, nameField.text);
        SceneManager.LoadScene("NetBoostrap");
    }
}
