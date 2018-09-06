using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMapCameraController : MonoBehaviour
{

    public static readonly float TITL_ANGLE = 10.0f;

    public static readonly int MAX_TITL_COUNT = 5;

    public BattleStageHolder holder;

    public BattleMapTiltController tiltController;

    private float moveSpeed = 0.3f;

    private Vector3 preMousePos;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{

        //    GameObject result;

        //    Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
        //    if (collition2d)
        //    {
        //        result = collition2d.transform.gameObject;
        //        Debug.Log("hitRay:" + result.name);
        //    }
        //}

        ////メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    //Rayが当たるオブジェクトがあった場合はそのオブジェクト名をログに表示
        //    Debug.Log("hitRay:" + hit.collider.gameObject.name);
        //}

        if (EventSystem.current.IsPointerOverGameObject())
        {
            // preMousePos = null;
            return;
        }

        MouseUpdate();

        Tilt();
        return;
    }

    private void MouseUpdate()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0.0f)
        {
            MouseWheel(scrollWheel);
        }


        if (Input.GetMouseButtonDown(1))
        {
            preMousePos = Input.mousePosition;
        }

        MouseDrag(Input.mousePosition);
    }

    private void MouseWheel(float delta)
    {

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Camera camera = GetCamera();

        // cs.orthographicSize -= scroll;

        float view = camera.fieldOfView - scroll * 10;

        // cs.fieldOfView = Mathf.Clamp(value: view, min: 0.1f, max: 45f);
        camera.fieldOfView = view;

        return;
    }

    private Camera camera;

    public Camera GetCamera()
    {
        if (camera != null)
        {
            return camera;
        }

        GameObject came = GameObject.Find("BattleMapCamera");
        camera = came.GetComponent<Camera>();

        return camera;
    }

    public Transform GetTransform ()
    {
        return this.transform;
    }

    public Vector3 MoveCamera(Vector3 pos)
    {
        Vector3 current = transform.position;

        Vector3 trans = pos - current;

        // transform.Translate(trans);

        transform.Translate(new Vector3(trans.x, trans.y));
        // transform.position = new Vector3(pos.x, pos.y);

        return current;
    }

    private void MouseDrag(Vector3 mousePos)
    {
        Vector3 diff = mousePos - preMousePos;

        if (Input.GetMouseButton(1))
        {
            transform.Translate(-diff * Time.deltaTime * moveSpeed);
        }

        preMousePos = mousePos;
    }

    /// <summary>
    /// 傾ける
    /// </summary>
    public void Tilt()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Tilt(true);
        }

        // 戻す
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Tilt(false);
        }
    }

    /// <summary>
    /// 傾ける
    /// </summary>
    public void Tilt(bool isTilt)
    {
        Vector3 v31 = new Vector3(0, transform.position.y, 0);
        Vector3 v32 = new Vector3(1, 0, 0);

        // 傾ける
        if (isTilt)
        {
            // 最大傾きなら何もしない
            if (MAX_TITL_COUNT <= holder.BattleMapSetting.TiltCount)
            {
                return;
            }

            transform.RotateAround(v31, v32, -TITL_ANGLE);
            tiltController.TiltAll(TITL_ANGLE);
            holder.BattleMapSetting.TiltCount++;
        }

        // 戻す
        else
        {
            // 戻してあったら何もしない
            if (holder.BattleMapSetting.TiltCount <= 0)
            {
                return;
            }

            transform.RotateAround(v31, v32, TITL_ANGLE);
            tiltController.TiltAll(-TITL_ANGLE);
            holder.BattleMapSetting.TiltCount--;
        }


        // 傾けるごとにずれるので固定しておく
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

    }

}


