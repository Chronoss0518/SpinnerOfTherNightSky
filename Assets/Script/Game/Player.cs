using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
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

    [SerializeField, ReadOnly]
    CardData[] cardData = null;


    public GameObject stoneModel { get; private set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    public ControllerBase playerController { get { return controller; } }

    public ItemZoneManager itemZone {  get { return myItemZone; } }

    public TrashZoneManager trashZone { get { return myTrashZone; } }

    public MagicZoneManager magicZone { get {return myMagicZone; } }

    public void Init(CardData[] _cardData)
    {
        cardData = _cardData;

        if (book!= null)
        {
            book.Init();
            book.InitCard(cardData);
        }
    }

    public void SetCPUController()
    {

    }

    public void SetNetController()
    {
        controller = gameObject.AddComponent<NetWorkController>();
    }

    public void SetPlayerController()
    {

    }


}
