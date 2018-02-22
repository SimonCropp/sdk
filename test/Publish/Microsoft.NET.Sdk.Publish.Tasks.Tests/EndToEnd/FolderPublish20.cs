﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.NET.Sdk.Publish.Tasks.Tests.EndToEnd
{
    public class FolderPublish20
    {
        public string BaseTestDirectory
        {
            get
            {
                return Path.Combine(AppContext.BaseDirectory, nameof(FolderPublish20));
            }
        }

        public const string DotNetExeName = "dotnet";
        public const string DotNetInstallArgs = "new -i Microsoft.dotnet.web.projecttemplates.1.x::1.0.0-*";
        public const string DotNetNewAdditionalArgs = "";
        private readonly ITestOutputHelper _testOutputHelper;

        public FolderPublish20(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core")]
        [InlineData("netcoreapp2.1", "Debug", "core")]
        public void EmptyWebCore(string templateFramework, string configuration, string msBuildType)
        {
            string projectName = $"{nameof(EmptyWebCore)}_{Path.GetRandomFileName()}";

            // Arrange
            string dotNetNewArguments = $"new web --framework {templateFramework} {DotNetNewAdditionalArgs}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true, testOutputHelper: _testOutputHelper);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType);
        }


        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core")]
        [InlineData("netcoreapp2.1", "Debug", "core")]
        public void WebAPICore(string templateFramework, string configuration, string msBuildType)
        {
            string projectName = $"{nameof(WebAPICore)}_{Path.GetRandomFileName()}";

            // Arrange
            string dotNetNewArguments = $"new webapi --framework {templateFramework} {DotNetNewAdditionalArgs}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);
            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType, isStandAlone:false, resultUrl:"http://localhost:5000/api/Values");
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "none", "false")]
        [InlineData("netcoreapp2.1", "Debug", "core", "none", "false")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "false")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "true")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true")]
        public void MvcCore(string templateFramework, string configuration, string msBuildType, string auth, string useLocalDB)
        {
            string projectName = $"{nameof(MvcCore)}_{Path.GetRandomFileName()}";

            string additionalOptions = string.Empty;
            // Arrange
            if (bool.TryParse(useLocalDB, out bool localDBBool) && localDBBool)
            {
                additionalOptions = $"--use-local-db";
            }
            string dotNetNewArguments = $"new mvc --framework {templateFramework} --auth {auth} {DotNetNewAdditionalArgs} {additionalOptions}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType);
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "none", "false")]
        [InlineData("netcoreapp2.1", "Debug", "core", "none", "false")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "false")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "true")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true")]
        public void RazorCore(string templateFramework, string configuration, string msBuildType, string auth, string useLocalDB)
        {
            string projectName = $"{nameof(RazorCore)}_{Path.GetRandomFileName()}";

            string additionalOptions = string.Empty;
            // Arrange
            if (bool.TryParse(useLocalDB, out bool localDBBool) && localDBBool)
            {
                additionalOptions = $"--use-local-db";
            }
            string dotNetNewArguments = $"new razor --framework {templateFramework} --auth {auth} {DotNetNewAdditionalArgs} {additionalOptions}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType);
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "net461")]

        //[InlineData("netcoreapp2.1", "Release", "core", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "net462")]
        public void EmptyWebNET(string templateFramework, string configuration, string msBuildType, string targetFramework)
        {
            string projectName = $"{nameof(EmptyWebNET)}_{Path.GetRandomFileName()}";

            // Arrange
            string dotNetNewArguments = $"new web --framework {templateFramework} --target-framework-override {targetFramework} {DotNetNewAdditionalArgs}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType, isStandAlone:true);
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "net461")]

        //[InlineData("netcoreapp2.1", "Release", "core", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "net462")]
        public void WebAPINET(string templateFramework, string configuration, string msBuildType, string targetFramework)
        {
            string projectName = $"{nameof(WebAPINET)}_{Path.GetRandomFileName()}";

            // Arrange
            string dotNetNewArguments = $"new webapi --framework {templateFramework} --target-framework-override {targetFramework} {DotNetNewAdditionalArgs}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);
            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType, isStandAlone:true, resultUrl: "http://localhost:5000/api/Values");
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "none", "false", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "none", "false", "net461")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "false", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false", "net461")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "true", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true", "net461")]

        //[InlineData("netcoreapp2.1", "Release", "core", "none", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "none", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Release", "core", "Individual", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Release", "core", "Individual", "true", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true", "net462")]
        public void MvcNET(string templateFramework, string configuration, string msBuildType, string auth, string useLocalDB, string targetFramework)
        {
            string projectName = $"{nameof(MvcNET)}_{Path.GetRandomFileName()}";

            string additionalOptions = string.Empty;
            // Arrange
            if (bool.TryParse(useLocalDB, out bool localDBBool) && localDBBool)
            {
                additionalOptions = $"--use-local-db";
            }
            string dotNetNewArguments = $"new mvc --framework {templateFramework} --target-framework-override {targetFramework} --auth {auth} {DotNetNewAdditionalArgs} {additionalOptions}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType, isStandAlone:true);
        }

        [Theory]
        [InlineData("netcoreapp2.1", "Release", "core", "none", "false", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "none", "false", "net461")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "false", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false", "net461")]
        [InlineData("netcoreapp2.1", "Release", "core", "Individual", "true", "net461")]
        [InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true", "net461")]

        //[InlineData("netcoreapp2.1", "Release", "core", "none", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "none", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Release", "core", "Individual", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "Individual", "false", "net462")]
        //[InlineData("netcoreapp2.1", "Release", "core", "Individual", "true", "net462")]
        //[InlineData("netcoreapp2.1", "Debug", "core", "Individual", "true", "net462")]
        public void RazorNET(string templateFramework, string configuration, string msBuildType, string auth, string useLocalDB, string targetFramework)
        {
            string projectName = $"{nameof(RazorNET)}_{Path.GetRandomFileName()}";

            string additionalOptions = string.Empty;
            // Arrange
            if (bool.TryParse(useLocalDB, out bool localDBBool) && localDBBool)
            {
                additionalOptions = $"--use-local-db";
            }
            string dotNetNewArguments = $"new razor --framework {templateFramework} --target-framework-override {targetFramework} --auth {auth} {DotNetNewAdditionalArgs} {additionalOptions}";
            string testFolder = Path.Combine(BaseTestDirectory, projectName);

            // dotnet new
            int? exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotNetNewArguments, testFolder, out int? processId1, createDirectoryIfNotExists: true);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            Publish(testFolder, projectName, configuration, msBuildType, isStandAlone: true);

        }

        private void Publish(string testFolder, string projectName, string configuration, string msBuildType, bool isStandAlone = false, string resultUrl = "http://localhost:5000")
        {
            int? exitCode = 0;

            // dotnet restore
            string dotnetRestoreArguments = "restore";
            exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotnetRestoreArguments, testFolder, out int? processId2);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            // dotnet build
            string dotnetBuildArguments = "build";
            exitCode = new ProcessWrapper().RunProcess(DotNetExeName, dotnetBuildArguments, testFolder, out int? processId3);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            // msbuild publish
            string fileName = "msbuild";
            string publishOutputFolder = $"bin\\{configuration}\\PublishOutput";
            string dotnetPublishArguments = $"{projectName}.csproj /p:DeployOnBuild=true /p:Configuration={configuration} /p:PublishUrl={publishOutputFolder}";
            if (string.Equals(msBuildType, "core"))
            {
                dotnetPublishArguments = $"{fileName} {dotnetPublishArguments}";
                fileName = DotNetExeName;
            }
            exitCode = new ProcessWrapper().RunProcess(fileName, dotnetPublishArguments, testFolder, out int? processId4);
            Assert.True(exitCode.HasValue && exitCode.Value == 0);

            string publishOutputFolderFullPath = Path.Combine(testFolder, publishOutputFolder);

            Assert.True(File.Exists(Path.Combine(publishOutputFolderFullPath, "web.config")));


            try
            {
                Directory.Delete(testFolder, true);
            }
            catch { }


        }
    }
}
