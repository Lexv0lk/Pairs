using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SessionTimer : MonoBehaviour
{
    [SerializeField] private int _sessionsDelay;
    [SerializeField] private Button _playButton;
    [SerializeField] private Text _playButtonText;

    private void Start()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt("GameOver", 0)))
            StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        _playButton.interactable = false;
        string oldText = _playButtonText.text;

        int _timeLeft = _sessionsDelay * 60;
        while (_timeLeft >= 0)
        {
            _playButtonText.text = GetFormattedTime(_timeLeft);
            _timeLeft--;
            yield return new WaitForSecondsRealtime(1);
        }

        PlayerPrefs.SetInt("GameOver", 0);
        _playButtonText.text = oldText;
        _playButton.interactable = true;
    }

    private string GetFormattedTime(int seconds)
    {
        int secondsPart = seconds % 60;
        return seconds / 60 + ":" + (secondsPart < 10 ? "0" + secondsPart : secondsPart.ToString());
    }
}
