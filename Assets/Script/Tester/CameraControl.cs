using DG.Tweening;
using System.Collections;
using UniRx;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform _target;
    private CompositeDisposable _disposables;
    private bool _IsMoveCamera = false;

    private void Awake()
    {
        _disposables = new CompositeDisposable();
    }
    private void LateUpdate()
    {
        CameraViewUpdate();
    }
    public void InitData(Transform target)
    {
        _target = target;
        Subcribe();
    }
    private void Subcribe()
    {
        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Running)
            .Subscribe(state => 
            {
                _target = Gameplay.Instance.Tester.transform;
                StartCoroutine(MoveCameraFocus());
            }).AddTo(_disposables);

        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Init)
            .Subscribe(state =>
            {
                ClearEvent();
            }).AddTo(_disposables);

        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Lost || state == GamePlayState.Won)
            .Subscribe(state =>
            {
                ClearEvent();
            }).AddTo(_disposables);
    }
    private void ClearEvent()
    {
        _IsMoveCamera = false;
        _disposables.Clear();
    }
    private IEnumerator MoveCameraFocus()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 positonCameraView = new Vector3(_target.position.x, _target.position.y + 10, _target.position.z - 10);
        Vector3 rotationCameraView = new Vector3(45, 0, 0);
        transform.DOMove(positonCameraView, 1f);
        transform.DORotate(rotationCameraView, 1f)
            .OnComplete(() => 
            {
                _IsMoveCamera = true;
            });
    }
    private void CameraViewUpdate()
    {
        if (_target == null || !_IsMoveCamera) return;

        Vector3 positonCameraView = new Vector3(_target.position.x, _target.position.y + 10, _target.position.z - 10);
        Vector3 rotationCameraView = new Vector3(45, 0, 0);

        transform.position = positonCameraView;
        transform.eulerAngles = rotationCameraView;
    }
}
