using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quests : MonoBehaviour
{
    public Text questText;

    [System.Serializable]
    public class Quest
    {
        public string name;
        public string description;
        public bool complete;
    }

    public bool allComplete;
    public List<Quest> houseQuests;

    // Start is called before the first frame update
    void Start()
    {
        UpdateQuests();
    }

    public void CompleteQuest(string questToComplete)
    {
        foreach (Quest q in houseQuests)
        {
            if (q.name == questToComplete)
            {
                if (q.name == "Hora do Heroi" || q.name == "TROCA DE ROUPA")
                {
                    FindObjectOfType<Controle_player>().home = false;
                }
                q.complete = true;
                UpdateQuests();
            }
        }
    }

    public void UpdateQuests()
    {
        int completes = 0;
        questText.text = "";
        for (int i = 0; i < houseQuests.Count; i++)
        {
            string isComplete = "";
            if (houseQuests[i].name != "")
            {
                if (houseQuests[i].complete)
                {
                    isComplete = "Completa";
                }
                else
                {
                    isComplete = "Incompleta";

                }
            }
            else
            {
                isComplete = "";
            }

            questText.text += houseQuests[i].name + "\n" + houseQuests[i].description + "\n" + isComplete + "\n\n";

            if (houseQuests[i].complete)
            {
                completes++;
            }
        }

        if (completes == houseQuests.Count)
        {
            allComplete = true;
        }
    }
}
