using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemMatrixManager : MonoBehaviour
{
    //so it can be called from anywhere
    public static GemMatrixManager instance = null;

    //keeps a prefab of each possible Gem to fill random gem list
    public GameObject appleGemPrefab;
    public GameObject breadGemPrefab;
    public GameObject coconutGemPrefab;
    public GameObject flowerGemPrefab;
    public GameObject milkGemPrefab;
    public GameObject orangeGemPrefab;
    public GameObject vegetableGemPrefab;

    //keeps BG GameObject so it can properly position gems
    public GameObject screenBackgroundGO;
    public float screenBackgroundWidth, screenBackgroundHeight;

    public float gemWidth, gemHeight;


    //Keeps parent GameObject for the gems, so they can all be children of the same GO
    public GameObject gemMatrixGO;


    //holds all possible gems; to be used when generating a matrix, or when filling up after player makes chains
    private List<GameObject> gemList;

    public int numberOfRowsInGemMatrix = 6;
    public int numberOfColumnsInGemMatrix = 6;


    private List<List<GameObject>> gemMatrix { get; set; }
    private List<List <Vector2> > gemMatrixPositions { get; set; }//saves the x,y position of each gem on the canvas



    //Keeps track of highlighted gem, so we know which one we'll swap into
    public int i_indexHighlightedGem =-1;
    public int j_indexHighlightedGem =-1;

    

    /// <summary>
    /// Based on the Background GO's width and height, generate each gem's positions on-screen
    /// </summary>
    public void GenerateRandomGemPositions()
    {
        //Starts at the bottom left of the background;
        Vector2 startingPosition = new Vector2(-(screenBackgroundWidth / 2.0f), -(screenBackgroundHeight / 2.0f));

        //we will distribute 6 gems spaced equally, over 6 equally spaced lines, starting from the bottom

        //math is basically:
        //Position (i,j) =  {S.x + (j*width/6 + width/12), S.y + i*height/6 + height/12 }
        //Where S = starting point from the bottom left
        for (int i = 0; i < numberOfColumnsInGemMatrix; i++)
        {
            List<Vector2> row = new List<Vector2>();
            for (int j = 0; j < numberOfRowsInGemMatrix; j++)
            {
                Vector2 newPosition = new Vector2(startingPosition.x + j*(screenBackgroundWidth/numberOfRowsInGemMatrix) + (screenBackgroundWidth / (numberOfRowsInGemMatrix*2)),
                                                  startingPosition.y + i * (screenBackgroundHeight / numberOfColumnsInGemMatrix) + (screenBackgroundHeight / (numberOfColumnsInGemMatrix * 2)));
                row.Add(newPosition);//adds a random gem to the matrix
                
            }
            //inserts at the beginning of the list always, since we're doing in from back to front
            gemMatrixPositions.Insert(0, row);            
        }
        
    }

    
    /// <summary>
    /// Deletes the current gem in the given position (if there is any) and adds a new Gem to the desired position (if no gem type is chosen, it's chosen randomly)
    /// </summary>
    /// <param name="iPosition">i position (row) of the new gem</param>
    /// <param name="jPosition">j position (column) of the new gem</param>
    /// <param name="typeOfGemToAdd">Type of gem to add (if not filled, creates a RANDOM gem in its place)</param>
    public void CreateNewGem(int iPosition, int jPosition, GameObject typeOfGemToAdd = null)
    {
        //if a gem exists in said position, delete it just to be safe
        DeleteGem(iPosition, jPosition);

        //Create a new one in given position
        //instantiate gem as a child of gem matrix GO
        GameObject newGem;
        if(typeOfGemToAdd == null)
        {
            //create a random gem
            int index = Random.Range(0, numberOfColumnsInGemMatrix);
            //newGem = GameObject.Instantiate(gemList[index], gemMatrixGO.transform);
            newGem = GameObject.Instantiate(gemList[index], gemMatrixGO.transform, false);

        }
        else //use given gem from parameters to instantiate this new gem
        {
            //newGem = GameObject.Instantiate(typeOfGemToAdd, gemMatrixGO.transform);
            newGem = GameObject.Instantiate(typeOfGemToAdd, gemMatrixGO.transform, false);
        }

        //next, position the gem in its correct position

        //Also TAKE DOWNSCALING OF OBJECT INTO ACCOUNT
        //Vector3 gemPos = new Vector3(gemMatrixPositions[iPosition][jPosition].x * (1 / newGem.transform.localScale.x),
        //                             gemMatrixPositions[iPosition][jPosition].y * (1 / newGem.transform.localScale.y), 0);

        Vector2 gemPos = new Vector2(gemMatrixPositions[iPosition][jPosition].x ,
                                     gemMatrixPositions[iPosition][jPosition].y);

        
        //newGem.transform.position = gemPos; //set it outside so it doesn't get tangled with parent's relative position
        newGem.GetComponent<RectTransform>().anchoredPosition = gemPos; //set it outside so it doesn't get tangled with parent's relative position

        //makes it keep its position in matrix so we can easily find it
        newGem.GetComponent<Gem>().i_index = iPosition;
        newGem.GetComponent<Gem>().j_index = jPosition;

        //finally, add it to the matrix
        gemMatrix[iPosition][jPosition] = newGem;
    }


    //Delete Gem GameObject and from MatrixList
    public void DeleteGem(int iPosition, int jPosition)
    {
        //Debug.Log("Deleting gem in (" + iPosition + ", " + jPosition + ")...");
        GameObject.Destroy(gemMatrix[iPosition][jPosition]);
        //erase it from list (make its pointer null)
        gemMatrix[iPosition][jPosition] = null;
    }



    /// <summary>
    /// Generates a full randomized gem Matrix
    /// </summary>
    public void GenerateAndInstantiateRandomGemMatrix()
    {

        for (int i = 0; i < numberOfColumnsInGemMatrix; i++)
        {
            List<GameObject> row = new List<GameObject>();
            for (int j = 0; j < numberOfRowsInGemMatrix; j++)
            {
                int index = Random.Range(0, numberOfColumnsInGemMatrix);

                //instantiate gem as a child of gem matrix GO
                GameObject newGem = GameObject.Instantiate(gemList[index], gemMatrixGO.transform, false);


                //Also TAKE DOWNSCALING OF OBJECT INTO ACCOUNT
                //Vector3 gemPos = new Vector3(gemMatrixPositions[i][j].x*(1/newGem.transform.localScale.x), 
                //gemMatrixPositions[i][j].y*(1/newGem.transform.localScale.y), 0);

                Vector2 gemPos = new Vector2(gemMatrixPositions[i][j].x ,
                                             gemMatrixPositions[i][j].y);

                //newGem.transform.position = gemPos; //set it outside so it doesn't get tangled with parent's relative position
                newGem.GetComponent<RectTransform>().anchoredPosition = gemPos; //set it outside so it doesn't get tangled with parent's relative position

                //makes it keep its position in matrix so we can easily find it
                newGem.GetComponent<Gem>().i_index = i;
                newGem.GetComponent<Gem>().j_index = j;

                row.Add(newGem);//adds the random gem to the matrix
                
            }
            gemMatrix.Add(row);
        }
    }

    /// <summary>
    /// Tells whether or not gem should be 'highlighted' (slightly transparent)
    /// </summary>
    /// <param name="iPos">row position of gem in matrix</param>
    /// <param name="jPos">column position of gem in matrix</param>
    /// <param name="highlight">Whether to highlight gem or unhighlight it</param>
    public void ToggleGemHighlight(int iPos, int jPos, bool highlight)
    {
        float highlightValue = highlight ? 0.6f : 1.0f;

        //there is already a highlighted one, make its alpha 1 again
        Color currentColor = gemMatrix[iPos][jPos].GetComponent<Image>().color;
        gemMatrix[iPos][jPos].GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, highlightValue);


    }
    

    /// <summary>
    /// marks the new highlighted gem
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public void setNewHighlightedGem(int i, int j)
    {
        //make highlighted gem slightly 'lighter' (less alpha) and make old highlighted one normal again

        if(i_indexHighlightedGem != -1 && j_indexHighlightedGem !=-1)
        {
            //there is already a highlighted one, make its alpha 1 again
            ToggleGemHighlight(i_indexHighlightedGem, j_indexHighlightedGem, false);
        }

        //highlight the new gem
        ToggleGemHighlight(i, j, true);


        //keeps track of new highlighted gem
        i_indexHighlightedGem = i;
        j_indexHighlightedGem = j;
    }









    //==================================================================================================================================
    //Gem Chain methods
    //==================================================================================================================================

    /// <summary>
    /// Swap positions between two Gem Game Objects (does NOT check if they are adjacent or not)
    /// </summary>
    /// <param name="gem1iPos">row position of gem 1</param>
    /// <param name="gem1jPos">column position of gem 1</param>
    /// <param name="gem2iPos">row position of gem 2</param>
    /// <param name="gem2jPos">column position of gem 2</param>
    public void SwapGems(int gem1iPos, int gem1jPos, int gem2iPos, int gem2jPos)
    {
        //firstly, unhighlight currently highlighted gem (do it for both to be safe)
        ToggleGemHighlight(gem1iPos, gem1jPos, false);
        ToggleGemHighlight(gem2iPos, gem2jPos, false);

        GameObject tmpGem = gemMatrix[gem1iPos][gem1jPos];

        //Delete Gem 1's instance and GO from the matrix, add Gem 2 in its place

        //DeleteGem(gem1iPos, gem1jPos);
        CreateNewGem(gem1iPos, gem1jPos, gemMatrix[gem2iPos][gem2jPos]);

        //now, delete Gem 2's instance and GO from matrix and replace it with Gem 1's copy
        //DeleteGem(gem2iPos, gem2jPos);
        CreateNewGem(gem2iPos, gem2jPos, tmpGem);

        
        //Re-enable Image, Gem Script and Box Collider components of new gem2
        //Needed because I am re-instantiating a previously destroyed GO. Unity disables all components before destroying it, so now instance also has them disabled
        gemMatrix[gem2iPos][gem2jPos].GetComponent<Image>().enabled = true;
        gemMatrix[gem2iPos][gem2jPos].GetComponent<Gem>().enabled = true;
        gemMatrix[gem2iPos][gem2jPos].GetComponent<BoxCollider2D>().enabled = true;

        //also, change gem2's GO name
        gemMatrix[gem2iPos][gem2jPos].name = gemMatrix[gem2iPos][gem2jPos].GetComponent<Gem>().gemName;


    }



    /// <summary>
    /// Checks if there is an empty space in the matrix; if there is, return the FIRST found [i,j] position; else, returns [-1,-1]
    /// </summary>
    /// <param name="iEmptyPos">row position of the first found empty position in matrix (starts from top left); if no empty pos, returns -1</param>
    /// <param name="jEmptyPos">column position of the first found empty position in matrix (starts from top left); if no empty pos, returns -1</param>
    public void CheckForEmptySpaces(out int iEmptyPos, out int jEmptyPos)
    {

        iEmptyPos = -1;
        jEmptyPos = -1;

        for (int i = 0; i < gemMatrix.Count; i++)
        {
            for (int j = 0; j < gemMatrix[i].Count; j++)
            {
                if (gemMatrix[i][j] == null)
                {
                    iEmptyPos = i;
                    jEmptyPos = j;
                    return;
                }
            }
        }
    }


    /// <summary>
    /// Moves the gem in position (i, j) to (i+1, j); erases it from current position and overwrites whatever is in (i+1, j) (DOES NOT CHECK IF IT'S BOTTOM LINE)
    /// </summary>
    /// <param name="i">row number </param>
    /// <param name="j">column number</param>
    public void MoveGemDown(int i, int j)
    {
        //Debug.Log("Moving " + gemMatrix[i][j].GetComponent<Gem>().gemName + "in (" + i + ", " + j + ") down...");
        
        //move upper gem to lower spot...
        CreateNewGem(i + 1, j, gemMatrix[i][j]);

        //...then finally erase upper gem
        DeleteGem(i, j);
    }



    /// <summary>
    /// After every player swap, checks for any possible completed chains, then deal with them accordingly
    /// </summary>
    /// <param name="recursiveEntry">If false, then play swap sound; else, it's coming from a second-or-onwards check for new chains, don't bother playing it then</param>
    /// <param name="firstRun">If false, then add possible points to score; else, it's just to clean possible initial chains, don't add any points</param>
    public void CheckForCompletedChains(bool recursiveEntry = false, bool firstRun = false)
    {
        //first, get completed chains (if any)

        List<List<Vector2>> completedChains = GetAllCompletedChains();
        if(completedChains.Count>0)
        {
            //play clear sound and add possible score only if it's not the first run
            if (!firstRun)
            {
                SoundManager.instance.PlayClearGemOneShot();


                //For each chain found, check its size and give out points accordingly; 
                //also delete the Gems/GOs in the chains

                foreach (List<Vector2> completedChain in completedChains)
                {
                    //give out points based on its size (only if it's not the initial run to clean the matrix of chains)

                    switch (completedChain.Count)
                    {
                        case 3:
                            ScoreManager.instance.AddToScore(ScoreManager.instance.Chain3_Score);
                            break;
                        case 4:
                            ScoreManager.instance.AddToScore(ScoreManager.instance.Chain4_Score);
                            break;
                        case 5:
                            ScoreManager.instance.AddToScore(ScoreManager.instance.Chain5_Score);
                            break;
                        case 6:
                            ScoreManager.instance.AddToScore(ScoreManager.instance.Chain6_Score);
                            break;
                        default:
                            Debug.Log("Error: Found chain of length " + completedChain.Count + ", should not be possible!! Chain contents: ");
                            foreach (Vector2 vec in completedChain)
                            {
                                Debug.Log("(" + vec.x + ", " + vec.y + ")");
                            }
                            break;
                    }
                }
            }

            object[] coroutineParams = new object[2] { completedChains, firstRun };
            //After chains are scored, call "Blink, Delete and move down" coroutine
            StartCoroutine("BlinkExistingChainsThenDelete", coroutineParams);
        }

        
        //else, no new chains were found, carry on with game; play Swap Sound only if it's not a recursive entry and it's not the first run
        //(added here so both sounds don't play at once)
        else
        {
            if(!recursiveEntry && !firstRun)
                SoundManager.instance.PlaySwapGemOneShot();//play Swap Sound
        }
    }









    /// <summary>
    /// Blink completed chains, then delete them and move down new ones
    /// </summary>
    /// <param name="coroutineParams">Passes 2 parameters: \n1)List{List{Vector2 } } completedChains: list of all completed chains after last swap; 
    /// \n2) bool firstRun: if true, don't run 'highlight' animation </param>
    public IEnumerator BlinkExistingChainsThenDelete(object[] coroutineParams)
    {
        List<List<Vector2> > completedChains = (List<List<Vector2> >)coroutineParams[0];
        bool firstRun = (bool)coroutineParams[1];


        bool highlight = true;

        //make all of the chain gems highlighted, wait for .25s, then un-highlight them (do this four times)
        //ONLY RUNS IF IT'S NOT THE FIRST RUN
        if (!firstRun)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (List<Vector2> chain in completedChains)
                {
                    foreach (Vector2 gem in chain)
                    {
                        ToggleGemHighlight((int)gem.x, (int)gem.y, highlight);
                    }
                }
                //toggle bool, then wait for 1/4 seconds
                highlight = !highlight;
                yield return new WaitForSeconds(0.2f);
            }
        }

        //call for deletion of gems
        foreach (List<Vector2> chain in completedChains)
        {
            foreach (Vector2 GemPosition in chain)
            {
                //hasn't been erased yet by another chain
                if (gemMatrix[(int)GemPosition.x][(int)GemPosition.y] != null)
                {
                    DeleteGem((int)GemPosition.x, (int)GemPosition.y);
                }
            }
        }

        //After gems were deleted, make remaining floating gems move down 
        MoveDownFloatingGems();

        //finally, fill newly-emptied spaces with new gems
        CheckAndFillRemainingEmptySpaces(firstRun);
    }



    
    /// <summary>
    /// Returns all of the completed chains after player's last movement
    /// </summary>
    /// <returns></returns>
    public List<List<Vector2> > GetAllCompletedChains()
    {
        List<List<Vector2>> completedChains = new List<List<Vector2>>();
        
        //Check for possible chains in all lines
        for(int i = 0;i < numberOfRowsInGemMatrix; i++)
        {
            List<List<Vector2>> chainsInCurrentLine = CheckForChainsInLine(i);

            //if it found a chain, add it to completed chains list
            if(chainsInCurrentLine.Count>0)
            {
                foreach(List<Vector2> chain in chainsInCurrentLine)
                {
                    completedChains.Add(chain);
                }
            }
        }

        //do the same for all columns
        for (int j = 0; j < numberOfColumnsInGemMatrix; j++)
        {
            List<List<Vector2>> chainsInCurrentColumn = CheckForChainsInColumn(j);

            //if it found a chain, add it to completed chains list
            if (chainsInCurrentColumn.Count > 0)
            {
                foreach (List<Vector2> chain in chainsInCurrentColumn)
                {
                    completedChains.Add(chain);
                }
            }
        }
        
        //
        return completedChains;
    }


    /// <summary>
    /// Check for chains in
    /// </summary>
    /// <param name="lineToCheck"></param>
    /// <returns></returns>
    public List<List<Vector2> > CheckForChainsInLine(int lineToCheck)
    {
        List<List<Vector2>> chainsInCurrentLine = new List<List<Vector2>>();

        List<Vector2> currentChain = new List<Vector2>();
        
        for(int j = 0;j<gemMatrix[lineToCheck].Count;j++)
        {
            if(currentChain.Count==0)
            {
                //add to list since it's empty
                
                currentChain.Add(new Vector2(lineToCheck, j));
            }
            else
            {
                //check if all of the gems in the current chain list are the same as the current gem
                //if they all are, add current gem to current chain
                bool chainWasBroken = false;
                for(int k = 0;k<currentChain.Count;k++)
                {
                    //if they have different names, chain was broken 
                    if(gemMatrix[(int)currentChain[k].x][(int)currentChain[k].y].GetComponent<Gem>().gemName !=gemMatrix[lineToCheck][j].GetComponent<Gem>().gemName)
                    {
                        chainWasBroken = true;
                        break;
                    }
                }

                //if chain WAS broken, check if current gems in list form a minimum chain; if so, send it to main list of chains in line
                if (chainWasBroken)
                {
                    if (currentChain.Count >= 3)
                    {
                        //send to main chain list
                        //string positions = "";
                        //foreach(Vector2 position in currentChain)
                        //{
                        //    positions += "(" + position.x + ", " + position.y + "), ";
                        //}
                        //Debug.Log("Found a chain at " + positions);
                        chainsInCurrentLine.Add(currentChain);
                    }

                    //regardless, erase the current chain and start anew, adding the new gem to it
                    //Gotta create a new list, or else it'll alter the contents of the chains added to the main chain list
                    currentChain = new List<Vector2>();
                    currentChain.Add(new Vector2(lineToCheck, j));
                }

                //else, the chain was not broken, so just add the gem to the list
                else
                {
                    currentChain.Add(new Vector2(lineToCheck, j));
                }
            }
        }
        //at the end of the line, there may be a final chain in the list; if its size is at least 3, add it to main list
        if (currentChain.Count >= 3)
        {
            //send to main chain list
            //string positions = "";
            //foreach (Vector2 position in currentChain)
            //{
            //    positions += "(" + position.x + ", " + position.y + "), ";
            //}
            //Debug.Log("Found a chain at " + positions);
            chainsInCurrentLine.Add(currentChain);
        }

        return chainsInCurrentLine;
    }


    /// <summary>
    /// Check for chains in a given column
    /// </summary>
    /// <param name="columnToCheck">index of the column to check</param>
    /// <returns></returns>
    public List<List<Vector2>> CheckForChainsInColumn(int columnToCheck)
    {
        List<List<Vector2>> chainsInCurrentColumn = new List<List<Vector2>>();

        List<Vector2> currentChain = new List<Vector2>();

        for (int i = 0; i < numberOfRowsInGemMatrix; i++)
        {
            if (currentChain.Count == 0)
            {
                //add to list since it's empty
                currentChain.Add(new Vector2(i, columnToCheck));
            }

            else
            {
                //check if all of the gems in the current chain list are the same as the current gem
                //if they all are, add current gem to current chain
                bool chainWasBroken = false;
                for (int k = 0; k < currentChain.Count; k++)
                {
                    //if they have different names, chain was broken 
                    if (gemMatrix[(int)currentChain[k].x][(int)currentChain[k].y].GetComponent<Gem>().gemName != gemMatrix[i][columnToCheck].GetComponent<Gem>().gemName)
                    {
                        chainWasBroken = true;
                        break;
                    }
                }

                //if chain WAS broken, check if current gems in list form a minimum chain; if so, send it to main list of chains in column

                if (chainWasBroken)
                {
                    if (currentChain.Count >= 3)
                    {
                        //send to main chain list
                        //string positions = "";
                        //foreach (Vector2 position in currentChain)
                        //{
                        //    positions += "(" + position.x + ", " + position.y + "), ";
                        //}
                        //Debug.Log("Found a chain at " + positions);
                        chainsInCurrentColumn.Add(currentChain);
                    }

                    //regardless, erase the current chain and start anew, adding the new gem to it
                    //Gotta create a new list, or else it'll alter the contents of the chains added to the main chain list
                    currentChain = new List<Vector2>();
                    currentChain.Add(new Vector2(i, columnToCheck));
                }

                //else, the chain was not broken, so just add the gem to the list
                else
                {
                    currentChain.Add(new Vector2(i, columnToCheck));
                }
            }
        }
        //at the end of the column, there may be a final chain in the list; if its size is at least 3, add it to main list
        if (currentChain.Count >= 3)
        {
            //send to main chain list
            //string positions = "";
            //foreach (Vector2 position in currentChain)
            //{
            //    positions += "(" + position.x + ", " + position.y + "), ";
            //}
            //Debug.Log("Found a chain at " + positions);
            chainsInCurrentColumn.Add(currentChain);
        }

        return chainsInCurrentColumn;
    }



    
    /// <summary>
    /// Checks if there is an empty space in the matrix; if there is, return the FIRST found [i,j] position; else, returns [-1,-1]
    /// </summary>
    /// <param name="iEmptyPos">row position of the first found empty position in matrix (starts from top left); if no empty pos, returns -1</param>
    /// <param name="jEmptyPos">column position of the first found empty position in matrix (starts from top left); if no empty pos, returns -1</param>
    public void CheckForFloatingGems(out int iEmptyPos, out int jEmptyPos)
    {

        iEmptyPos = -1;
        jEmptyPos = -1;

        //no point in checking first line, go from second onwards
        for (int i = 1; i < gemMatrix.Count; i++)
        {
            for (int j = 0; j < gemMatrix[i].Count; j++)
            {
                //current position is empty and the one above it is not; found a hanging gem, return the HANGING GEM'S position, not the empty one
                if (gemMatrix[i][j] == null && gemMatrix[i-1][j]!=null)
                {
                    iEmptyPos = i-1;
                    jEmptyPos = j;
                    return;
                }
            }
        }
    }


    /// <summary>
    /// Goes through matrix, pushing down gems until they hit their possible 'bottom'
    /// </summary>
    public void MoveDownFloatingGems()
    {
        //
        int i = -1, j = -1;

        //Make first check
        CheckForFloatingGems(out i, out j);

        while (i != -1 && j != -1)
        {
            //if it found a floating gem, move it down
            MoveGemDown(i, j);


            //check again for the next possible floating gem (if any)
            CheckForFloatingGems(out i, out j);
        }
    }






    /// <summary>
    /// After the completed chains of the round are deleted and every hanging gem is moved down, fill all empty spaces with new random gems 
    /// </summary>
    /// <param name="firstRun">if true, don't run 'highlight' animation in next run (used for recursions in First Run</param>
    public void CheckAndFillRemainingEmptySpaces(bool firstRun)
    {
       
        //For now, just create them in the empty spaces
        for (int i = 0; i < gemMatrix.Count; i++)
        {
            for (int j = 0; j < gemMatrix[i].Count; j++)
            {
                //current position is empty, fill it with random new gem
                if (gemMatrix[i][j] == null)
                {
                    //Debug.Log("Final part: create new gem at (" + i + ", " + j + ")...");
                    CreateNewGem(i, j);
                }
            }
        }
        
        //one last time, check for any new chains that may have been made with these new gems
        CheckForCompletedChains(true, firstRun);
    }






    //==================================================================================================================================
    //initialization methods
    //==================================================================================================================================

        

    private void Awake()
    {
        
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Error: More than one instance of Gem Matrix Manager script in action in " + this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //fill possible gems to list so we can randomize the initial matrix
        gemList = new List<GameObject>();

        gemList.Add(appleGemPrefab);
        gemList.Add(breadGemPrefab);
        gemList.Add(coconutGemPrefab);
        gemList.Add(flowerGemPrefab);
        gemList.Add(milkGemPrefab);
        gemList.Add(orangeGemPrefab);
        gemList.Add(vegetableGemPrefab);

        

        //get screen width and height to calculate gem positions
        screenBackgroundWidth = screenBackgroundGO.GetComponent<RectTransform>().rect.width;
        screenBackgroundHeight =  screenBackgroundGO.GetComponent<RectTransform>().rect.height;

        //also gets gems width and height
        gemWidth = appleGemPrefab.GetComponent<RectTransform>().rect.width;
        gemHeight = appleGemPrefab.GetComponent<RectTransform>().rect.height;

        gemMatrixPositions = new List<List<Vector2>>();
        GenerateRandomGemPositions();

        //create Game Objects in matrix and instantiate them
        gemMatrix = new List< List<GameObject> >();
        GenerateAndInstantiateRandomGemMatrix();

        //Finally, Clean initial possible chains
        CheckForCompletedChains(false, true);
    }

}
