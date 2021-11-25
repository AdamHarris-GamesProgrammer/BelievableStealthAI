namespace Harris.Saving
{
    public interface ISaveable
    {
        //Saves the data associated with a particular component
        object Save();

        //Loads the data associated with a particular component
        void Load(object state);
    }
}