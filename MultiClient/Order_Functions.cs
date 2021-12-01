using System;
using System.Collections.Generic;
using System.Text;

public static class Order_Functions
{
    public static List<OrderEntry> parse_Order_Message(string message)
    {
        List<OrderEntry> temp_Order = new List<OrderEntry>();
        string[] order_Message = message.Split(',');

        for (int i = 0; i < order_Message.Length; i = i + 2)
        {
            OrderEntry temp = new OrderEntry(order_Message[i], Convert.ToInt32(order_Message[i + 1]));
            temp_Order.Add(temp);
        }

        return temp_Order;     
    }
}
