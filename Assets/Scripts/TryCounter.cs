using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TryCounter : MonoBehaviour
{
    public UnityAction TriesEnded;

    [SerializeField] private Text _text;
    [SerializeField] private CardMatcher _cardMatcher;

    private int _currentValue;

    private void OnEnable()
    {
        _cardMatcher.Mistaken += OnMistaken;
    }

    private void OnDisable()
    {
        _cardMatcher.Mistaken -= OnMistaken;
    }

    public void SetTries(int value)
    {
        _currentValue = value;
        _text.text = _currentValue.ToString();
    }

    private void OnMistaken()
    {
        _currentValue--;
        _text.text = _currentValue.ToString();
        if (_currentValue == 0)
            TriesEnded?.Invoke();
    }
}
