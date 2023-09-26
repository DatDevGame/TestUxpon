using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    public TMP_InputField InputField => _quantityCompInputField;

    private CompositeDisposable _disposable;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _testerHealthPointText;
    [SerializeField] private TMP_InputField _quantityCompInputField;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _retryButton;
    private void Awake()
    {
        _disposable = new CompositeDisposable();
    }
    private void Start()
    {
        Subcribe();
        SetEvent();
    }
    public void UpdateHealthPointTester(int hp)
    {
        _testerHealthPointText.SetText("Tester Hp: "+hp);
    }
    private void Subcribe()
    {
        Gameplay.Instance.CurrentState
            .Subscribe(state =>
            {
                bool isActive = state == GamePlayState.Init;
                _quantityCompInputField.gameObject.SetActive(isActive);
                _quantityText.gameObject.SetActive(isActive);
                _playButton.gameObject.SetActive(isActive);
            }).AddTo(_disposable);

        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Running)
            .Subscribe(state =>
            {
                _retryButton.gameObject.SetActive(true);
                _testerHealthPointText.gameObject.SetActive(true);
            }).AddTo(_disposable);
    }
    private void SetEvent()
    {
        _playButton.onClick.AddListener(() => Gameplay.Instance.Fire(GamePlayTrigger.Play));
        _retryButton.onClick.AddListener(() => 
        {
            Gameplay.Instance.Fire(GamePlayTrigger.Retry);
            _retryButton.gameObject.SetActive(false);
            _testerHealthPointText.gameObject.SetActive(false);
        });
    }
}
