using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinwhaleStudio.TravelAgency
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Zoom")]
        public float startZoom = 30.0f;
        public float zoomSpeed = 1.5f;
        public float zoomMin = 10.0f;
        public float zoomMax = 80.0f;

        [Header("World")]
        public Transform worldLeft;
        public Transform groundLeft;
        public Transform worldRight;
        public Transform groundRight;
        public Transform worldCenter;
        private float worldWidth = 409.6f;

        private Vector3 MouseDownPosition;

        void Start()
        {
            Camera.main.orthographicSize = startZoom;
        }

        void Update()
        {
            // 카메라 위치에 따른 월드 위치 조정 (무한 카메라)
            RaycastHit cameraHit;
            Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)), out cameraHit, 1 << LayerMask.NameToLayer("Ground"));
            if (cameraHit.transform != null)
            {
                if (cameraHit.transform.Equals(groundLeft))
                {
                    // 센터보다 왼쪽에 있는데 worldLeft.pos.x보다 왼쪽으로 가는 순간 오른쪽 월드를 왼쪽으로 옮김.
                    if (worldLeft.position.x < worldCenter.position.x)
                    {
                        if (Camera.main.transform.position.x < worldLeft.position.x)
                        {
                            worldRight.position = new Vector3(worldLeft.position.x - worldWidth, 0.0f, 0.0f);
                            worldCenter.position = new Vector3(worldLeft.position.x - (worldWidth / 2.0f), 0.0f, 0.0f);
                        }
                    }
                    // 왼쪽으로 옮겨져서 센터가 왼쪽에 있는데 worldLeft.pos.x보다 오른쪽으로 가는 순간 제자리로 돌림.
                    else if (worldLeft.position.x > worldCenter.position.x)
                    {
                        if (Camera.main.transform.position.x > worldLeft.position.x)
                        {
                            worldRight.position = new Vector3(worldLeft.position.x + worldWidth, 0.0f, 0.0f);
                            worldCenter.position = new Vector3(worldLeft.position.x + (worldWidth / 2.0f), 0.0f, 0.0f);
                        }
                    }
                }
                else
                {
                    // 센터보다 오른쪽에 있는데 worldRight.pos.x보다 오른쪽으로 가는 순간 왼쪽 월드를 오른쪽으로 옮김.
                    if (worldRight.position.x > worldCenter.position.x )
                    {
                        if( Camera.main.transform.position.x > worldRight.position.x )
                        {
                            worldLeft.position = new Vector3(worldRight.position.x + worldWidth, 0.0f, 0.0f);
                            worldCenter.position = new Vector3(worldRight.position.x + (worldWidth / 2.0f), 0.0f, 0.0f);
                        }
                    }
                    // 오른쪽으로 옮겨져서 센터가 오른쪽에 있는데 worldRight.pos.x보다 왼쪽으로 가는 순간 제자리로 돌림.
                    else if (worldRight.position.x < worldCenter.position.x)
                    {
                        if (Camera.main.transform.position.x < worldRight.position.x)
                        {
                            worldLeft.position = new Vector3(worldRight.position.x - worldWidth, 0.0f, 0.0f);
                            worldCenter.position = new Vector3(worldRight.position.x - (worldWidth / 2.0f), 0.0f, 0.0f);
                        }
                    }
                }
            }

            // 마우스 휠 사용했을 때.
            if (Input.mouseScrollDelta.y != 0)
            {
                // 마우스 휠 이동거리만큼 만큼 줌.
                var preZoomPosition = getWorldPoint(Input.mousePosition);
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (Input.mouseScrollDelta.y * zoomSpeed), zoomMin, zoomMax);

                // 줌되면서 마우스 클릭한 방향으로 이동.
                var delta = getWorldPoint(Input.mousePosition) - preZoomPosition;
                Camera.main.transform.Translate(-delta.x, -delta.y, 0);
            }

            // 마우스 왼쪽 버튼 클릭했을 때.
            if (Input.GetMouseButtonDown(0))
            {
                MouseDownPosition = getWorldPoint(Input.mousePosition);
            }

            // 마우스로 드래그 중일때.
            if (Input.GetMouseButton(0))
            {
                // 마우스 이동거리만큼 카메라 이동.
                var delta = getWorldPoint(Input.mousePosition) - MouseDownPosition;
                Camera.main.transform.Translate(-delta.x, -delta.y, 0);
            }
        }

        private Vector3 getWorldPoint(Vector2 screenPoint)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
            return hit.point;
        }
    }
}