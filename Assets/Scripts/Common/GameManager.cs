using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager> {
	[SerializeField] IntVariable lives;
	[SerializeField] IntVariable score;
	[SerializeField] FloatVariable health;
	[SerializeField] FloatVariable timer;

	[SerializeField] GameObject respawn;
	[SerializeField] GameObject player;
	[SerializeField] GameObject boss;
	[SerializeField] GameObject mover;

	[Header("Events")]
	[SerializeField] VoidEvent gameStartEvent;
	[SerializeField] VoidEvent respawnEvent;

	public enum State {
		TITLE,
		START_GAME,
		PLAY_GAME,
		GAME_OVER,
		GAME_WON
	}
	private State state = State.TITLE;

	public void OnEnable() {
		respawnEvent.Subscribe(Respawn);
	}

	void Update() {
		switch (state) {
			case State.TITLE:
				UIManager.Instance.SetActive("Title", true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				break;
			case State.START_GAME:
				UIManager.Instance.SetActive("Title", false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				// reset values
				timer.value = 300;
				lives.value = 5;
				health.value = 100;
				score.value = 0;

				mover.SetActive(true);
				player.SetActive(true);
				boss.SetActive(true);

				gameStartEvent.RaiseEvent();

				state = State.PLAY_GAME;
				break;
			case State.PLAY_GAME:
				UIManager.Instance.SetActive("Play", true);
				// game timer
				timer.value = timer - Time.deltaTime;
				score.value += (int)Time.deltaTime;
				if (timer <= 0 || lives.value <= 0) {
					state = State.GAME_OVER;
				}
				break;
			case State.GAME_OVER:
				mover.SetActive(false);
				player.SetActive(false);
				boss.SetActive(false);
				UIManager.Instance.SetActive("Play", false);
				UIManager.Instance.SetActive("GameOver", true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				break;
			case State.GAME_WON:
				mover.SetActive(false);
				player.SetActive(false);
				boss.SetActive(false);
				UIManager.Instance.SetActive("Play", false);
				UIManager.Instance.SetActive("GameWon", true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				break;
			default:
				break;
		}

		UIManager.Instance.Health = health;
		UIManager.Instance.Timer = timer;
		UIManager.Instance.Score = score;
		UIManager.Instance.Lives = lives;
	}

	public void OnStartGame() {
		state = State.START_GAME;
	}

	public void EndGame() { 
		Application.Quit();
	}

	public void Respawn() { 
		player.transform.position = respawn.transform.position;
		timer.value -= 50;
	}
}