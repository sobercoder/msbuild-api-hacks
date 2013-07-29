using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class MsBuildLogger : Logger
{
	private StringBuilder errorLog = new StringBuilder();

	/// <summary>
	/// This will gather error info about the projects being built
	/// </summary>
	public IList<BuildTarget> Targets { get; private set; }
	public IList<BuildError> Errors { get; private set; }
	public IList<BuildWarning> Warnings { get; private set; }

	/// <summary>
	/// This will gather general info about the projects being built
	/// </summary>
	public IList<string> BuildDetails { get; private set; }

	/// <summary>
	/// Initialize is guaranteed to be called by MSBuild at the start of the build
	/// before any events are raised.
	/// </summary>
	public override void Initialize(IEventSource eventSource)
	{
		BuildDetails = new List<string>();

		Targets = new List<BuildTarget>();
		Errors = new List<BuildError>();
		Warnings = new List<BuildWarning>();

		// For brevity, we'll only register for certain event types.
		eventSource.ProjectStarted += new ProjectStartedEventHandler(eventSource_ProjectStarted);
		eventSource.TargetFinished += new TargetFinishedEventHandler(eventSource_TargetFinished);
		eventSource.ErrorRaised += new BuildErrorEventHandler(eventSource_ErrorRaised);
		eventSource.WarningRaised += new BuildWarningEventHandler(eventSource_WarningRaised);
		eventSource.ProjectFinished += new ProjectFinishedEventHandler(eventSource_ProjectFinished);
	}

	void eventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
	{
		BuildDetails.Add(e.Message);
	}

	void eventSource_TargetFinished(object sender, TargetFinishedEventArgs e)
	{
		BuildTarget target = new BuildTarget() {
			Name = e.TargetName,
			File = e.TargetFile,
			Succeeded = e.Succeeded,
			Outputs = e.TargetOutputs
		};

		Targets.Add(target);
	}

	void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
	{
		// BuildErrorEventArgs adds LineNumber, ColumnNumber, File, amongst other parameters
		BuildError error = new BuildError() {
			File = e.File,
			Timestamp = e.Timestamp,
			LineNumber = e.LineNumber,
			ColumnNumber = e.ColumnNumber,
			Code = e.Code,
			Message = e.Message,
		};

		Errors.Add(error);
	}

	void eventSource_WarningRaised(object sender, BuildWarningEventArgs e)
	{
		BuildWarning warning = new BuildWarning()
		{
			File = e.File,
			Timestamp = e.Timestamp,
			LineNumber = e.LineNumber,
			ColumnNumber = e.ColumnNumber,
			Code = e.Code,
			Message = e.Message,
		};

		Warnings.Add(warning);
	}

	void eventSource_ProjectFinished(object sender, ProjectFinishedEventArgs e)
	{
		Console.WriteLine(e.Message);
	}

	/// <summary>
	/// Shutdown() is guaranteed to be called by MSBuild at the end of the build, after all
	/// events have been raised.
	/// </summary>
	public override void Shutdown()
	{
		// Done logging, let go of the file
		// Errors = errorLog.ToString();
	}
}
