using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

class Program
{
	static void Main(string[] args)
	{
		string projectFileName = @"C:\Users\Sandipan\Documents\Visual Studio 2010\Projects\Base64\Base64\Base64.csproj";
		var projectCollection = new ProjectCollection();
		projectCollection.DefaultToolsVersion = "4.0";
		// Console.WriteLine("Available Toolsets: " + string.Join(", ", projectCollection.Toolsets.Select(item => item.ToolsVersion)));
		// Console.WriteLine("Toolset currently being used: " + projectCollection.DefaultToolsVersion);
		Project project = projectCollection.LoadProject(projectFileName);

		// ConsoleLogger logger = new ConsoleLogger();
		MsBuildLogger logger = new MsBuildLogger();
		// logger.Verbosity = LoggerVerbosity.Detailed;
		List<ILogger> loggers = new List<ILogger>();
		loggers.Add(logger);
		projectCollection.RegisterLoggers(loggers);

		// there are a lot of properties here, these map to the MsBuild CLI properties
		//Dictionary<string, string> globalProperties = new Dictionary<string, string>();
		//globalProperties.Add("Configuration", "Debug");
		//globalProperties.Add("Platform", "x86");
		//globalProperties.Add("OutputPath", @"D:\Output");

		//BuildParameters buildParams = new BuildParameters(projectCollection);
		//MsBuildLogger logger = new MsBuildLogger();
		//// logger.Verbosity = LoggerVerbosity.Detailed;
		//List<ILogger> loggers = new List<ILogger>() { logger };
		//buildParams.Loggers = loggers;
		//BuildRequestData buildRequest = new BuildRequestData(projectFileName, globalProperties, "4.0", new string[] { "Build" }, null);
		//BuildResult buildResult = BuildManager.DefaultBuildManager.Build(buildParams, buildRequest); // this is where the magic happens - in process MSBuild

		//buildResult.ResultsByTarget.ToList().ForEach(item => new Action(delegate() {
		//    Console.WriteLine(item.Key + ", " + item.Value.ResultCode.ToString());
		//    Console.WriteLine(string.Join(", ", item.Value.Items.ToList().Select(target => target.ItemSpec)));
		//}).Invoke());
		try {
			project.Build();
		} finally {
			projectCollection.UnregisterAllLoggers();
			Console.WriteLine("TARGETS\n" + string.Join("\n", logger.Targets.Select(item => string.Format("[{0}, {1}]", item.Name, item.Succeeded ? "Succeeded" : "Failed"))));
			Console.WriteLine("ERRORS\n" + string.Join("\n", logger.Errors));
			Console.WriteLine("WARNINGS\n" + string.Join("\n", logger.Warnings));
		}

		Console.ReadKey(true);
	}
}
