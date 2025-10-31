using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using sb.eventbus;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera stairsCamera;
    [SerializeField] private CinemachineVirtualCamera boardCamera;

    [Header("Settings")]
    [SerializeField] private float duration = 2;

    private EventListener<NewAreaOpenedEvent> onFloorAreaOppened;
    

    private void OnEnable()
    {
        onFloorAreaOppened = new EventListener<NewAreaOpenedEvent>(ChangeCamera);
        EventBus<NewAreaOpenedEvent>.AddListener(onFloorAreaOppened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOpenedEvent>.RemoveListener(onFloorAreaOppened);
    }


    private void ChangeCamera(NewAreaOpenedEvent e)
    {
        if (e.areaType == FillableAreaTypes.Floor)
        {
            StartCoroutine(ChangeCameraRoutine(stairsCamera));
        }
        else if (e.areaType == FillableAreaTypes.Board)
        {
            boardCamera.gameObject.SetActive(true);
        }
    }

    private IEnumerator ChangeCameraRoutine(CinemachineVirtualCamera camera)
    {
        camera.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(duration);
        
        camera.gameObject.SetActive(false);
    }
}
