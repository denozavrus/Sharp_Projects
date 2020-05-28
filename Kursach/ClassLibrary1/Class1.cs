using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Card 
    {
        public List<string> suits = new List<string>(4) { "Черви", "Пики", "Крести", "Буби" };
        public List<int> values = new List<int>() {0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }; 
        public Card (int value, string suit)
        {
            this.value = value;
            this.suit = suit; 
        }
        private int value;
        private string suit;
        public int Value
        { get => value;

            set
            {
                if (values.Contains(value))
                    this.value = value;
            } 

        }
        public string Suit
        { get => suit;

            set
            {
                if (suits.Contains(value))
                    this.suit = value; 
            }
        }
    }

    public class Table 
    {
        public decimal Pot { get; set; }
        public List<Card> Cards { get; set; }

        public Table()
        {

        }

        public Table (List<Card> cards,int pot)
        {
            this.Pot = pot; 
            this.Cards = cards; 
        }
        public bool IsTurn ()
        {
            if (Cards.Count(s => s != null) >= 4)
                return true;
            return false;  
        }
        public bool IsRiver ()
        {
            if (Cards.Count(s => s !=null) == 5)
                return true;
            return false;
        }
    }

    public class CardCombination
    {
        public CardCombination (Card card1, Card card2)
        {
            this.Card1 = card1;
            this.Card2 = card2;
        }
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
    }

    public class Combinations
    {
        public int IsPair1 = 0;
        public int IsDPair1 = 0;
        public int IsSet1 = 0;
        public int IsStreet1 = 0;
        public int IsFlash1 = 0;
        public int IsFull1 = 0;
        public int IsCare1 = 0;
        public int IsSFlash1 = 0;

        public bool IsPair (Table table, CardCombination combination)
        {
            foreach (var i in table.Cards)
                if ((i.Value == combination.Card1.Value) || (i.Value == combination.Card2.Value))
                {
                    IsPair1 = i.Value;
                    return true;
                }
            return false; 
        }
        public bool IsDPair (Table table, CardCombination combination)
        {
            CardCombination combination1 = new CardCombination(combination.Card1, combination.Card2); 
                if (IsPair(table, new CardCombination(combination1.Card1, new Card(0, "Буби"))) && IsPair(table, new CardCombination(combination1.Card2, new Card(0, "Буби"))))
                {
                    IsDPair1 = (combination1.Card1.Value > combination1.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value; 
                    return true;
                }
            return false; 
        }
        public bool IsSet (Table table, CardCombination combination)
        {
            if ((table.Cards.Count(s => s.Value == combination.Card1.Value) == 2) || (table.Cards.Count(s => s.Value == combination.Card2.Value) == 2))
                IsSet1 = (combination.Card1.Value > combination.Card2.Value) ? combination.Card1.Value : combination.Card2.Value;
            foreach (var i in table.Cards)
                if ((i.Value == combination.Card1.Value) && (i.Value == combination.Card2.Value))
                {
                    IsSet1 = (combination.Card1.Value > combination.Card2.Value) ? combination.Card1.Value : combination.Card2.Value;
                    return true;
                }
            return false;
        }
        public bool IsStreet(Table table, CardCombination combination)
        {
            CardCombination combination1 = new CardCombination(combination.Card1, combination.Card2);
            List<int> array = new List<int> {0,0,0,0,0,0,0};
            array[0] = combination1.Card1.Value;
            array[1] = combination1.Card2.Value;

            for (int i = 2; i<=6; i++)
                array[i] = table.Cards[i - 2].Value;
            array.Sort();
            int a = 0;
            int str = 0;
            for (int i = 1; i<=6; i++)
            {
                if (a == 4)
                    str = array[i];
                
                if (array[i] > array[i - 1])
                    a++;

                else
                    a = 0;
            }
            if (str != 0)
            {
                IsStreet1 = str;
                return true;
            }
            else return false;
        }
        public bool IsFlash(Table table, CardCombination combination)
        {
            CardCombination combination1 = new CardCombination(combination.Card1, combination.Card2);
            List<string> list = new List<string> {null,null,null,null, null, null, null, null,null,null };
            list[0] = combination1.Card1.Suit;
            list[1] = combination1.Card2.Suit;
            for (int i = 2; i <= 6; i++)
                list[i] = table.Cards[i - 2].Suit;
            if (list.FindAll(u => u == "Черви").Count == 5 )
            {
                if ((combination1.Card1.Suit == "Черви") && (combination1.Card2.Suit == "Черви"))
                {
                    IsFlash1 = (combination1.Card1.Value > combination.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value;
                    return true;
                }
                if ((combination1.Card1.Suit == "Черви") && (combination1.Card2.Suit != "Черви"))
                {
                    IsFlash1 = combination1.Card1.Value;
                    return true;
                }
                if ((combination1.Card1.Suit != "Черви") && (combination1.Card2.Suit == "Черви"))
                {
                    IsFlash1 = combination1.Card2.Value;
                    return true;
                }
            }
            if (list.FindAll(u => u == "Буби").Count == 5)
            {
                if ((combination1.Card1.Suit == "Буби") && (combination1.Card2.Suit == "Буби"))
                {
                    IsFlash1 = (combination1.Card1.Value > combination1.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value;
                    return true;
                }
                if ((combination1.Card1.Suit == "Буби") && (combination1.Card2.Suit != "буби"))
                {
                    IsFlash1 = combination1.Card1.Value;
                    return true;
                }
                if ((combination1.Card1.Suit != "Буби") && (combination1.Card2.Suit == "Буби"))
                {
                    IsFlash1 = combination1.Card2.Value;
                    return true;
                }
            }
            if (list.FindAll(u => u == "Крести").Count == 5)
            {
                if ((combination1.Card1.Suit == "Крести") && (combination1.Card2.Suit == "Крести"))
                {
                    IsFlash1 = (combination1.Card1.Value > combination1.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value;
                    return true;
                }
                if ((combination1.Card1.Suit == "Крести") && (combination1.Card2.Suit != "Крести"))
                {
                    IsFlash1 = combination1.Card1.Value;
                    return true;
                }
                if ((combination1.Card1.Suit != "Крести") && (combination1.Card2.Suit == "Крести"))
                {
                    IsFlash1 = combination1.Card2.Value;
                    return true;
                }
            }
            if (list.FindAll(u => u == "Пики").Count == 5)
            {
                if ((combination1.Card1.Suit == "Пики") && (combination1.Card2.Suit == "Пики"))
                {
                    IsFlash1 = (combination1.Card1.Value > combination1.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value;
                    return true;
                }
                if ((combination1.Card1.Suit == "Пики") && (combination1.Card2.Suit != "Пики"))
                {
                    IsFlash1 = combination1.Card1.Value;
                    return true;
                }
                if ((combination1.Card1.Suit != "Пики") && (combination1.Card2.Suit == "Пики"))
                {
                    IsFlash1 = combination1.Card2.Value;
                    return true;
                }
            }
            return false; 
        }
        public bool IsFull(Table table, CardCombination combination)
        {
            CardCombination combination1 = new CardCombination(combination.Card1, combination.Card2); 
                if ( 
                    (IsSet(table,new CardCombination(combination1.Card1, new Card(0, "Буби"))) && IsPair(table,new CardCombination(new Card (0,"Буби"),combination1.Card2))) 
                    || (IsSet(table, new CardCombination(combination1.Card2, new Card(0, "Буби"))) && IsPair(table, new CardCombination(new Card(0, "Буби"), combination1.Card1)))
                    )
                {
                    IsFull1 = (combination1.Card1.Value > combination1.Card2.Value) ? combination1.Card1.Value : combination1.Card2.Value;
                    return true;
                }
            return false;
        }
        public bool IsCare(Table table, CardCombination combination)
        {
            if (((table.Cards.Count(s => s.Value == combination.Card1.Value) == 2) && (combination.Card1.Value == combination.Card2.Value))
                || (table.Cards.Count(s => s.Value == combination.Card1.Value) == 3) || (table.Cards.Count(s => s.Value == combination.Card2.Value) == 2))
            {
                IsCare1 = (combination.Card1.Value > combination.Card2.Value) ? combination.Card1.Value : combination.Card2.Value;
                return true; 
            }
            return false;
        }
        public bool IsSFlash(Table table, CardCombination combination)
        {
            if ((IsFlash(table,combination)) && (IsStreet(table,combination)))
            {
                IsSFlash1 = (combination.Card1.Value > combination.Card2.Value) ? combination.Card1.Value : combination.Card2.Value;
                return true;
            }
            return false;
        }
        public bool IsRoyle(Table table, CardCombination combination)
        {
            if (IsSFlash(table, combination) && (IsStreet1 == 14))
                return true; 
            return false;
        }
    }

    public class Opponent: Combinations
    {
        public List<CardCombination> range;
        public Opponent (int stack, List<CardCombination> combination)
        {
            range = combination;
        }
    }

    public class Player : Combinations
    {
        private decimal stack;
        public decimal Stack { get => stack; set => stack = value; }
        public List<CardCombination> range;
        public List<double> equity; 

        public Player(decimal stack, List<CardCombination> combination)
        {
            this.stack = stack;
            range = combination;
        }

        public void Winrate (Player player, Opponent opponent, Table table, ref List<double> equity)
        {
            List<Card> Deck = new List<Card> {new Card(2,"Черви"), new Card(3, "Черви"),new Card(4, "Черви"),new Card(5, "Черви"),new Card(6, "Черви"),new Card(7, "Черви"),new Card(8, "Черви"),
                new Card(9, "Черви"),new Card(10, "Черви"),new Card(11, "Черви"),new Card(12, "Черви"),new Card(13, "Черви"),new Card(14, "Черви"),

                new Card(2,"Крести"), new Card(3, "Крести"),new Card(4, "Крести"),new Card(5, "Крести"),new Card(6, "Крести"),new Card(7, "Крести"),new Card(8, "Крести"),
                new Card(9, "Крести"),new Card(10, "Крести"),new Card(11, "Крести"),new Card(12, "Крести"),new Card(13, "Крести"),new Card(14, "Крести"),

                new Card(2,"Буби"), new Card(3, "Буби"),new Card(4, "Буби"),new Card(5, "Буби"),new Card(6, "Буби"),new Card(7, "Буби"),new Card(8, "Буби"),
                new Card(9, "Буби"),new Card(10, "Буби"),new Card(11, "Буби"),new Card(12, "Буби"),new Card(13, "Буби"),new Card(14, "Буби"),

                new Card(2,"Пики"), new Card(3, "Пики"),new Card(4, "Пики"),new Card(5, "Пики"),new Card(6, "Пики"),new Card(7, "Пики"),new Card(8, "Пики"),
                new Card(9, "Пики"),new Card(10, "Пики"),new Card(11, "Пики"),new Card(12, "Пики"),new Card(13, "Пики"),new Card(14, "Пики")

            };

            foreach(var i in table.Cards)
                for (int a = 0; a< Deck.Count;a++)
                    if ((i.Suit == Deck[a].Suit) && (i.Value == Deck[a].Value))
                    {
                        Deck.RemoveAt(a);
                        a--; 
                    }
            foreach (var i in table.Cards)
                for (int a = 0; a < player.range.Count; a++)
                    if (((player.range[a].Card1.Suit == i.Suit) && (player.range[a].Card1.Value == i.Value)) || ((player.range[a].Card2.Suit == i.Suit) && (player.range[a].Card2.Value == i.Value)))
                    {
                        player.range.RemoveAt(a);
                        a--;
                    }
            foreach (var i in table.Cards)
                for (int a = 0; a < opponent.range.Count; a++)
                    if (((opponent.range[a].Card1.Suit == i.Suit) && (opponent.range[a].Card1.Value == i.Value)) || ((opponent.range[a].Card2.Suit == i.Suit) && (opponent.range[a].Card2.Value == i.Value)))
                    {
                        opponent.range.RemoveAt(a);
                        a--;
                    }
            int MC = 10000; 
            if (player.range.Count > 50)
            {
                MC = 1000; 
            }
            List<Card> Deck1 = Deck.GetRange(0,Deck.Count);
            foreach (var i in player.range)
            {
                int wins = 0;
                Deck1 = Deck.GetRange(0, Deck.Count);
                List<CardCombination> OpRange = opponent.range.GetRange(0,opponent.range.Count);
                OpRange.RemoveAll(s => ((s.Card1.Value == i.Card1.Value) && (s.Card1.Suit == i.Card1.Suit))||((s.Card1.Value == i.Card2.Value)&&(s.Card1.Suit == i.Card2.Suit)));
                OpRange.RemoveAll(s => ((s.Card2.Value == i.Card1.Value) && (s.Card2.Suit == i.Card1.Suit)) || ((s.Card2.Value == i.Card2.Value) && (s.Card2.Suit == i.Card2.Suit)));
                Deck1.RemoveAll(s => ((i.Card1.Suit == s.Suit)&&(i.Card1.Value == s.Value)) || ((i.Card2.Suit == s.Suit) && (i.Card2.Value == s.Value))); 
                for (int a = 0; a<MC; a++)
                {
                    Random rnd = new Random();
                    CardCombination OpHand = OpRange[rnd.Next(0,OpRange.Count)];
                    if (table.IsTurn() && (!table.IsRiver()))
                        table.Cards.Add(Deck1[rnd.Next(0,Deck1.Count)]);

                    //Проверка на Рояль

                    if (player.IsRoyle(table, i) && (!opponent.IsRoyle(table, OpHand)))
                    {
                        wins++;
                        continue; 
                    }
                    if (!player.IsRoyle(table, i) && (opponent.IsRoyle(table, OpHand)))
                        continue;

                    //Проверка на Стрит Флеш

                    if (player.IsSFlash(table, i) && (!opponent.IsSFlash(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsSFlash(table, i) && (opponent.IsSFlash(table, OpHand)))
                        continue;

                    if (player.IsSFlash(table, i) && opponent.IsSFlash(table, OpHand))
                        if (player.IsSFlash1 > opponent.IsSFlash1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Проверка на Каре

                    if (player.IsCare(table, i) && (!opponent.IsCare(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsCare(table, i) && (opponent.IsCare(table, OpHand)))
                        continue;

                    if (player.IsCare(table, i) && opponent.IsCare(table, OpHand))
                        if (player.IsCare1 > opponent.IsCare1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    // Проверка на Фулл Хаус

                    if (player.IsFull(table, i) && (!opponent.IsFull(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsFull(table, i) && (opponent.IsFull(table, OpHand)))
                        continue;

                    if (player.IsFull(table, i) && opponent.IsFull(table, OpHand))
                        if (player.IsFull1 > opponent.IsFull1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    // Проверка на Флеш 

                    if (player.IsFlash(table, i) && (!opponent.IsFlash(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsFlash(table, i) && (opponent.IsFlash(table, OpHand)))
                        continue;

                    if (player.IsFlash(table, i) && opponent.IsFlash(table, OpHand))
                        if (player.IsFlash1 > opponent.IsFlash1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Проверка на Стрит 

                    if (player.IsStreet(table, i) && (!opponent.IsStreet(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsStreet(table, i) && (opponent.IsStreet(table, OpHand)))
                        continue;

                    if (player.IsStreet(table, i) && opponent.IsStreet(table, OpHand))
                        if (player.IsStreet1 > opponent.IsStreet1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Проверка на Сет

                    if (player.IsSet(table, i) && (!opponent.IsSet(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsSet(table, i) && (opponent.IsSet(table, OpHand)))
                        continue;

                    if (player.IsSet(table, i) && opponent.IsSet(table, OpHand))
                        if (player.IsSet1 > opponent.IsSet1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Проверка на двойную пару

                    if (player.IsDPair(table, i) && (!opponent.IsDPair(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsDPair(table, i) && (opponent.IsDPair(table, OpHand)))
                        continue;

                    if (player.IsDPair(table, i) && opponent.IsDPair(table, OpHand))
                        if (player.IsDPair1 > opponent.IsDPair1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Провера на Пару

                    if (player.IsPair(table, i) && (!opponent.IsPair(table, OpHand)))
                    {
                        wins++;
                        continue;
                    }
                    if (!player.IsPair(table, i) && (opponent.IsPair(table, OpHand)))
                        continue;

                    if (player.IsPair(table, i) && opponent.IsPair(table, OpHand))
                        if (player.IsPair1 > opponent.IsPair1)
                        {
                            wins++;
                            continue;
                        }
                        else continue;

                    //Проверка на старшую карту 

                    int plmax = (i.Card1.Value > i.Card2.Value) ? i.Card1.Value : i.Card2.Value;
                    int oppmax = (OpHand.Card1.Value > OpHand.Card2.Value) ? OpHand.Card1.Value : OpHand.Card2.Value;
                    if (plmax > oppmax)
                        wins++;
                }

                equity.Add(Math.Round((Convert.ToDouble(wins)/Convert.ToDouble(MC) * 100.0),3)); 
            }
        }
    }
}
