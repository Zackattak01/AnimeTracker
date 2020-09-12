using System;
using System.Collections.Generic;
using System.Text;

namespace AnimeTracker
{
	public class AnimeInfo
	{
		public string Name { get; set; }
		public string SeasonCount { get; set; }
		public string EpisodeCount { get; set; }
		public string Url { get; set; }

		public AnimeInfo(string name, int seasonCount, int episodeCount)
		{
			Name = name;
			SeasonCount = seasonCount.ToString();
			EpisodeCount = episodeCount.ToString();
		}

		public AnimeInfo(string name, string seasonCount, string episodeCount, string url = "")
		{
			Name = name;
			SeasonCount = seasonCount;
			EpisodeCount = episodeCount;

			Url = url;
		}

		//this is here because newtonsoft.json was complaining
		public AnimeInfo()
		{

		}
	}
}
