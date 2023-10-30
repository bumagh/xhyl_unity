public class XLDT_CardDesk : XLDT_BaseEntity
{
	public int roomId;

	public string name;

	public int autoKick;

	public int minGold;

	public int gameXianHong;

	public int exchange;

	public int minYaFen;

	public int wheelLocal;

	public int dayLocal;

	public int onceExchangeValue;

	public int state;

	public int baseYaFen;

	public int orderBy;

	public long sumYaFen;

	public long sumDeFen;

	public XLDT_Seat[] seats = new XLDT_Seat[8];
}
