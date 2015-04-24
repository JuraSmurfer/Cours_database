using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
	/// <summary>
	/// V tato třída implementuje bezpečné a opakující se operace
	/// </summary>
	public abstract class Utils
	{
		/// <summary>Vrati cislo</summary>
		protected int GetInt(string input)
		{
			int num = 0;
			if (string.IsNullOrEmpty(input))
				return num;

			return int.TryParse(input, out num) ? num : 0;
		}
		/// <summary>Vrati datum</summary>
		protected DateTime GetDateTime(string input)
		{
			DateTime date;
			var culture = CultureInfo.InvariantCulture;
			var format = "d. M. yyyy";
			var dateTime = DateTime.ParseExact("1. 1. 1993", format, culture);

			if (string.IsNullOrEmpty(input))
				return dateTime;
			return DateTime.TryParse(input, out date) ? date : dateTime;
			//return DateTime.TryParse(input, out date) ? DateTime.ParseExact(input, format, culture) : dateTime;
		}
		/// <summary>Vrati string</summary>
		protected string GetString(EPes input) { return input.ToString(); }
		protected string GetString(int input) { return input.ToString(); }
		protected string GetString(string input) { return input; }
		protected string GetString(DateTime input) { return input.ToString(CultureInfo.InvariantCulture); }
	}
}
