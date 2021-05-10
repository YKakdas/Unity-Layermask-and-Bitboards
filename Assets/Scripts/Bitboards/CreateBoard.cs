using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour {

	public GameObject[] tiles;
	public GameObject house;
	public Text score;
	private const int NUMBER_OF_ROWS = 8;
	private const int NUMBER_OF_COLUMNS = 8;

	void Start() {
		for(int row = 0; row < NUMBER_OF_ROWS; row++) {
			for(int column = 0; column < NUMBER_OF_COLUMNS; column++) {
				int randomTile = Random.Range(0 , tiles.Length);
				Vector3 tilePosition = new Vector3(column , 0f , row);
				GameObject tile = Instantiate(tiles[randomTile] , tilePosition , Quaternion.identity);
				tile.name = tile.tag + "_" + row + "_" + column;
			}
		}
	}

	// Update is called once per frame
	void Update() {

	}
}
