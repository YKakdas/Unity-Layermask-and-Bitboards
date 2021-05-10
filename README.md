Unity Trials

1. Layer Mask - Raycast  
    - https://docs.unity3d.com/ScriptReference/LayerMask.html
    - Specifies Layers to use in a Physics.Raycast.A GameObject can use up to 32 LayerMasks supported by the Editor. The first 8 of these Layers are specified by Unity; the following 24 are controllable by the user.Bitmasks represent the 32 Layers and define them as true or false. Each bitmask describes whether the Layer is used. As an example, bit 5 can be set to 1 (true). This will allow the use of the built-in Water setting.

2. Bitboards  
    - https://en.wikipedia.org/wiki/Bitboard
    - https://www.chessprogramming.org/Bitboards
    - A bit board is a data structure designed to efficienlt encode a game board state as a set of bits.(Chess board) Instead of using a bunch of individual integers to represent the state of each board position we can use a bit.
    - Birboards have 3 main advantages:
        - Encode state more efficiently
        - Read and write with bitwise operations
        - Bitwise operations run parallel.  

    - We need different bit boards for different states because bitboards only can hold 0 or 1.
We can hold bitboard for black cells and put 1s for them, then we can make another bitboard for whites and put 1s for white cells. If we have 2 players we will need two more boards for their states. Lets say blue and red players. If we want to check which black squares that red player uses, just make black & red bitboards. Red & blue will give empty cells. Since we represent the bitboards as 2d but actually stays as 1d in memory, we can apply some rules for finding right indexes.
    - index = row * width + column
    - number[row][column] = number[row * width + column]
    - For instance, if I want to set 1st row 5th column bit, actually I want to set 13th bit of bit sequence. So, to set that bit, make new bit array then left shift it by 13 so that I will have 1000000000000. Then |(or) this with actual bitboard. That will make 13th bit 1. If I want to retrieve the value of bit, do the same operations and instead of | make &. If result != 0, then that bit is 1.
    - About Bitboard Gameplay:
        - Player only can construct houses on dirt and desert tiles.
        - Player gets 10 points for each dirt house buildings and 5 for desert.
        - Player will get -2 penalty if tries to build house other than on dirt and desert.
        - Player can build house with left mouse click.
        - Player can deconstruct house with right mouse click.
        - Game will spawn random trees on dirts and deserts.
        - Player will not able to put houses if area occupied with tree.
        - Goal is to earn as much as points before game spawn trees.

NOTES: 
- Quaternion.identity: This quaternion corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes.
- Random.Range: if values float, max value is inclusive otherwise exclusive. 
