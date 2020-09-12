using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace AnimeTracker
{
	public static class JsonManager
	{
		private static string path = Environment.CurrentDirectory + "/save.json";

		public static void Save(object saveData)
		{
			string jsonString = JsonConvert.SerializeObject(saveData);

			File.WriteAllText(path, jsonString);
		}

		public static Dictionary<int, List<AnimeInfo>> Load()
		{
			if (!File.Exists(path))
				File.Create(path).Dispose();

			string jsonString = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Dictionary<int, List<AnimeInfo>>>(jsonString);
		}
	}
}
