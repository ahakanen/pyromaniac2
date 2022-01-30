using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BurnManager : MonoBehaviour
{
	[SerializeField] Tilemap hitpointsMap;
	Dictionary<Vector3Int, float> tileHitpoints = new Dictionary<Vector3Int, float>();
	Dictionary<Vector3Int, TileStatus> tileStatuses = new Dictionary<Vector3Int, TileStatus>();
	Dictionary<Vector3Int, float> tileBurnDurations = new Dictionary<Vector3Int, float>();
	[SerializeField] Color burnedColor = Color.black;

	public void ChangeHitpoints(Vector3Int gridPosition, float changeBy)
	{
		TileBase tile = GameManager.Instance.mapManager.map.GetTile(gridPosition);
		if (tile == null)
		{
			return;
		}
		if (!tileHitpoints.ContainsKey(gridPosition))
		{
			SetHitpoints(gridPosition, GameManager.Instance.mapManager.dataFromTiles[tile].hp);
		}
		tileHitpoints[gridPosition] = tileHitpoints[gridPosition] + changeBy;

		// start burning if hp reaches 0

		if (tileHitpoints[gridPosition] <= 0)
		{
			if (!tileStatuses.ContainsKey(gridPosition))
			{
				tileStatuses[gridPosition] = TileStatus.Burning;
				tileBurnDurations[gridPosition] = GameManager.Instance.mapManager.dataFromTiles[tile].duration;
			}
		}
	}

	public void SetHitpoints(Vector3Int gridPosition, float SetTo)
	{
		tileHitpoints[gridPosition] = SetTo;
	}

	public float GetHitpoints(Vector3Int gridPosition)
	{
		if (!tileHitpoints.ContainsKey(gridPosition))
		{
			TileBase tile = GameManager.Instance.mapManager.map.GetTile(gridPosition);
			return(GameManager.Instance.mapManager.dataFromTiles[tile].hp);
		}
		return (tileHitpoints[gridPosition]);
	}

	public float GetBurnTimer(Vector3Int gridPosition)
	{
		if (!tileBurnDurations.ContainsKey(gridPosition))
		{
			Debug.Log("Tile is not burning yet btw!");
			TileBase tile = GameManager.Instance.mapManager.map.GetTile(gridPosition);
			return (GameManager.Instance.mapManager.dataFromTiles[tile].duration);
		}
		return (tileBurnDurations[gridPosition]);
	}

	public TileStatus GetTileStatus(Vector3Int gridPosition)
	{
		if (!tileStatuses.ContainsKey(gridPosition))
		{
			TileBase tile = GameManager.Instance.mapManager.map.GetTile(gridPosition);
			return (GameManager.Instance.mapManager.dataFromTiles[tile].tileStatus);
		}
		return (tileStatuses[gridPosition]);
	}

	public void VisualizeBurn()
	{
		foreach (var entry in tileHitpoints)
		{
			if (entry.Value <= 0f)
			{
				hitpointsMap.SetTileFlags(entry.Key, TileFlags.None);
				hitpointsMap.SetColor(entry.Key, burnedColor);
				hitpointsMap.SetTileFlags(entry.Key, TileFlags.LockColor);
			}
		}
	}

	public void UpdateBurnDurations()
	{
		// some magic to iterate through burning tiles directory safely
		List<Vector3Int> keys = new List<Vector3Int>();
		foreach (var entry in tileBurnDurations)
		{
			keys.Add(entry.Key);
			
		}
		for (int i = 0; i < keys.Count; i++)
		{
			tileBurnDurations[keys[i]] = tileBurnDurations[keys[i]] - 1;
			if (tileBurnDurations[keys[i]] < 0) // if fire burns out
			{
				tileBurnDurations.Remove(keys[i]);
				tileStatuses[keys[i]] = TileStatus.Dead;
			}
		}
		UpdateBurnSpreadDamage(keys);
	}

	public void UpdateBurnSpreadDamage(List<Vector3Int> keys)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			DamageAdjacentTiles(keys[i]);
		}
	}

	public void DamageAdjacentTiles(Vector3Int gridLocation)
	{
		Vector3Int left = gridLocation;
		left.x = left.x - 1;
		Vector3Int right = gridLocation;
		right.x = right.x + 1;
		Vector3Int above = gridLocation;
		above.y = above.y + 1;
		Vector3Int below = gridLocation;
		below.y = below.y - 1;
		ChangeHitpoints(left, -1);
		ChangeHitpoints(right, -1);
		ChangeHitpoints(above, -1);
		ChangeHitpoints(below, -1);
	}
}
