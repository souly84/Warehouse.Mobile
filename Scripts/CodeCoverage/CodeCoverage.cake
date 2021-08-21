var target = Argument("target", "Report");

#addin nuget:?package=Cake.Coverlet&version=2.4.2
#tool nuget:?package=ReportGenerator&version=4.6.4

/*  Specify the relative paths to your tests projects here. */
var testProjectsRelativePaths = new string[]
{
    "../../StoreMobile.AppService.Droid/StoreMobile.AppService.UnitTests/StoreMobile.AppService.UnitTests.csproj"
};

/*  Change the output artifacts and their configuration here. */
var parentDirectory = Directory("..");
var coverageDirectory = parentDirectory + Directory("CodeCoverageResults");
var cuberturaFileName = "results";
var cuberturaFileExtension = ".cobertura.xml";
var reportTypes = "Html";
var coverageFilePath = coverageDirectory + File(cuberturaFileName + cuberturaFileExtension);
var jsonFilePath = coverageDirectory + File(cuberturaFileName + ".json");;

Task("Clean")
    .Does(() =>
{
    if (!DirectoryExists(coverageDirectory))
        CreateDirectory(coverageDirectory);
    else
        CleanDirectory(coverageDirectory);
});

Task("Test")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var testSettings = new DotNetCoreTestSettings()
    {
        // 'trx' files will be used to publish the results of tests' execution in an Azure DevOps pipeline.
        ArgumentCustomization = args => args.Append($"--logger trx")
    };

    var coverletSettings = new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputDirectory = coverageDirectory,
        CoverletOutputName = cuberturaFileName
    };

    if (testProjectsRelativePaths.Length == 1)
    {
        coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.cobertura;
        DotNetCoreTest(testProjectsRelativePaths[0], testSettings, coverletSettings);
    }
    else if (testProjectsRelativePaths.Length > 0)
    {
        DotNetCoreTest(testProjectsRelativePaths[0], testSettings, coverletSettings);

        coverletSettings.MergeWithFile = jsonFilePath;

        for (int i = 1; i < testProjectsRelativePaths.Length; i++)
        {
            if (i == testProjectsRelativePaths.Length - 1)
            {
                coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.cobertura;
            }

            DotNetCoreTest(testProjectsRelativePaths[i], testSettings, coverletSettings);
        }
    }
});

Task("Report")
    .IsDependentOn("Test")
    .Does(() =>
{
    var reportSettings = new ReportGeneratorSettings
    {
        ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
    };
    ReportGenerator(coverageFilePath, coverageDirectory, reportSettings);
});

RunTarget(target);