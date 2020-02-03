using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Text _title;

    public void Init(int currencyReward)
    {
        _title.text = $"Вы заработали {currencyReward} валюты!";
    }
}
