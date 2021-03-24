using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMap : MonoBehaviour
{
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] chestPositions;

    public Transform[] PlayersPositions => playerPositions;
    public Transform[] ChestPositions => chestPositions;
}
