using FullInspector;

public class FK3_UpdateFullInspectorRootDirectory : fiSettingsProcessor
{
	public void Process()
	{
		fiSettings.RootDirectory = "Assets/Tools/FullInspector2/";
	}
}
