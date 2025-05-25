using ChUnity.Input;
using Unity.Collections;
using UnityEngine;

public class StarPosSheet : PanelPosBase
{
    [SerializeField, ReadOnly]
    Vector2 movePoint;
    [SerializeField, ReadOnly]
    Vector3 moveVec;

    [SerializeField]
    StarPosGrid grid = null;

    [SerializeField]
    GameObject buttons = null;

    [SerializeField, ReadOnly]
    Vector3 localTouchPos;

    bool touchInitFlg = false;

    PointerManager pointer = PointerManager.instance;

    Manager manager = Manager.ins;

    [SerializeField,ReadOnly]
    Manager.HandType handType = Manager.HandType.None;

    [SerializeField]
    GameObject rightButtons = null, leftButtons = null;

    [SerializeField]
    GameObject starPosPrefab = null;

    [SerializeField, ReadOnly]
    GameObject[][] starPosList = null;

    [SerializeField,ReadOnly]
    StoneBoardManager stoneBoard = null;

    [SerializeField,ReadOnly]
    bool movePanelFlg = false;

    Camera useCam = null;

    public void SetUseCamera(Camera _cam)
    {
        if (_cam == null) return;
        useCam = _cam;
    }

    public void PointDownGrip()
    {
        movePanelFlg = true;
    }
    
    public void PointUpGrip()
    {
        movePanelFlg = false;
        touchInitFlg = false;
    }


    override protected void InitHoryzontalList(int _nowCount)
    {
        starPosList[_nowCount] = new GameObject[PANEL_COUNT_X];
    }

    override protected void InitVerticalList()
    {
        starPosList = null;
        starPosList = new GameObject[PANEL_COUNT_Y][];
    }

    override protected Vector3 SetStartPos(Vector3 _startPos) 
    {
        _startPos.y = 0.0f;
        return _startPos;
    }

    public void SetStoneBoard(StoneBoardManager _stoneBoard)
    {
        stoneBoard = _stoneBoard;
        transform.localPosition = new Vector3(transform.localPosition.x, stoneBoard.stonePosTop, transform.localPosition.z);
    }

    public void AddStarPos(GameObject _obj, int _x, int _y)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x] = _obj;
    }

    public void SetStarPosFlg(int _x,int _y,bool _flg)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x].SetActive(_flg);
    }

    public bool IsRange(int _x, int _y)
    {
        return
            _x >= 0 && _x < PANEL_COUNT_X &&
            _y >= 0 && _y < PANEL_COUNT_Y;
    }

    public void ClearPos()
    {
        for (int i = 0; i < PANEL_COUNT_Y; i++)
        {
            for (int j = 0; j < PANEL_COUNT_X; j++)
            {
                starPosList[j][i].SetActive(false);
            }
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    protected override void CreateOobject(float _vPos, Vector2Int _pos, GameObject _verticalPos, PanelPosManager _builder)
    {
        var starPos = Instantiate(starPosPrefab, _verticalPos.transform);

        starPos.name = $"X[{_pos.x}]Y[{_pos.y}]";
        starPos.transform.localPosition = new Vector3(_vPos, 0.0f, 0.0f);
        starPos.SetActive(false);

        starPosList[_pos.y][_pos.x] = starPos;
    }

    void Update()
    {
        UpdateHandType();
        UpdatePosition();
    }

    void UpdateHandType()
    {
        if (handType == manager.handType) return;
        if (leftButtons == null || rightButtons == null || buttons == null) return;

        handType = manager.handType;
        bool rightHandFlg = manager.handType == Manager.HandType.Right;
        buttons.transform.SetParent((rightHandFlg ? rightButtons : leftButtons).transform);
        buttons.transform.localPosition = Vector3.zero;
    }

    void UpdatePosition()
    {
        if (stoneBoard == null) return;

        MoveTest();

        var pos = Vector3.zero;

        pos.x -= MinTest(transform.localPosition.x, stoneBoard.transform.localPosition.x - size.x);
        pos.z -= MinTest(transform.localPosition.z, stoneBoard.transform.localPosition.z - size.z);

        pos.x += MaxTest(transform.localPosition.x, stoneBoard.transform.localPosition.x + size.x);
        pos.z += MaxTest(transform.localPosition.z, stoneBoard.transform.localPosition.z + size.z);

        transform.localPosition += pos;
    }

    float MinTest(float _base, float _target)
    {
        float tmpSize = _base - _target;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }
    float MaxTest(float _base, float _target)
    {
        float tmpSize = _target - _base;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }

    void MoveTest()
    {
        if (useCam == null) return;
        if (!movePanelFlg) return;

        if(!touchInitFlg)
        {
            touchInitFlg = true;
            localTouchPos =  transform.localPosition - transform.worldToLocalMatrix.MultiplyPoint(grid.touchPosition);
            return;
        }

        float tmpY = transform.localPosition.y;
        float tmpLen = tmpY - useCam.transform.position.y;

        moveVec = pointer.GetCameraToWorldPosition(useCam, pointer.endPoint, Mathf.Abs(tmpLen));

        Vector3 tmp = moveVec + localTouchPos;

        tmp.y = tmpY;
        transform.localPosition = tmp;
    }

}
