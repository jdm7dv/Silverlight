using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.IO;

namespace Mix08
{
  public static class LocalStorage
  {
    private static IsolatedStorageFile _isoStore;

    // Gives public access to the raw IsolatedStorageFile for the app
    public static IsolatedStorageFile IsoStore
    {
      get
      {
        if (_isoStore == null)
        {
          _isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        }
        return _isoStore;
      }
    }

    // Returns the amount of free space available to the app
    public static long FreeSapceInBytes
    {
      get
      {
        return IsoStore.AvailableFreeSpace;
      }
    }

    // Returns a populated string dictionary that has been stored in the App's IsoStore
    public static Dictionary<string, string> ReadDictionary(string name)
    {
      Dictionary<string, string> dict = new Dictionary<string, string>();

      if (IsoStore.FileExists(name))
      {
        using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(name, FileMode.Open, IsoStore))
        {
          using (StreamReader reader = new StreamReader(isoStream))
          {
            string line;
            string[] kvp;
            // Read and display lines from the file until the end of the file is reached.
            while ((line = reader.ReadLine()) != null)
            {
              kvp = line.Split(',');
              dict.Add(kvp[0], kvp[1]);
            }

            return dict;
          }
        }
      }

      return null;
    }

    // Stores a string dictionary in the App's IsoStore
    public static void WriteDictionary(string name, Dictionary<string, string> dict)
    {
      // Destroy any existing entry in this slot
      DeleteItem(name);

      using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(name, FileMode.Create, IsoStore))
      {
        using (StreamWriter writer = new StreamWriter(isoStream))
        {
          foreach (KeyValuePair<string, string> kvp in dict)
          {
            // TODO: this isn't going to work well with strings which have a comma in them
            writer.WriteLine("{0}, {1}", kvp.Key, kvp.Value);
          }
        }
      }
    }

    // Returns a string  that has been stored in the App's IsoStore
    public static string ReadString(string name)
    {
      if (IsoStore.FileExists(name))
      {
        using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(name, FileMode.Open, IsoStore))
        {
          using (StreamReader reader = new StreamReader(isoStream))
          {
            return reader.ReadToEnd();
          }
        }
      }

      return null;
    }

    // Stores a string in the App's IsoStore
    public static void WriteString(string name, string value)
    {
      // Destroy any existing dictionary in this slot
      DeleteItem(name);

      using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(name, FileMode.Create, IsoStore))
      {
        using (StreamWriter writer = new StreamWriter(isoStream))
        {
          writer.WriteLine(value);
        }
      }
    }

    // Returns true if the string specified corresponds to a file stream in the App's IsoStore
    public static bool ItemExists(string name)
    {
      return IsoStore.FileExists(name);
    }

    // Removes the specified file stream from the App's IsoStore
    public static void DeleteItem(string name)
    {
      if (IsoStore.FileExists(name))
      {
        IsoStore.DeleteFile(name);
      }
    }
  }
}
