using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelScriptManager : MonoBehaviour
{
    public static LevelScriptManager Instance;
    
    public GameObject[] HeroesInPlay;

    // Yarn Spinner runner to control the start dialogue nodes to play
    public GameObject DialogueRunner;
    public DialogueRunner dialogueRunner;

    public string[] dialogueNodes;
    public int dialogueIndex = 0; // iterator

    public Enemy? m_CurrentEncounterAction; // current enemy that has triggered a dialogue

    private Spawner[] m_Spawners;
    public bool m_PauseSpawning = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DialogueRunner = GameObject.FindGameObjectWithTag("DialogueRunner");
            dialogueRunner = DialogueRunner.GetComponent<DialogueRunner>();
            dialogueNodes = new string[] { "CORE_AutomaticThoughts_Level01_01", "CORE_AutomaticThoughts_Level01_02" };
            return;
        }
        Destroy(gameObject);
    }

    void Start()
    {
        m_Spawners = FindObjectsOfType<Spawner>();
        
        HeroesInPlay = GameObject.FindGameObjectsWithTag("Hero");

        dialogueRunner.startNode = "CORE_AutomaticThoughts_Level01_01";
        
        InvokeRepeating("HandleSpawners", 0.0f, 3.0f);
    }

    public void HandleSpawners()
    {
        // Decide if should spawn now or wait
        foreach (Spawner spawner in m_Spawners)
        {
            if (!m_PauseSpawning)
            {
                spawner.StartSpawning();   
            }
        }
    }

    public void StartNewCBTNode() {
        // Those are special enemies that talk : others are minions
        if (!dialogueRunner.IsDialogueRunning && dialogueIndex < dialogueNodes.Length)
        {
            dialogueRunner.StartDialogue(dialogueNodes[dialogueIndex]);
            ++dialogueIndex;
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    public void SlowTime()
    {
        Time.timeScale = 0.8f;
    }

    public void RestoreTime()
    {
        Time.timeScale = 1.0f;
    }

    public void PauseAllSpawners()
    {
        m_PauseSpawning = true;

        foreach (Spawner spawner in m_Spawners)
        {
            spawner.StopCoroutine("SpawnInstance");
        }
    }

    public void SlowDownEncounter(GameObject actor)
    {
        if (actor != null)
        {
            actor.GetComponent<Enemy>().SetSpeed(0.0f);
            actor.GetComponent<Enemy>().SetColliderState(false);
        }
    }

    public void SlowDownAllActors()
    {
        foreach (GameObject enemy in TowerDefenseManager.Instance.m_EnemiesAlive)
        {
            SlowDownEncounter(enemy);
        }
    }
    
    public void RestoreAllActorsSpeed()
    {
        foreach (GameObject enemy in TowerDefenseManager.Instance.m_EnemiesAlive)
        {
            ResetEncounterAction(enemy);
        }
    }

    public void ResetEncounterAction(GameObject actor)
    {
        if (actor != null)
        {
            actor.GetComponent<Enemy>().SetSpeed(5.0f);
            actor.GetComponent<Enemy>().SetColliderState(true);
        }

        m_CurrentEncounterAction = null;
    }

    public void DecideOutcome()
    {
        print("DecideOutcome()");

        // Read global outcome var set by encounter
        // if true, destroy enemy
        bool result = dialogueRunner.VariableStorage.TryGetValue<bool>("$action_outcome", out bool success);
        if (!result)
        {
            print("Couldn't find var");
            return;
        }

        if(!success)
        {
            print("Enemy won!");
        } else
        {
            print("Hero won!");
            if (m_CurrentEncounterAction != null)
            {
                print("Destroying enemy");
                // Not sure why this isn't working
                m_CurrentEncounterAction.DestroyMe();
                m_CurrentEncounterAction = null;
            }
        }
        m_PauseSpawning = false;
    }
}
