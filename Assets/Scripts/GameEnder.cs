using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private TryCounter _tryCounter;
    [SerializeField] private EndGamePanel _endGamePanel;
    [SerializeField] private Score _score;
    [SerializeField] private User _user;

    private void OnEnable()
    {
        _tryCounter.TriesEnded += OnTriesEnded;
    }

    private void OnDisable()
    {
        _tryCounter.TriesEnded -= OnTriesEnded;
    }

    private void OnTriesEnded()
    {
        int reward = GetRewardCurrency();
        PlayerPrefs.SetInt("Currency", PlayerPrefs.GetInt("Currency", 0) + reward);
        PlayerPrefs.SetInt("GameOver", 1);
        _endGamePanel.Init(reward);
        _endGamePanel.gameObject.SetActive(true);
    }

    private int GetRewardCurrency()
    {
        return _score.Value * 15 * _user.Level;
    }
}
