using System;
using UnityEngine;

public class BitboardUtil : ScriptableObject {
	public const int NUMBER_OF_ROWS = 8;
	public const int NUMBER_OF_COLUMNS = 8;

	public long SetCellState(long bitboard , int row , int column) {
		long newBit = 1L << (row * NUMBER_OF_ROWS + column);
		return (bitboard | newBit);
	}

	public long SetCellState(long bitboard , int position) {
		long newBit = 1L << position;
		return (bitboard | newBit);
	}

	public bool GetCellState(long bitboard , int row , int column) {
		long mask = 1L << (row * NUMBER_OF_ROWS + column);
		return ((bitboard & mask) != 0);
	}

	public bool GetCellState(long bitboard , int position) {
		long mask = 1L << position;
		return ((bitboard & mask) != 0);
	}

	public long RemoveCellState(long bitboard , int position) {
		long mask = ~(1L << position);
		return (bitboard & mask);
	}

	public int GetCellCount(long bitboard) {
		int count = 0;
		while(bitboard != 0) {
			bitboard &= bitboard - 1;
			count++;
		}
		return count;
	}
	private void printBitboard(long bitboard) {
		Debug.Log("Cell count is: " + GetCellCount(bitboard));
		Debug.Log(Convert.ToString(bitboard , 2).PadLeft(64 , '0'));
	}
}
