using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
	public Tilemap map;
	public List<TileData> tileDataList;
	public Dictionary<TileBase, TileData> dataFromTiles;
	[SerializeField] float originalTimer = 60f;
	float timer;

	private void Awake()
	{
		dataFromTiles = new Dictionary<TileBase, TileData>();

		foreach (TileData tileData in tileDataList)
		{
			foreach (TileBase tile in tileData.tiles)
			{
				dataFromTiles.Add(tile, tileData);
			}
		}
		timer = originalTimer;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int gridPosition = map.WorldToCell(mousePosition);

			TileBase clickedTile = map.GetTile(gridPosition);

			float duration = dataFromTiles[clickedTile].duration;
			float hp = dataFromTiles[clickedTile].hp;
			TileStatus tileStatus = dataFromTiles[clickedTile].tileStatus;

			Debug.Log("starting tiledata at " + gridPosition + " flammability: " + hp + " duration: " + duration + " tileStatus: " + tileStatus);
			Debug.Log("Current hp at tile: " + GameManager.Instance.burnManager.GetHitpoints(gridPosition));
			Debug.Log("Current duration at tile: " + GameManager.Instance.burnManager.GetBurnTimer(gridPosition));
			Debug.Log("Current status at tile: " + GameManager.Instance.burnManager.GetTileStatus(gridPosition));
		}
		if (Input.GetMouseButtonDown(1))
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int gridPosition = map.WorldToCell(mousePosition);

			TileBase clickedTile = map.GetTile(gridPosition);

			float duration = dataFromTiles[clickedTile].duration;
			float hp = dataFromTiles[clickedTile].hp;
			TileStatus tileStatus = dataFromTiles[clickedTile].tileStatus;

			GameManager.Instance.burnManager.ChangeHitpoints(gridPosition, -1);
			GameManager.Instance.burnManager.VisualizeBurn();
			Debug.Log("Current hp at tile: " + GameManager.Instance.burnManager.GetHitpoints(gridPosition));
			Debug.Log("Current duration at tile: " + GameManager.Instance.burnManager.GetBurnTimer(gridPosition));
			Debug.Log("Current status at tile: " + GameManager.Instance.burnManager.GetTileStatus(gridPosition));
		}
		// timer to deal with burn timers
		timer--;
		if (timer < 0)
		{
			GameManager.Instance.burnManager.UpdateBurnDurations();
			timer = originalTimer;
			GameManager.Instance.burnManager.VisualizeBurn();
		}
	}
}
