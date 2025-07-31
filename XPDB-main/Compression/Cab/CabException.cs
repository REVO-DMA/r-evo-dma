using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;

namespace XPDB.Compression.Cab
{
    public class CabException : ArchiveException
	{
		public CabException(string message, Exception innerException) : this(0, 0, message, innerException) { }

		public CabException(string message) : this(0, 0, message, null) { }

		public CabException() : this(0, 0, null, null) { }

		internal CabException(int error, int errorCode, string message, Exception innerException) : base(message, innerException)
		{
			Error = error;
			ErrorCode = errorCode;
		}

		internal CabException(int error, int errorCode, string message) : this(error, errorCode, message, null) { }

		protected CabException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
            ArgumentNullException.ThrowIfNull(info);
            Error = info.GetInt32("cabError");
			ErrorCode = info.GetInt32("cabErrorCode");
		}

		public int Error { get; private set; }

		public int ErrorCode { get; private set; }

        private static ResourceManager errorResources;
        internal static ResourceManager ErrorResources
		{
			get
			{
				if (errorResources == null)
                    errorResources = new ResourceManager(typeof(CabException).Namespace + ".Errors", typeof(CabException).Assembly);
                
				return errorResources;
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
            ArgumentNullException.ThrowIfNull(info);
            info.AddValue("cabError", Error);
			info.AddValue("cabErrorCode", ErrorCode);
			base.GetObjectData(info, context);
		}

		internal static string GetErrorMessage(int error, int errorCode, bool extracting)
		{
			int num = (extracting ? 2000 : 1000);
			string text = ErrorResources.GetString((num + error).ToString(CultureInfo.InvariantCulture.NumberFormat), CultureInfo.CurrentCulture);
			if (text == null)
			{
				text = ErrorResources.GetString(num.ToString(CultureInfo.InvariantCulture.NumberFormat), CultureInfo.CurrentCulture);
			}
			if (errorCode != 0)
			{
				string @string = ErrorResources.GetString("1", CultureInfo.CurrentCulture);
				text = string.Format(CultureInfo.InvariantCulture, "{0} " + @string, new object[] { text, errorCode });
			}
			return text;
		}
	}
}
