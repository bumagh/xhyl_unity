using System.Collections.Generic;

public class OptOutProvider : TestProvider<OptOut>
{
	public override bool Compare(OptOut before, OptOut after)
	{
		return before.publicField == after.publicField && before.publicAutoProperty == after.publicAutoProperty && before.publicManualProperty == after.publicManualProperty && before.GetPrivateField() == after.GetPrivateField() && before.GetPrivateAutoProperty() == after.GetPrivateAutoProperty() && before.GetIgnoredField() != after.GetIgnoredField() && before.GetIgnoredAutoProperty() != after.GetIgnoredAutoProperty();
	}

	public override IEnumerable<OptOut> GetValues()
	{
		yield return new OptOut(1, 1, 1, 1, 1, 1, 1);
	}
}
