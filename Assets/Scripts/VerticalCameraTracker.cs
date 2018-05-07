using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class VerticalCameraTracker : MonoBehaviour {
    public GameObject target;
    private Camera camera;
    public float minY;
    public bool isMinFromInitialPosition;
    private readonly float screenCenter = 0.5f;
    private void Awake()
    {
        setupTrackingStream();
    }

    private void setupTrackingStream()
    {
        camera = GetComponent<Camera>();
        var trigger = target.GetComponent<ObservableUpdateTrigger>() ?? target.AddComponent<ObservableUpdateTrigger>();
        var rigid = target.GetComponent<Rigidbody2D>();
        if (!rigid) return;
        if (isMinFromInitialPosition)
            minY = transform.position.y;
        var velocityChanged = trigger.UpdateAsObservable()
            .Select(_ => rigid.velocity.y)
            .DistinctUntilChanged();
        velocityChanged
            .Where(y => 0 < y)
            .Select(y => camera.WorldToViewportPoint(target.transform.position).y)
            .Where(y => screenCenter < y)
            .Subscribe(_ =>
            {
                var newPosition = target.transform.position;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            });
        velocityChanged
            .Where(y => y < 0)
            .Select(y => camera.WorldToViewportPoint(target.transform.position).y)
            .Subscribe(_ =>
            {
                Vector3 newPosition;
                if(transform.position.y<=minY){
                        newPosition = transform.position;
                        newPosition.y = minY;
                        transform.position = newPosition;
                        return;
                }
                newPosition = target.transform.position;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            });
    }
}
