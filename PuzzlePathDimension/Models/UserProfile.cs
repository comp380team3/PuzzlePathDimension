﻿using System;
using System.Xml.Serialization;

namespace PuzzlePathDimension {
  public class LevelStatus {
    [XmlAttribute("completed")]
    public bool Completed { get; set; }

    [XmlIgnore]
    public TimeSpan FastestTime { get; set; }

    [XmlAttribute("score")]
    public int Score { get; set; }


    [XmlAttribute("fastest-time")]
    public int FastestTimeInSeconds {
      get { return (int)FastestTime.TotalSeconds; }
      set { FastestTime = TimeSpan.FromSeconds(value); }
    }
  }

  [XmlRoot("Profile")]
  public class UserProfile {
    [XmlElement("Progress")]
    public SerializableDictionary<string, LevelStatus> Progress { get; set; }

    [XmlElement("Preferences")]
    public UserPrefs Prefs { get; set; }
  }
}