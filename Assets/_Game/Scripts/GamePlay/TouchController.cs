using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float moveSpeed = 0.1f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;


    public Camera camera; // Camera cần điều chỉnh zoom
    public float zoomSpeed; // Tốc độ zoom
    public float minZoom; // Giới hạn zoom nhỏ nhất
    public float maxZoom; // Giới hạn zoom lớn nhất

    private bool isRotate = true;
    public bool isTouchColor = true;


    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {


        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                CheckRotateOrTouchColoring(touchStartPos);
                TouchColoring(touchStartPos);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchEndPos = touch.position;
                TouchColoring(touchEndPos);
                RotateModel(touchStartPos, touchEndPos, touch);
                touchStartPos = touchEndPos;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            CheckFillColorOriginalOrNumberOriginal();
            Zoom(touch0, touch1);
            //if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            //{
            //    Vector2 touch0PrevPosWorld = camera.ScreenToWorldPoint(touch0PrevPos);
            //    Vector2 touch0PosWorld = camera.ScreenToWorldPoint(touch0.position);
            //    Vector2 deltaPos = touch0PosWorld - touch0PrevPosWorld;

            //    transform.position -= new Vector3(deltaPos.x, deltaPos.y, 0) * moveSpeed;
            //}
        }


    }

    private void TouchColoring(Vector2 touchPos)
    {
        if (!isTouchColor) return;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cube cube = hit.collider.gameObject.GetComponent<Cube>();

            if (hit.collider != null && LevelManager.Ins.getNumberCurrentSelect() == cube.ID && cube.isFill == false)
            {

                cube.isFill = true;
                LevelManager.Ins.CountCheckWin();
                LevelManager.Ins.CheckWinNumber();
                hit.transform.GetComponent<Renderer>().material = LevelManager.Ins.currentLevel.materialsColorFill[LevelManager.Ins.getNumberCurrentSelect()];


            }
            else if (cube != null && !cube.isFill && LevelManager.Ins.isPaint)
            {
                List<Cube> connectedCubes = cube.GetConnectedCubesWithRaycast();
                foreach (Cube connectedCube in connectedCubes)
                {
                    connectedCube.isFill = true;
                    LevelManager.Ins.CountCheckWin();
                    LevelManager.Ins.CheckWinNumber();
                    connectedCube.GetComponent<Renderer>().material = LevelManager.Ins.currentLevel.materialsColorFill[cube.ID];
                }
                LevelManager.Ins.SetStateButtonPaint(false);

            }
            


        }

    }

    private void CheckRotateOrTouchColoring(Vector2 touchPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cube cube = hit.collider.gameObject.GetComponent<Cube>();
            if (hit.collider != null && LevelManager.Ins.getNumberCurrentSelect() == cube.ID && cube.isFill == false)
            {
                SetRotateAndTouchColor(false, true);
            }
            else if (hit.collider != null && LevelManager.Ins.isPaint && cube.isFill == false)
            {
                SetRotateAndTouchColor(false, true);
            }
            else if (hit.collider != null && (LevelManager.Ins.getNumberCurrentSelect() == cube.ID && cube.isFill == true))
            {
                SetRotateAndTouchColor(true, false);
            }
            else if (hit.collider != null && LevelManager.Ins.getNumberCurrentSelect() != cube.ID)
            {
                SetRotateAndTouchColor(true, false);
            }
            else if (hit.collider == null)
            {
                SetRotateAndTouchColor(true, false);
            }
            
            //code dieu kien o day

        }
    }
    private void SetRotateAndTouchColor(bool isRotate, bool isTouchColor)
    {
        this.isRotate = isRotate;
        this.isTouchColor = isTouchColor;
    }

    private void RotateModel(Vector2 touchStartPos, Vector2 touchEndPos, Touch touch)
    {
        if (!isRotate || IsPointerOverUI(touch))
        {
            return;
        }


        float deltaX = touchEndPos.x - touchStartPos.x;
        float deltaY = touchEndPos.y - touchStartPos.y;

        float angleY = -deltaX * rotationSpeed;
        float angleX = deltaY * rotationSpeed;

        transform.RotateAround(transform.position, transform.up, angleY);

        transform.RotateAround(transform.position, Vector3.right, angleX);

    }
    private bool IsPointerOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    public void ZoomOutFake(int value)
    {
        value--;
        float newZoom = (camera.fieldOfView + (float)value * zoomSpeed);
        camera.fieldOfView = newZoom;
        CheckFillColorOriginalOrNumberOriginal();
    }
    public void ZoomInFake(int value)
    {
        value++;
        float newZoom = (camera.fieldOfView + (float)value * zoomSpeed);
        camera.fieldOfView = newZoom;
        CheckFillColorOriginalOrNumberOriginal();
    }
    public void Zoom(Touch touch0, Touch touch1)
    {
        if (camera.fieldOfView < 25)
        {
            print("zoom color");
            LevelManager.Ins.FillNumberOriginal();
        }
        else
        {
            LevelManager.Ins.FillColorOriginal();
        }
        touch0 = Input.GetTouch(0);
        touch1 = Input.GetTouch(1);

        // Tính toán khoảng cách giữa hai touch trong frame hiện tại và frame trước
        Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
        Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

        float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
        float touchDeltaMag = (touch0.position - touch1.position).magnitude;

        float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;


        float newZoom = Mathf.Clamp(camera.fieldOfView - deltaMagnitudeDiff * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        camera.fieldOfView = newZoom;

    }
    private void CheckFillColorOriginalOrNumberOriginal()
    {
        if (camera.fieldOfView < 25)
        {
            LevelManager.Ins.FillNumberOriginal();
        }
        else if (camera.fieldOfView >= 25)
        {
            LevelManager.Ins.FillColorOriginal();
        }
    }

    public Cube FindClosestCubeWithId(int id)
    {
        Cube closestCube = null;
        float minDistance = float.MaxValue;

        foreach (GameObject cubeObject in LevelManager.Ins.cubesOfLevel)
        {
            Cube cube = cubeObject.GetComponent<Cube>();
            if (cube != null && cube.ID == id && !cube.isFill)
            {
                float distance = Vector3.Distance(Camera.main.transform.position, cube.transform.position);
                if (distance < minDistance)
                {
                    closestCube = cube;
                    minDistance = distance;
                }
            }
        }

        return closestCube;
    }

    public IEnumerator RotateAndZoomToCube(Cube targetCube)
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 targetPosition = targetCube.transform.position;
        Quaternion initialRotation = cameraTransform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position);
        float initialFOV = Camera.main.fieldOfView;
        float targetFOV = 20f; // Chọn FOV nhỏ hơn để zoom vào

        float duration = 1f; // Thời gian để hoàn thành xoay và zoom
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            cameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            Camera.main.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            yield return null;
        }

        // Đảm bảo camera đã xoay và zoom chính xác vào vị trí cube
        cameraTransform.rotation = targetRotation;
        Camera.main.fieldOfView = targetFOV;
    }
    public IEnumerator RotateAndZoomToCube2(Cube targetCube)
    {
        Transform modelTransform = this.transform; // Model transform là transform của GameObject chứa script này
        Transform cameraTransform = Camera.main.transform;
        Vector3 targetPosition = targetCube.transform.position;
        Quaternion initialModelRotation = modelTransform.rotation;
        Vector3 directionToCube = targetPosition - modelTransform.position;
        Quaternion targetModelRotation = Quaternion.LookRotation(directionToCube);

        float initialFOV = Camera.main.fieldOfView;
        float targetFOV = 20f; // Chọn FOV nhỏ hơn để zoom vào

        float rotateDuration = 1.5f; // Thời gian để hoàn thành xoay
        float zoomDuration = 0.5f; // Thời gian để hoàn thành zoom (giảm thời gian để zoom nhanh hơn)
        float elapsed = 0f;

        // Xoay model trước
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;

            modelTransform.rotation = Quaternion.Slerp(initialModelRotation, targetModelRotation, t);

            yield return null;
        }

        // Đảm bảo model đã xoay chính xác vào vị trí cube
        modelTransform.rotation = targetModelRotation;

        // Reset elapsed time để bắt đầu quá trình zoom
        elapsed = 0f;

        // Zoom vào vị trí cube
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;

            Camera.main.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            yield return null;
        }

        // Đảm bảo camera đã zoom chính xác
        Camera.main.fieldOfView = targetFOV;
    }

    public IEnumerator RotateAndZoomToCube3(Cube targetCube)
    {
        Transform modelTransform = this.transform; // Model transform là transform của GameObject chứa script này
        Transform cameraTransform = Camera.main.transform;
        Vector3 targetPosition = targetCube.transform.position;
        Vector3 directionToCube = (targetPosition - modelTransform.position).normalized;

        Vector3 initialForward = modelTransform.forward;
        Vector3 initialUp = modelTransform.up;

        float initialFOV = Camera.main.fieldOfView;
        float targetFOV = 20f; // Chọn FOV nhỏ hơn để zoom vào

        float rotateDuration = 1.5f; // Thời gian để hoàn thành xoay
        float zoomDuration = 0.5f; // Thời gian để hoàn thành zoom (giảm thời gian để zoom nhanh hơn)
        float elapsed = 0f;

        // Xoay model trước
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;

            Vector3 currentForward = Vector3.Slerp(initialForward, directionToCube, t);
            Vector3 currentUp = Vector3.Slerp(initialUp, Vector3.up, t);

            float angleY = Vector3.SignedAngle(initialForward, currentForward, Vector3.up);
            float angleX = Vector3.SignedAngle(initialUp, currentUp, Vector3.right);

            modelTransform.RotateAround(modelTransform.position, modelTransform.up, angleY * Time.deltaTime / rotateDuration);
            modelTransform.RotateAround(modelTransform.position, Vector3.right, angleX * Time.deltaTime / rotateDuration);

            yield return null;
        }

        // Đảm bảo model đã xoay chính xác vào vị trí cube
        modelTransform.rotation = Quaternion.LookRotation(directionToCube);

        // Reset elapsed time để bắt đầu quá trình zoom
        elapsed = 0f;

        // Zoom vào vị trí cube
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;

            Camera.main.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, t);

            yield return null;
        }

        // Đảm bảo camera đã zoom chính xác
        Camera.main.fieldOfView = targetFOV;
    }





}

