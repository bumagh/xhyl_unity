using FullInspector;

public class UpdateFullInspectorRootDirectory : fiSettingsProcessor
{
	public void Process()
	{
		fiSettings.RootDirectory = "Assets/Tools/FullInspector2/";
	}
}
