using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumVisibilityTest : MonoBehaviour
{

    public class Panel
    {
        public Vector3 leftTop = new Vector3(-1, 1, -1);
        public Vector3 rightTop = new Vector3(1, 1, -1);
        public Vector3 rightBottom = new Vector3(1, -1, -1);
        public Vector3 leftBottom = new Vector3(-1, -1, -1);

        public Panel()
        {
            Init();
        }

        public Panel(Vector3 _leftTop, Vector3 _rightTop, Vector3 _rightBottom, Vector3 _leftBottom)
        {
            Init(_leftTop, _rightTop, _rightBottom, _leftBottom);
        }

        public void Init()
        {
            Init(
                new Vector3(-1, 1, -1),
                new Vector3(1, 1, -1),
                new Vector3(1, -1, -1),
                new Vector3(-1, -1, -1));
        }

        public void Init(Vector3 _leftTop, Vector3 _rightTop, Vector3 _rightBottom, Vector3 _leftBottom)
        {
            leftTop = _leftTop;
            rightTop = _rightTop;
            rightBottom = _rightBottom;
            leftBottom = _leftBottom;
        }

        public Panel front
        {
            get
            {
                return new Panel(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1.0f, -1.0f, -1.0f), new Vector3(-1.0f, -1.0f, -1.0f));
            }
        }

        public Panel back
        {
            get
            {
                return new Panel(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f));
            }
        }

        public Panel top
        {
            get
            {
                return new Panel(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, -1.0f), new Vector3(-1.0f, 1.0f, -1.0f));
            }
        }

        public Panel bottom
        {
            get
            {
                return new Panel(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(1.0f, 1.0f, -1.0f));
            }
        }

        public Panel left
        {
            get
            {
                return new Panel(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1.0f, -1.0f, -1.0f));
            }
        }

        public Panel right
        {
            get
            {
                return new Panel(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1.0f, -1.0f, 1.0f));
            }
        }


    }

    new public Camera camera = null;

    public BoxCollider testObject = null;

    private bool visible = false;

    private List<Panel> lookPanelList = new List<Panel>();

    // Update is called once per frame
    void Update()
    {
        if (camera == null) return;
        if (testObject == null) return;


        Matrix4x4 viewMat = camera.worldToCameraMatrix;

        Matrix4x4 projMat = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false);

        Vector3 test = Vector3.one;

        test = testObject.transform.localToWorldMatrix * test;

        test = viewMat * test;

        test =  projMat * test;

        List<Panel> testPanels = new List<Panel>();




    }
}
