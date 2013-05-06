using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PuzzlePathDimension {
  class LevelSaver {

    //long method i know i will shorten it soon
    public static void SaveLevel(EditableLevel level) {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      String name = Configuration.UserDataPath + Path.DirectorySeparatorChar + "Level" + Path.DirectorySeparatorChar + "Custom.xml";

      XmlWriter writer = XmlWriter.Create(name, settings);
      writer.WriteStartDocument();
      writer.WriteStartElement("level");
      writer.WriteAttributeString("name", "Custom Level");
      writer.WriteAttributeString("balls", level.Attempts.ToString());
      writer.WriteAttributeString("par-seconds", level.ParTime.ToString());
      writer.WriteAttributeString("toolbox-types", "RBHV");
      foreach (Platform platform in level.MoveablePlatforms) {
        writer.WriteStartElement("platform");
        writer.WriteAttributeString("breakable", platform.Breakable.ToString());
        writer.WriteAttributeString("x", platform.Origin.X.ToString());
        writer.WriteAttributeString("y", platform.Origin.Y.ToString());
        writer.WriteAttributeString("width", platform.Width.ToString());
        writer.WriteAttributeString("height", platform.Height.ToString());
        writer.WriteEndElement();
      }
      foreach (Platform platform in level.Platforms) {
        writer.WriteStartElement("platform");
        writer.WriteAttributeString("breakable", platform.Breakable.ToString());
        writer.WriteAttributeString("x", platform.Origin.X.ToString());
        writer.WriteAttributeString("y", platform.Origin.Y.ToString());
        writer.WriteAttributeString("width", platform.Width.ToString());
        writer.WriteAttributeString("height", platform.Height.ToString());
        writer.WriteEndElement();
      }
      writer.WriteStartElement("goal");
      writer.WriteAttributeString("x", level.Goal.Origin.X.ToString());
      writer.WriteAttributeString("y", level.Goal.Origin.Y.ToString());
      writer.WriteEndElement();

      writer.WriteStartElement("launcher");
      writer.WriteAttributeString("x", level.Launcher.Position.X.ToString());
      writer.WriteAttributeString("y", level.Launcher.Position.Y.ToString());
      writer.WriteEndElement();

      foreach(DeathTrap deathTrap in level.DeathTraps){
        writer.WriteStartElement("deathtrap");
        writer.WriteAttributeString("x", deathTrap.Origin.X.ToString());
        writer.WriteAttributeString("y", deathTrap.Origin.Y.ToString());
        writer.WriteEndElement();
      }

      foreach (Treasure treasure in level.Treasures) {
        writer.WriteStartElement("treasure");
        writer.WriteAttributeString("x", treasure.Origin.X.ToString());
        writer.WriteAttributeString("y", treasure.Origin.Y.ToString());
        writer.WriteEndElement();
      }

      writer.WriteEndDocument();
      writer.Flush();
      writer.Close();
    }
  }
}
