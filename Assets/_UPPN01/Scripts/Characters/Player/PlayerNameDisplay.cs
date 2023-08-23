using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;
using System;

public class PlayerNameDisplay : MonoBehaviour
{

    [SerializeField]
    private TMP_Text displayNameText;
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        DisplayText(string.Empty, _player.PlayerName.Value);
        _player.PlayerName.OnValueChanged += DisplayText;
    }
    public void ShutDown()
    {
        _player.PlayerName.OnValueChanged -= DisplayText;
    }
    private void DisplayText(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        displayNameText.text = newValue.ToString();
    }
}
