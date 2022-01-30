using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileStatus
{
	Alive,
	Burning,
	Dead,
}

[CreateAssetMenu]
public class TileData : ScriptableObject
{
	public TileBase[] tiles;

	public float duration;
	public float hp;
	public TileStatus tileStatus;
}
