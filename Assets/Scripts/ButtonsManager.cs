using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    
    //Swap control for each possible direction

    
    public void SwapGemRight()
    {
        //button only works if player hasn't lost yet
        if (RoundsManager.instance.playerLost)
            return;


            //won't run if no highlighted gem
            if (GemMatrixManager.instance.i_indexHighlightedGem==-1 || GemMatrixManager.instance.j_indexHighlightedGem==-1)
        {
            return;
        }


        int gemIPosition = GemMatrixManager.instance.i_indexHighlightedGem;
        int gemJPosition = GemMatrixManager.instance.j_indexHighlightedGem;

        //check if current highlighted gem CAN go to the right, i.e., if it's not on matrix's edge
        if(gemJPosition < GemMatrixManager.instance.numberOfColumnsInGemMatrix-1)
        {
            //actually swap them

            GemMatrixManager.instance.SwapGems(gemIPosition, gemJPosition, gemIPosition, gemJPosition + 1);

            //unmark the highlighted gem
            GemMatrixManager.instance.i_indexHighlightedGem = -1;
            GemMatrixManager.instance.j_indexHighlightedGem = -1;

            //at the end of every swap, check for possible new chains
            GemMatrixManager.instance.CheckForCompletedChains();
        }
    }


    public void SwapGemLeft()
    {
        //button only works if player hasn't lost yet
        if (RoundsManager.instance.playerLost)
            return;

        //won't run if no highlighted gem
        if (GemMatrixManager.instance.i_indexHighlightedGem == -1 || GemMatrixManager.instance.j_indexHighlightedGem == -1)
        {
            return;
        }

        int gemIPosition = GemMatrixManager.instance.i_indexHighlightedGem;
        int gemJPosition = GemMatrixManager.instance.j_indexHighlightedGem;

        //check if current highlighted gem CAN go to the left, i.e., if it's not on matrix's edge
        if (gemJPosition > 0)
        {
            //actually swap them

            GemMatrixManager.instance.SwapGems(gemIPosition, gemJPosition, gemIPosition, gemJPosition - 1);

            //unmark the highlighted gem
            GemMatrixManager.instance.i_indexHighlightedGem = -1;
            GemMatrixManager.instance.j_indexHighlightedGem = -1;

            //at the end of every swap, check for possible new chains
            GemMatrixManager.instance.CheckForCompletedChains();

        }
    }
    public void SwapGemUp()
    {
        //button only works if player hasn't lost yet
        if (RoundsManager.instance.playerLost)
            return;

        //won't run if no highlighted gem
        if (GemMatrixManager.instance.i_indexHighlightedGem == -1 || GemMatrixManager.instance.j_indexHighlightedGem == -1)
        {
            return;
        }

        int gemIPosition = GemMatrixManager.instance.i_indexHighlightedGem;
        int gemJPosition = GemMatrixManager.instance.j_indexHighlightedGem;

        //check if current highlighted gem CAN go up, i.e., if it's not on matrix's edge
        if (gemIPosition > 0)
        {
            //actually swap them

            GemMatrixManager.instance.SwapGems(gemIPosition, gemJPosition, gemIPosition -1 , gemJPosition);


            //unmark the highlighted gem
            GemMatrixManager.instance.i_indexHighlightedGem = -1;
            GemMatrixManager.instance.j_indexHighlightedGem = -1;

            //at the end of every swap, check for possible new chains
            GemMatrixManager.instance.CheckForCompletedChains();
        }
    }
    public void SwapGemDown()
    {
        //button only works if player hasn't lost yet
        if (RoundsManager.instance.playerLost)
            return;

        //won't run if no highlighted gem
        if (GemMatrixManager.instance.i_indexHighlightedGem == -1 || GemMatrixManager.instance.j_indexHighlightedGem == -1)
        {
            return;
        }

        int gemIPosition = GemMatrixManager.instance.i_indexHighlightedGem;
        int gemJPosition = GemMatrixManager.instance.j_indexHighlightedGem;

        //check if current highlighted gem CAN go down, i.e., if it's not on matrix's edge
        if (gemIPosition < GemMatrixManager.instance.numberOfRowsInGemMatrix -1)
        {
            //actually swap them

            GemMatrixManager.instance.SwapGems(gemIPosition, gemJPosition, gemIPosition +1 , gemJPosition);

            //unmark the highlighted gem
            GemMatrixManager.instance.i_indexHighlightedGem = -1;
            GemMatrixManager.instance.j_indexHighlightedGem = -1;

            //at the end of every swap, check for possible new chains
            GemMatrixManager.instance.CheckForCompletedChains();
        }
    }


    public void Update()
    {
        //WASD work like Up/Left/Down/Right buttons
        if(Input.GetKeyDown(KeyCode.W))
        {
            SwapGemUp();
        }

        else if(Input.GetKeyDown(KeyCode.A))
        {
            SwapGemLeft();
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            SwapGemDown();
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            SwapGemRight();
        }
    }


}
