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
        HeroesInPlay = GameObject.FindGameObjectsWithTag("Hero");

        dialogueRunner.startNode = "CORE_AutomaticThoughts_Level01_01";
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

    public void SlowDownEncounter()
    {
        if (m_CurrentEncounterAction != null)
        {
            m_CurrentEncounterAction.GetComponent<Enemy>().SlowDown();
            m_CurrentEncounterAction.GetComponent<Enemy>().DisableCollider();
        }
    }

    public void ResetEncounterAction()
    {
        m_CurrentEncounterAction.GetComponent<Enemy>().RestoreSpeed();
        m_CurrentEncounterAction.GetComponent<Enemy>().EnableCollider();

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
            ResetEncounterAction();
        } else
        {
            print("Hero won!");
            if (Instance.m_CurrentEncounterAction != null)
            {
                print("Destroying enemy");
                // Not sure why this isn't working
                Instance.m_CurrentEncounterAction.DestroyMe();
                Instance.m_CurrentEncounterAction = null;
            }
        }
    }
}
