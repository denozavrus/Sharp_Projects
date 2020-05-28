using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
	interface IProduct<T>
	{
		long Number { get; set;}
		T Money { get; set;}
		string Name { get; set;}
		int Amount { get; set; }
		bool Drop(); 
	}

	class Snacks<T>: IProduct<T> 
	{
		long number;
		T money;
		string name;
		int amount;

		public override string ToString()
		{
			return "Номер: " + Number + " Название: " + Name + " Цена: " + Money; 
		}

		public Snacks (long number, T money, string name, int amount)
		{
			this.Number = number;
			this.Money = money;
			this.Name = name;
			this.Amount = amount; 
		}
		public long Number { get { return number; }
			set
			{
				if (value >= 0)
					number = value; 
			}
		}
		public T Money {
			get { return money; }
			set
			{
					money = value; 
			}
		}
		public string Name { get { return name; } set {name = value; } }

		public int Amount { get { return amount; }
			set
			{
				if (value > 0)
					amount = value; 
			}
		}

		public bool Drop()
		{
			if (Amount == 0)
			{
				Console.WriteLine ("Товар закончился");
				return false; 
			}
			Console.WriteLine (Name + " упал в специальное отделение"); 
			Amount -= 1;
			return true; 
		}
	}

	interface IController
	{
		bool IsDelivered (); 
	}

	class DeliverControl : IController
	{
		public bool IsDelivered()
		{
			return true;
		}
	}

	class Terminal<T>
	{
		public void OperationEnd()
		{
			cashReader.OperationEnd ();
			if (cardReader != null)
				cardReader.OperationEnd ();
			ChosenProduct = null;
		}

		static IProduct<T> ChosenProduct; 
		List<IProduct<T>> products; 
		public IController controller; 
		CashReader cashReader;
		CardReader cardReader; 

		public Terminal (CardReader card, CashReader cash, IController controller, List<IProduct<T>> product)
		{
			this.products = product;
			this.controller = controller; 
			cashReader = cash;
			cardReader = card; 
		}
		//
		public void Show ()
		{
			Console.WriteLine ("Пожалуйста ознакомьтесь с ассортиментом товаров:"); 
			foreach (var i in products)
			{
				Console.WriteLine (i.ToString());
			}
		}
		//
		public void Choose ()
		{
			while (true)
			{
				Console.WriteLine ("Напишите номер выбранного вами товара");
				var str = Convert.ToInt64 (Console.ReadLine ());

				if (products.Any (a => a.Number == str))
				{
					ChosenProduct = products.Find (a => a.Number == str);
					return;
				}
				Console.WriteLine ("Ошибка в номере товара, пожалуйста введите еще раз");
			}
		}
		//
		public void Payment ()
		{
			while (true)
			{
				Console.WriteLine ("Выберете способ оплаты. Для выбора наличных нажмите 1, для выбора карты нажмите 2");
				var op = Console.ReadLine ();
				if (Convert.ToInt32 (op) == 1)
				{
					cashReader.Price = Convert.ToDouble (ChosenProduct.Money);
					while (true)
					{
						Console.WriteLine ("Внесите сумму");
						cashReader.Balance += cashReader.InsertMoney (Convert.ToDouble (Console.ReadLine ()));
						if (cashReader.IsEnoughMoney ())
						{
							ChosenProduct.Drop ();
							cashReader.change = cashReader.CalculateChange ();
							Console.WriteLine ("Ваша сдача составляет: " + cashReader.change);
							if (!controller.IsDelivered ())
							{
								Console.WriteLine ("Товар не доставлен, извиняемся за неудобства:");
								cashReader.change = cashReader.Price;
								Console.WriteLine ("Ваша сдача составляет: " + cashReader.change);
								return;
							}
							return;
						}
						else
						{
							Console.WriteLine ("Внесено недостаточно средств");
						}
					}
				}
				if (Convert.ToInt32 (op) == 2)
				{
					if (cardReader == null)
					{
						Console.WriteLine ("Извините, в данный момент картридер не работает");
						return;
					}
					Console.WriteLine ("Пожалуйста введите номер карты");
					string cardNumber = Console.ReadLine ();
					if (cardReader.CheckBalance (cardNumber, Convert.ToDouble (ChosenProduct.Money)))
					{
						cardReader.Substract (cardNumber, Convert.ToDouble (ChosenProduct.Money));
						Console.WriteLine ("Деньги были списаны с вашего счета");
						ChosenProduct.Drop ();
					}
					return; 
				}

				if (!( Convert.ToInt32 (op) == 2 ) | !( Convert.ToInt32 (op) == 1 ))
					Console.WriteLine ("Пожалуйста, выберете корректный вариант оплаты");
			}
		}
	}

	class CashReader
	{
		double price;
		double balance; 
		public double change; 
		public double Balance { get { return balance; } set { balance = value; } }
		public double Price { get { return price; } set { price = value;  } }

		public CashReader()
		{
			change = 0.0;
			Price = 0.0;
			Balance = 0.0;

		}
		public CashReader(double balance, double price)
		{
			Balance = balance;
			Price = price;
			change = Balance - Price; 
		}

		public bool IsEnoughMoney ()
		{
			if (Balance > Price)
				return true;
			return false; 
		}

		public double CalculateChange ()
		{
			change = Balance - Price;
			return change; 
		}
		
		public double InsertMoney (double amount)
		{
			Balance = amount;
			return Balance; 
		}

		public CashReader OperationEnd()
		{
			return new CashReader (); 
		}

	}

	class CardReader
	{
		string CardNumber;
		double price; 

		public CardReader ()
		{
			CardNumber = null;
			price = 0; 
		}
		public CardReader (string cardnumber, double price)
		{
			this.CardNumber = cardnumber;
			this.price = price; 
		}
		public bool CheckBalance (string CardNumber, double price)
		{
			//...
			double balance = 3000.0;
			if (price > balance)
				return true;
			return false; 
		}
		public void Substract (string CardNumber, double price)
		{
			//...
		}

		public CardReader OperationEnd()
		{
			return new CardReader (); 
		}
	}

}
