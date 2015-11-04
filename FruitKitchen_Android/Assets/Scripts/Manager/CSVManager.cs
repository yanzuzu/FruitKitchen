using UnityEngine;
using System.Collections;

public class CSVManager : ManagerComponent< CSVManager > {
	public CSVTable StageInfoTable = new CSVTable();
	public CSVTable ItemInfoTable = new CSVTable();
	public CSVTable StoreSaleTable = new CSVTable();
	// Use this for initialization
	void Awake () {
		// load Stage Info table
		StageInfoTable.Setup("StageInfo");
		ItemInfoTable.Setup("Items");
		StoreSaleTable.Setup("StoreSale");
	}

}
