using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    // Set the boundaries of the screen
    private float screenWidth;
    private float screenHeight;

    // The object to be moved
    private Transform objectToMove;

    // Start is called before the first frame update
    void Start()
    {
        // Get the screen dimensions
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Get the object to move
        objectToMove = GetComponent<Transform>();

        // Call the function to move the object
        MoveObject();
    }

    // Function to move the object to a random position within the screen view
    void MoveObject()
    {
        // Set a random position within the screen view
        float randomX = Random.Range(0, screenWidth);
        float randomY = Random.Range(0, screenHeight);
        Vector3 randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, 0));

        // Move the object to the random position
        objectToMove.position = randomPosition;
    }
}
