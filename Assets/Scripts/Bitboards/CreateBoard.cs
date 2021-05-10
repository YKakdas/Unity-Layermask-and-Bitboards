using TMPro;
using UnityEngine;

public class CreateBoard : MonoBehaviour {
	public GameObject[] tilePrefabs;
	public GameObject housePrefab;
	public GameObject treePrefab;
	public TMP_Text text;

	private GameObject[] addedTiles;
	private long dirtBitboard = 0;
	private long desertBitboard = 0;
	private long treeBitboard = 0;
	private long playerBitboard = 0;
	private BitboardUtil bitboardUtil;
	private int penalty = 0;

	void Start() {
		addedTiles = new GameObject[64];
		bitboardUtil = (BitboardUtil)ScriptableObject.CreateInstance("BitboardUtil");
		for(int row = 0; row < BitboardUtil.NUMBER_OF_ROWS; row++) {
			for(int column = 0; column < BitboardUtil.NUMBER_OF_COLUMNS; column++) {
				int randomTile = UnityEngine.Random.Range(0 , tilePrefabs.Length);
				Vector3 tilePosition = new Vector3(row , 0f , column);
				GameObject tile = Instantiate(tilePrefabs[randomTile] , tilePosition , Quaternion.identity);
				tile.name = tile.tag + "_" + row + "_" + column;
				addedTiles[row * BitboardUtil.NUMBER_OF_ROWS + column] = tile;
				if(tile.tag == "Dirt") {
					dirtBitboard = bitboardUtil.SetCellState(dirtBitboard , row , column);
				} else if(tile.tag == "Desert") {
					desertBitboard = bitboardUtil.SetCellState(desertBitboard , row , column);
				}
			}
		}
		InvokeRepeating("PlantTreeRandomly" , 1 , 0.1f);
	}

	void Update() {
		plantHouseOnClick();
	}

	private void PlantTreeRandomly() {
		int randomTileRow = Random.Range(0 , BitboardUtil.NUMBER_OF_ROWS);
		int randomTileColumn = Random.Range(0 , BitboardUtil.NUMBER_OF_COLUMNS);
		GameObject tile = addedTiles[randomTileRow * BitboardUtil.NUMBER_OF_ROWS + randomTileColumn];
		bool isDirtAvailable = isDirtAndAvailable(randomTileRow , randomTileColumn);
		bool isDesertAvailable = isDesertAndAvailable(randomTileRow , randomTileColumn);
		if(isDirtAvailable || isDesertAvailable) {
			GameObject tree = Instantiate(treePrefab);
			tree.name = "Tree" + "_" + randomTileRow + "_" + randomTileColumn;
			tree.transform.parent = tile.transform;
			tree.transform.localPosition = Vector3.zero;
			treeBitboard = bitboardUtil.SetCellState(treeBitboard , randomTileRow , randomTileColumn);
		}
	}

	private void plantHouseOnClick() {
		bool isAddition = false;
		if(Input.GetMouseButtonDown(0)) {
			isAddition = true;
		} else if(Input.GetMouseButtonDown(1)) {
			isAddition = false;
		} else {
			return;
		}
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray , out hit)) {
			Transform hitTransform = hit.collider.gameObject.transform;
			int hitPosition = ((int)hitTransform.position.x) * BitboardUtil.NUMBER_OF_ROWS + (int)hitTransform.position.z;
			if(isAddition) {
				if(isDirtAndAvailable(hitPosition) || isDesertAndAvailable(hitPosition)) {
					InstantiateHouse(hitTransform , (int)hitTransform.position.x , (int)hitTransform.position.z);
				} else {
					penalty -= 2;
				}
			} else {
				if(isHousePlantedOnDirt(hitPosition) || isHousePlantedOnDesert(hitPosition)) {
					foreach(Transform child in hitTransform) {
						if(child.tag == "House") {
							Destroy(child.gameObject);
							playerBitboard = bitboardUtil.RemoveCellState(playerBitboard , hitPosition);
						}
					}
				}
			}
			UpdateScore();
		}
	}

	private void InstantiateHouse(Transform parent , int row , int column) {
		GameObject house = Instantiate(housePrefab);
		house.name = "House" + "_" + row + "_" + column;
		house.transform.parent = parent;
		house.transform.localPosition = Vector3.zero;
		playerBitboard = bitboardUtil.SetCellState(playerBitboard , row * BitboardUtil.NUMBER_OF_ROWS + column);
	}

	private void UpdateScore() {
		int totalScore = bitboardUtil.GetCellCount(playerBitboard & dirtBitboard) * 10 + bitboardUtil.GetCellCount(playerBitboard & desertBitboard) * 5;
		totalScore += penalty;
		text.text = "SCORE: " + totalScore;
	}

	private bool isDirtAndAvailable(int row , int column) {
		return bitboardUtil.GetCellState(dirtBitboard & ~playerBitboard & ~treeBitboard , row , column);
	}

	private bool isDesertAndAvailable(int row , int column) {
		return bitboardUtil.GetCellState(desertBitboard & ~playerBitboard & ~treeBitboard , row , column);
	}

	private bool isDirtAndAvailable(int position) {
		return bitboardUtil.GetCellState(dirtBitboard & ~playerBitboard & ~treeBitboard , position);
	}

	private bool isDesertAndAvailable(int position) {
		return bitboardUtil.GetCellState(desertBitboard & ~playerBitboard & ~treeBitboard , position);
	}

	private bool isHousePlantedOnDirt(int position) {
		return bitboardUtil.GetCellState(dirtBitboard & playerBitboard , position);
	}

	private bool isHousePlantedOnDesert(int position) {
		return bitboardUtil.GetCellState(desertBitboard & playerBitboard , position);
	}

}