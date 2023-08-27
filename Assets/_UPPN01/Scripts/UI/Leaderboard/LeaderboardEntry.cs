using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayText;
    private LeaderboardEntryState _state;

    public ulong ClienId { get => _state.ClientId; }
    public int Coins { get => _state.Coins;
        set {
            _state.Coins = value;
            UpdateText();
        }
    }
    public void Initialize(LeaderboardEntryState state)
    {
        _state = state;
        displayText.text = $"1. {_state.PlayerName} ({_state.Coins})";
    }

    public void UpdateText()
    {
        displayText.text = $"{transform.GetSiblingIndex() + 1}. {_state.PlayerName} ({_state.Coins})";
    }
}
