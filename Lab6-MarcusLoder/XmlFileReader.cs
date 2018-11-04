using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Xml;
using System.IO;

namespace Lab6_MarcusLoder
{
    public class XmlVocabFileParserTides
    {
        // constants to use for XML element names and dictionary keys
        /*public const string SPANISH = "spanish";
		public const string ENGLISH = "english";
		public const string POS = "pos"; // part of speech
		public const string WORD = "word";   // XML element containing spanish, english, and pos elements*/

        public const string HIGH = "highlow";
        public const string HEIGHT = "pred_in_cm";
        public const string TIME = "time";
        public const string DAY = "day";
        public const string DATE = "date"; // part of speech
        public const string Item = "item";   // XML element containing spanish, english, and pos elements
                                             // This list will be filled with dictionary objects
        List<IDictionary<string, object>> tideList;   // backing variable for TideList property

        public List<IDictionary<string, object>> TideList { get { return tideList; } }

        public XmlVocabFileParserTides(Stream xmlStream)
        {
            // SimpleAdapter requires a list of JavaDictionary<string,object> objects
            tideList = new List<IDictionary<string, object>>();

            /*
			//for testing
			var item1 = new JavaDictionary<string, object> ();
			item1.Add (SPANISH, "mono");
			item1.Add (ENGLISH, "monkey");
			item1.Add ("partOfSpeech", "noun");
			vocabList.Add(item1);
			var item2 = new JavaDictionary<string, object> ();
			item2.Add (SPANISH, "agua");
			item2.Add (ENGLISH, "water");
			item2.Add (ENGLISH, "noun");
			vocabList.Add(item2);
			var item3 = new JavaDictionary<string, object> ();
			item3.Add (SPANISH, "saltar");
			item3.Add (ENGLISH, "to jump");
			item3.Add (ENGLISH, "verb");
			vocabList.Add(item3);
			*/

            // Parse the xml file and fill the list of JavaDictionary objects with vocabulary data
            using (XmlReader reader = XmlReader.Create(xmlStream))
            {
                JavaDictionary<string, object> tide = null;
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case Item:
                                // New word
                                tide = new JavaDictionary<string, object>();
                                break;
                            case TIME:
                                // Add spanish word
                                if (reader.Read() && tide != null)
                                {
                                    tide.Add(TIME, reader.Value.Trim());
                                }
                                break;
                            case DAY:
                                // Add Day
                                if (reader.Read() && tide != null)
                                {
                                    tide.Add(DAY, reader.Value.Trim());
                                }
                                break;
                            case DATE:
                                // Add Date
                                if (reader.Read() && tide != null)
                                {
                                    tide.Add(DATE, reader.Value.Trim());
                                }
                                break;
                            case HEIGHT:
                                // Add part height
                                if (reader.Read() && tide != null)
                                {
                                    tide.Add(HEIGHT, reader.Value.Trim());
                                }
                                break;
                            case HIGH:
                                // Add High or Low
                                if (reader.Read() && tide != null)
                                {
                                    if (reader.Value.Trim() == "H")
                                    {
                                        tide.Add(HIGH, "High");
                                    }
                                    else
                                    {
                                        tide.Add(HIGH, "Low");
                                    }
                                }
                                break;
                        }
                    }
                    else if (reader.Name == Item)
                    {
                        // reached </word>
                        tideList.Add(tide);
                        tide = null;
                    }

                }
            }

        }
    }
}