using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Grabber : MonoBehaviour
{

    public GameObject selectedObject;
    [SerializeField] private LayerMask grabLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

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
                GroundManager.Instance.Select(); //밑에 타일 띄우기
                Cursor.visible = false;
            }
            //마우스로 몬스터를 놓을 때
            else
            {   //놓을려는 장소의 layer가 ground이면서 playerground이고 몬스터가 채워져 있지 않을 때 몬스터롤 놓을 수 있다.
                if (Physics.Raycast(selectedObject.transform.position, selectedObject.transform.up * -1f, out hit, Mathf.Infinity, groundLayerMask)
                    && hit.transform.GetComponent<Ground>().isPlayerGround && !hit.transform.GetComponent<Ground>().filledMonster)
                {
                    selectedObject.transform.position = hit.collider.transform.position + new Vector3(0, 1, 0);
                    selectedObject = null;
                    GroundManager.Instance.Select();
                    Cursor.visible = true;
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

}
