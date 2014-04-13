namespace FileViewer
{
    public class MockLineProvider : ILineProvider
    {
        private const string File = @"System.IO.FileLoadException was unhandled by user code
  HResult=-2146234299
  Message=Could not load file or assembly 'Microsoft.TestCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35' or one of its dependencies. Strong name signature could not be verified.  The assembly may have been tampered with, or it was delay signed but not fully signed with the correct private key. (Exception from HRESULT: 0x80131045)
  Source=mscorlib
  FileName=Microsoft.TestCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
  FusionLog=""
  StackTrace:
       at System.Reflection.RuntimeAssembly._nLoad(AssemblyName fileName, String codeBase, Evidence assemblySecurity, RuntimeAssembly locationHint, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)
       at System.Reflection.RuntimeAssembly.nLoad(AssemblyName fileName, String codeBase, Evidence assemblySecurity, RuntimeAssembly locationHint, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)
       at System.Reflection.RuntimeAssembly.InternalLoadAssemblyName(AssemblyName assemblyRef, Evidence assemblySecurity, RuntimeAssembly reqAssembly, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)
       at System.Reflection.RuntimeAssembly.InternalLoad(String assemblyString, Evidence assemblySecurity, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean forIntrospection)
       at System.Reflection.RuntimeAssembly.InternalLoad(String assemblyString, Evidence assemblySecurity, StackCrawlMark& stackMark, Boolean forIntrospection)
       at System.AppDomain.Load(String assemblyString)
       at System.AppDomain.Load(String assemblyString)
       at System.Web.WebPages.TestUtils.AppDomainUtils.RunInSeparateAppDomain(AppDomainSetup setup, Action action) in C:\Projects\Codeplex\aspnetwebstack\test\Microsoft.TestCommon\AppDomainUtils.cs:line 33
       at System.Web.WebPages.TestUtils.AppDomainUtils.RunInSeparateAppDomain(Action action) in C:\Projects\Codeplex\aspnetwebstack\test\Microsoft.TestCommon\AppDomainUtils.cs:line 16
       at Microsoft.Web.Helpers.Test.MapsTest.GetProviderHtml_DoesNotContainBadRazorCompilation() in C:\Projects\Codeplex\aspnetwebstack\test\Microsoft.Web.Helpers.Test\MapsTest.cs:line 64
  InnerException: System.IO.FileLoadException
       HResult=-2146234299
       Message=Could not load file or assembly 'Microsoft.TestCommon' or one of its dependencies. Strong name signature could not be verified.  The assembly may have been tampered with, or it was delay signed but not fully signed with the correct private key. (Exception from HRESULT: 0x80131045)
       FileName=Microsoft.TestCommon
       FusionLog==== Pre-bind state information ===
LOG: DisplayName = Microsoft.TestCommon
 (Partial)
WRN: Partial binding information was supplied for an assembly:
WRN: Assembly Name: Microsoft.TestCommon | Domain ID: 3
WRN: A partial bind occurs when only part of the assembly display name is provided.
WRN: This might result in the binder loading an incorrect assembly.
WRN: It is recommended to provide a fully specified textual identity for the assembly,
WRN: that consists of the simple name, version, culture, and public key token.
WRN: See whitepaper http://go.microsoft.com/fwlink/?LinkId=109270 for more information and common solutions to this issue.
LOG: Appbase = file:///C:/Users/Nicolas/AppData/Local/NCrunch/11868/3/bin/Debug/Test
LOG: Initial PrivatePath = C:\Users\Nicolas\AppData\Local\NCrunch\11868\3\bin\Debug\Test
Calling assembly : (Unknown).
===
LOG: This bind starts in default load context.
LOG: Found application configuration file (C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\Extensions\Remco Software\NCrunch for Visual Studio 2013\nCrunch.TaskRunner45.x86.exe.Config).
LOG: Using application configuration file: C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\Extensions\Remco Software\NCrunch for Visual Studio 2013\nCrunch.TaskRunner45.x86.exe.Config
LOG: Using host configuration file: 
LOG: Using machine configuration file from C:\Windows\Microsoft.NET\Framework\v4.0.30319\config\machine.config.
LOG: Policy not being applied to reference at this time (private, custom, partial, or location-based assembly bind).
LOG: Attempting download of new URL file:///C:/Users/Nicolas/AppData/Local/NCrunch/11868/3/bin/Debug/Test/Microsoft.TestCommon.DLL.
LOG: Using application configuration file: C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\Extensions\Remco Software\NCrunch for Visual Studio 2013\nCrunch.TaskRunner45.x86.exe.Config
LOG: Using host configuration file: 
LOG: Using machine configuration file from C:\Windows\Microsoft.NET\Framework\v4.0.30319\config\machine.config.
LOG: Post-policy reference: Microsoft.TestCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
ERR: Failed to complete setup of assembly (hr = 0x80131045). Probing terminated.

       StackTrace:
       InnerException:";

        private int _currentLine;
        private static readonly string[] Lines = File.Split('\n');

        public string GetNextLine()
        {
            if (_currentLine >= Lines.Length)
                return null;

            return Lines[_currentLine++];
        }
    }
}
