using System;

public class BuildError
{
	public string File { get; set; }
	public DateTime Timestamp { get; set; }
	public int LineNumber { get; set; }
	public int ColumnNumber { get; set; }
	public string Code { get; set; }
	public string Message { get; set; }

	public override string ToString()
	{
		return string.Format("[{0}, {1}({2}, {3})] {4}: {5}",
			this.Timestamp,
			this.File,
			this.LineNumber,
			this.ColumnNumber,
			this.Code,
			this.Message);
	}
}
