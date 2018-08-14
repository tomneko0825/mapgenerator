using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMapCameraController : MonoBehaviour
{

    public static readonly float TITL_ANGLE = 10.0f;

    public static readonly int MAX_TITL_COUNT = 8;

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

        GameObject came = GameObject.Find("BattleMapCamera");
        Camera cs = came.GetComponent<Camera>();

        cs.orthographicSize -= scroll;
        return;
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
        Vector3 v31 = new Vector3(0, transform.position.y, 0);
        Vector3 v32 = new Vector3(1, 0, 0);

        float angle = TITL_ANGLE;

        // 傾ける
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 最大傾きなら何もしない
            if (8 <= holder.BattleMapSetting.TiltCount)
            {
                return;
            }

            transform.RotateAround(v31, v32, -TITL_ANGLE);
            tiltController.TiltAll(TITL_ANGLE);
            holder.BattleMapSetting.TiltCount++;
        }

        // 戻す
        if (Input.GetKeyDown(KeyCode.DownArrow))
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


