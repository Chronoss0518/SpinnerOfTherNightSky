using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [System.Serializable]
    private class CardObject
    {
        public RawImage image = null;
        public Animator animator = null;
    }

    [SerializeField, ReadOnly]
    public bool selectFlg = true;

    [SerializeField]
    CardObject front = new CardObject(), back = new CardObject();

    [SerializeField,ReadOnly]
    CardData data = null;

    public string cardName { get { return data.name; } }

    public string description { get { return data.description; } }

    public int[] script { get { return data.script; } }

    public int initBookPos { get { return data.initBookPos; } }

    public bool isSelect { get { return selectFlg; } }

    public CardData.CardType type { get; private set; } = CardData.CardType.Magic;

    System.Action selectAction = ()=>{};

    public void SetAction(System.Action _selectAction)
    {
        if (_selectAction == null) return;
        selectAction = _selectAction;
    }
    void Start()
    {
        SetSelectFlg(false);
    }

    public void SetFrontTexture(Texture2D _tex){ if (front.image != null) front.image.texture = _tex; }
    public void SetBackTexture(Texture2D _tex) { if (back.image != null) back.image.texture = _tex; }

    public void SetSelectFlg(bool _flg)
    {
        if (selectFlg == _flg) return;

        SetAnimation(front, _flg);
        SetAnimation(back, _flg);

        selectFlg = _flg;
    }

    public void Init(CardData _data)
    {
        data = _data;
        InitMagicCardScript(data);
        InitItemCardScript(data);
    }

    public void SelectAction()
    {
        if (selectAction != null) selectAction();
    }

    void InitMagicCardScript(CardData _data)
    {
        if (data.cardType != (int)CardData.CardType.Magic) return;
        var script = gameObject.AddComponent<MagicCardScript>();
        var magicData = (MagicCardData)_data;
        script.SetAttribute(magicData.month);
        script.SetPoint(magicData.point);
        script.SetBaseCard(this);
    }

    void InitItemCardScript(CardData _data)
    {
        if (data.cardType != (int)CardData.CardType.Item) return;
        var script = gameObject.AddComponent<ItemCardScript>();
        var itemData = (ItemCardData)_data;
        script.SetType((ItemCardScript.ItemType)itemData.itemType);
        script.SetBaseCard(this);
    }

    void SetAnimation(CardObject _obj, bool _flg)
    {
        if (_obj.animator == null) return;

        _obj.animator.SetBool("SelectFlg", _flg);
    }


}
