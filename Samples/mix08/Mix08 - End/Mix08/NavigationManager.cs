using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace Mix08
{
  /// <summary>
  /// 
  /// </summary>
  public class NavigationManager
  {
    private const string _historyPagePath = "NavigationHistory.html";
    public const char QueryParamDelimiter = '&';

    public enum LinkMode
    {
      /// <summary>
      /// History will not be reflected in the browser's querystring.
      /// </summary>
      None,
      /// <summary>
      /// History will be reflected in the browser's querystring.
      /// </summary>
      Deep
    };

    private List<INavigationState> _states;

    private LinkMode _mode = LinkMode.None;

    private string _currentBookmark = string.Empty;

    private string _historyFrameId;
    private string _historyDivId;

    /// <summary>
    /// Used to poll for browser querystring changes.
    /// </summary>
    private DispatcherTimer _timer;

    #region Construction/Singleton Accessor
    protected NavigationManager()
    {
      _historyDivId = HtmlPage.Plugin.Id + "_historyDiv";
      _historyFrameId = HtmlPage.Plugin.Id + "_historyFrame";

      InitializeHistoryDiv();
      InitializeHistoryFrame();

      _states = new List<INavigationState>();

      _timer = new DispatcherTimer();
      _timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
      _timer.Tick += OnTimerTick;
      _timer.Start();
    }

    #region Singleton
    /// <summary>
    /// Private singleton class.  Static construction.
    /// </summary>
    private sealed class NavigationManagerSingleton
    {
      private static NavigationManager _instance = new NavigationManager();

      static NavigationManagerSingleton() { }

      public static NavigationManager Instance
      {
        get { return _instance; }
      }
    }

    public static NavigationManager Instance
    {
      get
      {
        return NavigationManagerSingleton.Instance;
      }
    }
    #endregion

    #region Init Containers

    private HtmlElement InitializeHistoryDiv()
    {
      HtmlElement historyDiv = HtmlPage.Document.GetElementById(_historyDivId);

      if (historyDiv == null)
      {
        historyDiv = HtmlPage.Document.CreateElement("div");
        historyDiv.Id = _historyDivId;
        historyDiv.SetStyleAttribute("display", "none");
        HostElement.AppendChild(historyDiv);
      }

      return historyDiv;
    }

    private HtmlElement InitializeHistoryFrame()
    {
      HtmlElement historyFrame = HtmlPage.Document.GetElementById(_historyFrameId);

      if (historyFrame == null)
      {
        historyFrame = HtmlPage.Document.CreateElement("iframe");
        historyFrame.Id = _historyFrameId;
        historyFrame.SetAttribute("name", _historyFrameId); // name since FF won't resolve by I
        historyFrame.SetAttribute("src", _historyPagePath);
        historyFrame.SetStyleAttribute("display", "none");

        HostElement.AppendChild(historyFrame);
      }

      return historyFrame;
    }
    #endregion

    #endregion


    #region Save/Update
    public void Save()
    {
      _timer.Stop();

      StringBuilder anchorBuilder = new StringBuilder();

      foreach (INavigationState state in _states)
      {
        NavigationPropertyDictionary properties = state.GetState();
        anchorBuilder.Append(properties.GetEncodedPropertyString());
        anchorBuilder.Append(QueryParamDelimiter);
      }

      string anchor = anchorBuilder.ToString();

      BrowserHistoryFrameQuery = anchor;
      BrowserBookmark = anchor;
      _currentBookmark = anchor;

      _timer.Start();
    }

    private void UpdateAllStates()
    {
      List<NavigationPropertyDictionary> properties = ParseProperties(_currentBookmark);

      foreach (INavigationState state in _states)
      {
        state.LoadState(properties.GetStateDictionary(state));
      }
    }
    #endregion

    #region Register/Unregister
    internal void Register(INavigationState state)
    {
      if (!_states.Contains(state))
      {
        _states.Add(state);
      }
    }

    internal void Unregister(INavigationState state)
    {
      _states.Remove(state);
    }
    #endregion


    #region ParseProperties
    private List<NavigationPropertyDictionary> ParseProperties(string query)
    {
      List<NavigationPropertyDictionary> properties = new List<NavigationPropertyDictionary>();
      string[] propertyStrings = query.Split(QueryParamDelimiter);
      foreach (string property in propertyStrings)
      {
        if (!string.IsNullOrEmpty(property))
          try
          {
            properties.Add(new NavigationPropertyDictionary(property));
          }
          catch (ArgumentException) { /* ignore unrecognized properties */ }
      }

      return properties;
    }
    #endregion

    #region OnTimerTick
    private void OnTimerTick(object sender, EventArgs e)
    {
      if (BrowserHistoryFrameQuery != _currentBookmark)
      {
        _currentBookmark = BrowserHistoryFrameQuery;

        BrowserBookmark = _currentBookmark;
        UpdateAllStates();
      }
      else if ((_mode == LinkMode.Deep) && (BrowserBookmark != _currentBookmark))
      {
        _currentBookmark = BrowserBookmark;

        BrowserHistoryFrameQuery = _currentBookmark;
        UpdateAllStates();
      }
    }
    #endregion

    #region Html Accessors
    private void EnsureAnchorExists(string name)
    {
      if (HtmlPage.Document.GetElementById(name) == null)
      {
        HtmlElement anchor = HtmlPage.Document.CreateElement("a");
        anchor.SetAttribute("name", name);
        HistoryDiv.AppendChild(anchor);
      }
    }

    private HtmlElement HostElement
    {
      get
      {
        return HtmlPage.Document.GetElementById(HtmlPage.Plugin.Id).Parent;
      }
    }

    private HtmlElement HistoryDiv
    {
      get
      {
        return HtmlPage.Document.GetElementById(_historyDivId);
      }
    }

    private HtmlElement HistoryFrameElement
    {
      get
      {
        return HtmlPage.Document.GetElementById(_historyFrameId);
      }
    }

    private HtmlWindow BrowserHistoryFrame
    {
      get
      {
        return (HtmlWindow)((ScriptObject)HtmlPage.Window.GetProperty("frames")).GetProperty(_historyFrameId);
      }
    }


    private string BrowserHistoryFrameQuery
    {
      get
      {
        string href = (string)((ScriptObject)BrowserHistoryFrame.GetProperty("location")).GetProperty("href");

        int queryStartIndex = href.LastIndexOf('?');

        return (queryStartIndex > 0 ? href.Substring(href.LastIndexOf('?') + 1) : string.Empty);
      }
      set
      {
        HistoryFrameElement.SetAttribute("src", _historyPagePath + "?" + value);
      }
    }

    private string BrowserBookmark
    {
      get
      {
        return System.Windows.Browser.HtmlPage.Window.CurrentBookmark;
      }
      set
      {
        if (_mode == LinkMode.Deep)
        {
          System.Windows.Browser.HtmlPage.Window.NavigateToBookmark(value);
        }
      }
    }
    #endregion

    #region Properties
    public LinkMode LinkingMode
    {
      get
      {
        return _mode;
      }
      set
      {
        _mode = value;
      }
    }
    #endregion
  }

  /// <summary>
  /// Implemented by classes that wish to have their history tracked by a NavigationManager.
  /// </summary>
  public interface INavigationState
  {
    NavigationPropertyDictionary GetState();
    void LoadState(NavigationPropertyDictionary properties);
    string Id { get; }
  }

  /// <summary>
  /// Contains navigation property information for a single INavigationState.
  /// </summary>
  public class NavigationPropertyDictionary : Dictionary<string, string>
  {

    private const char PropertyDelimiter = ':';
    private const char KeyValueDelimiter = '=';
    private static readonly string EncodedStatePropertiesRegex = "^[A-Za-z0-9%]+" + PropertyDelimiter + "([A-Za-z0-9%]+" + KeyValueDelimiter + "[A-Za-z0-9%]*" + PropertyDelimiter + ")*$";

    private string _parentStateId;

    public NavigationPropertyDictionary(INavigationState parentState)
      : base()
    {
      _parentStateId = parentState.Id;
    }

    public NavigationPropertyDictionary(int capacity)
      : base(capacity)
    {

    }

    public string ParentStateId
    {
      get
      {
        return _parentStateId;
      }
    }

    public NavigationPropertyDictionary(string encodedStateProperties)
      : base()
    {
      //expected format: <parentStateId> PROPERTYDELIMITER <key0> KEYVALUEDELIMITER <val0> PROPERTYDELIMITER <key1> ...

      if (!Regex.Match(encodedStateProperties, EncodedStatePropertiesRegex, RegexOptions.Singleline).Success)
      {
        throw new ArgumentException("Incorrect input format.", "encodedStateProperties");
      }

      _parentStateId = HttpUtility.UrlDecode(encodedStateProperties.Substring(0, encodedStateProperties.IndexOf(PropertyDelimiter)));

      string[] properties = encodedStateProperties.Substring(encodedStateProperties.IndexOf(PropertyDelimiter) + 1).Split(PropertyDelimiter);

      foreach (string property in properties)
      {
        if (!string.IsNullOrEmpty(property))
        {
          int separatorIndex = property.IndexOf(KeyValueDelimiter);

          string key = HttpUtility.UrlDecode(property.Substring(0, separatorIndex));
          string value = HttpUtility.UrlDecode(property.Substring(separatorIndex + 1, property.Length - 1 - separatorIndex));

          this.Add(key, value);
        }
      }
    }

    public string GetEncodedPropertyString()
    {
      StringBuilder propertyString = new StringBuilder();

      // Append state id
      propertyString.Append(_parentStateId);
      propertyString.Append(PropertyDelimiter);

      // Append each property
      foreach (KeyValuePair<string, string> kvp in this)
      {
        if (!string.IsNullOrEmpty(kvp.Value))
        {
          propertyString.Append(HttpUtility.UrlEncode(kvp.Key));
          propertyString.Append(KeyValueDelimiter);
          propertyString.Append(HttpUtility.UrlEncode(kvp.Value));
          propertyString.Append(PropertyDelimiter);
        }
      }

      return propertyString.ToString();
    }

  }

  public static class NavigationExtensions
  {
    public static void RegisterForNavigationManagement(this INavigationState state)
    {
      NavigationManager.Instance.Register(state);
    }

    public static void UnregisterForNavigationManagement(this INavigationState state)
    {
      NavigationManager.Instance.Unregister(state);
    }

    internal static NavigationPropertyDictionary GetStateDictionary(this List<NavigationPropertyDictionary> properties, string stateId)
    {
      //var stuff = properties.Single(prop => prop.ParentStateId == stateId);

      foreach (NavigationPropertyDictionary dictionary in properties)
      {
        if (dictionary.ParentStateId.Equals(stateId))
        {
          return dictionary;
        }
      }

      return null;
    }

    internal static NavigationPropertyDictionary GetStateDictionary(this List<NavigationPropertyDictionary> properties, INavigationState state)
    {
      if (properties.ContainsState(state))
      {
        return properties.GetStateDictionary(state.Id);
      }
      else
      {
        return new NavigationPropertyDictionary(state);
      }
    }

    internal static bool ContainsState(this List<NavigationPropertyDictionary> properties, string stateId)
    {

      foreach (NavigationPropertyDictionary dictionary in properties)
      {
        if (dictionary.ParentStateId.Equals(stateId))
        {
          return true;
        }
      }

      return false;
    }

    internal static bool ContainsState(this List<NavigationPropertyDictionary> properties, INavigationState state)
    {
      return properties.ContainsState(state.Id);
    }
  }
}
