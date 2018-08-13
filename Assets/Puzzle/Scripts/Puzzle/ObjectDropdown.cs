using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDropdown : MonoBehaviour {

    public void OnValueChanged(int result)
    {
        Dropdown objectDropdown = GameObject.Find("ObjectDropdown").GetComponent<Dropdown>();
        bool writeBlock = true;
        BlockKind blockKind = BlockKind.Red;
        GarbageKind garbageKind = GarbageKind.None;
        switch(objectDropdown.value)
        {
            case 0:
                writeBlock = false;
                break;
            case 1:
                blockKind = BlockKind.Red;
                break;
            case 2:
                blockKind = BlockKind.Blue;
                break;
            case 3:
                blockKind = BlockKind.Green;
                break;
            case 4:
                blockKind = BlockKind.Yellow;
                break;
            case 5:
                blockKind = BlockKind.Purple;
                break;
            case 6:
                garbageKind = GarbageKind.Garbage;
                break;
            case 7:
                garbageKind = GarbageKind.Hard;
                break;
            case 8:
                blockKind = BlockKind.Red;
                garbageKind = GarbageKind.Dark;
                break;
            case 9:
                blockKind = BlockKind.Blue;
                garbageKind = GarbageKind.Dark;
                break;
            case 10:
                blockKind = BlockKind.Green;
                garbageKind = GarbageKind.Dark;
                break;
            case 11:
                blockKind = BlockKind.Yellow;
                garbageKind = GarbageKind.Dark;
                break;
            case 12:
                blockKind = BlockKind.Purple;
                garbageKind = GarbageKind.Dark;
                break;
        }

        PuzzleController puzzleController = GameObject.Find("PuzzleController").GetComponent<PuzzleController>();
        puzzleController.writeBlock = writeBlock;
        puzzleController.writeBlockKind = blockKind;
        puzzleController.writeGarbageKind = garbageKind;
    }
}
