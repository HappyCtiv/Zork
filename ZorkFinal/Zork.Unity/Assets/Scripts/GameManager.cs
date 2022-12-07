using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private TextMeshProUGUI HealthText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    [SerializeField]
    private ScrollRect scrollRec;

    [SerializeField]
    private GameObject viewPort;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChange += Player_LocationChange;
        _game.Player.MoveChange += Player_MoveChange;
        _game.Player.ScoreChange += Player_ScoreChange;
        _game.Player.HealthChange += Player_HealthChange;
        _game.Run(InputService, OutputService);
    }

    private void Player_HealthChange(object sender, int health)
    {
        HealthText.text = $"HP: {health.ToString()}";
    }

    private void Player_ScoreChange(object sender, int score)
    {
        ScoreText.text = $"Score: {score.ToString()}";
    }
    private void Player_MoveChange(object sender, int move)
    {
        MovesText.text = $"Moves: {move.ToString()}";
    }

    private void Player_LocationChange(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private void Start()
    {
        InputService.SetFocus();
        LocationText.text = _game.Player.CurrentRoom.Name;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
            var pos = viewPort.transform.position;
            scrollRec.content.localPosition = pos;
        }


        if (_game.IsRunning == false) 
        { 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }


    private Game _game;
}
