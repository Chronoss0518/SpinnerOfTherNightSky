using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager = null;

    [SerializeField]
    Book book = null;

    [SerializeField]
    ItemZoneManager myItemZone = null;

    [SerializeField]
    TrashZoneManager myTrashZone = null;

    [SerializeField]
    MagicZoneManager myMagicZone = null;

    [SerializeField]
    ControllerBase controller = null;

    public GameObject stoneModel { get; private set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    public ItemZoneManager ItemZone {  get { return myItemZone; } }

    public TrashZoneManager TrashZone { get { return myTrashZone; } }

    public MagicZoneManager MagicZone { get {return myMagicZone; } }


    public void SetCPUController()
    {

    }

    public void SetNetController()
    {

    }

    public void SetPlayerController()
    {

    }


}
