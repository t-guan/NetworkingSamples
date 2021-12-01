using System;

public class OrderEntry
{
	public string productName;
	public int quantity;
	public OrderEntry(string product_Name, int order_Quantity)
	{
		productName = product_Name;
		quantity = order_Quantity;
	}
}
