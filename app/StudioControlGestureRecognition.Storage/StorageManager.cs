using StudioControlGestureRecognition.Exchange.Storage;
using StudioControlGestureRecognition.Storage.Utils;
using System.Diagnostics;

namespace StudioControlGestureRecognition.Storage
{
    public class StorageManager
    {
        private readonly string _dataStorageFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SYW-SC-GR");

        private readonly string _preferencesFilePath;

        public StorageManager()
        {
            if (!Directory.Exists(_dataStorageFolderPath))
            {
                Directory.CreateDirectory(_dataStorageFolderPath);
            }

            _preferencesFilePath = Path.Combine(_dataStorageFolderPath, "preferences.json");

            if (!File.Exists(_preferencesFilePath))
            {
                File.Create(_preferencesFilePath);
            }

        }

        public Preferences LoadPreferences()
        {
            try
            {
                Preferences? preferences = JsonUtils.Deserialize<Preferences>(File.ReadAllText(_preferencesFilePath));

                return preferences ?? new Preferences();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return new Preferences();
            }
        }

        public bool StorePreferences(Preferences preferences)
        {
            try
            {
                File.WriteAllText(_preferencesFilePath, JsonUtils.Serialize(preferences));

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return false;
            }
        }
    }
}