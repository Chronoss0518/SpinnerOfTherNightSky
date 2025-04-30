using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    GameObject stone = null;

    [SerializeField, ReadOnly]
    CardData[] cardData = null;

    [SerializeField]
    Book book = null;

    [SerializeField]
    GameObject landscapeScreenPos = null, verticalScreenPos = null;

    [SerializeField]
    PlayerControllerUI verticalPlayerUIs = null, landscapePlayerUIs = null;

    [SerializeField]
    GameObject bookObjectPos = null;

    [SerializeField]
    GameObject visibleButtonCanvas = null;

    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    [SerializeField, ReadOnly]
    Manager.DisplayAspectType beforeType = Manager.DisplayAspectType.None;

    public bool initFlg { get; private set; } = false;
    public GameObject stoneModel { get { return stone; } }

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    public ControllerBase controller { get { return controllerCom; } }

    public Book bookZone { get { return book; } }

    public ItemZoneManager itemZone {  get { return myItemZone; } }

    public TrashZoneManager trashZone { get { return myTrashZone; } }

    public MagicZoneManager magicZone { get {return myMagicZone; } }

    public void SelectTargetStart(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        if ((_action.zoneType | ScriptManager.ZoneType.Book) > 0 && book != null) book.SelectTargetTest(_action, _runPlayer);
        if ((_action.zoneType | ScriptManager.ZoneType.ItemZone) > 0 && itemZone != null) itemZone.SelectTargetTest(_action, _runPlayer);
        if ((_action.zoneType | ScriptManager.ZoneType.TrashZone) > 0 && trashZone != null) trashZone.SelectTargetTest(_action, _runPlayer);
        if ((_action.zoneType | ScriptManager.ZoneType.MagicZone) > 0 && magicZone != null) magicZone.SelectTargetTest(_action, _runPlayer);
    }

    public void SelectTargetEnd()
    {
        if (book != null) book.SelectTargetDown();
        if (itemZone != null) itemZone.SelectTargetDown();
        if (trashZone != null) trashZone.SelectTargetDown();
        if (magicZone != null) magicZone.SelectTargetDown();
    }

    public void SelectTargetItemZoneStart()
    {
        if (itemZone != null) itemZone.SelectPositionTargetUp();
    }

    public void SelectTargetItemZoneEnd()
    {
        if (itemZone != null) itemZone.SelectPositionTargetDown();
    }

    public void Init(
        CardData[] _cardData,
        bool _createBookObject = false)
    {
        cardData = _cardData;

        itemZone.Init(gameManager);

        initFlg = true;

        if (!_createBookObject) return;
        if (book == null) return;
        if (book.initFlg) return;

        book.Init();
        book.InitCard(this, gameManager, cardData);

        UpdateBookParent();

    }

    public void SetCPUController()
    {
        controllerCom = gameObject.AddComponent<CPUController>();
        RemoveUnLocalPlayerObject();
    }

    public void SetNetController()
    {
        controllerCom = gameObject.AddComponent<NetWorkController>();
        RemoveUnLocalPlayerObject();
    }

    public void SetLocalPlayerController()
    {
        var tmp = gameObject.AddComponent<LocalPlayerController>();
        tmp.SetVerticalUIs(verticalPlayerUIs);
        tmp.SetLandscapeUIs(landscapePlayerUIs);

        gameManager.SetTextObject(verticalPlayerUIs.descriptionText, landscapePlayerUIs.descriptionText);
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

    void RemoveUnLocalPlayerObject()
    {
        if (book != null)
            Destroy(book.gameObject);
        if (bookObjectPos != null)
            Destroy(bookObjectPos);
        if (visibleButtonCanvas != null)
            Destroy(visibleButtonCanvas);
        if (verticalPlayerUIs != null)
            Destroy(verticalPlayerUIs.gameObject);
        if (landscapePlayerUIs != null)
            Destroy(landscapePlayerUIs.gameObject);
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
