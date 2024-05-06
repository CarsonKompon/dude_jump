using System;
using System.Collections.Generic;
using Sandbox;

public sealed class DudeGameManager : Component
{
	public static DudeGameManager Instance { get; private set; }
	[Property] public DudeJumpPlayer Player { get; set; }
	[Property] public DudeCamera Camera { get; set; }
	[Property] public int ScoreSpawnInterval { get; set; } = 10;
	[Property] public float SpawnAhead { get; set; } = 100;
	[Property] GameObject PlatformPrefab { get; set; }
	[Property] List<GameObject> OtherPrefabs { get; set; }

	public bool IsPlaying { get; private set; } = false;
	[Property] public int Score { get; set; } = 0;
	int lastScoreSpawn = 0;

	protected override void OnAwake()
	{
		Instance = this;
	}

	protected override void OnStart()
	{
		for (int i = 0; i < 4; i++)
		{
			SpawnPlatformAt(50 + i * 80);
		}
		StartGame();
	}

	protected override void OnUpdate()
	{
		Log.Info(IsPlaying);

		if (IsPlaying)
		{
			Score = Math.Max((int)(Player.Transform.Position.z / 10f), Score);

			if (Score > lastScoreSpawn + ScoreSpawnInterval)
			{
				lastScoreSpawn += ScoreSpawnInterval;

				SpawnPlatformAt(Player.Transform.Position.z + SpawnAhead);
			}
		}
		else
		{
			if (Input.Pressed("Jump"))
			{
				Game.ActiveScene.LoadFromFile("scenes/game.scene");
			}
		}
	}

	public void StartGame()
	{
		if (IsPlaying) return;

		Score = 0;
		lastScoreSpawn = 0;
		IsPlaying = true;
	}

	public void EndGame()
	{
		if (!IsPlaying) return;

		IsPlaying = false;
		Sandbox.Services.Stats.SetValue("highscore", Score);
	}

	void SpawnPlatformAt(float height)
	{
		var platform = SceneUtility.Instantiate(PlatformPrefab);
		platform.Transform.Position = new Vector3(0, Random.Shared.Float(-140f, 140f), height);
		platform.Transform.Scale = platform.Transform.Scale.WithY(Random.Shared.Float(0.7f, 1.1f));

		if (Random.Shared.Float() < 0.3f)
		{
			var others = new List<GameObject>();
			others.Add(PlatformPrefab);
			others.AddRange(OtherPrefabs);
			var prefab = others[Random.Shared.Next(0, others.Count)];
			var obj = SceneUtility.Instantiate(prefab);
			obj.Transform.Position = new Vector3(0, Random.Shared.Float(-140f, 140f), height + Random.Shared.Float(-60f, 60f));
		}
	}
}