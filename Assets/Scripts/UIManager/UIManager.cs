﻿using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Gestion du score")]
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextHighScore;
    [SerializeField] private ScoreManager ScoreManager;


    [Header("affichage de la Vie")]
    [SerializeField] private ParticleManager ParticleManager;

    [SerializeField] private GameObject ViePacMan;
    private List<GameObject> Pv = new List<GameObject>();


    [Header("affichage de multiplicateur")]
    private List<GameObject> MultiPoint = new List<GameObject>();
    [SerializeField] private GameObject fruit;


    [Header("affichage du power up")]
    private GameObject PowerUp;


    [Header("affichage de la progression du niveau")]
    [SerializeField] private LevelDisplayer level;
    [SerializeField] private Image CercleProgres;

    [SerializeField] private int _progressionMax;   //total de pac gomme
    [SerializeField] private int _progression;      //pac gomme manger

    [Header("gameover")]
    [SerializeField] private GameObject UIGameOver;
    [SerializeField] private GameObject UIWin;

    [SerializeField] private int pacmanState;


    //=========================Il sert pour le Debug 
    /*private void Update()
    {
    }*/

    //=========================Système d'adition de point    
    public string AddPoint(int score)
    {
        int limit = 100000;
        string zero = "";
        // tant que le score sera inférieur a limit, zero aura un 0 de plus et limit sera divisée par 10 à chaque fois jusqu'à que la condition soit fausse
        while (score < limit)
        {
            zero += "0";
            limit /= 10;
        }
        return (zero + score).ToString();
    }

    //=========================Gestion des textes

    #region Vie PacMan
    //=========================Initialisation de l'affichage de la vie
    public void InitShowPV(int vie)
    {
        //on fait aparaitre les pac-mans vie selon un ordre précis et on les stockes dans un tableau. 
        for (int i = 0; i < vie; i++)
        {
            GameObject newPv = Instantiate(ViePacMan, this.transform);
            //y = position du score - l'écartement entre les vies - la distance d'écartement entre les vies * le combientième de vie (deuxième, troisième, ...)
            newPv.transform.position += new Vector3(TextScore.transform.position.x, TextScore.transform.position.y - 12f - (1.5f * i), 0f);
            Pv.Add(newPv);
        }
    }

    //=========================Gestion de la vie et de son affichage
    public bool Touch()
    {
        if (Pv.Count > 0)
        {
            ParticleManager.ParticulePacMan(Pv[Pv.Count - 1].transform);
            Destroy(Pv[Pv.Count - 1]);
            Pv.RemoveAt(Pv.Count - 1);
            PlusDePower();
            return true;
        }
        else { return false; }
    }
    #endregion


    #region Multiplicateurs
    //=========================Gestion de l'affichage des multiplicateurs.
    //Addition d'un fruit multiplicateur
    public void AddMultiPoint()
    {
        if (MultiPoint.Count < 5)
        {
            var newFruit = Instantiate(fruit, this.transform);

            float currentPos = 6f;
            float largeur = 2f;
            float longueur = 4f;
            switch (MultiPoint.Count)
            {
                case 0:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x - longueur / 2, TextScore.transform.position.y - currentPos - 0, 0f);
                    break;
                case 1:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x + longueur / 2, TextScore.transform.position.y - currentPos - 0, 0f);
                    break;
                case 2:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x - longueur / 2, TextScore.transform.position.y - currentPos - largeur, 0f);
                    break;
                case 3:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x + longueur / 2, TextScore.transform.position.y - currentPos - largeur, 0f);
                    break;
                case 4:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x, TextScore.transform.position.y - currentPos - largeur / 2, 0f);
                    break;
            }

            MultiPoint.Add(newFruit);
        }
        else
        {
            Debug.Log("Les fruit sont au max !!!");
        }
    }

    //=========================Supression des multiplicateur
    /*public void DestroyFruits()
    {
        while (MultiPoint.Count > 0)
        {
            Destroy(MultiPoint[MultiPoint.Count - 1]);
            MultiPoint.RemoveAt(MultiPoint.Count - 1);
        }
    }*/
    #endregion

    #region Power Up
    //=========================Gestion de l'affichage du power up
    //Ajout de du power up et affichage
    public void TouchPower(GameObject newPowerUp)
    {
        if (PowerUp == null)
        {
            PowerUp = Instantiate(newPowerUp, this.transform);
            PowerUp.transform.position += new Vector3(TextHighScore.transform.position.x, TextHighScore.transform.position.y - 2f, 0f);
        }
    }


    //=========================Na plus le power up
    public void PlusDePower()
    {
        Destroy(PowerUp);
    }
    #endregion

    #region Progresion
    //=========================Initialisation de la progression
    public void SetProgressionMax()
    {
        _progressionMax = 0;
        _progression = 0;
            for (int x = 0; x < LevelParser.mapWidth; x++)
                for (int y = 0; y < LevelParser.mapHeight; y++)
                    if (level.displayTiles[x, y].type == TileType.Ball || level.displayTiles[x, y].type == TileType.Super || level.displayTiles[x, y].type == TileType.Fruit)
                        _progressionMax++;
                
            
        
    }
    //=========================Récupération des parmètres de progression du niveau
    public void GetProgression()
    {
        _progression++;
        UpdateProgression();
    }

    //=========================Gestion de l'affichage de la progression
    private void UpdateProgression()
    {
        float progresse = (float)_progression / (float)_progressionMax;
        CercleProgres.fillAmount = progresse;

        if (_progression >= _progressionMax) {
            UIWin.SetActive(true);
            pacmanState = 1;
            ScoreManager.SaveScoreWin();
            PlayerPrefs.SetInt("PacmanWon", pacmanState);

            SceneManager.LoadScene("Pacman Parsed");
        }
    }
    #endregion

    //=========================Affichage du game over
    public void GameOver()
    {
        pacmanState = 0;
        PlayerPrefs.SetInt("PacmanWon", pacmanState);
        UIGameOver.SetActive(true);
        StartCoroutine(LoadDelay());
    }

    IEnumerator LoadDelay()
    {
        GameManager.isGame = false;
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("Menu");
    }

    public int PacmanWon ()
    {
        pacmanState = PlayerPrefs.GetInt("PacmanWon");
        return pacmanState;
    }
}
