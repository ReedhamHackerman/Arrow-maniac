using UnityEngine;

public class MainMap : MonoBehaviour
{
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] chestPositions;

    [SerializeField] private float chestSpawnDuration;

    public Transform[] PlayersPositions => playerPositions;
    public Transform[] ChestPositions => chestPositions;
    public float ChestSpawnDuration => chestSpawnDuration;
}
