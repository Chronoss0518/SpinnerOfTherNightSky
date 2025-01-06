using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    ItemZoneManager myItemZone;

    [SerializeField]
    TrashZoneManager myTrashZone;

    [SerializeField]
    MagicZoneManager myMagicZone;

    public GameObject stoneModel { get; private set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    public ItemZoneManager ItemZone {  get { return myItemZone; } }

    public TrashZoneManager TrashZone { get { return myTrashZone; } }

    public MagicZoneManager MagicZone { get {return myMagicZone; } }


}
