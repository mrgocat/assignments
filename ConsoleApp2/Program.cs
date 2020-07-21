using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace Assignments1
{
    class Program
    {

        public static void Main()
        {
            // Customers listCust = new Customers(); // I'm not using this.. 'cause it means nothing
            List<Customer> listCust = new List<Customer>();

            listCust.Add(new Customer(1, "Ray Kim"));
            listCust.Add(new Customer(2, "Bhumi Purohit"));
            listCust.Add(new Customer(3, "Vishalkumar Patel"));
            listCust.Add(new Customer(4, "AAA bbb"));
            listCust.Add(new Customer(5, "CCC CDDD"));
            listCust.Add(new Customer(6, "EEE FFF"));

            // Orders listOrder = new Orders();// I'm not using this.. 'cause it means nothing
            List<Order> listOrder = new List<Order>(); 
            listOrder.Add(new Order(1, 1, 5));
            listOrder.Add(new Order(2, 4, 1));
            listOrder.Add(new Order(3, 2, 2));
            listOrder.Add(new Order(4, 2, 5));
            listOrder.Add(new Order(5, 3, 3));
            listOrder.Add(new Order(6, 4, 1));
            listOrder.Add(new Order(7, 3, 6));
            listOrder.Add(new Order(8, 1, 5));

            Console.WriteLine("");
            Console.WriteLine("tempList1 &&&&&&&&&&&&&&&&&&&&&&&&");
            IEnumerable<Customer> tempList = listCust.OrderBy(x => x.CustomerName);
            foreach (var item in tempList)
            {
                Console.WriteLine(item.CustomerName);
            }

            Console.WriteLine("");
            Console.WriteLine("tempList2 &&&&&&&&&&&&&&&&&&&&&&&&");
            IEnumerable<Order> tempList2
                = listOrder.OrderBy(x => x.CustomerNo);
            foreach (var item in tempList2)
            {
                Console.WriteLine(item.OrderNo + ", " + item.CustomerNo + ", " + item.NumOfItems);
            }

            Console.WriteLine("");
            Console.WriteLine("tempList3 &&&&&&&&&&&&&&&&&&&&&&&&");
            var tempList3 =
                from order in listOrder
                group order by order.CustomerNo into newGroup
                orderby newGroup.Key
                select new { newGroup.Key, SumItem = newGroup.Sum(x => x.NumOfItems)
                    , Count = newGroup.Count(), orders = newGroup } ; 

            foreach (var item in tempList3)
            {
                Console.WriteLine("group: " + item.Key + ", " + item.SumItem + ", " + item.Count);
                foreach(var order in item.orders)
                {
                    Console.WriteLine($"\t{order.OrderNo}, {order.CustomerNo}, {order.NumOfItems}");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("tempList4 &&&&&&&&&&&&&&&&&&&&&&&&");
            var tempList4 =
                from order in listOrder
                join cust in listCust on order.CustomerNo equals cust.CustomerNo 
                select new { order.OrderNo, order.CustomerNo, order.NumOfItems, cust.CustomerName };
            foreach (var item in tempList4)
            {
                Console.WriteLine(item.OrderNo + ", " + item.CustomerNo + ", " + item.NumOfItems + ", " + item.CustomerName);
            }

            Console.WriteLine("");
            Console.WriteLine("tempList5 &&&&&&&&&&&&&&&&&&&&&&&&");

            var tempList5 =
                from order in listOrder
                join cust in listCust on order.CustomerNo equals cust.CustomerNo
                group new { order.OrderNo, order.CustomerNo, order.NumOfItems, cust.CustomerName } by cust.CustomerName into myGroup
                orderby myGroup.Key
                select myGroup;
                ;
            foreach (var item in tempList5)
            {
                Console.WriteLine("Customer group:" + item.Key);
                foreach (var order in item)
                {
                    Console.WriteLine($"\t{order.OrderNo}, {order.CustomerName}, {order.NumOfItems}");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("tempList6 &&&&&&&&&&&&&&&&&&");
            var tempList6 =
                from order in listOrder
                join cust in listCust on order.CustomerNo equals cust.CustomerNo
                group new { order.OrderNo, order.CustomerNo, order.NumOfItems, cust.CustomerName } by cust.CustomerName into myGroup
                orderby myGroup.Key descending
                select myGroup;

            foreach (var item in tempList6)
            {
                Console.WriteLine("Customer group:" + item.Key);
                foreach (var order in item)
                {
                    Console.WriteLine($"\t{order.OrderNo}, {order.CustomerName}, {order.NumOfItems}");
                }
            }


        }
    }
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public Customer(int customerNo, string customerName)
        {
            CustomerNo = customerNo;
            CustomerName = customerName;
        }
    }
    public class Order
    {
        public int OrderNo { get; set; }
        public int CustomerNo { get; set; }
        public int NumOfItems { get; set; }
        public Order(int orderNo, int customerNo, int nomOfItems)
        {
            OrderNo = orderNo;
            CustomerNo = customerNo;
            NumOfItems = nomOfItems;
        }
    }
    public class Customers : IEnumerable<Customer> // useless coding.... 
    {
        private List<Customer> list = new List<Customer>();
        public IEnumerator<Customer> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public void add(Customer item)
        {
            list.Add(item);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class Orders : IEnumerable<Order> // useless coding.... 
    {
        private List<Order> list = new List<Order>();
        public IEnumerator<Order> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public void add(Order item)
        {
            list.Add(item);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}