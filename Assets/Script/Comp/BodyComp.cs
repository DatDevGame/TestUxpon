using UnityEngine;

public class BodyComp : MonoBehaviour
{
    [SerializeField] private Transform _hand;
    public Transform Hand => _hand;
}
