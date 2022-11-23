using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Grabber : Singleton<Grabber>
{

    public GameObject selectedObject;
    private int groundLayer;
    private Vector3 offSetHight = new Vector3(0f, 1f, 0f);
    public Vector3 SelectPosition;

    private void Awake()
    {
        groundLayer = 6;
    }
    private void Update()
    {
        Grab();
    }

    public void Grab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            
            //마우스로 몬스터를 집을 때
            if (selectedObject == null)
            {
                hit = GrabRay();
                if (!hit.transform.TryGetComponent(out IGrabable grabable)) return;
                selectedObject = hit.collider.gameObject;
                SelectPosition = selectedObject.transform.position;
                GroundManager.Instance.Select(); //밑에 타일 띄우기
            }
            //마우스로 몬스터를 놓을 때
            else
            {   //놓을려는 장소의 layer가 ground이면서 playerground이고 몬스터가 채워져 있지 않을 때 몬스터롤 놓을 수 있다.
                if (Physics.Raycast(selectedObject.transform.position, selectedObject.transform.up * -1f, out hit, 10f))
                {
                    if (hit.transform.gameObject.layer == groundLayer && hit.transform.GetComponent<Ground>().isPlayerGround)
                    {
                        if (!hit.transform.GetComponent<Ground>().filledMonster)
                        {
                            if (GameManager.Instance.CurPopulation < GameManager.Instance.totalPopulation || hit.transform.GetComponent<Ground>().groundType == GROUND_TYPE.WaitingSeat)
                            {
                                selectedObject.transform.position = hit.transform.position + offSetHight;
                                selectedObject = null;
                                GroundManager.Instance.Select();
                            }
                            else
                            {
                                    return;
                            }
                        }
                        else
                        {   //자리 바꾸기
                            hit.transform.GetComponent<Ground>().ChangePosition(SelectPosition);
                            selectedObject.transform.position = hit.collider.transform.position + offSetHight;
                            selectedObject = null;
                            GroundManager.Instance.Select();
                        }    
                    }
                }

            }
        }
        //마우스로 몬스터를 집고 있을 때
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, 1.3f, worldPosition.z);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            ClickForSell();
        }
    }

    private RaycastHit GrabRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Mathf.Infinity) ;
        return hit;
    }

    public void ClickForSell()
    {
        if (selectedObject == null) return;
        StoreManager.Instance.SellMonster(selectedObject);
        GroundManager.Instance.Select(); //선택했을 때 타일 끄기
        selectedObject = null;
    }
}
