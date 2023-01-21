using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static event Action OnLobbyPopup;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI[] _textMeshArray;

    private void Start()
    {
        OnLobbyPopup?.Invoke();
        _levelText.text = $"Level Selected: {LevelManager.GetCurrentLevel()}";
    }

    private void OnEnable()
    {
        // _startGameButton.onClick.AddListener(OnStartGame);
        CharacterManager.OnCharacterJoined += OnCharacterJoined;
    }

    private void OnDisable()
    {
        // _startGameButton.onClick.RemoveListener(OnStartGame);
        CharacterManager.OnCharacterJoined -= OnCharacterJoined;
    }

    private void OnCharacterJoined(int count)
    {
        var index = count - 1;
        _textMeshArray[index].gameObject.SetActive(false);
    }

    private void OnStartGame()
    {
        if (CharacterManager.Instance.CurrentPlayersCount <= 0)
        {
            // Debug.LogError("Must Press Join");
            var mainCanvas = UIManager.GetMainCanvas();
            var popup = UIManager.GetUIObject<PopupUI>(UIType.Popup);
            popup.transform.SetParent(mainCanvas.transform,false);
            popup.ShowMessage("Must Press Join...");
            return;
        }
        var fadeTransition = UIManager.GetUIObject<FadeTransitionUI>(UIType.FadeTransition);
        fadeTransition.ExitScene(Data.GAME_SCENE);
        //SceneManager.LoadScene(Data.GAME_SCENE);
    }
}
