using TMPro;
using UnityEngine;

public class ScoreSheetUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreUI;

    public void OnUpdateUI(int score)
    {
        string scoreMsg = $"Score: {score:F0}";
        _scoreUI.text = scoreMsg;
    }
}