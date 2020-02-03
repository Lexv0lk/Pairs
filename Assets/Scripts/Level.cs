using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private RoundRestarter _roundRestarter;
    [SerializeField] private Text _text;

    private void OnEnable()
    {
        _roundRestarter.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _roundRestarter.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        _text.text = "Уровень: " + _roundRestarter.Level;
    }
}
