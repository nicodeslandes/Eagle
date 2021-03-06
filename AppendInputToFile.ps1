param ($fileName)

[io.directory]::SetCurrentDirectory((Get-Location))

while($true) {
	$k = [console]::ReadKey().KeyChar
	if ($k -eq 'q') { break; }
	if ($k -eq "`r") { $k = "`n"; write "" }

	$path = [io.path]::GetFullPath($fileName)
	if (! [io.file]::Exists($path))
	{
		[io.file]::WriteAllText($path, $k, [text.encoding]::UTF8)
	}
	else
	{
		[io.file]::AppendAllText($path, $k, [text.encoding]::UTF8)
	}
}