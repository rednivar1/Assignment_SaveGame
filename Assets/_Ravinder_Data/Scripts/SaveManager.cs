using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaveManager_Exclusives;

public class SaveManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform player = default;
	[SerializeField] private Button saveButton = default;
	[Space]
	[SerializeField] private GameObject zombunny = default;
	[SerializeField] private GameObject zombear = default;
	[SerializeField] private GameObject hellephant = default;

	//Other variables.
	PlayerHealth playerHealth => player.GetComponent<PlayerHealth>();
	public static bool loadSavedGame = false;
	public const string Save_Data_Key = "SAVE_DATA";

	private void Start()
	{
		//Assigning the required references.
		if (saveButton) saveButton.onClick.AddListener(OnClickSaveButton);
		else Debug.LogError("saveButton reference is missing.");
		player = GameObject.FindGameObjectWithTag("Player").transform;

		//Check for a saved game.
		if (loadSavedGame) LoadSavedGame();
	}

	public void OnClickSaveButton()
	{
		Debug.Log("Saving progress...");

		//Return the control if player health is 0 or he already died.
		if (playerHealth.CurrentHealth <= 0 && !playerHealth.isDead) return;

		//Stop the time and save the game.
		Time.timeScale = 0;
		SaveGame();
	}

	public void SaveGame()
	{
		//Storing all the required values into an object.
		GameState currentState;
		currentState.score = ScoreManager.score;
		currentState.playerHealth = playerHealth.CurrentHealth;
		currentState.playerPosition = new ObjectData(player.name, player.position.x, player.position.z, player.rotation.eulerAngles.y);
		EnemyMovement[] allEnemies = GameObject.FindObjectsOfType<EnemyMovement>();
		currentState.enemyPositions = new List<ObjectData>();
		for (int i = 0; i < allEnemies.Length; i++)
		{
			Transform enemyTransform = allEnemies[i].transform;
			currentState.enemyPositions.Add(new ObjectData(allEnemies[i].name, enemyTransform.position.x, enemyTransform.position.z, enemyTransform.rotation.eulerAngles.y));
		}

		//Converting object into JSON format.
		string jsonObject = JsonUtility.ToJson(currentState);

		//Saving the JSON into local storage using PlayerPrefs.
		PlayerPrefs.SetString(Save_Data_Key, jsonObject);

		//Showing a message after saving the game.
		Utilities.ShowMessage(Messages.GameSaved, () => Time.timeScale = 1);
	}

	public void LoadSavedGame()
	{
		Debug.Log("Loading old game...");

		//Loading the old game state from playerPrefs to GameState struct.
		GameState savedGameState = JsonUtility.FromJson<GameState>(PlayerPrefs.GetString(SaveManager.Save_Data_Key));

		ScoreManager.score = savedGameState.score;
		playerHealth.ForceSetHealth(savedGameState.playerHealth);
		player.position = new Vector3(savedGameState.playerPosition.xPosition, player.position.y, savedGameState.playerPosition.zPosition);

		for (int i = 0; i < savedGameState.enemyPositions.Count; i++)
		{
			Transform enemy = null;
			if (savedGameState.enemyPositions[i].name.StartsWith("Zombunny")) enemy = Instantiate(zombunny).GetComponent<Transform>();
			else if (savedGameState.enemyPositions[i].name.StartsWith("Zombear")) enemy = Instantiate(zombear).GetComponent<Transform>();
			else if(savedGameState.enemyPositions[i].name.StartsWith("Hellephant")) enemy = Instantiate(hellephant).GetComponent<Transform>();

			if(enemy){
				enemy.position = new Vector3(savedGameState.enemyPositions[i].xPosition, 0, savedGameState.enemyPositions[i].zPosition);
				enemy.rotation = Quaternion.Euler(0, savedGameState.enemyPositions[i].yRotation, 0);
			}
		}
	}

	public static void ResetMembers()
	{
		loadSavedGame = false;
	}
}

namespace SaveManager_Exclusives
{
	[System.Serializable]
	public struct GameState
	{
		public int score;
		public int playerHealth;
		public ObjectData playerPosition;
		public List<ObjectData> enemyPositions;
	}

	[System.Serializable]
	public struct ObjectData
	{
		public string name;
		public float xPosition, zPosition;
		public float yRotation;

		public ObjectData(string newName, float newXPosition, float newZPosition, float newYRotation)
		{
			name = newName;
			xPosition = newXPosition;
			zPosition = newZPosition;
			yRotation = newYRotation;
		}
	}
}