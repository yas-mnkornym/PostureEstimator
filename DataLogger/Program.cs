using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			var fileName = "";

			ISensorReader reader = null;
			using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
			using(var writer = new AxisWriter(fs, true)){
				while(reader.HasNext()){
					var val = reader.ReadNext();
					writer.Write(val);
				}
				writer.Flush();
			}
		}
	}

	interface ISensorReader
	{
		bool HasNext();
		SensorValue ReadNext();
	}

	public struct SensorValue
	{
		public int X;
		public int Y;
		public double Value;
	}

	class AxisWriter : IDisposable
	{
		TextWriter writer_ = null;

		public AxisWriter(Stream stream, bool leaveOpen)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			writer_ = new StreamWriter(stream, Encoding.UTF8, 2048, leaveOpen);
		}

		public void Write(SensorValue value)
		{
			Write(value.X, value.Y, value.Value);
		}

		public void Write(int x, int y, double value = 0)
		{
			writer_.WriteLine(string.Format("{0}\t{1}\t{2}", x, y, value));
		}

		public void Flush()
		{
			writer_.Flush();
		}

		bool isDisposed_ = false;
		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed_) { return; }
			if (disposing) {
				if (writer_ != null) {
					writer_.Dispose();
					writer_ = null;
				}
			}
			isDisposed_ = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}	
}
