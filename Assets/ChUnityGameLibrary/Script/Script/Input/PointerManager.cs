using UnityEngine;

namespace ChUnity.Input
{
    /// <summary>
    /// �}�E�X�̃N���b�N��X�}�[�g�t�H���Ȃǂ̃^�b�`�����ʒu���Ǘ�����V���O���g���N���X�B
    /// �}�E�X�Ɋւ��Ă̓N���b�N�����{�^�����Ǘ�����B
    /// instance���擾���AUpdate���s�����Ƃŏ����X�V����B
    /// </summary>
    [System.Serializable]
    public class PointerManager
    {

        //SingleTon//
        public static PointerManager instance { get; private set; } = new PointerManager();

        PointerManager() { mouseButtonClickCount = 0; }

        //Define//

        public const int MOUSE_BUTTON_CHECK_COUNT = 3;

        //Enums//

        public enum NormalMouseButton
        {
            PrimaryButton,//(�ʏ펞���{�^��)//
            SecondryButton,//(�ʏ펞�E�{�^��)//
            MiddleButton
        }

        //Properties//
        public Vector2 startPoint { get; private set; } = Vector2.zero;

        public Vector2 beforePoint { get; private set; } = Vector2.zero;

        public Vector2 endPoint { get; private set; } = Vector2.zero;

        public Vector2 mousePoint { get; private set; } = Vector2.zero;



        public bool IsMouseButtonClick(NormalMouseButton _normalMouseButton)
        {
            return mouseButtonClickFlg[(int)_normalMouseButton];
        }

        // Update is called once per frame
        public void Update()
        {
            beforePoint = endPoint;
            UpdateMousePoint();
            UpdateTouchPoint();
        }

        void UpdateTouchPoint()
        {
            if (UnityEngine.Input.touchCount <= 0) return;

            var touch = UnityEngine.Input.GetTouch(0);

            var tmpPoint = touch.rawPosition;
            tmpPoint.x /= Screen.width;
            tmpPoint.y /= Screen.height;

            startPoint = tmpPoint;

            tmpPoint = touch.position;
            tmpPoint.x /= Screen.width;
            tmpPoint.y /= Screen.height;

            endPoint = tmpPoint;

        }

        void UpdateMousePoint()
        {
            var tmpPoint = UnityEngine.Input.mousePosition;
            tmpPoint.x /= Screen.width;
            tmpPoint.y /= Screen.height;

            mousePoint = tmpPoint;

            int beforeCount = mouseButtonClickCount;

            for (int i = 0; i < MOUSE_BUTTON_CHECK_COUNT; i++)
            {

                bool beforeFlg = mouseButtonClickFlg[i];
                mouseButtonClickFlg[i] = UnityEngine.Input.GetMouseButton(i);

                mouseButtonClickCount = beforeFlg == mouseButtonClickFlg[i] ? 
                    mouseButtonClickCount : mouseButtonClickFlg[i] ?
                        mouseButtonClickCount + 1 :
                        mouseButtonClickCount - 1;

            }

            if (mouseButtonClickCount > 0)
            {
                if(beforeCount <= 0)
                {
                    startPoint = mousePoint;
                    beforePoint = mousePoint;
                }
                endPoint = mousePoint;
            }

        }

        bool[] mouseButtonClickFlg = new bool[MOUSE_BUTTON_CHECK_COUNT];


        [SerializeField]
        int mouseButtonClickCount = 0;


    }

}
