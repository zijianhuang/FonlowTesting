using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PowerShellTests
    {
        [Fact]
        public void TestBasic()
        {
            using var ps = PowerShell.Create();
            ps.AddCommand("Get-Date");
            var rs = ps.Invoke();
            Assert.NotEmpty(rs);
        }

        [Fact]
        public void TestLaunchNotepad()
        {
            using var ps = PowerShell.Create();
            ps.AddCommand("Start-Process").AddArgument("notepad").AddParameter("-passthru");//.AddParameter("-Wait");
            var rs = ps.Invoke();
            Assert.NotEmpty(rs);
            Process process = rs[0].BaseObject as Process;

            using var ps2 = PowerShell.Create();
            ps2.AddCommand("Stop-Process").AddParameter("-Id", process.Id);
            ps2.Invoke();
        }

        [Fact]
        public void TestCopyItemWithArguments()
        {
            using var ps = PowerShell.Create();
            ps.AddCommand("Copy-Item").AddArgument("../../../PowerShellTests.cs").AddArgument("./_PowerShellTests.cs"); // argument cannot have space
            var rs = ps.Invoke();
            Assert.Empty(rs);
            Assert.False(ps.HadErrors);
            Assert.Empty(ps.Streams.Error);
        }

        [Fact]
        public void TestCopyItemWithParameters()
        {
            using var ps = PowerShell.Create();
            ps.AddCommand("Copy-Item").AddParameter("-Path", "../../../PowerShellTests.cs").AddParameter("-Destination", "./CopiedPowerShellTests.cs");
            var rs = ps.Invoke();
            Assert.Empty(rs);
            Assert.False(ps.HadErrors);
            Assert.Empty(ps.Streams.Error);
        }

        [Fact]
        public void TestCopyItemNotExists()
        {
            using var ps = PowerShell.Create();
            ps.AddCommand("Copy-Item").AddParameter("-Path", "../../../PowerShellKKK.cs").AddParameter("-Destination", "./CopiedPowerShellTests.cs");
            var rs = ps.Invoke();
            Assert.Empty(rs);
            Assert.True(ps.HadErrors);
            Assert.NotEmpty(ps.Streams.Error);
        }

        [Fact]
        public void TestCopyItemWithScript()
        {
            using var ps = PowerShell.Create();
            ps.AddScript("Copy-Item ../../../PowerShellTests.cs ./A_PowerShellTests.cs");
            var rs = ps.Invoke();
            Assert.Empty(rs);
            Assert.False(ps.HadErrors);
            Assert.Empty(ps.Streams.Error);
        }

        //[Fact]
        //public void TestCopyItemWithStartJobThrowsError()
        //{
        //    using var ps = PowerShell.Create();
        //    ps.AddScript("Start-ThreadJob -ScriptBlock {Copy-Item ../../../PowerShellTests.cs ./A_PowerShellTests.cs}");
        //    var rs = ps.Invoke();
        //    Assert.Empty(rs);
        //    Assert.True(ps.HadErrors); //not supported as of .NET 8, Microsoft.PowerShell.SDK v7.4.3
        //    //Assert.Empty(ps.Streams.Error);
        //}

        //[Fact]
        //public void TestThreadJobThrowsError()//not supported as of .NET 8, Microsoft.PowerShell.SDK v7.4.3
        //{
        //    using var ps = PowerShell.Create();
        //    ps.AddCommand("Import-Module").AddArgument("ThreadJob").AddCommand("Start-ThreadJob")
        //    .AddParameter("-ScriptBlock", "{Copy-Item ../../../PowerShellTests.cs ./A_PowerShellTests.cs}");
        //    var rs = ps.Invoke();
        //    //Assert.Empty(rs);
        //    //Assert.True(ps.HadErrors);
        //    //Assert.NotEmpty(ps.Streams.Error);
        //}

        //[Fact]
        //public void TestThreadJobThrows()//not supported as of .NET 8, Microsoft.PowerShell.SDK v7.4.3
        //{
        //    using Runspace runSpace = RunspaceFactory.CreateRunspace();
        //    runSpace.Open();
        //    var pipeline = runSpace.CreatePipeline();
        //    pipeline.Commands.Add("Import-Module");
        //    pipeline.Commands[0].Parameters.Add("-Name", "ThreadJob");
        //    pipeline.Invoke();
        //    using var ps = PowerShell.Create(runSpace);
        //    ps.AddCommand("Start-ThreadJob")
        //    .AddParameter("-ScriptBlock", "{Copy-Item ../../../PowerShellTests.cs ./A_PowerShellTests.cs}");
        //    var rs = ps.Invoke();
        //    //Assert.Empty(rs);
        //    //Assert.True(ps.HadErrors);
        //    //Assert.NotEmpty(ps.Streams.Error);
        //}


    }
}
