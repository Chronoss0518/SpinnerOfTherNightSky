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
    ItemZoneManager myItemZone = null;

    [SerializeField]
    TrashZoneManager myTrashZone = null;

    [SerializeField]
    MagicZoneManager myMagicZone = null;

    [SerializeField]
    ControllerBase controllerCom = null;

    [SerializeField, ReadOnly]
    CardData[] cardData = null;

    [SerializeField]
    Book book = null;

    [SerializeField]
    GameObject landscapeScreenPos = null, verticalScreenPos = null;

    [SerializeField]
    GameObject bookObjectPos = null;

    [SerializeField]
    GameObject visibleButtonCanvas = null;

    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    [SerializeField, ReadOnly]
    Manager.DisplayAspectType beforeType = Manager.DisplayAspectType.None;


    public GameObject stoneModel { get; private set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    public ControllerBase controller { get { return controllerCom; } }

    public ItemZoneManager itemZone {  get { return myItemZone; } }

    public TrashZoneManager trashZone { get { return myTrashZone; } }

    public MagicZoneManager magicZone { get {return myMagicZone; } }

    public void Init(
        CardData[] _cardData,
        bool _createBookObject = false)
    {
        cardData = _cardData;

        if (!_createBookObject) return;
        if (book == null) return;
        if (book.initFlg) return;

        book.Init();
        book.InitCard(cardData);

        UpdateBookParent();
    }

    public void SetCPUController()
    {
        controllerCom = gameObject.AddComponent<CPUController>();
        if (book != null)
           Destroy(book.gameObject);
        if (bookObjectPos != null)
            Destroy(bookObjectPos);
        if (visibleButtonCanvas != null)
            Destroy(visibleButtonCanvas);
    }

    public void SetNetController()
    {
        controllerCom = gameObject.AddComponent<NetWorkController>();
        if (book != null)
            Destroy(book.gameObject);
        if (bookObjectPos != null)
            Destroy(bookObjectPos);
        if (visibleButtonCanvas != null)
            Destroy(visibleButtonCanvas);
    }

    public void SetLocalPlayerController()
    {
        var tmp = gameObject.AddComponent<LocalPlayerController>();
        tmp.SetVerticalUIs(gameManager.verticalPlayerControllerUIs);
        tmp.SetLandscapeUIs(gameManager.landscapePlayerControllerUIs);
        controllerCom = tmp;
    }

    public void Action()
    {
        if (controllerCom == null) return;
        controllerCom.UpActionFlg();
    }

    void Update()
    {
        UpdateBookParent();
        UpdateMoveBookIntoCamera();
    }

    void UpdateBookParent()
    {

        if (book == null) return;
        if (landscapeScreenPos == null) return;
        if (verticalScreenPos == null) return;
        if (beforeType == manager.aspectType) return;
        if (manager.aspectType == Manager.DisplayAspectType.None) return;
        beforeType = manager.aspectType;

        var pos = landscapeScreenPos;
        if (beforeType == Manager.DisplayAspectType.VerticalScreen)
            pos = verticalScreenPos;

        book.transform.SetParent(pos.transform);
        book.transform.localPosition = Vector3.zero;
        book.transform.localRotation = Quaternion.identity;

    }

    void UpdateMoveBookIntoCamera()
    {
        if (book == null) return;
        
    }

}
