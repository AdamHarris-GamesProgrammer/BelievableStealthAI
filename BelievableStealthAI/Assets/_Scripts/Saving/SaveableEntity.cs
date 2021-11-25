using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Harris.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [Tooltip("The unique ID is automatically generated in a scene file if " +
        "left empty. Do not set in a prefab unless you want all instances to " + 
        "be linked.")]
        //this is auto generated if it is removed
        [SerializeField] string uniqueIdentifier = "";

        //Holds the lookup of all savable entities in the game
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        //Returns the unique identifier of this object
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object SaveObject()
        {
            //Holds the saved state
            Dictionary<string, object> state = new Dictionary<string, object>();
            //Loops through all ISavables on this object
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                //Save the data associated with the ISaveable
                state[saveable.GetType().ToString()] = saveable.Save();
            }
            //Returns the dictionary of all saved data
            return state;
        }

        public void LoadObject(object state)
        {
            //Holds the objests saved states
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            ///Loops through each saveable in this object
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                //Gets the type of the save data
                string typeString = saveable.GetType().ToString();
                //Checks we have this data
                if (stateDict.ContainsKey(typeString))
                {
                    //Loads the data into the component
                    saveable.Load(stateDict[typeString]);
                }
            }
        }

        //Only wants to be ran in editor as this handles generating the unique id
#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            //Make a new serialized object based on this and then get the uniqueIdentifier property from it
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            //If the UID is null or empty or is not unique
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                //then generate a new id
                property.stringValue = System.Guid.NewGuid().ToString();
                //Apply the id change, and save it
                serializedObject.ApplyModifiedProperties();
            }

            //Set the lookup for it
            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            //Checks if we contain this key
            if (!globalLookup.ContainsKey(candidate)) return true;

            //check if the lookup is this object (if so it's allowed)
            if (globalLookup[candidate] == this) return true;

            //remove the Id if it is not needed
            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                //ID accepted
                return true;
            }

            //if the lookup is not the same as the passed in uid then remove it
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                //ID Accepted
                return true;
            }

            //ID not accepted
            return false;
        }
    }
}