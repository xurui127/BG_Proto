﻿using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;

    [Header("Pan Settings")]
    [SerializeField] float panSpeed = 10f;
    const float panRangeMax = 10;
    const float panRangeMin = -10;

    const float boundX = 10f;
    const float boundZ = 10f;

    const float zoomSpeed = 2f;
    const float minZoom = 10f;
    const float maxZoom = 50f;

    Vector3 dragOrigin;

    bool canMoveCamera = true;
    bool allowCameraControl = true;


    internal static UnityEvent<bool> OnCameraMovementToggleEvent = new();

    private void OnEnable()
    {
        OnCameraMovementToggleEvent.RemoveListener(RequestCameraMove);
        OnCameraMovementToggleEvent.AddListener(RequestCameraMove);
    }

    private void OnDisable()
    {
        OnCameraMovementToggleEvent.RemoveListener(RequestCameraMove);
    }

    private void Update()
    {
        if (!canMoveCamera) return;

        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        if (!canMoveCamera) return;

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            ResetCameraFollowTarget();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-mouseDelta.x, 0, -mouseDelta.y) * panSpeed * Time.deltaTime;
            Vector3 newPos = cam.transform.position + move;

            newPos.x = Mathf.Clamp(newPos.x, panRangeMin, panRangeMax);
            newPos.z = Mathf.Clamp(newPos.z, panRangeMin, panRangeMax);


            if (Mathf.Abs(newPos.x) <= boundX && Mathf.Abs(newPos.z) <= boundZ)
            {
                cam.transform.position = newPos;
                dragOrigin = Input.mousePosition;
            }
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            float fov = cam.m_Lens.FieldOfView;
            fov -= scroll * zoomSpeed * 10f;
            fov = Mathf.Clamp(fov, minZoom, maxZoom);
            cam.m_Lens.FieldOfView = fov;
        }
    }

    internal void SetCanmerFollowTarget(Transform target)
    {
        if (cam.Follow != null) return;

        StartCoroutine(MoveToTargerConroutine(target));
        IEnumerator MoveToTargerConroutine(Transform target)
        {
            cam.Follow = target;

            float startFOV = cam.m_Lens.FieldOfView;
            float targetFOV = 50f;
            float fovDuration = 0.3f;
            float fovElapsed = 0f;

            while (fovElapsed < fovDuration)
            {
                fovElapsed += Time.deltaTime;
                cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, fovElapsed / fovDuration);
                yield return null;
            }

            cam.m_Lens.FieldOfView = targetFOV;
        }

    }

    private void ResetCameraFollowTarget()
    {
        cam.Follow = null;
    }

    internal void RequestCameraMove(bool enable)
    {
        if (!allowCameraControl) return;
        canMoveCamera = enable;
    }

    internal void LockCameraControl()
    {
        allowCameraControl = false;
        canMoveCamera = false; 
    }

    internal void UnlockCameraControl()
    {
        allowCameraControl = true;
        canMoveCamera = true; 
    }
}
