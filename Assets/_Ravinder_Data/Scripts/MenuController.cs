using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Button startNewGameButton;
	[SerializeField] private Button loadGameButton;

	private void Start()
	{
		//Assigning the required references.
		if (startNewGameButton) startNewGameButton.onClick.AddListener(OnClickStartNewGameButton);
		else Debug.LogError("startNewGameButton reference is missing.");
		if (loadGameButton) loadGameButton.onClick.AddListener(OnClickLoadGameButton);
		else Debug.LogError("loadGameButton reference is missing.");

		//Resetting required data.
		SaveManager.ResetMembers();
	}

	public void OnClickStartNewGameButton()
	{
		SceneManager.LoadScene("Level 01");
	}

	public void OnClickLoadGameButton()
	{
		if (PlayerPrefs.HasKey(SaveManager.Save_Data_Key))
		{
			SaveManager.loadSavedGame = true;
			SceneManager.LoadScene("Level 01");
		}
		else
		{
			Utilities.ShowMessage(Messages.NoOldGameFound);
		}
	}
}
