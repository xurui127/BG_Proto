using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{

    internal bool isPlacedItem = false;
    [SerializeField] internal bool isPlacedFruit = false;
    [SerializeField] internal bool isPlacedPot = false;
    [SerializeField] internal bool isPlacedCharacter = false;
    [SerializeField] internal bool isPlacedTrap = false;
    [SerializeField] internal ItemBehaviour itemBehaviour;

    List<CharacterBehaviour> characterOnTile = new();

    readonly Vector3 yOffset = new(0, 0.4f, 0);

    private void OnDrawGizmos()
    {
        DrawTilePosition();
    }

    private void DrawTilePosition()
    {
        var pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pos, 0.1f);
    }

    internal void PlacedCharacter() => isPlacedCharacter = true;

    internal void PlacedItem() => isPlacedItem = true;

    internal void PlacedFruit() => isPlacedFruit = true;

    internal void PlacedPot() => isPlacedPot = true;

    internal void PlacedTrap() => isPlacedTrap = true;

    internal void SetCurrentBehaviour(ItemBehaviour behaviour) => itemBehaviour = behaviour;

    internal ItemBehaviour GetCurrentItemBehaviour()
    {
        if (itemBehaviour != null)
        {
            return itemBehaviour;
        }
        return null;
    }

    internal void ResetTilePlacedFruit(bool isReset)
    {
        if (isReset)
        {
            isPlacedFruit = false;
        }
    }

    internal void ResetTilePlacedCharacter() => isPlacedCharacter = false;

    internal void ResetTilePlacedTrap() => isPlacedTrap = false;

    internal void RegisterCharacter(CharacterBehaviour character)
    {
        if (!characterOnTile.Contains(character))
        {
            characterOnTile.Add(character);
        }
        UpdateCharacterOffsetOnTile();
    }

    internal void UnregisterCharacter(CharacterBehaviour character)
    {
        if (characterOnTile.Contains(character))
        {
            characterOnTile.Remove(character);
        }
        UpdateCharacterOffsetOnTile();
    }

    private void UpdateCharacterOffsetOnTile()
    {
        int count = characterOnTile.Count;
        if (count == 0) return;

        float spacing = 0.3f;

        Vector3 center = transform.position;

        Vector3 forward = characterOnTile[0].transform.forward;
        Vector3 right = characterOnTile[0].transform.right;

        Vector3 offsetForward = forward * spacing;
        Vector3 offsetRight = right * spacing;

        switch (count)
        {
            case 1:
                characterOnTile[0].SetOffset(center + yOffset);
                break;

            case 2:
                characterOnTile[0].SetOffset(center - offsetRight + yOffset); // left
                characterOnTile[1].SetOffset(center + offsetRight + yOffset); // right
                break;

            case 3:
                characterOnTile[0].SetOffset(center - offsetRight + offsetForward + yOffset);  // leftup
                characterOnTile[1].SetOffset(center + offsetRight + offsetForward + yOffset);  // rightup
                characterOnTile[2].SetOffset(center - offsetRight - offsetForward + yOffset);  // leftdown
                break;

            case 4:
                characterOnTile[0].SetOffset(center - offsetRight + offsetForward + yOffset);  // leftup
                characterOnTile[1].SetOffset(center + offsetRight + offsetForward + yOffset);  // rightup
                characterOnTile[2].SetOffset(center - offsetRight - offsetForward + yOffset);  // leftdown
                characterOnTile[3].SetOffset(center + offsetRight - offsetForward + yOffset);  // rightdown
                break;

            default:
                for (int i = 0; i < count; i++)
                    characterOnTile[i].SetOffset(center);
                break;
        }
    }
}
