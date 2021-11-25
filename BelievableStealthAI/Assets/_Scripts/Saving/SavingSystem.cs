using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harris.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            //Creates a dictionary to hold the unique ids and the saved objects associated with them
            Dictionary<string, object> state = LoadFile(saveFile);
            //Save state
            SaveState(state);
            //Save the state to the file
            SaveFile(saveFile, state);
        }

        public void Delete(string saveFile)
        {
            //Delete the file passed in
            File.Delete(GetPathFromSaveFile(saveFile));
        }


        public void Load(string saveFile)
        {
            //Load the state from the desired file
            LoadState(LoadFile(saveFile));
        }


        private Dictionary<string, object> LoadFile(string saveFile)
        {
            //Get the path from the provided save file
            string path = GetPathFromSaveFile(saveFile);
            //if the file does not exist, create a new dictionary
            if (!File.Exists(path)) return new Dictionary<string, object>();

            //the file does exist

            //Open the file with a binary formatter
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                //Deserialize the files contents into the dictionary
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFile, object state)
        {
            //Get the constructed path
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            //Create the file 
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                //Use a binary formatter to serialize the state data into the file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void SaveState(Dictionary<string, object> state)
        {
            //Loop through all saveable entities and save the object's data
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.SaveObject();
            }

            //Stores the current scene
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void LoadState(Dictionary<string, object> state)
        {
            //Loops through all saveable entities and loads the state into those objects
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id)) saveable.LoadObject(state[id]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            //Combine the provided path with the file extension
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}