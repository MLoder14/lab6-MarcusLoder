using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.IO;

namespace Lab6_MarcusLoder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : ListActivity
    {
        Tide[] periods;


        // Overriding OnCreate in the Activity super-class
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            

            // Development testing: load data into the ArrayAdapter by filling the array with Flora objects with "hard-coded" names
            /*
			items = new Flora[] {
				new Flora { Name = "Vegetables"},
				new Flora { Name = "Fruits"},
				new Flora { Name = "Flower Buds"},
				new Flora { Name = "Legumes"},
				new Flora { Name = "Bulbs"},
				new Flora { Name = "Tubers"}
			};
			*/

            // Load data into the ArrayAdapter by filling the array with Flora objects with names read from a file
            periods = LoadListOfTidesFromAssets();

            // Assign our ArrayAdapter to the ListActivity's ListAdapter property
            ListAdapter = new HomeScreenAdapter(this, items);

            // This is all you need to do to enable fast scrolling
            ListView.FastScrollEnabled = true;

        }

        // Overriding an event handler in the ListActivity super-class
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var tides = periods[position];
            Android.Widget.Toast.MakeText(this, tides.ToString(), Android.Widget.ToastLength.Short).Show();)
        }

        private Tide[] LoadListofTidesFromAssets()
        {
            var tides = new List<Tide>();
            var seedDataSteam = Assets.Open(@"TideData.txt");
            using (var reader = new StreamReader(seedDataSteam))
            {
                while (!reader.EndOfStream)
                {
                    var tide = new Tide { TypeandName = reader.ReadLine() };
                    tides.Add(tide);
                }
            }
            tides.Sort((x,y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));

            periods = tides.ToArray();

            return periods;
        }
    }
}

/************************************************************************
   ************************  HomeScreenAdapter class  **********************
   *************************************************************************/
public class Adapter : ArrayAdapter<string>
{
    string[] periods;
    Activity context;

    public Adapter(Activity context, string[] periods) : base()
    {
        this.context = context;
        this.periods = periods;
    }

    public override long GetItemId(int position)
    {
        return position; // base.GetItemId(position);
    }

    public override string this[int position]
    {
        get { return periods[position]; }
    }

    public override int Count
    {
        get { return periods.Length; }
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        View view = convertView; // re-use an existing view, if one is available
        if (view == null) // otherwise create a new one
            view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
        view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = periods[position];
        return view;
        //return base.GetView(position, convertView, parent);
    }

    /* -- Code for the ISectionIndexer implementation follows -- */

    String[] sections;
    Java.Lang.Object[] sectionsObjects;
    Dictionary<string, int> alphaIndex;

    public int GetPositionForSection(int section)
    {
        return alphaIndex[sections[section]];
    }

    public int GetSectionForPosition(int position)
    {
        return 1;
    }

    public Java.Lang.Object[] GetSections()
    {
        return sectionsObjects;
    }

    private void BuildSectionIndex()
    {
        alphaIndex = new Dictionary<string, int>();
        for (var i = 0; i < periods.Length; i++)
        {
            // Use the first character in the name as a key.
            var key = periods[i].Name.Substring(0, 1);
            if (!alphaIndex.ContainsKey(key))
            {
                alphaIndex.Add(key, i);
            }
        }

        sections = new string[alphaIndex.Keys.Count];
        alphaIndex.Keys.CopyTo(sections, 0);
        sectionsObjects = new Java.Lang.Object[sections.Length];
        for (var i = 0; i < sections.Length; i++)
        {
            sectionsObjects[i] = new Java.Lang.String(sections[i]);
        }
    }

}


/************************************************************************
************************  HomeScreenAdapter class  **********************
*************************************************************************/

/// <summary>
///   A simple class for holding the data that is read from VegeData.txt
/// </summary>
public class Flora
{
    public string Name { get; set; }
    public override string ToString()
    {
        return Name;
    }
} 
}