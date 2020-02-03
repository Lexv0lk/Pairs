using UnityEngine;

public class User : MonoBehaviour
{
    public int Level => _level;

    [SerializeField] private int _level;
}
