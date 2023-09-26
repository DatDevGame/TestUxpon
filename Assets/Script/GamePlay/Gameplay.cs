using Sirenix.OdinInspector;
using Stateless;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class Gameplay : SceneSingleton<Gameplay>
{
    private const int QUANTITY_DEFAULT = 1;

    protected StateMachine<GamePlayState, GamePlayTrigger> StateMachine { get; set; }
    public ReactiveProperty<GamePlayState> PreviousState { get; private set; }
    public ReactiveProperty<GamePlayState> CurrentState { get; private set; }

    public void Fire(GamePlayTrigger trigger) => StateMachine.Fire(trigger);
    public bool CanFire(GamePlayTrigger trigger) => StateMachine.CanFire(trigger);

    public GameObject Tester { get; private set; }
    public GameSceneUI GameSceneUI => _gameSceneUI;
    private List<GameObject> _listComp;
    [SerializeField, TabGroup("Script")] private GameSceneUI _gameSceneUI;
    [SerializeField, TabGroup("Tranform")] private Transform _cameraView;
    [SerializeField, TabGroup("Tranform")] private Transform _map;
    [SerializeField, TabGroup("NetworkAPI")] private NetworkAPI _networkAPI;


    private void Awake()
    {
        SetUpState();
    }
    private void SetUpState()
    {
        CurrentState = new ReactiveProperty<GamePlayState>(GamePlayState.Init);
        PreviousState = new ReactiveProperty<GamePlayState>(GamePlayState.Init);

        StateMachine = new StateMachine<GamePlayState, GamePlayTrigger>(CurrentState.Value);

        StateMachine.OnTransitioned((transition) =>
        {
            CurrentState.Value = transition.Destination;
            PreviousState.Value = transition.Source;
        });

        StateMachine.Configure(GamePlayState.Init)
            .OnEntry(RemoveAllObject)
            .Permit(GamePlayTrigger.Play, GamePlayState.Running);

        StateMachine.Configure(GamePlayState.Running)
            .OnEntry(SetupGameplay)
            .Permit(GamePlayTrigger.Win, GamePlayState.Won)
            .Permit(GamePlayTrigger.Lose, GamePlayState.Lost)
            .Permit(GamePlayTrigger.Retry, GamePlayState.Init);

        StateMachine.Configure(GamePlayState.Won)
            .OnEntry(Win)
            .Permit(GamePlayTrigger.Retry, GamePlayState.Init);

        StateMachine.Configure(GamePlayState.Lost)
            .OnEntry(Lose)
            .Permit(GamePlayTrigger.Retry, GamePlayState.Init);

        StateMachine.Activate();
    }
    private void SetupGameplay()
    {
        Resource resource = GameManager.Instance.Resource;

        InitTester(resource);
        InitComp(resource);
    }
    private void InitTester(Resource resource)
    {
        Tester = Instantiate(resource.TesterPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity, _map);
        var cameraControl = _cameraView.GetOrAddComponent<CameraControl>();
        cameraControl.InitData(Tester.transform);

        _networkAPI.CreateUser(resource.TesterSetting.NameObject,
            resource.TesterSetting.MoveBehaviorSetting.Run,
            resource.TesterSetting.LiveBehaviorSetting.Hp);

        Debug.Log("Init Tester");
    }
    private void InitComp(Resource resource)
    {
        _listComp = new List<GameObject>();
        int quantityComp = QUANTITY_DEFAULT;

        if (!string.IsNullOrWhiteSpace(_gameSceneUI.InputField.text))
            quantityComp = int.Parse(_gameSceneUI.InputField.text);

        for (int i = 0; i < quantityComp; i++)
        {
            int randomNumberX = Random.Range(-30, 30);
            int randomNumberZ = Random.Range(-30, 30);

            Vector3 randomVector = new Vector3(randomNumberX, 0 , randomNumberZ);
            GameObject compPrefab = Instantiate(resource.CompPrefab, randomVector, Quaternion.identity, _map);
            _listComp.Add(compPrefab);
            Debug.Log("Init Comp: "+ (i + 1));
        }
    }
    private void Win()
    {
        Debug.Log("Win");
    }
    private void Lose()
    {
        _networkAPI.DeleteUser();
        Debug.Log("Lose");
    }
    private void RemoveAllObject()
    {
        for (int i = 0; i < _listComp.Count; i++)
            Destroy(_listComp[i]);

        if (Tester != null)
            Destroy(Tester.gameObject);

        _listComp.Clear();
    }
}
public enum GamePlayState
{
    Init,
    Lobby,
    Running,
    Won,
    Lost,
}

public enum GamePlayTrigger
{
    SetupGamePlay,
    Play,
    Win,
    Lose,
    Retry
}
