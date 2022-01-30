using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("UI References")]
	public MapManager mapManager;
	public BurnManager burnManager;
	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("Game Manager is null");
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}
}
