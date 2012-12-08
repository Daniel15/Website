using System.Web.Script.Serialization;

namespace Daniel15.Web.Models.Home
{
	/// <summary>
	/// Google Talk chat status
	/// </summary>
	public class ChatStatusModel
	{
		public string Icon { get; set; }
		public string StatusText { get; set; }
		public string ChatUrl { get; set; }

		[ScriptIgnore]
		public ChatStatus Status { get; set; }

		/// <summary>
		/// A hack for the <see cref="JavaScriptSerializer"/> which serializes enums as their 
		/// numeric value :(
		/// </summary>
		public string StatusString
		{
			get { return Status.ToString(); }
		}
		

		public enum ChatStatus
		{
			Online,
			Busy,
			Offline,
		}
	}
}