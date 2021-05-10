using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour {
	public GameObject[] tilePrefabs;
	public GameObject housePrefab;
	public GameObject treePrefab;
	public Text score;

	private GameObject[] addedTiles;
	private const int NUMBER_OF_ROWS = 8;
	private const int NUMBER_OF_COLUMNS = 8;
	private long dirtBitboard = 0;
	private long treeBitboard = 0;
	private long playerBitboard = 0;

	void Start() {
		addedTiles = new GameObject[64];
		for(int row = 0; row < NUMBER_OF_ROWS; row++) {
			for(int column = 0; column < NUMBER_OF_COLUMNS; column++) {
				int randomTile = UnityEngine.Random.Range(0 , tilePrefabs.Length);
				Vector3 tilePosition = new Vector3(row , 0f , column);
				GameObject tile = Instantiate(tilePrefabs[randomTile] , tilePosition , Quaternion.identity);
				tile.name = tile.tag + "_" + row + "_" + column;
				addedTiles[row * NUMBER_OF_ROWS + column] = tile;
				if(tile.tag == "Dirt") {
					dirtBitboard = SetCellState(dirtBitboard , row , column);
				}
			}
		}
		InvokeRepeating("PlantTreeRandomly" , 1 , 0.1f);
	}

	void Update() {
		plantHouseOnClick();
	}

	private long SetCellState(long bitboard , int row , int column) {
		long newBit = 1L << (row * NUMBER_OF_ROWS + column);
		return (bitboard | newBit);
	}

	private long SetCellState(long bitboard , int position) {
		long newBit = 1L << position;
		return (bitboard | newBit);
	}

	private bool GetCellState(long bitboard , int row , int column) {
		long mask = 1L << (row * NUMBER_OF_ROWS + column);
		return ((bitboard & mask) != 0);
	}

	private bool GetCellState(long bitboard , int position) {
		long mask = 1L << position;
		return ((bitboard & mask) != 0);
	}

	private int GetCellCount(long bitboard) {
		int count = 0;
		while(bitboard != 0) {
			bitboard &= bitboard - 1;
			count++;
		}
		return count;
	}

	private void PlantTreeRandomly() {
		int randomTileRow = UnityEngine.Random.Range(0 , NUMBER_OF_ROWS);
		int randomTileColumn = UnityEngine.Random.Range(0 , NUMBER_OF_COLUMNS);
		GameObject tile = addedTiles[randomTileRow * NUMBER_OF_ROWS + randomTileColumn];
		if(GetCellState(dirtBitboard & ~playerBitboard & ~treeBitboard , randomTileRow , randomTileColumn)) {
			GameObject tree = Instantiate(treePrefab);
			tree.name = "Tree" + "_" + randomTileRow + "_" + randomTileColumn;
			tree.transform.parent = tile.transform;
			tree.transform.localPosition = Vector3.zero;
			treeBitboard = SetCellState(treeBitboard , randomTileRow , randomTileColumn);
		}
	}

	private void plantHouseOnClick() {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray , out hit)) {
				Transform hitTransform = hit.collider.gameObject.transform;
				int hitPosition = ((int)hitTransform.position.x) * NUMBER_OF_ROWS + (int)hitTransform.position.z;
				if(GetCellState(dirtBitboard & ~treeBitboard & ~playerBitboard , hitPosition)) {
					GameObject house = Instantiate(housePrefab);
					house.name = "House" + "_" + (int)hitTransform.position.x + "_" + (int)hitTransform.position.z;
					house.transform.parent = hitTransform;
					house.transform.localPosition = Vector3.zero;
					playerBitboard = SetCellState(playerBitboard , hitPosition);
				}
			}
		}
	}

	private void printBitboard(long bitboard) {
		Debug.Log("Cell count is: " + GetCellCount(bitboard));
		Debug.Log(Convert.ToString(bitboard , 2).PadLeft(64 , '0'));
	}

}