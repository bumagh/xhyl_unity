public class STTF_FishDesk : STTF_BaseEntity
{
	public int roomId;

	public string name;

	public int minGold;

	public int minGunValue;

	public int maxGunValue;

	public int addstepGunValue;

	public int exchange;

	public int onceExchangeValue;

	public int gameDiff;

	public int siteType;

	public int autoKick;

	public long sumYaFen;

	public long sumDaFen;

	public int orderBy;

	public long sumSeat1YaFen;

	public long sumSeat1DeFen;

	public long sumSeat2YaFen;

	public long sumSeat2DeFen;

	public long sumSeat3YaFen;

	public long sumSeat3DeFen;

	public long sumSeat4YaFen;

	public long sumSeat4DeFen;

	public STTF_Seat[] seats = new STTF_Seat[4];
}
