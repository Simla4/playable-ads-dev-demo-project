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

    private EventListener<NewAreaOppenedEvent> onFloorAreaOppened;
    

    private void OnEnable()
    {
        onFloorAreaOppened = new EventListener<NewAreaOppenedEvent>(ChangeCamera);
        EventBus<NewAreaOppenedEvent>.AddListener(onFloorAreaOppened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOppenedEvent>.RemoveListener(onFloorAreaOppened);
    }


    private void ChangeCamera(NewAreaOppenedEvent e)
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
